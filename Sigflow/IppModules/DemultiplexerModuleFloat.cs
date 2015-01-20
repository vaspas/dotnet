using System.Collections.Generic;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class DemultiplexerModuleFloat:IExecuteModule
    {
        public DemultiplexerModuleFloat()
        {
            Out=new List<ISignalWriter<float>>();
        }
        
        public unsafe bool? Execute()
        {
            if (!In.NextBlockSize.HasValue)
                return false;

            var channelsCount = Out.Count;

            var blockSize = In.NextBlockSize.Value / channelsCount;

            while (_data.Count<channelsCount)
                _data.Add(new float[blockSize]);
            
            var srcData = In.Take();
            if (srcData==null)
                return false;

            fixed (float* pSrcData = srcData)
                for (var ch = 0; ch < channelsCount; ch++)
                {
                    if (_data[ch].Length != blockSize)
                        _data[ch] = new float[blockSize];

                    int dstLen;
                    fixed (float* pDst = _data[ch])
                        ipp.sp.ippsSampleDown_32f(pSrcData, srcData.Length,
                                                  pDst, &dstLen,
                                                  channelsCount, &ch);

                    Out[ch].Write(_data[ch]);
                }

            In.Put(srcData);

            return true;
        }

        private readonly List<float[]> _data = new List<float[]>();

        public ISignalReader<float> In { get; set; }

        public IList<ISignalWriter<float>> Out { get; private set; }
    }
}
