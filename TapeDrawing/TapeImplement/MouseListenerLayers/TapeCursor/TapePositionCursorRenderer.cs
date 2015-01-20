using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.MouseListenerLayers.TapeCursor
{
    /// <summary>
    /// Отображение курсора мыши.
    /// </summary>
    public class TapePositionCursorRenderer:IRenderer
    {
        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        public int Position;

        public Color LineColor;

        public int LineWidth;

        public IPointTranslator Translator;
        
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Dst = rect;
            Translator.Src = new Rectangle<float>{Left = TapePosition.From, Right = TapePosition.To, Bottom = 0f, Top = 1f};

            var shapes = TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(Translator).Result;

            using (var pen = gr.Instruments.CreatePen(LineColor, LineWidth, LineStyle.Solid))
            using (var shape = shapes.CreateLines(pen))
            {
                shape.Render(new[]
                                 {
                                     new Point<float>{X=Position,Y=0}, 
                                     new Point<float>{X=Position,Y=1}
                                 });
            }
        }
    }
}
