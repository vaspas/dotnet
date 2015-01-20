using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.MouseListenerLayers.TapeCursor
{
    /// <summary>
    /// Отображение курсора мыши.
    /// </summary>
    public class TapeRefPositionCursorRenderer:IRenderer
    {
        public float Position;

        public Color LineColor;

        public int LineWidth { get; set; }

        public IPointTranslator Translator { get; set; }
        
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Dst = rect;
            Translator.Src = new Rectangle<float>{Left = 0,Right = 1,Bottom = 0,Top = 1};

            var shapes = TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(Translator).Result;

            using (var pen = gr.Instruments.CreatePen(LineColor, LineWidth, LineStyle.Solid))
            using (var shape = shapes.CreateLines(pen))
            {
                shape.Render(new[]
                                 {
                                     new Point<float>{X=Position,Y=0},
                                     new Point<float>{X=Position,Y=1},
                                 });
            }
        }
    }
}
