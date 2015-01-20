using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Рисует текстовые обозначения отметок прерываний
    /// </summary>
    public class CoordInterruptTextRenderer : BaseCoordGridRenderer, IRenderer
    {
        /// <summary>
        /// Фильтр отметок
        /// </summary>
        public Predicate<ICoordInterrupt> Filter { get; set; }
        /// <summary>
        /// Фильтр более важных отметок. Значения, прошедшие через фильтр имеют более высокий приоритет и не могут быть 
        /// загорожены отметками, прошедшими через фильтр Filter
        /// </summary>
        public Predicate<ICoordInterrupt> PriorityFilter { get; set; }
        /// <summary>
        /// Минимальное расстояние между отметками в пикселах. Если в указанных пределах присутствует другая метка
        /// с большим приоритетом, то текущая метка не ставится
        /// </summary>
        public int MinPixelsDistance { get; set; }
        /// <summary>
        /// Транслятор точек
        /// </summary>
        public IPointTranslator Translator { get; set; }
        /// <summary>
        /// Транслятор выравнивания текста
        /// </summary>
        public IAlignmentTranslator TextAlignmentTranslator { get; set; }
        /// <summary>
        /// Название шрифта
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// Размер шрифта
        /// </summary>
        public int FontSize { get; set; }
        /// <summary>
        /// Стиль шрифта
        /// </summary>
        public FontStyle FontStyle { get; set; }
        /// <summary>
        /// Выравнивание текста оносительно точки
        /// </summary>
        public Alignment Alignment { get; set; }
        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Угол поворота текста
        /// </summary>
        public float Angle { get; set; }
        /// <summary>
        /// Формат вывода значений
        /// </summary>
        public string TextFormatString { get; set; }


        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (TapePosition.From >= TapePosition.To)
                return;

            Translator.Src = new Rectangle<float>{Left = TapePosition.From, Right = TapePosition.To, Bottom = 0, Top = 1};
            Translator.Dst = rect;

            // Для ленты нужно получить список прерываний
            IEnumerable<ICoordInterrupt> interrupts = Source.GetCoordInterrupts(TapePosition.From, TapePosition.To);

            // Полуим список прерываний с более высоким приоритетом
            var hiInterrupts = (PriorityFilter != null)
                                   ? interrupts.Where(interrupt => PriorityFilter(interrupt))
                                   : new ICoordInterrupt[0];

            // Получим список прерываний, которые нужно отобразить
            if (Filter != null)
                interrupts = interrupts.Where(interrupt => Filter(interrupt));

            // Посчитаем минимальное расстояние в индексах между прерываниями
            var p1 = Translator.Translate(new Point<float>(TapePosition.From, 0));
            var p2 = Translator.Translate(new Point<float>(TapePosition.To, 0));
            var pDist = Math.Max(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
            var minIndexDistance = (float) MinPixelsDistance*(TapePosition.To - TapePosition.From)/pDist;

            // Составим список прерываний, которые можно отображать
            var drawInterrupts =
                interrupts.Where(interrupt => CheckMinDistance(interrupt, hiInterrupts, minIndexDistance));

            // Нарисовать линии прерываний)
            foreach (var interrupt in drawInterrupts)
                DrawText(gr, interrupt.Index, interrupt.Title);
        }

        private static bool CheckMinDistance(ICoordInterrupt interrupt, IEnumerable<ICoordInterrupt> hiInterrupts, float minIndexDistance)
        {
            if (hiInterrupts.Contains(interrupt))
                return true;

            return hiInterrupts.All(hiInterrupt => Math.Abs(hiInterrupt.Index - interrupt.Index) >= minIndexDistance);
        }

        private void DrawText(IGraphicContext context, int index, string text)
        {
            using (var font = context.Instruments.CreateFont(FontName, FontSize, Color, FontStyle))
            using (var shape = context.Shapes.CreateText(font, TextAlignmentTranslator.Translate(Alignment), Angle))
            {
                shape.Render(text, Translator.Translate(new Point<float>{X=index,Y= 0.5f}));
            }
        }
    }

}
