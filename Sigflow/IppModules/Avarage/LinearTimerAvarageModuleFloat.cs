using System.Threading;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Avarage
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class LinearTimerAvarageModuleFloat:IExecuteModule, IMasterModule
    {
        private volatile bool _ready;

        private Timer _timer;
        private int _timerPeriod;

        public bool Start()
        {
            _ready = false;
            _timer = new Timer(o => _ready=true, null, 0, Period);
            _timerPeriod = Period;

            return true;
        }

        public void BeforeStop()
        {
         
        }

        public void AfterStop()
        {
            _timer.Dispose();
        }

        public unsafe bool? Execute()
        {
            var period = Period;
            if (_timerPeriod != period)
                _timer.Change(0, period);

            if (!In.NextBlockSize.HasValue)
                return false;

            var blockSize = In.NextBlockSize.Value;

            if(_data.Length!= blockSize)
                _data=new float[blockSize];

            var src = In.Take();
            if (src == null)
                return false;
            
            fixed (float* pData = _data, pSrc=src)
            {
                if(_counter==0)
                    ipp.sp.ippsZero_32f(pData, blockSize);

                ipp.sp.ippsAdd_32f_I(pSrc, pData, blockSize);

                _counter++;

                if (_ready)
                {
                    _ready = false;

                    ipp.sp.ippsMulC_32f_I((float)1 / _counter, pData, blockSize);

                    Out.Write(_data);
                    _counter = 0;
                }
            }

            In.Put(src);

            return true;
        }

        private float[] _data=new float[0];
        private int _counter;

        
        /// <summary>
        /// Период срабатывания в миллисекундах.
        /// </summary>
        public int Period { get; set; }

        public ISignalReader<float> In { get; set; }
        public ISignalWriter<float> Out { get; set; }
    }
}
