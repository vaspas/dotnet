using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SharpDX;
using SharpDX.Direct3D9;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using Font = SharpDX.Direct3D9.Font;

namespace TapeDrawingSharpDx.Shapes
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
