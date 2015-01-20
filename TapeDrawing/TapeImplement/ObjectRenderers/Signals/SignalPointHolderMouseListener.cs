using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.ObjectRenderers.Signals
{
    /// <summary>
    /// Обработчик мышки для определения находиться ли курсор в зоне точки графика.
    /// </summary>
    public class SignalPointHolderMouseListener : IMouseMoveListener
    {
        /// <summary>
        /// Источник данных сигнала
        /// </summary>
        public ISignalPointSource Source;

        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        /// <summary>
        /// Границы отображения сигнала на ленте.
        /// </summary>
        public IScalePosition<float> Diapazone;

        /// <summary>
        /// Транслятор точек
        /// </summary>
        public IPointTranslator Translator;

        /// <summary>
        /// Размер области срабатывания в пикселях.
        /// </summary>
        public float ZonePixels;
        
        private Point<float>? _selectedPoint;
        public Point<float>? SelectedPoint 
        {
            get { return _selectedPoint; }
            private set
            {
                _selectedPoint = value;

                if (OnSelectedPointChanged != null)
                    OnSelectedPointChanged();
            }
        }

        public event Action OnSelectedPointChanged;

        public void OnMouseMove(Point<float> point, Rectangle<float> rect)
        {
            if(!_isContains)
            {
                SelectedPoint = null;
                return;
            }

            if (TapePosition.From >= TapePosition.To)
                return;

            Translator.Src = new Rectangle<float>
            {
                Left = TapePosition.From,
                Right = TapePosition.To,
                Bottom = Diapazone.From,
                Top = Diapazone.To
            };
            Translator.Dst = rect;
            
            Point<float>? nearestPoint=null;
            var nearestDistance = float.MaxValue;

            // Составим отрисовываемые точки
            var signalPoint = Source.GetStartPoint(TapePosition.From, TapePosition.To);
            while (signalPoint != null && signalPoint.Value.X < TapePosition.To)
            {
                var translated=Translator.Translate(signalPoint.Value);

                var dist = (float)(Math.Pow(translated.X - point.X, 2) + Math.Pow(translated.Y - point.Y, 2));

                if(nearestDistance>dist)
                {
                    nearestDistance = dist;
                    nearestPoint = signalPoint;
                }

                signalPoint = Source.GetNextPoint();
            }

            if (nearestPoint != null && nearestDistance <= Math.Pow(ZonePixels, 2))
                SelectedPoint = nearestPoint;
            else
                SelectedPoint = null;
        }

        private bool _isContains;
        public void OnMouseLeave()
        {
            _isContains = false;
        }

        public void OnMouseEnter()
        {
            _isContains = true;
        }
    }
}
