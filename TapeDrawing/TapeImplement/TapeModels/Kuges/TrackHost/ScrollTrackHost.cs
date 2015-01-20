using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Layers;
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.Kuges.Track;

namespace TapeImplement.TapeModels.Kuges.TrackHost
{
    public class ScrollTrackHost:BaseTrackHost
    {
        public T CreateTrack<T>(TrackSizeAbsolute size) where T : BaseTrackModel, new()
        {
            return CreateTrack<T>(size, false);
        }

        public T CreateTrack<T>(TrackSizeAbsolute size, bool clip) where T : BaseTrackModel, new()
        {
            var trackLayer = new EmptyLayer {Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0)};
            var scalelayer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0, null, 0, 0, TapeModel.ScaleSize, 0) };
            var datalayer = new RendererLayer 
            { 
                Area = AreasFactory.CreateMarginsArea(TapeModel.ScaleSize, 0, 0, 0) ,
                Settings = new RendererLayerSettings { Clip = clip }
            };
            trackLayer.Add(scalelayer);
            trackLayer.Add(datalayer);

            var newtrack = new T
            {
                TapeModel = TapeModel,
                TrackLayer = trackLayer,
                DataLayer = datalayer,
                ScaleLayer = scalelayer
            };

            Tracks.Add(new TrackItem
            {
                Layer = trackLayer,
                Size = size,
                Model = newtrack
            });

            return newtrack;
        }

        public T CreateHost<T>(TrackSizeAbsolute size) where T : BaseTrackHost, new()
        {
            var trackLayer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0) };

            var newtrack = new T
            {
                TapeModel = TapeModel
            };

            Tracks.Add(new TrackItem
            {
                Layer = trackLayer,
                Size = size,
                Model = newtrack
            });

            return newtrack;
        }

        /// <summary>
        /// Значение от 0 до 1.
        /// </summary>
        public float ScrollValue
        {
            get { return _scrollArea.ScrollValue; }
            set { _scrollArea.ScrollValue = value; }
        }

        /// <summary>
        /// Перекрытие. При значениях >1 изображение полностью видно на экране.
        /// Обновляется после перерисовки.
        /// </summary>
        public float Overlap
        {
            get { return _scrollArea.Overlap; }
        }

        /// <summary>
        /// Фиксированное значение смещения, при условии что Overlap>1.
        /// </summary>
        public float? FixedScrollValueIfOverlap { get; set; }

        private VerticalScrollArea _scrollArea;


        internal override void BuildTracks(ILayer parent)
        {
            var clipLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new FakeRenderer()
            };
            parent.Add(clipLayer);

            _scrollArea = new VerticalScrollArea
                              {
                                  FixedScrollValueIfOverlap = FixedScrollValueIfOverlap
                              };
            var scrollLayer = new EmptyLayer
            {
                Area = _scrollArea
            };
            clipLayer.Add(scrollLayer);


            //вставляем дорожки
            float currentValue = 0;
            foreach (var track in Tracks)
            {
                var layer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0, 0, currentValue, null, 0, track.Size.Value) };
                layer.Add(track.Layer);
                scrollLayer.Add(layer);
                currentValue += track.Size.Value;
            }
            _scrollArea.Height = currentValue;
        }
    }
}
