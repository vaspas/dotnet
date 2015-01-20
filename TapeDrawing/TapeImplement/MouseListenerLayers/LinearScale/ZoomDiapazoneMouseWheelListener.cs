using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.MouseListenerLayers.LinearScale
{
    public class ZoomDiapazoneMouseWheelListener<T> : IMouseWheelListener, IMouseWheelHandler, IKeyProcess, IFocusProcess
        where T : struct,IComparable<T>, IEquatable<T>
    {
        public bool Shift;

        public bool Control;
        
        
        public IScalePosition<T> Diapazone { get; set; }

        public float Factor { get; set; }

        public Action OnChanged { get; set; }

        Action IMouseWheelHandler.HandleMouseWheel { get; set; }

        public void OnMouseWheel(int val)
        {
            if (_shift != Shift || _control != Control)
                return;

            var change = (T)(((dynamic)Diapazone.To - Diapazone.From) * Factor * val);

            Diapazone.Set((dynamic)Diapazone.From + change, (dynamic)Diapazone.To - change);

            OnChanged();

            (this as IMouseWheelHandler).HandleMouseWheel();
        }
        
        private bool _shift;
        private bool _control;

        public void OnKeyDown(KeyboardKey key)
        {
            if (key == KeyboardKey.Shift && !_shift)
            {
                _shift = true;
            }
            if (key == KeyboardKey.Control && !_control)
            {
                _control = true;
            }
        }

        public void OnKeyUp(KeyboardKey key)
        {
            if (key == KeyboardKey.Shift)
            {
                _shift = false;
            }
            if (key == KeyboardKey.Control)
            {
                _control = false;
            }
        }

        public void LostFocus()
        {
            _shift = false;
            _control = false;
        }
    }
}

