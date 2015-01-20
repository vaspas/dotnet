using System;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Kuges.TrackHost;

namespace TapeImplement.TapeModels.Kuges
{
    public class TapeModel:StackTrackHost
    {
        public TapeModel()
        {
            TapeModel = this;
        }

        public IScalePosition<int> TapePosition { get; set; }

        public ILayer MainLayer { get; set; }

        public int ScaleSize { get; set; }

        public Action Redraw { get; set; }


        public void BuildMainLayer()
        {
            MainLayer.Clear();

            MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SimpleRenderers.FillAllRenderer(
                    new Color(255, 255, 255))
            });

            BuildTracksFor(this, MainLayer);
        }

        private static void BuildTracksFor(BaseTrackHost host, ILayer parent)
        {
            host.BuildTracks(parent);

            host.Tracks.ToList()
                .FindAll(t => t.Model is BaseTrackHost)
                .ForEach(t => BuildTracksFor(t.Model as BaseTrackHost, t.Layer));
        }
    }
}
