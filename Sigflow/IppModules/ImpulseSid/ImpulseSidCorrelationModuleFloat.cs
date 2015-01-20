
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.ImpulseSid
{
    /// <summary>
    /// Модуль для рассчета корреляционной функции импульсной СИД.
    /// </summary>
    class ImpulseSidCorrelationModuleFloat:IExecuteModule
    {
        public ISignalReader<float> In { get; set; }

        public ISignalWriter<float> Out { get; set; }

        /// <summary>
        /// Длина импульса в отсчетах сигнала.
        /// </summary>
        public int ImpulseLength
        {
            get { return _impulseLength; }
            set
            {
                if (_impulseLength == value)
                    return;
                _impulseLength = value;
                _propertyChanged = true;
            }
        }
        /// <summary>
        /// Относительная частота заполнения импульсов сигнала объекта.
        /// </summary>
        public float CarrierRelativeFrequency
        {
            get { return _carrierRelativeFrequency; }
            set
            {
                if (_carrierRelativeFrequency == value)
                    return;
                _carrierRelativeFrequency = value;
                _propertyChanged = true;
            }
        }
        /// <summary>
        /// Флаг учета импульса в виде колокольчика.
        /// </summary>
        public bool Cos2
        {
            get { return _cos2; }
            set
            {
                if (_cos2 == value)
                    return;
                _cos2 = value;
                _propertyChanged = true;
            }
        }
        /// <summary>
        /// Размер принимаемого блока.
        /// </summary>
        public int BlockSize
        {
            get { return _blockSize; }
            set
            {
                if (_blockSize == value)
                    return;
                _blockSize = value;

                _propertyChanged = true;
            }
        }

        private int _impulseLength;
        private float _carrierRelativeFrequency;
        private bool _cos2;
        private int _blockSize;

        private bool _propertyChanged = true;

        private readonly ConcurrentWorker _worker=new ConcurrentWorker();

        private float[] _buffer = new float[0];

        public unsafe bool? Execute()
        {
            if(_propertyChanged)
            {
                _worker.Close();
                _worker.CarrierRelativeFrequency = _carrierRelativeFrequency;
                _worker.Cos2 = _cos2;
                _worker.ImpulseLength = _impulseLength;
                _worker.BlockSize = _blockSize;
                _worker.Prepare();

                if(_buffer.Length!=_blockSize)
                    _buffer=new float[_blockSize];

                _propertyChanged = false;
            }

            if (!In.ReadTo(_buffer))
                return false;

            fixed(float* ptr=_buffer)
                _worker.DoWork(ptr);

            Out.Write(_buffer);

            return true;
        }
    }
}
