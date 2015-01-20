using System;
using TapeDrawing.Core;

namespace TapeImplement.MouseListenerLayers.TapeCursor
{
    /// <summary>
    /// Обработчик мышки для работы с курсором. Реагирует на вращение колеса мыши.
    /// </summary>
    public class TapeRefPositionCursorMouseWheelListener : IMouseWheelListener, IMouseWheelHandler
    {
        public TapeRefPositionCursorRenderer Renderer { get; set; }
        
        public IScalePosition<int> TapePosition { get; set; }

        public float Coeff { get; set; }

        public Action PositionChanged { get; set; }

        Action IMouseWheelHandler.HandleMouseWheel { get; set; }

        public void OnMouseWheel(int delta)
        {
            var newPosition = TapePosition.From + Renderer.Position * (TapePosition.To - TapePosition.From) + delta * Coeff;

            var newRelPosition = (newPosition - TapePosition.From)/(TapePosition.To - TapePosition.From);

            if(newRelPosition>1)
                newRelPosition = newRelPosition%1;
            else if(newRelPosition<0)
                newRelPosition = (int) Math.Abs(newRelPosition - 1) - newRelPosition;

            Renderer.Position = newRelPosition;

            PositionChanged();

            (this as IMouseWheelHandler).HandleMouseWheel();
        }

    }
}
