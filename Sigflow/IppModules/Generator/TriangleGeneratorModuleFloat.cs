
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Generator
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class TriangleGeneratorModuleFloat : IExecuteModule
    {
        /// <summary>
        /// Эффективное значение.
        /// </summary>
        public float Value { get; set; }

        private float _phase;
        public float Phase
        {
            get { return _phase; }
            set { _phase = value; }
        }

        public float TriangleAsym { get; set; }

        /// <summary>
        /// Относительная частота тона. Не более половины частоты квантования.
        /// </summary>
        public float RelativeFrequency { get; set; }

        public int BlockSize { get; set; }


        public ISignalWriter<float> Out { get; set; }


        private float[] _data=new float[0];

        public unsafe bool? Execute()
        {
            var blockSize = BlockSize;
            if (_data.Length != blockSize)
                _data = new float[blockSize];

            fixed (float* pBlock = _data)
            fixed (float* pPhase = &_phase)
                    ipp.sp.ippsTriangle_Direct_32f(pBlock, _data.Length, Value, RelativeFrequency, TriangleAsym, pPhase);
                
            Out.Write(_data);

            return null;
        }
    }
}
