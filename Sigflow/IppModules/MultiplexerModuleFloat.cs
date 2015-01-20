using System.Collections.Generic;
using System.Linq;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class MultiplexerModuleFloat:IExecuteModule
    {
        public MultiplexerModuleFloat()
        {
            In = new List<ISignalReader<float>>();
        }
        
        public unsafe bool? Execute()
        {
            if (In.Count==0 || In.Any(r=>!r.NextBlockSize.HasValue))
                return false;

            var blockSize = In[0].NextBlockSize.Value;

            if (!In.All(r => r.NextBlockSize.Value==blockSize && r.Available >= blockSize))
                return false;

            if (_data.Length != blockSize * In.Count)
            {
                _data = new float[blockSize * In.Count];
                _buffer = new float[blockSize * In.Count];
            }
            
            fixed (float* pData = _data)
                fixed (float* pBuffer = _buffer)
                {
                    ipp.sp.ippsZero_32f(pData, _data.Length);

                    for (var i = 0; i < In.Count; i++)
                    {
                        var srcData = In[i].Take();

                        var srcLength = 0;
                        var phase = i;
                        fixed (float* pSrc = srcData)
                            ipp.sp.ippsSampleUp_32f(pSrc, blockSize,
                                                    pBuffer, &srcLength,
                                                    In.Count, &phase);

                        ipp.sp.ippsAdd_32f_I(pBuffer, pData, _data.Length);

                        In[i].Put(srcData);
                    }
                }

            Out.Write(_data);

            return true;
        }

        private float[] _data = new float[0];
        private float[] _buffer = new float[0];

        public IList<ISignalReader<float>> In { get; private set; }

        public ISignalWriter<float> Out { get; set; }
    }
}
