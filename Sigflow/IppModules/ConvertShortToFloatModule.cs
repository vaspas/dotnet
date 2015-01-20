using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class ConvertShortToFloatModule:IExecuteModule
    {
        public ConvertShortToFloatModule()
        {
            Norma = 1;
        }

        public unsafe bool? Execute()
        {
            if (!In.NextBlockSize.HasValue)
                return false;

            var blockSize = In.NextBlockSize.Value;

            if (_data.Length != blockSize)
                _data = new float[blockSize];

            var srcData = In.Take();
            if (srcData==null)
                return false;

            fixed (short* pSrcData = srcData)
            fixed (float* pDstData = _data)
            {
                ipp.sp.ippsConvert_16s32f(pSrcData, pDstData, blockSize);
                ipp.sp.ippsMulC_32f_I(Norma, pDstData, blockSize);
            }
            
            Out.Write(_data);

            In.Put(srcData);

            return true;
        }

        public float Norma { get; set; }

        private float[] _data=new float[0];

        public ISignalReader<short> In { get; set; }

        public ISignalWriter<float> Out { get; set; }
    }
}
