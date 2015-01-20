using System.Collections.Generic;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules
{
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
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

                    fixed (float* pDst = _data[ch])
                        for (var i = 0; i < blockSize; i++)
                            *(pDst + i) = *(pSrcData + i * channelsCount + ch);
                    
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
