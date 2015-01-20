
namespace TapeDrawing.Core.Primitives
{
    public struct Color
    {
        public Color(string hex)
        {
            var argb = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

            A = (byte)(argb >> 24);
            R = (byte)(argb >> 16);
            G = (byte)(argb >> 8);
            B = (byte)(argb);
        }

        public Color(int argb)
        {
            A = (byte)(argb >> 24);
            R = (byte)(argb >> 16);
            G = (byte)(argb >> 8);
            B = (byte)(argb);
        }

        public Color(byte r, byte g, byte b)
        {
            A = 255;
            R = r;
            G = g;
            B = b;
        }

        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public int ToArgb()
        {
            return (A << 24) + (R << 16) + (G << 8) + B;
        }
    }
}
