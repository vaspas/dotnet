using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Analiz.FractionalOctaveAnalysis
{
    /// <summary>
    /// –абочий модуль дл€ рассчета долектавных спектров вещественного сигнала.
    /// </summary>
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public sealed class FractionalOctaveAnalysisModule:IExecuteModule
    {
        private DAnaliz _analiz = new DAnaliz();


        private bool _propertyChanged = true;

        private int _blockSize;
        /// <summary>
        /// –азмер блока обрабатываемого сигнала.
        /// </summary>
        public int BlockSize
        {
            get { return _blockSize; }
            set
            {
                if (value == _blockSize)
                    return;

                _blockSize = value;
                _propertyChanged = true;
            }
        }

        private int _frequency;
        public int Frequency
        {
            get { return _frequency; }
            set
            {
                if (value == _frequency)
                    return;

                _frequency = value;
                _propertyChanged = true;
            }
        }

        private int _filtersPerOctave;
        /// <summary>
        /// ¬озвращает и устанавливает кол-во фильтров на октаву.
        /// ¬озможные значени€: 1,3,12.
        /// </summary>
        public int FiltersPerOctave
        {
            get { return _filtersPerOctave; }
            set
            {
                if (value != 1 && value != 3 && value != 12)
                    throw new ArgumentException();

                if (value == _filtersPerOctave)
                    return;

                _filtersPerOctave = value;
                _propertyChanged = true;
            }
        }

        private int _grid;
        /// <summary>
        /// ¬озвращает и устанавливает значение типа сетки (2 или 10).
        /// </summary>
        public int Grid
        {
            get { return _grid; }
            set
            {
                if (value != 2 && value != 10)
                    throw new ArgumentException();

                if (value == _grid)
                    return;

                _grid = value;
                _propertyChanged = true;
            }
        }

        private int _octavesCount;
        /// <summary>
        /// ¬озвращает и устанавливает кол-во октав (от 3 до 20).
        /// </summary>
        public int OctavesCount
        {
            get { return _octavesCount; }
            set
            {
                if (value < 3 || value > 20)
                    throw new ArgumentException();

                if (value == _octavesCount)
                    return;

                _octavesCount = value;
                _propertyChanged = true;
            }
        }

        private float _ripple;
        /// <summary>
        /// ¬озвращает и устанавливает значение пульсации (от 0.01 до 0.3).
        /// </summary>
        public float Ripple
        {
            get { return _ripple; }
            set
            {
                if (value < 0.01f || value > 0.3)
                    throw new ArgumentException();

                if (value == _ripple)
                    return;

                _ripple = value;
                _propertyChanged = true;
            }
        }

        private int _nzv;
        /// <summary>
        /// ¬озвращает и устанавливает значение пор€док/2 (значени€ от 3 до 10).
        /// </summary>
        public int Nzv
        {
            get { return _nzv; }
            set
            {
                if (value < 3 || value > 10)
                    throw new ArgumentException();

                if (value == _nzv)
                    return;

                _nzv = value;
                _propertyChanged = true;
            }
        }

        private float[] _readBuffer=new float[0];

        public ISignalReader<float> In { get; set; }

        public ISignalWriter<float> Out { get; set; }


        public object _sync=new object();

        public void Init()
        {
            var actialBlockSize = BlockSize;
            var actialFilterPerOctave = FiltersPerOctave;

            var analiz = new DAnaliz();
            //подготавлтваем анализатор
            analiz.Prepare(actialBlockSize,
                            Frequency,
                            Math.Pow(Grid == 10 ? Math.Pow(10, 0.3) : 2, (double)1 / actialFilterPerOctave),
                            Ripple,
                            OctavesCount,
                            actialFilterPerOctave,
                            Nzv);

            lock(_sync)
            {
                if (_analiz != null)
                    _analiz.Dispose();

                if (_readBuffer.Length != actialBlockSize)
                    _readBuffer = new float[actialBlockSize];

                _analiz = analiz;
            }

            _propertyChanged = false;
        }

        public bool? Execute()
        {
            if (_propertyChanged)
                Init();

            lock (_sync)
            {
                if (!In.ReadTo(_readBuffer))
                    return false;

                var spectr = _analiz.Calculate(_readBuffer);

                Out.Write(spectr);
            }

            return true;
        }
    }
}
