using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Рисует текст для единиц длины, рисование происходит между отметками начиная с
    /// (Interrupt [n]).Index+1, заканчивая (Interrupt [n+1]).Index-1)
    /// </summary>
    public class CoordUnitTextRenderer : CoordUnitBaseRenderer, IRenderer
    {
        /// <summary>
        /// Единицы измерения длины, например "м"
        /// </summary>
        public string Unit;

        /// <summary>
        /// Транслятор точек
        /// </summary>
        public IPointTranslator Translator;

        /// <summary>
        /// Название шрифта
        /// </summary>
        public string FontName;

        /// <summary>
        /// Размер шрифта
        /// </summary>
        public int FontSize;

        /// <summary>
        /// Стиль шрифта
        /// </summary>
        public FontStyle FontStyle;

        /// <summary>
        /// Выравнивание текста оносительно точки для случая увеличения координат.
        /// </summary>
        public Alignment IncreaseAlignment;

        /// <summary>
        /// Выравнивание текста оносительно точки для случая уменьшения координат.
        /// </summary>
        public Alignment DecreaseAlignment;

        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color Color;

        /// <summary>
        /// Угол поворота текста
        /// </summary>
        public float Angle;

        /// <summary>
        /// Формат вывода значений
        /// </summary>
        public string TextFormatString;

        /// <summary>
        /// Фильтр отображаемых значений шкалы
        /// </summary>
        public Predicate<float> ValueFilter;

        public int BorderPixelsDistance { get; set; }

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
                                     Right=TapePosition.To,
                                     Bottom=0,
                                     Top=1
                                 };
            Translator.Dst = rect;

            foreach (Unit unit in CoordHelper.GetUnits(Source, TapePosition))
            {
                // Нарисуем штрихи с посчитанным шагом
                if (unit.BeginCoordinate < unit.EndCoordinate)
                    DrawTexts(gr, unit, IncreaseAlignment);
                else
                {
                    DrawTexts(gr, unit.Revert(), DecreaseAlignment);
                }

            }
        }

        private void DrawTexts(IGraphicContext context, Unit unit, Alignment alignment)
        {
            using (var font = context.Instruments.CreateFont(FontName, FontSize, Color, FontStyle))
            using (var shape = context.Shapes.CreateText(font, alignment, Angle))
            foreach (
                CoordinateData data in
                    CoordHelper.GetCoordinateData(Source, TapePosition, MinPixelsDistance, this, PriorityRenderers,
                                                  unit,Translator))
            {
                // Отфильтруем значение
                if (ValueFilter != null)
                    if (!ValueFilter(data.Coordinate)) continue;

                shape.Render(data.Coordinate.ToString(TextFormatString) + Unit,
                             Translator.Translate(new Point<float> { X = data.Index, Y = 0.5f }));
            }
        }
    }
}
