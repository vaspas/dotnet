using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingSharpDx2D1.Shapes
{
    /// <summary>
    /// Фигура для рисования текста
    /// </summary>
    class TextShape : BaseShape, ITextShape
    {

        public void Render(string text, Point<float> point)
        {
        }

        public Size<float> Measure(string text)
        {
            return new Size<float>();
        }

    }
}
