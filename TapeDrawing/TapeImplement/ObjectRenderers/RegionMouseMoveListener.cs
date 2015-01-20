using System;
using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.ObjectRenderers
{
    public class RegionMouseMoveListener<T> : IMouseMoveListener
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

        public Func<T, int> GetFrom;
        public Func<T, int> GetTo;

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

            foreach (var r in Source.GetData(TapePosition.From, TapePosition.To))
            {
                if(GetFrom(r)>p.X || GetTo(r)<p.X)
                    continue;

                _selected.Add(r);
            }

            if(SelectedChanged!=null)
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
