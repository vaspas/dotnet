using System;
using IppModules.Analiz.NarrowBandSpectrum.AutoSpectrum;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Analiz.NarrowBandSpectrum
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class NarrowBandSpectrumModule:IExecuteModule
    {
        public unsafe bool? Execute()
        {
            int writeBlockSize;
            int blockSizePower2;
            int analizBlockSize;
            int readBlockSize;
            bool propertyChanged;

            lock (_sync)
            {
                writeBlockSize = WriteBlockSize;
                blockSizePower2 = BlockSizePower2;
                analizBlockSize = (int)Math.Pow(2, blockSizePower2);    
                readBlockSize = ReadBlockSize;
                propertyChanged = _propertyChanged;
                _propertyChanged = false;
            }

            if (_srcData.Length != readBlockSize)
                _srcData = new float[readBlockSize];
            if (_readArr.Length != analizBlockSize)
                _readArr = new float[analizBlockSize];
            if (_writeArr.Length != writeBlockSize)
                _writeArr = new float[writeBlockSize];

            if (propertyChanged)
            {
                _realAutoSpectrum = new RealAutoSpectrum();
                _realAutoSpectrum.PrepareAutoSpectrum(blockSizePower2, WinType, SpectrumUnit, Fqu);
            }

            if (!In.ReadTo(_srcData))
                return false;
            
            //сдвигаем данные в массивах
            //размер данных для смещения
            var shiftSize = analizBlockSize - readBlockSize;
            //сдвигаем если есть что сдвигать
            if (shiftSize != 0)
            {
                Buffer.BlockCopy(_readArr, readBlockSize*sizeof (float),
                                 _readArr, 0,
                                 shiftSize*sizeof (float));
            }

            //получаем вещественную часть
            Buffer.BlockCopy(_srcData, 0,
                             _readArr, shiftSize*sizeof (float),
                             readBlockSize*sizeof (float));
            //рассчитываем спектр
            fixed (float* pReadArr = _readArr, pWriteArr = _writeArr)
                _realAutoSpectrum.CalculateAutoSpectrum(pReadArr, pWriteArr);

            if(Out!=null)
                Out.Write(_writeArr);

            if (OutRe != null)
                OutRe.Write(_realAutoSpectrum.FftTransformRe);
            if (OutIm != null)
                OutIm.Write(_realAutoSpectrum.FftTransformIm);

            return true;
        }

        /// <summary>
        /// Входной сигнал.
        /// </summary>
        public ISignalReader<float> In { get; set; }

        /// <summary>
        /// Спектр сигнала.
        /// </summary>
        public ISignalWriter<float> Out { get; set; }

        /// <summary>
        /// Реальная часть комплексного спектра после преобразования.
        /// </summary>
        public ISignalWriter<float> OutRe { get; set; }

        /// <summary>
        /// Мнимая часть комплексного спектра после преобразования.
        /// </summary>
        public ISignalWriter<float> OutIm { get; set; }

        #region ///// private fields /////

        /// <summary>
        /// Минимальный размер блока, как степень двойки.
        /// </summary>
        public static readonly ushort MinBlockSizePower2 = 6;

        private RealAutoSpectrum _realAutoSpectrum;

        private float[] _srcData=new float[0];
        /// <summary>
        /// Массив принятых вещественных чисел сигнала.
        /// </summary>
        private float[] _readArr = new float[0];
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
            get { return (int)Math.Pow(2, (double)BlockSizePower2 - 1); }
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

        #endregion
    }
}
