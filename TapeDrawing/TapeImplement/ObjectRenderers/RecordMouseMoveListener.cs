using System;
using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.ObjectRenderers
{
    public class RecordMouseMoveListener<T> : IMouseMoveListener
    {
        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition
        {
            get { return _tapePosition; }
            set
            {
                if (_tapePosition == value)
                    return;

                if (_tapePosition != null)
                    _tapePosition.PositionChanged -= OnMouseLeave;

                _tapePosition = value;
                _tapePosition.PositionChanged += OnMouseLeave;
            }
        }

        private IScalePosition<int> _tapePosition;


        /// <summary>
        /// Источник данных для отображения
        /// </summary>
        public IObjectSource<T> Source;

        public Func<T, int> GetIndex;

        public int SelectedAreaPixels;

        /// <summary>
        /// Объект для преобразования точек
        /// </summary>
        public IPointTranslator Translator;

        public bool ClearOnMouseMoveOverside;

        private readonly List<T> _selected=new List<T>();
        public IList<T> Selected
        {
            get { return _selected; }
        }

        public Action<IList<T>> SelectedChanged;

        public void OnMouseMove(Point<float> point, Rectangle<float> rect)
        {
            Translator.Src = rect;
            Translator.Dst = new Rectangle<float>
                                 {Left = TapePosition.From, Right = TapePosition.To, Bottom = 0f, Top = 1f};

            var p = Translator.Translate(point);
            
            if(ClearOnMouseMoveOverside && (p.Y<0 || p.Y>1))
            {
                Clear();
                return;
            }

            _selected.Clear();

            var p1 = Translator.Translate(new Point<float>(point.X + SelectedAreaPixels, point.Y + SelectedAreaPixels));
            var p2 = Translator.Translate(new Point<float>(point.X - SelectedAreaPixels, point.Y - SelectedAreaPixels));

            var from = Math.Min(p1.X, p2.X);
            var to = Math.Max(p1.X, p2.X);

            foreach (var r in Source.GetData(TapePosition.From, TapePosition.To))
            {
                var index = GetIndex(r);

                if(from>index || to<index)
                    continue;

                _selected.Add(r);
            }
            if (SelectedChanged != null)
                SelectedChanged(Selected);
        }

        private void Clear()
        {
            if (_selected.Count == 0)
                return;

            _selected.Clear();

            if (SelectedChanged != null)
                SelectedChanged(Selected);
        }

        public void OnMouseLeave()
        {
            Clear();
        }

        public void OnMouseEnter()
        {
            
        }


    }
}
