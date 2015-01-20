using TapeDrawing.Core.Layer;

namespace TapeImplement.TapeModels.Kuges
{
    public class TrackItem
    {
        public TrackSize Size { get; set; }

        public ILayer Layer { get; set; }

        public object Model { get; set; }
    }

}
