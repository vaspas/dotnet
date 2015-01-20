using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace WinFormsTest
{
    class TranslatorTestRenderer : IRenderer
    {
        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Dst = rect;
            Translator.Src = new Rectangle<float>
                                 {
                                     Left = 0,
                                     Right = 1000,
                                     Bottom = -100,
                                     Top = 100
                                 };
            var p1 = new Point<float>
                         {
                             X=50,
                             Y=0
                         };
            var p2 = new Point<float>
            {
                X = 50,
                Y = 50
            };

            var a1 = new Point<float>
            {
                X = 0,
                Y = 0
            };
            var a2 = new Point<float>
            {
                X = 1000,
                Y = 0
            };

            var shapesFactory = ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(Translator).Result;

            using (var pen = gr.Instruments.CreatePen(new Color { A = 255, R = 255 }, 2, LineStyle.Dash))
            using (var lineShape = shapesFactory.CreateLines(pen))
            {
                lineShape.Render(new List<Point<float>> { p1, p2 });
                lineShape.Render(new List<Point<float>> { a1, a2 });
            }

            using (var font = gr.Instruments.CreateFont("Arial", 8, new Color { A = 255, R = 255 }, FontStyle.None))
            using (var textShape = shapesFactory.CreateText(font, TextAlignment, TextAngle))
            {
                textShape.Render("200", new Point<float> {X = 200, Y = 0});
                textShape.Render("400", new Point<float> { X = 400, Y = 0 });
                textShape.Render("600", new Point<float> { X = 600, Y = 0 });
                textShape.Render("800", new Point<float> { X = 800, Y = 0 });
            }
        }

        public IPointTranslator Translator { get; set; }

        public Alignment TextAlignment { get; set; }

        public float TextAngle { get; set; }
    }
}
