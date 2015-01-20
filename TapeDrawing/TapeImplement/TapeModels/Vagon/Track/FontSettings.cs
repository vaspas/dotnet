using TapeDrawing.Core.Primitives;

namespace TapeImplement.TapeModels.Vagon.Track
{
    public class FontSettings
    {
        public FontSettings()
        {
            Color = new Color(0, 0, 0);
            Name = "Nina";
            Style = FontStyle.None;
            Size = 10;
            Angle = 0;
        }

        public Color Color { get; set; }

        public string Name { get; set; }

        public FontStyle Style { get; set; }

        public int Size { get; set; }

        public float Angle { get; set; }
    }
}
