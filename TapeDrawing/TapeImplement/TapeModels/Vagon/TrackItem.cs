using TapeDrawing.Core.Layer;
using TapeImplement.TapeModels.Vagon.Track;

namespace TapeImplement.TapeModels.Vagon
{
    public class TrackItem
    {
        public TrackSize Size { get; set; }

        public ILayer Layer { get; set; }

        public BaseTrackModel Model { get; set; }
    }

}
