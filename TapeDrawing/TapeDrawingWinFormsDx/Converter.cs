using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinFormsDx
{
    static class Converter
    {
		public static int Convert(LineStyle style, float width)
		{
			switch (style)
			{
				case LineStyle.Dash:
					return 0x77777777;
				case LineStyle.Dot:
                    //сделал float для ширины, но линию не настраивал, 17.07.2013
					switch ((int)width)
					{
						case 1:
							return 0x55555555;
						case 2:
							return 0x33333333;
						case 3:
							return 0x1E38F1C7;
						case 4:
							return 0x0F0F0F0F;
						case 5:
							return 0x7E07C1F;
						default:
							return 0x00FF00FF;
					}
				default:
					return -1;
			}
		}

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
            var fontStyle = System.Drawing.FontStyle.Regular;

            if ((style & FontStyle.Bold) != 0)
                fontStyle |= System.Drawing.FontStyle.Bold;
            if ((style & FontStyle.Italic) != 0)
                fontStyle |= System.Drawing.FontStyle.Italic;

            return fontStyle;
        }

		public static DrawTextFormat Convert(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, bool wordWrap)
		{
			var format = wordWrap ? DrawTextFormat.WordBreak : DrawTextFormat.SingleLine;

			switch (horizontalAlignment)
			{
				case HorizontalAlignment.Left:
					format |= DrawTextFormat.Left;
					break;
				case HorizontalAlignment.Right:
					format |= DrawTextFormat.Right;
					break;
				case HorizontalAlignment.Center:
					format |= DrawTextFormat.Center;
					break;
			}

			switch (verticalAlignment)
			{
				case VerticalAlignment.Top:
					format |= DrawTextFormat.Top;
					break;
				case VerticalAlignment.Bottom:
					format |= DrawTextFormat.Bottom;
					break;
				case VerticalAlignment.Center:
					format |= DrawTextFormat.VerticalCenter;
					break;
			}

			return format;
		}

		public static int ConvertToInt(Color color)
		{
			int argb = 0;
			argb += color.A;
			argb = argb << 8;
			argb += color.R;
			argb = argb << 8;
			argb += color.G;
			argb = argb << 8;
			argb += color.B;

			return argb;
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
