using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace ComparativeTapeTest.Tapes.CurvePanelLayers
{
    /// <summary>
    /// Рендерер для отображения текста
    /// </summary>
    public class TableTextRenderer : IRenderer
    {
        /// <summary>
        /// Текст для отображения
        /// </summary>
        public string[,] TableData { get; set; }

        /// <summary>
        /// Название шрифта
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// Размер текста
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// Стиль текста
        /// </summary>
        public FontStyle Style { get; set; }
        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color Color { get; set; }

        private readonly IPointTranslator _translator = PointTranslatorConfigurator.CreateLinear().Translator;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            _translator.Src = new Rectangle<float> { Left = 0f, Right = 1f, Bottom = 1f, Top = 0f };
            _translator.Dst = rect;

            var rowsCount = TableData.GetLength(0);
            var columnsCount = TableData.GetLength(1);

            using (var font = gr.Instruments.CreateFont(FontName, Size, Color, Style))
            using (var shape = gr.Shapes.CreateText(font, Alignment.None, 0))
            {
                for (var row = 0; row < rowsCount; row++)
                    for (var column = 0; column < columnsCount; column++)
                    {

                        shape.Render(TableData[row, column],
                                     _translator.Translate(new Point<float>
                                                               {
                                                                   X=(float) column/columnsCount + 0.5f/columnsCount,
                                                                   Y=(float) row/rowsCount + 0.5f/rowsCount
                                                               }));
                    }
            }
        }

    }
}
