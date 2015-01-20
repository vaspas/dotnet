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
    public class SignalPointRenderer : IRenderer
    {
        /// <summary>
        /// Источник данных сигнала
        /// </summary>
        public ISignalPointSource Source;

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

        public bool RenderAsRegions;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (TapePosition.From >= TapePosition.To)
                return;

            Translator.Src = new Rectangle<float>
                                 {
                                     Left = TapePosition.From,
                                     Right = TapePosition.To,
                                     Bottom = Diapazone.From,
                                     Top = Diapazone.To
                                 };
            Translator.Dst = rect;

            var shapesFactory = ShapesFactoryConfigurator.For(gr.Shapes).Translate(Translator).Result;
            
            // Составим отрисовываемые точки
            var points = new List<Point<float>>();
            var point = Source.GetStartPoint(TapePosition.From, TapePosition.To);
            while (point != null && point.Value.X < TapePosition.To)
            {
                points.Add(point.Value);
                point = Source.GetNextPoint();
            }
            if (point != null)
                points.Add(point.Value);
            if (points.Count < 2) return;

            // Если первая точка левее границы, то заменим ее на первую в участке
            if (points[0].X < TapePosition.From)
            {
                points[0] = new Point<float>
                                {
                                    X = TapePosition.From,
                                    Y = Interpolation(points[0].X, points[0].Y,
                                                      points[1].X, points[1].Y, TapePosition.From)
                                };
            }

            // Если последняя точка не самая крайняя из возможных, добавим еще одну точку
            if (points.Last().X > TapePosition.To)
            {
                points[points.Count - 1] = new Point<float>
                                               {
                                                   X = TapePosition.To,
                                                   Y = Interpolation(points[points.Count - 2].X,
                                                                     points[points.Count - 2].Y,
                                                                     points.Last().X, points.Last().Y, TapePosition.To)
                                               };
            }

            using (var pen = gr.Instruments.CreatePen(LineColor, LineWidth, LineStyle))
            using (var shape = shapesFactory.CreateLines(pen))
            {
                if (!RenderAsRegions)
                    shape.Render(points);
                else
                {
                    for (var i = 0; i < points.Count-1; i += 2)
                        shape.Render(new [] {points[i], points[i + 1]});
                }
            }
        }

        public static float Interpolation(float x0, float y0, float x1, float y1, float x)
        {
            return y0 + ((y1 - y0) * (x - x0)) / (x1 - x0);
        }
    }
}
