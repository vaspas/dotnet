using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Avarage
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class ExponentialAvarangeModuleFloat:IExecuteModule
    {
        public unsafe bool? Execute()
        {
            if (!In.NextBlockSize.HasValue)
                return false;

            var blockSize = In.NextBlockSize.Value;

            if (_data == null || _data.Length != blockSize)
            {
                _data = new float[blockSize];
                _expdata=new float[blockSize];
            }

            var src = In.Take();
            if (src == null)
                return false;

            var kExp = KExp;

            fixed (float* pData = _data, pExpData=_expdata, pSrc=src)
            {
                ipp.sp.ippsMulC_32f(pSrc, 1f - kExp, pExpData, blockSize);
                ipp.sp.ippsMulC_32f_I(kExp, pData, blockSize);
                ipp.sp.ippsAdd_32f_I(pExpData, pData, blockSize);
            }

            Out.Write(_data);

            In.Put(src);

            return true;
        }

        private float[] _data;
        private float[] _expdata;
        
        public float KExp { get; set; }

        public ISignalReader<float> In { get; set; }
        public ISignalWriter<float> Out { get; set; }
    }
}
