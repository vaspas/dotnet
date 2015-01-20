using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace TapeImplement.ObjectRenderers
{
    /// <summary>
    /// Рендерер точечных объектов
    /// </summary>
    public class RecordTextRenderer<T> : IRenderer
    {
        /// <summary>
        /// Угол наклона текста
        /// </summary>
        public float Angle;

        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        /// <summary>
        /// Источник данных для отображения
        /// </summary>
        public IObjectSource<T> Source;

        /// <summary>
        /// Объект для преобразования точек
        /// </summary>
        public IPointTranslator Translator;

        /// <summary>
        /// Выравнивание текста
        /// </summary>
        public Alignment Alignment;

        public string FontType;

        public int FontSize;

        public Color FontColor;

        public FontStyle FontStyle;

        public Func<T, string> ObjectPresentation;

        /// <summary>
        /// Функция получения положения данных.
        /// </summary>
        public Func<T, int> GetIndex;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (TapePosition.From >= TapePosition.To)
                return;

            Translator.Src = new Rectangle<float> { Left = TapePosition.From, Right = TapePosition.To, Bottom = -1, Top = 1 };
            Translator.Dst = rect;

            var shapesFactory = ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(Translator).Result;

            using (var font = gr.Instruments.CreateFont(FontType, FontSize, FontColor, FontStyle))
            using (var textShape = shapesFactory.CreateText(font, Alignment, Angle))
            {
                // Запросим данные
                var dataList = Source.GetData(TapePosition.From, TapePosition.To);

                // Отобразим объекты
                foreach (var r in dataList)
                {
                    textShape.Render(ObjectPresentation(r), new Point<float> { X = GetIndex(r), Y = 0 });
                }
            }
        }
    }
}
