using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Layer;

namespace TapeImplement.TapeModels.Vagon.Track
{
    public class BaseTrackModel
    {
        internal TapeModel TapeModel { get; set; }

        internal ILayer DataLayer { get; set; }

        internal ILayer ScaleLayer { get; set; }

        private readonly List<IExtension> _extensions = new List<IExtension>();

        public T GetExtension<T>()
            where T : class,IExtension
        {
            return _extensions.FirstOrDefault(e => e is T) as T;
        }

        public void AddExtension(IExtension extension)
        {
            _extensions.Add(extension);
        }
    }
}
