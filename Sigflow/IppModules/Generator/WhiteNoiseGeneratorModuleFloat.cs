
using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Generator
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class WhiteNoiseGeneratorModuleFloat:IExecuteModule
    {
        /// <summary>
        /// Вспомогательная переменная для генерации шума.
        /// </summary>
        private uint _seedNoise = (uint)(new Random()).Next();

        /// <summary>
        /// Эффективное значение.
        /// </summary>
        public float Value { get; set; }

        public int BlockSize { get; set; }


        public ISignalWriter<float> Out { get; set; }


        private float[] _data=new float[0];

        public unsafe bool? Execute()
        {
            var blockSize = BlockSize;
            if(_data.Length!=blockSize)
                _data = new float[blockSize];

            fixed (float* pBlock = _data)
                fixed (uint* pSeed = &_seedNoise)
                    ipp.sp.ippsRandGauss_Direct_32f(pBlock, _data.Length, 0, Value, pSeed);
            
            Out.Write(_data);

            return null;
        }
    }
}
