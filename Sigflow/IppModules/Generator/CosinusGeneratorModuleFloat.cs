
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Generator
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class CosinusGeneratorModuleFloat:IExecuteModule
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

        /// <summary>
        /// Относительная частота тона. Не более половины частоты квантования.
        /// </summary>
        public float RelativeFrequency { get; set; }

        public int BlockSize { get; set; }


        public ISignalWriter<float> Out { get; set; }


        private float[] _data=new float[0];

        public unsafe bool? Execute()
        {
            var blockSize=BlockSize;
            if(_data.Length!=blockSize)
                _data=new float[blockSize];

            fixed (float* pBlock = _data)
                fixed (float* pPhase = &_phase)
                    ipp.sp.ippsTone_Direct_32f(pBlock, _data.Length, Value, RelativeFrequency, pPhase, ipp.IppHintAlgorithm.ippAlgHintNone);

            Out.Write(_data);

            return null;
        }
    }
}
