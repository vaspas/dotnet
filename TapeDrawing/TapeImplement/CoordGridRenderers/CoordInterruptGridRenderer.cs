using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Рисует линии для отметок
    /// </summary>
    public class CoordInterruptGridRenderer : BaseCoordGridRenderer, IRenderer
    {
        /// <summary>
        /// Фильтр отметок
        /// </summary>
        public Predicate<ICoordInterrupt> Filter;

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
        public IPointTranslator Translator { get; set; }

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (TapePosition.From >= TapePosition.To)
                return;

            Translator.Src = new Rectangle<float>{Left = TapePosition.From, Right = TapePosition.To, Bottom = 0,Top = 1};
            Translator.Dst = rect;

            // Для ленты нужно получить список прерываний и отфильтровать их
            IEnumerable<ICoordInterrupt> interrupts = Source.GetCoordInterrupts(TapePosition.From, TapePosition.To);
            if (Filter != null)
                interrupts = interrupts.Where(interrupt => Filter(interrupt));

            // Нарисовать линии прерываний
            foreach (var interrupt in interrupts)
                DrawLine(gr, interrupt.Index);
        }

        private void DrawLine(IGraphicContext context, int index)
        {
            using (var pen = context.Instruments.CreatePen(LineColor, LineWidth, LineStyle))
            using (var lineShape = context.Shapes.CreateLines(pen))
            {
                var points = new List<Point<float>>
                                 {
                                     Translator.Translate(new Point<float>{X=index, Y=0}),
                                     Translator.Translate(new Point<float>{X=index, Y=1})
                                 };

                lineShape.Render(points);
            }
        }
    }
}
