using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest.Renderers
{
    class BackgroundRenderer : IRenderer
    {
        public Color Color { get; set; }

        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
           using(var shape=gr.Shapes.CreateFillAll(Color))
            shape.Render();
        }
    }
}
