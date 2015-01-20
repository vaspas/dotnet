using System.Windows.Forms;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinForms
{
    static class Converter
    {
        public static System.Drawing.Drawing2D.DashStyle Convert(LineStyle style)
        {
            switch (style)
            {
                case LineStyle.Solid:
                    return System.Drawing.Drawing2D.DashStyle.Solid;
                case LineStyle.Dot:
                    return System.Drawing.Drawing2D.DashStyle.Dot;
                case LineStyle.Dash:
                    return System.Drawing.Drawing2D.DashStyle.Dash;
                default:
                    return System.Drawing.Drawing2D.DashStyle.Solid;
            }
        }

        public static System.Drawing.Color Convert(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Drawing.FontStyle Convert(FontStyle style)
        {
            var fontStyle=System.Drawing.FontStyle.Regular;

            if ((style & FontStyle.Bold) !=0)
                fontStyle |= System.Drawing.FontStyle.Bold;
            if ((style & FontStyle.Italic) != 0)
                fontStyle |= System.Drawing.FontStyle.Italic;

            return fontStyle;
        }

        public static MouseButton Convert(MouseButtons b)
        {
            switch (b)
            {
                case MouseButtons.Left:
                    return MouseButton.Left;
                case MouseButtons.Middle:
                    return MouseButton.Center;
                case MouseButtons.Right:
                    return MouseButton.Right;
                default:
                    return MouseButton.None;
            }
        }
    }
}
