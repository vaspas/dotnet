using System.Threading;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Avarage
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class LinearAvarageModuleFloat:IExecuteModule
    {
        public unsafe bool? Execute()
        {
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
                if(_counter<1)
                    ipp.sp.ippsZero_32f(pData, blockSize);

                ipp.sp.ippsAdd_32f_I(pSrc, pData, blockSize);

                _counter++;

                var count = _count;

                if (_counter >= count)
                {
                    ipp.sp.ippsMulC_32f_I(1f / (int)_counter, pData, blockSize);

                    Out.Write(_data);
                    _counter -= count > 1 ? count : 1;
                }
            }

            In.Put(src);

            return true;
        }

        private float[] _data=new float[0];
        private double _counter;

        private double _count;
        public double Count 
        {
            get { return _count; }
            set { Interlocked.Exchange(ref _count, value); }
        }

        public ISignalReader<float> In { get; set; }
        public ISignalWriter<float> Out { get; set; }
    }
}
