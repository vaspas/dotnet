using TapeDrawing.Core.Primitives;

namespace TapeImplement.TapeModels.VagonPrint.Track
{
    public class LineSettings
    {
        public LineSettings()
        {
            Color = new Color(120, 120, 120);
            Width = 0.1f;
            Style = LineStyle.Solid;
        }

        public Color Color;

        public float Width;

        public LineStyle Style;
    }
}
