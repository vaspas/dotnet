using System;
using TapeDrawing.Core;

namespace TapeImplement.MouseListenerLayers.TapeCursor
{
    /// <summary>
    /// Обработчик мышки для работы с курсором. Реагирует на вращение колеса мыши.
    /// </summary>
    public class TapePositionCursorMouseWheelListener : IMouseWheelListener, IMouseWheelHandler
    {
        public TapePositionCursorRenderer Renderer { get; set; }
        
        public IScalePosition<int> TapePosition { get; set; }

        public float Coeff { get; set; }

        public Action PositionChanged { get; set; }

        Action IMouseWheelHandler.HandleMouseWheel { get; set; }

        public void OnMouseWheel(int delta)
        {
            Renderer.Position += (int)(delta*Coeff);

            PositionChanged();

            (this as IMouseWheelHandler).HandleMouseWheel();
        }

    }
}
