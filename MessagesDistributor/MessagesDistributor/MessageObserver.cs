using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MessagesDistributor
{
    public class MessageObserver:IMessageObserver
    {
        public MessageObserver()
        {
            _decreseAction = () => 
            {
                Trace.WriteLine("finishaction " + _myMessage);
                Interlocked.Decrement(ref _counter);

                CheckComplete();
            };
        }

        private int _counter;

        private readonly Action _decreseAction;
        

        public Action RegisterAction(object sender, string name)
        {
            Trace.WriteLine("startaction "+_myMessage);
            
            Interlocked.Increment(ref _counter);

            return _decreseAction;
        }

        private readonly object _sync=new object();
        private bool _completed;

        public void CheckComplete()
        {
            lock (_sync)
            {
                if (_completed)
                    return;
                
                if (_counter != 0)
                    return;

                if (_internalObservers.Count != 0)
                    return;
                
                OnComplete();
                _completed = true;

                Trace.WriteLine("complete " + _myMessage);
            }
        }

        private readonly List<MessageObserver> _internalObservers = new List<MessageObserver>();

        private string _myMessage="nullMessage";

        /// <summary>
        /// Создает объект наблюдателя для нового сообщения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public IMessageObserver CreateSubObserver(object message, object sender)
        {
            var newMessageObserver = new MessageObserver {Sender = sender,_myMessage = message.ToString()};

            Trace.WriteLine("create " + message);
            lock (_sync)
                _internalObservers.Add(newMessageObserver);

            newMessageObserver.OnComplete +=
                () =>
                    {
                        lock (_sync)
                            _internalObservers.Remove(newMessageObserver);
                        CheckComplete();
                    };

            return newMessageObserver;
        }

        public event Action OnComplete = delegate { };

        public object Sender { get; private set; }
    }
}
