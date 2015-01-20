using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <summary>
    /// Операция сложения с константой.
    /// </summary>
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class AddConstantModuleFloat : IExecuteModule
    {
        public ISignalReader<float> In { get; set; }

        public ISignalWriter<float> Out { get; set; }


        /// <summary>
        /// Значение для сложения. Потокобезопасная операция, можно устанавливать в процессе работы схемы.
        /// </summary>
        public float Constant { get; set; }

        private float[] _data=new float[0];

        public unsafe bool? Execute()
        {
            if (!In.NextBlockSize.HasValue)
                return false;

            var blockSize = In.NextBlockSize.Value;
            
            if (_data.Length != blockSize)
                _data = new float[blockSize];
            
            var block = In.Take();

            fixed (float* pData = _data, srcData = block)
                ipp.sp.ippsAddC_32f(srcData, Constant, pData, blockSize);

            Out.Write(_data);

            In.Put(block);

            return true;
        }
    }
}
