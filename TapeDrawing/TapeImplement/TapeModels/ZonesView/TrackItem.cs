using TapeDrawing.Core.Layer;
using TapeImplement.TapeModels.ZonesView.Track;

namespace TapeImplement.TapeModels.ZonesView
{
    public class TrackItem
    {
        public TrackSize Size { get; set; }

        public ILayer Layer { get; set; }

        public TrackModel Model { get; set; }
    }

}
