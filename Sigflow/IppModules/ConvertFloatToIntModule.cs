using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class ConvertFloatToIntModule:IExecuteModule
    {
        public ConvertFloatToIntModule()
        {
            ScaleFactor = 1;
        }

        public unsafe bool? Execute()
        {
            if (!In.NextBlockSize.HasValue)
                return false;

            var blockSize = In.NextBlockSize.Value;

            if (_data.Length != blockSize)
                _data = new int[blockSize];

            var srcData = In.Take();
            if (srcData==null)
                return false;

            fixed (float* pSrcData = srcData)
            fixed (int* pDstData = _data)
            {
                ipp.sp.ippsConvert_32f32s_Sfs(pSrcData, pDstData, blockSize,ipp.IppRoundMode.ippRndZero,ScaleFactor);
            }
            
            Out.Write(_data);

            In.Put(srcData);

            return true;
        }

        public int ScaleFactor { get; set; }

        private int[] _data=new int[0];

        public ISignalReader<float> In { get; set; }

        public ISignalWriter<int> Out { get; set; }
    }
}
