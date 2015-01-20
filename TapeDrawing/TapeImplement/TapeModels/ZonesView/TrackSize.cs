
namespace TapeImplement.TapeModels.ZonesView
{
    public abstract class  TrackSize
    {
        public float Value { get; set; }
    }

    public class TrackSizeAbsolute : TrackSize
    {
    }

    public class TrackSizeRelative : TrackSize
    {
    }
}
