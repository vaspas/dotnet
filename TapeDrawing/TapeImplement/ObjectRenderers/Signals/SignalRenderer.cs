using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace TapeImplement.ObjectRenderers.Signals
{
    /// <summary>
    /// Рендерер для рисования графика
    /// </summary>
    public class SignalRenderer : IRenderer
    {
        /// <summary>
        /// Источник данных сигнала
        /// </summary>
        public ISignalSource Source;

        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        /// <summary>
        /// Границы отображения сигнала на ленте.
        /// </summary>
        public IScalePosition<float> Diapazone;

        /// <summary>
        /// Ширина линии
        /// </summary>
        public float LineWidth;

        /// <summary>
        /// Цвет линии
        /// </summary>
        public Color LineColor;

        /// <summary>
        /// Стиль линии
        /// </summary>
        public LineStyle LineStyle;

        /// <summary>
        /// Транслятор точек
        /// </summary>
        public IPointTranslator Translator;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (TapePosition.From >= TapePosition.To)
                return;

            var tapeLength = TapePosition.To - TapePosition.From;

            Translator.Src = new Rectangle<float>
            {
                Left = 0,
                Right = tapeLength,
                Bottom = Diapazone.From,
                Top = Diapazone.To
            };
            Translator.Dst = rect;

            var shapesFactory = ShapesFactoryConfigurator.For(gr.Shapes).Translate(Translator).Result;

            using (var pen = gr.Instruments.CreatePen(LineColor, LineWidth, LineStyle))
            using (var shape = shapesFactory.CreateLines(pen))
            {
				// Составим отрисовываемые точки
				var points = new List<Point<float>>();
                float index = Source.GetStartIndex(TapePosition.From) - TapePosition.From;
                while (Source.IsDataAvailable && (points.Count == 0 || points.Last().X < tapeLength))
                {
                    points.Add(new Point<float>{X=index, Y= Source.GetNextValue()});
                    index += Source.Step;
				}
            	if (points.Count < 2) return;

                if (points.First().X < 0)
                {
                    points[0] = new Point<float>
                                    {
                                        X = 0,
                                        Y = Interpolation(points[0].X, points[0].Y,
                                                          points[1].X, points[1].Y, 0)
                                    };
                }

                if (points.Last().X > tapeLength)
                {
                    points[points.Count-1] = new Point<float>
                    {
                        X = tapeLength,
                        Y = Interpolation(points[points.Count - 2].X,
                                      points[points.Count - 2].Y,
                                      points.Last().X, points.Last().Y, tapeLength)
                    };
				}

				shape.Render(points);
            }
        }

        public static float Interpolation(float x0, float y0, float x1, float y1, float x)
        {
            return y0 + ((y1 - y0) * (x - x0)) / (x1 - x0);
        }
    }
}
