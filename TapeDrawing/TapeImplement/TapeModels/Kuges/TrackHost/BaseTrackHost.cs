
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Layer;

namespace TapeImplement.TapeModels.Kuges.TrackHost
{
    public abstract class BaseTrackHost
    {
        public TapeModel TapeModel { get; set; }
        
        internal readonly List<TrackItem> Tracks = new List<TrackItem>();

        private readonly List<IExtension> _extensions = new List<IExtension>();

        public T GetExtension<T>()
            where T : class,IExtension
        {
            return _extensions.First(e => e is T) as T;
        }

        public void AddExtension(IExtension extension)
        {
            _extensions.Add(extension);
        }

        internal abstract void BuildTracks(ILayer parent);
    }
}
