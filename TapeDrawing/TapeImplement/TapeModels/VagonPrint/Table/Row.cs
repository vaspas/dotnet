using System.Collections.Generic;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.TapeModels.VagonPrint.Table
{
    /// <summary>
    /// Строка состоит из матрицы ячеек.
    /// </summary>
    public class Row : List<List<ICell>>
    {
        /// <summary>
        /// Наличие рамки.
        /// </summary>
        public bool IsBorderEnabled { get; set; }

        /// <summary>
        /// Наличие линии-курсора.
        /// </summary>
        public bool IsCursorEnabled { get; set; }

        /// <summary>
        /// Индекс точки на ленте.
        /// </summary>
        public int Index { get; set; }


        public void AddText(int rowNumber, int columnNumber, string text, Alignment alignment)
        {
            AddTextInternal(rowNumber, columnNumber, text, alignment, FontStyle.None, null);
        }

        public void AddText(int rowNumber, int columnNumber, string text, Alignment alignment, FontStyle fontStyle, Color color)
        {
            AddTextInternal(rowNumber, columnNumber, text, alignment, fontStyle, color);
        }

        private void AddTextInternal(int rowNumber, int columnNumber, string text, Alignment alignment, FontStyle fontStyle, Color? color)
        {
            Increase(this, rowNumber);
            IncreaseNull(this[rowNumber], columnNumber);

            this[rowNumber][columnNumber] =
                new TextCell
                {
                    Alignment = alignment,
                    Color = color,
                    FontStyle = fontStyle,
                    Text = text
                };
        }

        private static void Increase<T>(ICollection<T> list, int number) where T : class,new()
        {
            while (number >= list.Count)
                list.Add(new T());
        }

        private static void IncreaseNull<T>(ICollection<T> list, int number) where T:class
        {
            while (number >= list.Count)
                list.Add(null);
        }
    }
}
