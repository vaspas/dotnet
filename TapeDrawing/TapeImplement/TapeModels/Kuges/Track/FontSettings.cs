using TapeDrawing.Core.Primitives;

namespace TapeImplement.TapeModels.Kuges.Track
{
    public class FontSettings
    {
        public FontSettings()
        {
            Color = new Color(0, 0, 0);
            Name = "Arial";
            Style = FontStyle.None;
            Size = 10;
            Angle = 0;
        }

        public Color Color;

        public string Name;

        public FontStyle Style;

        public int Size;

        public float Angle;
    }
}
