using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.MouseListenerLayers.LinearScale
{
    public class ShiftDiapazoneMouseMoveListener<T> : IMouseMoveListener, IMouseButtonHandler, IMouseButtonListener
        where T : struct,IComparable<T>, IEquatable<T>
    {
        public MouseButton Button { get; set; }

        public IPointTranslator Translator { get; set; }
        
        public IScalePosition<T> Diapazone { get; set; }
        
        public Action OnChanged { get; set; }

        private void ChangePosition()
        {
            var shift = (T)(((dynamic)_startTo - _startFrom) * (_startPoint - _currentPosition));

            Diapazone.Set(
            (dynamic)_startFrom + shift, (dynamic)_startTo + shift);

            OnChanged();
        }

        private float _currentPosition;

        private float _startPoint;
        private T _startFrom;
        private T _startTo;
        
        public void OnMouseMove(Point<float> point, Rectangle<float> rect)
        {
            Translator.Src = rect;
            Translator.Dst = new Rectangle<float> {Left = 0f, Right = 1f, Bottom = 0f, Top = 1f};

            _currentPosition = Translator.Translate(point).X;
            
            if(_mousePressed)
                ChangePosition();
        }

        public void OnMouseLeave()
        {
        }

        public void OnMouseEnter()
        {
        }

        Action IMouseButtonHandler.HandleMouseDown { get; set; }
        Action IMouseButtonHandler.HandleMouseUp { get; set; }

        private bool _mousePressed;

        public void OnMouseDown(MouseButton button)
        {
            if (Button != button)
                return;

            _startPoint = _currentPosition;
            _startFrom = Diapazone.From;
            _startTo = Diapazone.To;

            _mousePressed = true;

            ChangePosition();

            (this as IMouseButtonHandler).HandleMouseDown();
        }

        public void OnMouseUp(MouseButton button)
        {
            if (Button != button)
                return;
            
            if(_mousePressed && _startPoint!=_currentPosition)
                (this as IMouseButtonHandler).HandleMouseUp();

            _mousePressed = false;
        }
    }
}

