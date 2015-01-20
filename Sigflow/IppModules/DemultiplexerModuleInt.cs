using System.Collections.Generic;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class DemultiplexerModuleInt:IExecuteModule
    {
        public DemultiplexerModuleInt()
        {
            Out=new List<ISignalWriter<int>>();
        }
        
        public unsafe bool? Execute()
        {
            if (!In.NextBlockSize.HasValue)
                return false;

            var channelsCount = Out.Count;

            var blockSize = In.NextBlockSize.Value / channelsCount;

            while (_data.Count<channelsCount)
                _data.Add(new int[blockSize]);
            
            var srcData = In.Take();
            if (srcData==null)
                return false;

            fixed (int* pSrcData = srcData)
                for (var ch = 0; ch < channelsCount; ch++)
                {
                    if (_data[ch].Length != blockSize)
                        _data[ch] = new int[blockSize];

                    int dstLen;
                    fixed (int* pDst = _data[ch])
                        ipp.sp.ippsSampleDown_32f((float*)pSrcData, srcData.Length,
                                                  (float*)pDst, &dstLen,
                                                  channelsCount, &ch);

                    Out[ch].Write(_data[ch]);
                }

            In.Put(srcData);

            return true;
        }

        private readonly List<int[]> _data = new List<int[]>();

        public ISignalReader<int> In { get; set; }

        public IList<ISignalWriter<int>> Out { get; private set; }
    }
}
