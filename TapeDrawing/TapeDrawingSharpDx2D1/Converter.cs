using System.Windows.Forms;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingSharpDx2D1
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

        public static SharpDX.Color Convert(Color color)
        {
            return new SharpDX.Color(color.R, color.G, color.B, color.A);
        }

        public static SharpDX.Vector4 ConvertToVertex(Color color)
        {
            return new SharpDX.Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
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

        public static SharpDX.ColorBGRA ConvertBGRA(Color color)
        {
            return new SharpDX.ColorBGRA(color.R, color.G, color.B, color.A);
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
