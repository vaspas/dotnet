using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules.Generator
{
    public class ZeroDataModuleFloat : IExecuteModule
    {

        public int BlockSize { get; set; }


        public ISignalWriter<float> Out { get; set; }


        private float[] _data = new float[0];

        public unsafe bool? Execute()
        {
            var blockSize = BlockSize;
            if (_data.Length != blockSize)
                _data = new float[blockSize];

            Out.Write(_data);

            return null;
        }
    }
}
