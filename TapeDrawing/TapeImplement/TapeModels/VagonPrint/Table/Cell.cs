using System.IO;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.TapeModels.VagonPrint.Table
{
    public interface ICell
    { }

    public class TextCell : ICell
    {
        public string Text { get; set; }
        public Alignment Alignment { get; set; }
        public FontStyle FontStyle { get; set; }
        public Color? Color { get; set; }
    }

    public class ImageCell : ICell
    {
        public Stream Image { get; set; }
        public Alignment Alignment { get; set; }
    }
}
