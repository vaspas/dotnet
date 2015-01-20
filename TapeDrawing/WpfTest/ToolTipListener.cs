using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;

namespace WpfTest
{
    class ToolTipListener : IMouseMoveListener
    {
        public RectangleArea ToolTipLayerArea { get; set; }

        public ToolTipRenderer Renderer { get; set; }

        public void OnMouseMove(Point<float> point, Rectangle<float> rect)
        {
            ToolTipLayerArea.X = point.X - rect.Left;
            ToolTipLayerArea.Y = point.Y - rect.Bottom;

            OnRedraw();
        }

        public void OnMouseLeave()
        {
            Renderer.Enabled = false;
            OnRedraw();
        }

        public void OnMouseEnter()
        {
            Renderer.Enabled = true;
            OnRedraw();
        }

        public Action OnRedraw { get; set; }
    }
}
