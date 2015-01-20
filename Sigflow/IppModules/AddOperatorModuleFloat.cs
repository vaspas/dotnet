using System.Collections.Generic;
using System.Linq;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class AddOperatorModuleFloat:IExecuteModule
    {
        public AddOperatorModuleFloat()
        {
            In=new List<ISignalReader<float>>();
        }

        public IList<ISignalReader<float>> In { get; private set; }

        public ISignalWriter<float> Out { get; set; }


        private float[] _data=new float[0];

        public unsafe bool? Execute()
        {
            if (In.Count == 0 || In.Any(r=>!r.NextBlockSize.HasValue))
                return false;

            var blockSize = In[0].NextBlockSize.Value;

            if (!In.All(r => r.NextBlockSize.Value == blockSize && r.Available >= blockSize))
                return false;

            if (_data.Length != blockSize)
                _data = new float[blockSize];
            
            fixed (float* pData = _data)
            {
                ipp.sp.ippsSet_32f(0, pData, _data.Length);

                foreach (var signalReader in In)
                {
                    var block = signalReader.Take();

                    fixed (float* pBlock = block)
                        ipp.sp.ippsAdd_32f_I(pBlock, pData, blockSize);

                    signalReader.Put(block);
                }

                Out.Write(_data);
            }

            return true;
        }
    }
}
