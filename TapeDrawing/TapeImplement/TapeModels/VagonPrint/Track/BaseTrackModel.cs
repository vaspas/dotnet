using TapeDrawing.Core.Layer;

namespace TapeImplement.TapeModels.VagonPrint.Track
{
    public abstract class BaseTrackModel
    {
        public TapeModel TapeModel { get; internal set; }

        public ILayer DataLayer { get; internal set; }

        public ILayer ScaleLayer { get; internal set; }
    }
}
