using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sigflow.Module;

namespace Sigflow.Performance
{
    /// <summary>
    /// Не потокобезопасный класс.
    /// </summary>
    /// <remarks>
    /// INSPECTED 17/05/2013
    /// </remarks>
    public class Performer
    {
        public Performer()
        {
            ThreadPriority = ThreadPriority.Normal;
        }

        private readonly List<IModule> _modules = new List<IModule>();

        public void AddModule(IModule module)
        {
            //случай если поток обработки не запущен
            //выполняем в текущем потоке
            if (_actions == null)
            {
                _modules.Add(module);
                return;
            }

            //случай если поток обработки запущен
            //выполняем в потоке обработки
            lock (_actions)
                _actions.Add(() =>
                                 {
                                     _modules.Add(module);

                                     if (module is IMasterModule)
                                         (module as IMasterModule).Start();
                                 });
        }

        public void RemoveModule(IModule module)
        {
            //случай если поток обработки не запущен
            //выполняем в текущем потоке
            if (_actions == null)
            {
                _modules.Remove(module);
                return;
            }

            //случай если поток обработки запущен
            //выполняем в потоке обработки
            lock (_actions)
                _actions.Add(() =>
                                 {
                                     _modules.Remove(module);

                                     if (module is IMasterModule)
                                     {
                                         (module as IMasterModule).BeforeStop();
                                         (module as IMasterModule).AfterStop();
                                     }
                                 });
        }

        public bool Contains(IModule module)
        {
            //случай если поток обработки не запущен
            if (_actions == null)
                return _modules.Contains(module);
            
            //случай если поток обработки запущен
            lock (_actions)
                return _modules.Contains(module);
        }

        private List<Action> _actions;

        public void ChangeModule(IModule oldModule, IModule newModule)
        {
            //случай если поток обработки не запущен
            //выполняем в текущем потоке
            if (_actions == null)
            {
                _modules.Insert(_modules.IndexOf(oldModule), newModule);
                _modules.Remove(oldModule);

                return;
            }

            //случай если поток обработки запущен
            //выполняем в потоке обработки
            lock (_actions)
                _actions.Add(() =>
                                 {
                                     _modules.Insert(_modules.IndexOf(oldModule), newModule);
                                     _modules.Remove(oldModule);

                                     if (oldModule is IMasterModule)
                                     {
                                         (oldModule as IMasterModule).BeforeStop();
                                         (oldModule as IMasterModule).AfterStop();
                                     }

                                     if (newModule is IMasterModule)
                                         (newModule as IMasterModule).Start();
                                 });
        }

        public IBeat Beat { get; set; }

        public ThreadPriority ThreadPriority { get; set; }

        private readonly AutoResetEvent _event=new AutoResetEvent(false);
        
        private Thread _thread;

        private volatile bool _threadTerminated;

        private void SetEvent()
        {
            _event.Set();
        }

        public bool Start()
        {            
            _actions = new List<Action>();

            _event.Reset();
            _thread = new Thread(ThreadFunc)
                          {
                              IsBackground = true,
                              Name = "SigproPatternPerformerThread",
                              Priority = ThreadPriority
                          };
            _threadTerminated = false;
            _thread.Start();

            var started = true;
            foreach (var module in _modules.OfType<IMasterModule>().Reverse())
            {
                if (!module.Start())
                    started = false;
            }
	    Beat.Impulse += SetEvent;
            return started;
        }

        public void Stop()
        {
            //ждем завершения выполнения действий
            lock (_actions)
            {
                _actions.ForEach(a => a());
                _actions.Clear();

                _modules.OfType<IMasterModule>().ToList()
                    .ForEach(m => m.BeforeStop());
            }

            _threadTerminated = true;
            _event.Set();
            _thread.Join();

            _modules.OfType<IMasterModule>().ToList()
                .ForEach(m => m.AfterStop());

            _actions = null;
            Beat.Impulse -= SetEvent;
        }

        private void ThreadFunc()
        {
            var executeFlags = new List<bool>();

            while (!_threadTerminated)
            {
                //выполняем действия в процессе работы
                lock (_actions)
                {
                    _actions.ForEach(a => a());
                    _actions.Clear();
                }

                //ждем события для очередной обработки
                _event.WaitOne();

                //проверяем флаг, возможно это завершение работы
                if (_threadTerminated)
                    continue;
                    
                //далее выполняем обработку
               while (executeFlags.Count < _modules.Count)
                    executeFlags.Add(true);
                var executeNext = false;
                for (var i = 0; i < _modules.Count;i++ )
                {
                    if (!(_modules[i] is IExecuteModule))
                    {
                        executeFlags[i] = false;
                        continue;
                    }

                    var r = (_modules[i] as IExecuteModule).Execute();
                    
                    executeFlags[i] = r != null;
                    
                    if (r == true)
                        executeNext = true;
                }

                while (executeNext)
                {
                    executeNext = false;
                    for (var i = 0; i < _modules.Count; i++)
                    {
                        if (!executeFlags[i])
                            continue;

                        if ((_modules[i] as IExecuteModule).Execute() == true)
                            executeNext = true;
                    }
                }

                //старый вариант обработки
                /*var executeModules = _modules.OfType<IExecuteModule>().ToList();
                var executeNext = true;
                var toRemove = new List<IExecuteModule>();
                while (executeNext)
                {
                    toRemove.ForEach(m => executeModules.Remove(m));

                    executeNext = false;
                    foreach (var m in executeModules)
                    {
                        var r = m.Execute();
                        if(r==null)
                            toRemove.Add(m);
                        if (r == true)
                            executeNext = true;
                    }
                }*/
            }
        }
    }
}
