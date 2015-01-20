
using System;
using System.Threading;
using Sigflow.Module;
using Sigflow.Performance;

namespace Modules.Generator
{
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class MasterFrequencyModule:IMasterModule,IBeat
    {
        public event Action Impulse = delegate { };

        public int IntervalMilliseconds { get; set; }

        private Thread _thread;

        private readonly AutoResetEvent _terminate = new AutoResetEvent(false);

        public bool Start()
        {
            _terminate.Reset();
            _thread = new Thread(o => Func()) { IsBackground = true };
            _thread.Start();

            return true;
        }

        private void Func()
        {
            while (!_terminate.WaitOne(IntervalMilliseconds))
                Impulse();
        }

        public void BeforeStop()
        {
            _terminate.Set();

            _thread.Join();
            _thread = null;
        }

        public void AfterStop()
        {
        }
    }
}
