using System;
using IppModules.Analiz.NarrowBandSpectrum.AutoSpectrum;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Analiz.NarrowBandSpectrum
{
    /// <remarks>
    /// 
    /// </remarks>
    public class NarrowBandComplexSpectrumModule:IExecuteModule
    {
        public unsafe bool? Execute()
        {
            int writeBlockSize;
            int blockSizePower2;
            int analizBlockSize;
            int readBlockSize;
            bool exchangeHalfs;
            bool propertyChanged;

            lock (_sync)
            {
                writeBlockSize = WriteBlockSize;
                blockSizePower2 = BlockSizePower2;
                analizBlockSize = (int)Math.Pow(2, blockSizePower2);    
                readBlockSize = ReadBlockSize;
                propertyChanged = _propertyChanged;
                exchangeHalfs = _exchangeHalfs;
                _propertyChanged = false;
            }

            if (_srcDataRe.Length != readBlockSize)
                _srcDataRe = new float[readBlockSize];
            if (_srcDataIm.Length != readBlockSize)
                _srcDataIm = new float[readBlockSize];
            if (_readArrRe.Length != analizBlockSize)
                _readArrRe = new float[analizBlockSize];
            if (_readArrIm.Length != analizBlockSize)
                _readArrIm = new float[analizBlockSize];
            if (_writeArr.Length != writeBlockSize)
                _writeArr = new float[writeBlockSize];

            if (propertyChanged)
            {
                _realAutoSpectrum = new ComplexAutoSpectrum
                                        {
                                            ExchangeHalfs = exchangeHalfs
                                        };
                _realAutoSpectrum.PrepareAutoSpectrum(blockSizePower2, WinType, SpectrumUnit, Fqu);
            }

            if (!InRe.ReadTo(_srcDataRe))
                return false;
            if (!InIm.ReadTo(_srcDataIm))
                return false;
            
            //сдвигаем данные в массивах
            //размер данных для смещения
            var shiftSize = analizBlockSize - readBlockSize;
            //сдвигаем если есть что сдвигать
            if (shiftSize != 0)
            {
                Buffer.BlockCopy(_readArrRe, readBlockSize*sizeof (float),
                                 _readArrRe, 0,
                                 shiftSize*sizeof (float));
                Buffer.BlockCopy(_readArrIm, readBlockSize * sizeof(float),
                                 _readArrIm, 0,
                                 shiftSize * sizeof(float));
            }

            //получаем вещественную часть
            Buffer.BlockCopy(_srcDataRe, 0,
                             _readArrRe, shiftSize*sizeof (float),
                             readBlockSize*sizeof (float));
            //получаем мнимую часть
            Buffer.BlockCopy(_srcDataIm, 0,
                             _readArrIm, shiftSize * sizeof(float),
                             readBlockSize * sizeof(float));
            //рассчитываем спектр
            fixed (float* pReadArrRe = _readArrRe, pReadArrIm = _readArrIm, pWriteArr = _writeArr)
                _realAutoSpectrum.CalculateAutoSpectrum(pReadArrRe, pReadArrIm, pWriteArr);

            Out.Write(_writeArr);

            return true;
        }

        public ISignalReader<float> InRe { get; set; }
        public ISignalReader<float> InIm { get; set; }

        public ISignalWriter<float> Out { get; set; }

        #region ///// private fields /////

        /// <summary>
        /// Минимальный размер блока, как степень двойки.
        /// </summary>
        public static readonly ushort MinBlockSizePower2 = 6;

        private ComplexAutoSpectrum _realAutoSpectrum;

        private float[] _srcDataRe=new float[0];
        private float[] _srcDataIm = new float[0];
        /// <summary>
        /// Массив вещественной части принятых  чисел сигнала.
        /// </summary>
        private float[] _readArrRe = new float[0];
        /// <summary>
        /// Массив мнимой части принятых  чисел сигнала.
        /// </summary>
        private float[] _readArrIm = new float[0];
        /// <summary>
        /// Массив спектров для записи в узел.
        /// </summary>
        private float[] _writeArr = new float[0];

        #endregion
        

        #region ///// IUAnalizLiteMod /////
        
        /// <summary>
        /// Рассчитывает и возвращает установленный размер блока для выдачи результата.
        /// </summary>
        public int WriteBlockSize
        {
            get { return (int)Math.Pow(2, (double)BlockSizePower2 ); }
        }

        /// <summary>
        /// Рассчитывает и возвращает установленный размер блока для анализа.
        /// </summary>
        public int BlockSize
        {
            get { return (int)Math.Pow(2, BlockSizePower2); }
        }

        /// <summary>
        /// Рассчитывает и возвращает установленный размер блока чтения.
        /// </summary>
        public int ReadBlockSize
        {
            get
            {
                return (int)Math.Pow(2, ReadBlockSizePower2);
            }
        }

        private readonly object _sync=new object();

        public void SetupBlockSizeTreadSafe(ushort blockSizePower2, ushort readBlockSizePower2)
        {
            lock (_sync)
            {
                BlockSizePower2 = blockSizePower2;
                ReadBlockSizePower2 = readBlockSizePower2;
            }
        }

        private volatile bool _propertyChanged=true;

        private ushort _blockSizePower2;
        /// <summary>
        /// Возвращает и устанавливает размер блока для анализа, как степень двойки.
        /// </summary>
        public ushort BlockSizePower2
        {
            get { return _blockSizePower2; }
            set 
            {
                if(value<MinBlockSizePower2)
                    throw new ArgumentOutOfRangeException();

                if (_blockSizePower2 == value)
                    return;
                
                _blockSizePower2 = value;
                _propertyChanged = true;
            }
        }

        private ushort _readBlockSizePower2;
        /// <summary>
        /// Возвращает и устанавливает размер блока чтения как степень двойки.        
        /// </summary>
        /// <remarks>Используется для перекрытия блоков сигнала.</remarks>
        public ushort ReadBlockSizePower2
        {
            get { return _readBlockSizePower2; }
            set
            {
                if (value < MinBlockSizePower2)
                    throw new ArgumentOutOfRangeException();

                if (_readBlockSizePower2 == value)
                    return;

                _readBlockSizePower2 = value;
                _propertyChanged = true;
            }
        }

        private WindowType _winType;
        /// <summary>
        /// Возвращает и устанавливает тип окна.
        /// </summary>
        public WindowType WinType
        {
            get { return _winType; }
            set
            {
                if (_winType == value)
                    return;

                _winType = value;

                _propertyChanged = true;
            }
        }

        private SpectrumUnit _spectrumUnit;
        /// <summary>
        /// Возвращает и устанавливает тип спектра.
        /// После установки требуется перезапуск.
        /// </summary>
        public SpectrumUnit SpectrumUnit
        {
            get { return _spectrumUnit; }
            set
            {
                if (_spectrumUnit == value)
                    return;

                _spectrumUnit = value;

                _propertyChanged = true;
            }
        }

        private float _fqu;
        public float Fqu
        {
            get { return _fqu; }
            set
            {
                if (_fqu == value)
                    return;

                _fqu= value;

                _propertyChanged = true;
            }
        }

        private bool _exchangeHalfs = true;
        /// <summary>
        /// Возвращает и устанавливает флаг замены левой/правой части спектра.       
        /// </summary>
        public bool ExchangeHalfs
        {
            get { return _exchangeHalfs; }
            set
            {
                if (_exchangeHalfs == value)
                    return;

                _exchangeHalfs = value;
                _propertyChanged = true;
            }
        }

        #endregion
    }
}
