
using System;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core
{
    public interface IMouseListener
    {
    }

    public interface IMouseMoveListener : IMouseListener
    {
        void OnMouseMove(Point<float> point, Rectangle<float> rect);
        void OnMouseLeave();
        void OnMouseEnter();
    }

    public interface IMouseButtonListener : IMouseListener
    {
        void OnMouseDown(MouseButton button);
        void OnMouseUp(MouseButton button);
    }

    public interface IMouseButtonHandler : IMouseListener
    {
        Action HandleMouseDown { get; set; }
        Action HandleMouseUp { get; set; }
    }

    public interface IMouseWheelListener : IMouseListener
    {
        void OnMouseWheel(int delta);
    }

    public interface IMouseWheelHandler : IMouseListener
    {
        Action HandleMouseWheel { get; set; }
    }
}
