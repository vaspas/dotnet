using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.MouseListenerLayers.TapeArea
{
    /// <summary>
    /// Отображение курсора мыши.
    /// </summary>
    public class TapeRefAreaRenderer:IRenderer
    {
        public float PositionFrom;
        public float PositionTo;

        public Color Color;

        public IPointTranslator Translator;
        
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Dst = rect;
            Translator.Src = new Rectangle<float>{Left = 0, Right = 1, Bottom = 0, Top = 1};

            var shapes = TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(Translator).Result;

            using (var pen = gr.Instruments.CreateSolidBrush(Color))
            using (var shape = shapes.CreateFillRectangle(pen))
            {
                shape.Render(new Rectangle<float>{Left = PositionFrom, Right = PositionTo, Bottom = 0, Top = 1});
            }
        }
    }
}
