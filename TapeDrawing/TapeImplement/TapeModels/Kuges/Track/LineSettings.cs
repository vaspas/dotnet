using TapeDrawing.Core.Primitives;

namespace TapeImplement.TapeModels.Kuges.Track
{
    public class LineSettings
    {
        public LineSettings()
        {
            Color = new Color(0,0,0);
            Width = 1;
            Style = LineStyle.Solid;
        }

        public Color Color;

        public int Width;

        public LineStyle Style;
    }
}
