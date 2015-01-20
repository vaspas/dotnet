using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.ObjectRenderers.LinearScale
{
    public class ScaleLinesRenderer : ScaleBase, IRenderer
    {
        /// <summary>
        /// Ширина линии
        /// </summary>
        public int LineWidth;

        /// <summary>
        /// Цвет линии
        /// </summary>
        public Color LineColor;

        /// <summary>
        /// Стиль линии
        /// </summary>
        public LineStyle LineStyle;



        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Src = new Rectangle<float> { Left = Diapazone.From, Right = Diapazone.To, Bottom = 0, Top = 1 };
            Translator.Dst = rect;


            var shapes=TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator
                .For(gr.Shapes).Translate(Translator).Result;

            using (var pen = gr.Instruments.CreatePen(LineColor, LineWidth, LineStyle))
            using (var lineShape = shapes.CreateLines(pen))
            {
                var points = new List<Point<float>>();
                float side=-0.5f;
                foreach (var code in GetCodes())
                {
                    points.Add(new Point<float> {X = code, Y = side});
                    if (side < 0)
                        side = 1.5f;
                    else
                        side = -0.5f;
                    points.Add(new Point<float> { X = code, Y = side });
                }

                lineShape.Render(points);
            }
        }
    }
}
