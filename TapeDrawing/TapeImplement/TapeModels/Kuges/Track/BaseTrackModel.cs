using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Layer;

namespace TapeImplement.TapeModels.Kuges.Track
{
    public abstract class BaseTrackModel
    {
        public TapeModel TapeModel { get; internal set; }

        public ILayer TrackLayer { get; internal set; }

        public ILayer DataLayer { get; internal set; }

        public ILayer ScaleLayer { get; internal set; }

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

        public void AddToDataLayer(ILayer child)
        {
            DataLayer.Add(child);
        }

        public void AddToScaleLayer(ILayer child)
        {
            DataLayer.Add(child);
        }
    }
}
