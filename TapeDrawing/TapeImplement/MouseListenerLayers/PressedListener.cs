using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.MouseListenerLayers
{
    /// <summary>
    /// Обработчик мышки и клавиатуры для работы с курсором. Реагирует на нажатую кнопку мыши.
    /// </summary>
    public class PressedListener : IMouseMoveListener, IMouseButtonListener, IMouseButtonHandler, IKeyProcess, IFocusProcess
    {
        public MouseButton Button;


        public bool Shift;

        public bool Control;

        /// <summary>
        /// Позиция ленты, может быть null.
        /// </summary>
        public IScalePosition<int> TapePosition;

        /// <summary>
        /// Границы отображения сигнала на ленте, может быть null.
        /// </summary>
        public IScalePosition<float> Diapazone;

        public IPointTranslator Translator;


        public Action<Point<float>, Point<float>> PositionChanged;

        public Func<Point<float>?, Point<float>, bool> Completed;


        public Predicate<Point<float>> CanStart;


        private void ChangePosition()
        {
            if (_startPosition == null)
                return;

            PositionChanged( _startPosition.Value, _currentPosition.Value);
        }

        private bool Complete()
        {
            var result = false;

            if (Completed != null && _currentPosition != null)
                result=Completed(_startPosition, _currentPosition.Value);

            _startPosition = null;

            return result;
        }


        private Point<float>? _currentPosition;

        private Point<float>? _startPosition;

        public void OnMouseMove(Point<float> point, Rectangle<float> rect)
        {
            Translator.Src = rect;
            Translator.Dst = new Rectangle<float>
                                 {
                                     Left = TapePosition != null ? TapePosition.From : 0,
                                     Right = TapePosition != null ? TapePosition.To : 1,
                                     Bottom = Diapazone != null ? Diapazone.From : 0,
                                     Top = Diapazone != null ? Diapazone.To : 1
                                 };

            _currentPosition = Translator.Translate(point);

            if (CheckMouseButton() && CheckKeys())
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
            //_mouseButtons++;

            if (Button != button)
                return;

            _mousePressed = true;

            if (CheckMouseButton() && CheckKeys() && (CanStart==null || CanStart(_currentPosition.Value)))
            {
                _startPosition = _currentPosition;
                ChangePosition();

                if (CanStart != null)
                    (this as IMouseButtonHandler).HandleMouseDown();
            }

        }

        public void OnMouseUp(MouseButton button)
        {
            if (Button != button)
                return;

            _mousePressed = false;

            if(!CheckMouseButton())
            {
                if(Complete() && (CanStart != null))
                    (this as IMouseButtonHandler).HandleMouseUp();
            }
        }

        private bool CheckMouseButton()
        {
            //if (!_isContains)
            //    return false;

            //if(Button == MouseButton.None && _mouseButtons>0)
            //    return false;

            if(Button != MouseButton.None && !_mousePressed)
                return false;

            return true;
        }

        private bool CheckKeys()
        {
            return _shift == Shift && _control == Control;
        }

        private bool _shift;
        private bool _control;

        public void OnKeyDown(KeyboardKey key)
        {
            var changed=false;

            if (key == KeyboardKey.Shift && !_shift)
            {
                _shift = true;
                changed |= Shift;
            }
            if(key == KeyboardKey.Control && !_control)
            {
                _control = true;
                changed |= Control;
            }

            if (changed && CheckMouseButton() && CheckKeys())
            {
                _startPosition = _currentPosition;
                ChangePosition();
            }
        }

        public void OnKeyUp(KeyboardKey key)
        {
            var changed = false;

            if ( key == KeyboardKey.Shift)
            {
                _shift = false;
                changed |=Shift;
            }
            if (key == KeyboardKey.Control)
            {
                _control = false;
                changed |= Control;
            }

            if (changed && !CheckKeys())
                Complete();
        }

        public void LostFocus()
        {
            var changed = false;

            if (_shift)
            {
                _shift = false;
                changed |= Shift;
            }

            if (_control)
            {
                _control = false;
                changed |= Control;
            }

            if (changed && !CheckKeys())
                Complete();
        }
    }
}
