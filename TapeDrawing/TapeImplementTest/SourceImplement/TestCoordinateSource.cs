using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Primitives;
using TapeImplement.CoordGridRenderers;
using TapeImplement.ObjectRenderers;
using TapeImplement.ObjectRenderers.Signals;

namespace TapeImplementTest.SourceImplement
{
    /// <summary>
    /// Тестовый участок. Пока это прямолинейные данные с отметками, которые не искажают координат
    /// </summary>
    public class TestCoordinateSource : ICoordinateSource, ISignalSource, ISignalPointSource
    {
        public TestCoordinateSource()
        {
            InitRequests();
        }

        #region Implementation of ICoordinateSource
        /// <summary>
        /// Минимальное значение
        /// </summary>
        public float Min { get; set; }
        /// <summary>
        /// Максимальное значение
        /// </summary>
        public float Max { get; set; }
        /// <summary>
        /// Шаг индекса
        /// </summary>
        public float CoordinateStep { get; set; }
        /// <summary>
        /// Отметки прерываний
        /// </summary>
        public ICoordInterrupt[] Interrupts { get; set; }

        /// <summary>
        /// Позволяет получить все отметки для указанного диапазона ленты
        /// </summary>
        /// <param name="from">Начальный индекс диапазона</param>
        /// <param name="to">Конечный индекс диапазона</param>
        /// <returns>Массив отметок (прерываний ленты)
        /// !ВНИМАНИЕ! В начало и конец ленты должны быть добавлены прерывания. Тоесть на ленте обязательно
        /// должны быть 2 прерывания</returns>
        public ICoordInterrupt[] GetCoordInterrupts(int from, int to)
        {
            return Interrupts.Where(interrupt => (interrupt.Index >= from) && (interrupt.Index <= to)).ToArray();
        }

        /// <summary>
        /// Возвращает значение координаты в единицах длины для указанного индекса
        /// </summary>
        /// <param name="index">Индекс сигнала</param>
        /// <returns>Координата на пути. Если индекс вне диапазона, то должен всегда возвращаться 0
        /// !ВНИМАНИЕ! 0 не может использоваться как код ошибки, потомучто это валидное значение
        /// Использовать так: если на участке координата начала и конца совпала, то не обрабатывать</returns>
        public float GetUnitValue(int index)
        {
            // В качестве значения отсутствия индекса использую 0
			if (index < 0) return 0;

            var coord = CoordinateStep * index + Min;

			if (coord > Math.Max(Min, Max)) return 0;
			if (coord < Math.Min(Min, Max)) return 0;

            return coord;
        }
        #endregion

        #region Implementation of ISignalSource

        private Random _rnd;
        private int _step = 1;
        private int _currentSignalIndex;

        public float GetStartIndex(int fromIndex)
        {
            _rnd = new Random(0);
            _currentSignalIndex = fromIndex;

            _step = 1;
			if (fromIndex < 0) return 0;

			if ((CoordinateStep * _currentSignalIndex + Min) > Math.Max(Min, Max)) throw new ArgumentException();

            for (var i = 0; i < fromIndex; i++)
                _rnd.Next(-100, 100);

            return fromIndex;
        }

        public float Step
        {
            get { return _step; }
        }

        public float GetNextValue()
        {	
            _currentSignalIndex++;

            return _rnd.Next(-100, 100);
        }

        public bool IsDataAvailable
        {
            get
            {
                if (CoordinateStep > 0 && (CoordinateStep * _currentSignalIndex + Min) > Math.Max(Min, Max)) 
                    return false;
                if (CoordinateStep < 0 && (CoordinateStep * _currentSignalIndex + Min) < Math.Min(Min, Max))
                    return false;
                return true;
            }
        }
        
        #endregion

        #region Implementation of ISignalPointSource

        private int _currentSignalPointIndex;
        public Point<float>? GetStartPoint(int fromIndex, int toIndex)
        {
            _currentSignalPointIndex = fromIndex;
            if (_currentSignalPointIndex < 0) _currentSignalPointIndex = 0;

			if ((CoordinateStep * _currentSignalPointIndex + Min) > Math.Max(Min, Max)) throw new ArgumentException();
			if ((CoordinateStep * _currentSignalPointIndex + Min) < Math.Min(Min, Max)) throw new ArgumentException();

            return
                new Point<float> { X = _currentSignalPointIndex, Y = (float)Math.Round(100.0f * Math.Sin(_currentSignalPointIndex / 5.0f)) };
        }


        public Point<float>? GetNextPoint()
        {
            _currentSignalPointIndex++;
			if ((CoordinateStep * _currentSignalPointIndex + Min) > Math.Max(Min, Max)) throw new ArgumentException();
			if ((CoordinateStep * _currentSignalPointIndex + Min) < Math.Min(Min, Max)) throw new ArgumentException();
            return new Point<float> { X = _currentSignalPointIndex, Y = (float)Math.Round(100.0f * Math.Sin(_currentSignalPointIndex / 5.0f)) };
        }

        #endregion

        #region Implementation of IObjectSource

        private readonly List<IObjectRequest> _requests = new List<IObjectRequest>();

        private void InitRequests()
        {
            _requests.Add(new PointObjectRequest { Source = this });
            _requests.Add(new RegionObjectRequest { Source = this });
        }

        class ObjectSource<T>:IObjectSource<T> where T:class
        {
            public TestCoordinateSource Src;

            public IEnumerable<T> GetData(int from, int to)
            {
                var request = Src._requests.FirstOrDefault(r => r is IObjectRequest<T>);
                return request == null ? null : ((IObjectRequest<T>)request).Get(from, to);
            }

            public IEnumerable<T> GetData()
            {
                throw new NotImplementedException();
            }
        }

        public IObjectSource<T> As<T>() where T:class
        {
            return new ObjectSource<T>{Src=this};
        }

        #endregion

        #region Implementation of IHighDensitySignalSource

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        public float Density { set; private get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
        public float GetValue(int index)
        {
			if ((CoordinateStep * index + Min) > Math.Max(Min, Max)) throw new ArgumentException();
			if ((CoordinateStep * index + Min) < Math.Min(Min, Max)) throw new ArgumentException();

            return (float) Math.Round(100.0f*Math.Sin(Math.PI/2.0f + index/5.0f));
        }

        #endregion
    }
}
