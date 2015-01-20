using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Рисует линии (шкалу) для  единиц длины, рисование происходит между отметками начиная с 
    /// (Interrupt [n]).Index+1, заканчивая (Interrupt [n+1]).Index-1)
    /// </summary>
    public class CoordUnitGridRenderer : CoordUnitBaseRenderer, IRenderer
    {
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

            Translator.Src = new Rectangle<float> { Left = TapePosition.From, Right = TapePosition.To ,Bottom = 0, Top = 1};
            Translator.Dst = rect;

            foreach (Unit unit in CoordHelper.GetUnits(Source, TapePosition))
            {
                // Нарисуем штрихи с посчитанным шагом
                if (unit.BeginCoordinate < unit.EndCoordinate)
                    DrawLines(gr, unit);
                else
                {
                    var tmpUnit = new Unit
                                      {
                                          BeginCoordinate = unit.EndCoordinate,
                                          BeginIndex = unit.EndIndex,
                                          EndCoordinate = unit.BeginCoordinate,
                                          EndIndex = unit.BeginIndex
                                      };
                    DrawLines(gr, tmpUnit);
                }
            }
        }

        private void DrawLines(IGraphicContext context, Unit unit)
        {
            var points = new List<Point<float>>();
            var side = -0.5f;
            foreach (
                CoordinateData data in
                    CoordHelper.GetCoordinateData(Source, TapePosition, MinPixelsDistance, this, PriorityRenderers,
                                                  unit, Translator))
            {
                points.Add(new Point<float> {X = data.Index, Y = side});
                if (side < 0)
                    side = 1.5f;
                else
                    side = -0.5f;
                points.Add(new Point<float> { X = data.Index, Y = side });
            }

            if (points.Count < 2)
                return;

            var shapes = TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator.For(context.Shapes)
                .Translate(Translator).Result;

            using (var pen = context.Instruments.CreatePen(LineColor, LineWidth, LineStyle))
            using (var lineShape = shapes.CreateLines(pen))
                lineShape.Render(points);
        }
    }
}
