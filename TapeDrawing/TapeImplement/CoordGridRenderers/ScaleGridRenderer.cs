using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Renderer для шкалы значений сигнала.
    /// Рисует шкалу в соответствии с указанными в массиве значениями. Шкала должна отображаться во весь размер слоя
    /// </summary>
    public class ScaleGridRenderer : IRenderer
    {
        /// <summary>
        /// Список значений в линейке
        /// </summary>
        public float[] Values;

        /// <summary>
        /// Нижняя граница шкалы
        /// </summary>
        public Func<float> GetMin;

        /// <summary>
        /// Верхняя граница шкалы
        /// </summary>
        public Func<float> GetMax;

        /// <summary>
        /// Транслятор точек
        /// </summary>
        public IPointTranslator Translator;

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
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Src = new Rectangle<float> {Left = 0, Right = 1, Bottom = GetMin(), Top = GetMax()};
            Translator.Dst = rect;

            foreach (var value in Values) 
                DrawLine(gr, value);
        }

        private void DrawLine(IGraphicContext context, float value)
        {
            var shapes = ShapesFactoryConfigurator.For(context.Shapes)
                .Translate(Translator).Result;

            using (var pen = context.Instruments.CreatePen(LineColor, LineWidth, LineStyle))
            using (var lineShape = shapes.CreateLines(pen))
            {
                lineShape.Render(new[]
                                     {
                                         new Point<float> {X = 0, Y = value},
                                         new Point<float> {X = 1, Y = value}
                                     });
            }
        }
    }
}
