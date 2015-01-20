using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace WpfTest
{
    class MouseHoldListener : IMouseMoveListener, IMouseButtonListener, IRenderer, IMouseWheelListener
    {
        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (_isPressed)
            {
                var fillbrush = gr.Instruments.CreateSolidBrush(new Color(_colorValue, _colorValue, _colorValue));
                var fillshape = gr.Shapes.CreateFillRectangle(fillbrush);

                fillshape.Render(rect);

                if (fillbrush is IDisposable)
                    (fillbrush as IDisposable).Dispose();
                if (fillshape is IDisposable)
                    (fillshape as IDisposable).Dispose();
            }

            if (!IsHold)
                return;

            var pen = gr.Instruments.CreatePen(new Color(0, 0, 0), 3, LineStyle.Solid);
            var shape = gr.Shapes.CreateDrawRectangle(pen);

            shape.Render(rect);

            if (pen is IDisposable)
                (pen as IDisposable).Dispose();
            if (shape is IDisposable)
                (shape as IDisposable).Dispose();

            var font = gr.Instruments.CreateFont("Arial", 12, new Color(0, 0, 0), FontStyle.None);
            var fshape = gr.Shapes.CreateText(font, Alignment.None, 0);

            fshape.Render(string.Format("{0},{1}", _pos.X, _pos.Y), _pos);

            if (font is IDisposable)
                (font as IDisposable).Dispose();
            if (fshape is IDisposable)
                (fshape as IDisposable).Dispose();


        }

        private byte _colorValue = 200;

        public bool IsHold
        { get; set; }

        private Point<float> _pos;

        public void OnMouseMove(Point<float> point, Rectangle<float> rect)
        {
            _pos.X = point.X;
            _pos.Y = point.Y;
            OnRedraw();
        }

        public void OnMouseLeave()
        {
            IsHold = false;
            OnRedraw();
        }

        public void OnMouseEnter()
        {
            IsHold = true;
            OnRedraw();
        }

        private bool _isPressed;

        public void OnMouseDown(MouseButton button)
        {
            if (!IsHold)
                return;

            _isPressed = true;
            OnRedraw();
        }

        public void OnMouseUp(MouseButton button)
        {
            _isPressed = false;
            OnRedraw();
        }

        public void OnMouseWheel(int delta)
        {
            var v = _colorValue + delta * 10;
            if (v < byte.MinValue)
                v += byte.MaxValue;

            _colorValue = (byte)(v % byte.MaxValue);
            if (_isPressed)
                OnRedraw();
        }

        public Action OnRedraw { get; set; }
    }
}
