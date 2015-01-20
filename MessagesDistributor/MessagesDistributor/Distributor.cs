using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MessagesDistributor
{
    public class Distributor:IDistributor
    {
        private readonly List<WeakReference> _listeners = new List<WeakReference>();

        public SynchronizationContext SyncContext { get; set; }

        public void Connect(IMessageListener listener)
        {
            lock(_listeners)
                _listeners.Add(new WeakReference(listener));
        }

        private IEnumerable<T> GetListeners<T>()
        {
            lock (_listeners)
            {
                _listeners.RemoveAll(wr => wr.Target == null);
                
                return _listeners.ConvertAll(wr => wr.Target).FindAll(t => t != null).OfType<T>();               
            }
        }


        private readonly FakeMessageObserver _fakeMessageObserver=new FakeMessageObserver();
        
        public void DistributeAsync<T>(T message, object sender)
        {
            DistributeAsync(message, sender, null);
        }

        public void Distribute<T>(T message, object sender)
        {
            Distribute(message, sender, null);
        }

        public void DistributeAsyncExcludeSender<T>(T message, object sender)
        {
            DistributeAsyncExcludeSender(message, sender, null);
        }

        public void DistributeExcludeSender<T>(T message, object sender)
        {
            DistributeExcludeSender(message, sender, null);
        }


        public void DistributeAsync<T>(T message, object sender, IMessageObserver observer)
        {
            var obs = observer ?? _fakeMessageObserver;

            var onComplete = obs.RegisterAction(this, "DistributeAsync<T>");
            ThreadPool.QueueUserWorkItem(o =>
                                             {
                                                 Distribute(message, sender, obs);
                                                 onComplete();
                                             }, null);
        }

        public void Distribute<T>(T message, object sender, IMessageObserver observer)
        {
            var obs = observer ?? _fakeMessageObserver;

            Distribute(message, false,
                obs.CreateSubObserver(message, sender));
        }

        public void DistributeAsyncExcludeSender<T>(T message, object sender, IMessageObserver observer)
        {
            var obs = observer ?? _fakeMessageObserver;

            var onComplete = obs.RegisterAction(this, "DistributeAsyncExcludeSender<T>");
            ThreadPool.QueueUserWorkItem(o => 
            { 
                DistributeExcludeSender(message, sender, obs);
                onComplete();
            }, null);
        }

        public void DistributeExcludeSender<T>(T message, object sender, IMessageObserver observer)
        {
            var obs = observer ?? _fakeMessageObserver;

            Distribute(message, true, obs.CreateSubObserver(message, sender));
        }


        private void Distribute<T>(T message, bool exclude, IMessageObserver observer)
        {
            var listeners = GetListeners<IMessageListener>();

            //асинхронные команды
            foreach (var l in listeners.OfType<IStaAsyncMessageListener<T>>())
            {
                if (exclude && l == observer.Sender)
                    continue;

                var lt = l;

                var onComplete=observer.RegisterAction(lt, "ListenStaAsync");

                var t = new Thread(o =>
                                       {
                                           lt.Listen(message, observer);
                                           onComplete();
                                       });
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
            }

            //асинхронные команды
            foreach (var l in listeners.OfType<IAsyncMessageListener<T>>())
            {
                if (exclude && l == observer.Sender)
                    continue;

                var lt = l;

                var onComplete = observer.RegisterAction(lt, "ListenAsync");

                ThreadPool.QueueUserWorkItem(o => 
                {
                    lt.Listen(message, observer);
                    onComplete();
                });
            }

            //асинхронные команды в потоке UI
            foreach (var l in listeners.OfType<IAsyncUiMessageListener<T>>())
            {
                if (exclude && l == observer.Sender)
                    continue;

                var lt = l;

                var onComplete = observer.RegisterAction(lt, "ListenAsyncUi");

                SyncContext.Post(o =>
                                     {
                                         lt.Listen(message, observer);
                                         onComplete();
                                     }, null);
            }

            DistributeQueue(message, observer, exclude);

            //синхронные команды
            foreach (var l in listeners.OfType<ISyncMessageListener<T>>())
            {
                if (exclude && l == observer.Sender)
                    continue;
                l.Listen(message, observer);
            }

            //синхронные команды в потоке UI
            foreach (var l in listeners.OfType<ISyncUiMessageListener<T>>())
            {
                if (exclude && l == observer.Sender)
                    continue;
                var lt = l;
                SyncContext.Send(o => lt.Listen(message, observer), null);
            }
        }

        class QueueItem<T>
        {
            public T Message;
            public bool ExcludeSender;
            public IMessageObserver Observer;
            public Action OnComplete;
        }

        private readonly List<object> _queues=new List<object>();
        private void DistributeQueue<T>(T message, IMessageObserver observer, bool exclude)
        {
            Queue<QueueItem<T>> queue;
            lock (_queues)
            {
                queue = _queues.FirstOrDefault(o => o is Queue<QueueItem<T>>) as Queue<QueueItem<T>>;
                if (queue == null)
                {
                    queue = new Queue<QueueItem<T>>();
                    _queues.Add(queue);
                }
            }

            var startNewThread = false;

            lock (queue)
            {
                startNewThread = queue.Count == 0;
                queue.Enqueue(new QueueItem<T>
                                  {
                                      Message = message,
                                      Observer = observer,
                                      OnComplete = observer.RegisterAction(this, "MessageQueue"),
                                      ExcludeSender = exclude
                                  });
            }

            if(startNewThread)
                ThreadPool.QueueUserWorkItem(
                    obj =>
                        {
                            while (true)
                            {
                                var listeners = GetListeners<IMessageListener>();

                                QueueItem<T> m;
                                bool isLast;
                                lock (queue)
                                {
                                    m = queue.Peek();
                                    isLast = queue.Count == 1;
                                }

                                foreach (var l in listeners.OfType<IQueueMessageListener<T>>())
                                {
                                    if (m.ExcludeSender && l == m.Observer.Sender)
                                        continue;
                                    l.Listen(m.Message, m.Observer);
                                }

                                foreach (var l in listeners.OfType<IQueueUiMessageListener<T>>())
                                {
                                    if (m.ExcludeSender && l == m.Observer.Sender)
                                        continue;
                                    SyncContext.Send(o => l.Listen(m.Message, m.Observer), null);
                                }

                                if(isLast)
                                {
                                    foreach (var l in listeners.OfType<IQueueLastMessageListener<T>>())
                                    {
                                        if (m.ExcludeSender && l == m.Observer.Sender)
                                            continue;
                                        l.Listen(m.Message, m.Observer);
                                    }
                                    foreach (var l in listeners.OfType<IQueueUiLastMessageListener<T>>())
                                    {
                                        if (m.ExcludeSender && l == m.Observer.Sender)
                                            continue;
                                        SyncContext.Send(o => l.Listen(m.Message, m.Observer), null);
                                    }
                                }

                                lock (queue)
                                {
                                    var item=queue.Dequeue();
                                    item.OnComplete();
                                    if(queue.Count==0)
                                        return;
                                }
                            }
                        });
        }

        public int GetQueueSize<T>()
        {
            Queue<T> queue;
            lock (_queues)
            {
                queue = _queues.FirstOrDefault(o => o is Queue<T>) as Queue<T>;
                if (queue == null)
                    return 0;
            }
            
            lock (queue)
                return queue.Count;
        }
    }
}
