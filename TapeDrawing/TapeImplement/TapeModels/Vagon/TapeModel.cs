using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Vagon.Extensions;
using TapeImplement.TapeModels.Vagon.Track;

namespace TapeImplement.TapeModels.Vagon
{
    public class TapeModel
    {
        public TapeModel(IScalePosition<int> tapePosition)
        {
            Tracks=new List<TrackItem>();
            FrontTracks = new List<TrackItem>();

            TapePosition = tapePosition;

            InitExtensions();

            tapePosition.PositionChanged += OnCursorChanged;
        }

        private void InitExtensions()
        {
            _extensions.Add(new TapeArea());
            _extensions.Add(new RefPositionCursor());
            _extensions.Add(new CoordGrid());
            _extensions.Add(new DistScale());
            _extensions.Add(new TrackObjects());
            _extensions.Add(new CoordinateInfo());
            _extensions.Add(new SelectedObjects());
            _extensions.Add(new Extensions.ToolTip());
            _extensions.Add(new MovableShadow {Color = new Color(100, 0, 0, 255)});
            _extensions.Add(new TapeInfo());

            GetExtension<RefPositionCursor>().RefPositionCursorChanged += OnCursorChanged;
        }

        public IScalePosition<int> TapePosition { get; private set; }

        public ILayer MainLayer { get; set; }

        public int ScaleSize { get; set; }

        public int DistScaleSize { get; set; }

        public Action Redraw { get; set; }

        public bool Vertical { get; set; }

        internal DataTrackModel SelectedTrack { get; set; }

        
        public event Action<int> CursorPositionChanged = delegate { };

        internal List<TrackItem> Tracks { get; private set; }
        internal List<TrackItem> FrontTracks { get; private set; }

        private readonly List<IExtension> _extensions = new List<IExtension>();

        public T GetExtension<T>()
            where T : class,IExtension
        {
            return _extensions.FirstOrDefault(e => e is T) as T;
        }

        public void ClearTracks()
        {
            Tracks.Clear();
            FrontTracks.Clear();
            _extensions.Clear();

            InitExtensions();
        }

        public void BuildMainLayer()
        {
            MainLayer.Clear();

            MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SimpleRenderers.FillAllRenderer(
                    new Color(250, 250, 250))
            });

            GetExtension<DistScale>().Build(this);
            GetExtension<CoordGrid>().Build(this);
            GetExtension<CoordinateInfo>().Build(this);

            BuildTracks(MainLayer, Tracks);

            GetExtension<TrackObjects>().Build(this);
            GetExtension<MovableShadow>().Build(this);
            GetExtension<TapeArea>().Build(this);
            GetExtension<RefPositionCursor>().Build(this);
            GetExtension<SelectedObjects>().Build(this);

            BuildTracks(MainLayer, FrontTracks);

            GetExtension<TapeInfo>().Build(this);
            GetExtension<Extensions.ToolTip>().Build(this);
        }
        
        public T CreateTrack<T>(TrackSize size) where T : BaseTrackModel, new()
        {
            CreateTrack<BaseTrackModel>(FrontTracks, size);
            return CreateTrack<T>(Tracks, size);
        }

        public BaseTrackModel GetFrontTrackFor<T>(T track) where T : BaseTrackModel
        {
            return FrontTracks[Tracks.FindIndex(ti=>ti.Model==track)].Model as BaseTrackModel;
        }

        private T CreateTrack<T>(List<TrackItem> tracks, TrackSize size) where T : BaseTrackModel, new()
        {
            var trackLayer = new EmptyLayer { Area = CreateMarginsArea(0, 0, 0, 0) };
            var scalelayer = new EmptyLayer { Area = CreateMarginsArea(0, null, 0, 0, ScaleSize, 0) };
            var datalayer = new EmptyLayer { Area = CreateMarginsArea(ScaleSize, 0, 0, 0) };
            trackLayer.Add(scalelayer);
            trackLayer.Add(datalayer);

            var newtrack = new T
            {
                TapeModel = this,
                DataLayer = datalayer,
                ScaleLayer = scalelayer
            };

            tracks.Add(new TrackItem
            {
                Layer = trackLayer,
                Size = size,
                Model = newtrack
            });

            return newtrack;
        }
        
        private void BuildTracks(ILayer parent, List<TrackItem> tracks)
        {
            //все дорожки с относительными размерами
            var relativeTracks = tracks.ToList()
                .FindAll(t => t.Size is TrackSizeRelative);

            //группы дорожек с абсолютными размерами, между дорожками с относительными размерами
            var absoluteTracksGroup = new List<List<TrackItem>>();
            absoluteTracksGroup.Add(new List<TrackItem>());
            foreach (var item in tracks)
            {
                if (item.Size is TrackSizeAbsolute)
                    absoluteTracksGroup.Last().Add(item);
                else
                    absoluteTracksGroup.Add(new List<TrackItem>());
            }
            if (absoluteTracksGroup.Count == 1)
                absoluteTracksGroup.Add(new List<TrackItem>());

            //вставляем дорожки с абсолютными размерами в начале
            float currentValue = DistScaleSize;
            foreach (var track in absoluteTracksGroup[0])
            {
                var layer = new EmptyLayer { Area = CreateMarginsArea(0, 0, currentValue, null, 0, track.Size.Value) };
                layer.Add(track.Layer);
                parent.Add(layer);
                currentValue += track.Size.Value;
            }

            //вставляем дорожки с абсолютными размерами в конце
            currentValue = 0;
            foreach (var track in absoluteTracksGroup.Last())
            {
                var layer = new EmptyLayer { Area = CreateMarginsArea(0, 0, null, currentValue, 0, track.Size.Value) };
                layer.Add(track.Layer);
                parent.Add(layer);
                currentValue += track.Size.Value;
            }

            var relativeTracksLayer = new EmptyLayer
            {
                Area = CreateMarginsArea(
                    0, 0,
                    absoluteTracksGroup[0].Sum(t => t.Size.Value) / 2 + DistScaleSize,
                    absoluteTracksGroup.Last().Sum(t => t.Size.Value) / 2)
            };
            parent.Add(relativeTracksLayer);

            //вставляем дорожки с относительными размерами
            currentValue = 0;
            foreach (var track in relativeTracks)
            {
                var trackIndex = relativeTracks.IndexOf(track);

                var layerK = track.Size.Value / relativeTracks.Sum(t => t.Size.Value);

                var l1 = new EmptyLayer { Area = CreateRelativeArea(0, 1, currentValue, currentValue + layerK) };
                relativeTracksLayer.Add(l1);
                var l2 = new EmptyLayer
                {
                    Area = CreateMarginsArea(
                        0, 0,
                        absoluteTracksGroup[trackIndex].Sum(t => t.Size.Value) / 2,
                        absoluteTracksGroup[trackIndex + 1].Sum(t => t.Size.Value) / 2)
                };
                l1.Add(l2);
                l2.Add(track.Layer);

                currentValue += layerK;
            }

            //вставляем оставшиеся дорожки с абсолютными размерами
            currentValue = 0;
            for (var i = 1; i < absoluteTracksGroup.Count - 1; i++)
            {
                var k = relativeTracks[i - 1].Size.Value / relativeTracks.Sum(t => t.Size.Value);

                var l1 = new EmptyLayer
                {
                    Area = CreateRelativeArea(0, 1,
                        currentValue,
                        2 * k)
                };
                relativeTracksLayer.Add(l1);
                var l2 = new EmptyLayer
                {
                    Area = CreateMarginsArea(0, 0, null, null,
                                                            0, absoluteTracksGroup[i].Sum(t => t.Size.Value))
                };
                l1.Add(l2);

                float v = 0;
                absoluteTracksGroup[i].ForEach(t =>
                {
                    var layer = new EmptyLayer { Area = CreateMarginsArea(0, 0, null, v, 0, t.Size.Value)};
                    layer.Add(t.Layer);
                    l2.Add(layer);
                    v += t.Size.Value;
                });

                currentValue += k;

            }
        }

        private IArea<float> CreateMarginsArea(float? left, float? right, float? bottom, float? top, float width, float height)
        {
            return Vertical
                       ? AreasFactory.CreateMarginsArea(bottom, top, right, left, height, width)
                       : AreasFactory.CreateMarginsArea(left, right, top, bottom, width, height);
        }

        public IArea<float> CreateMarginsArea(float left, float right, float bottom, float top)
        {
            return Vertical
                       ? AreasFactory.CreateMarginsArea(bottom, top, right, left)
                       : AreasFactory.CreateMarginsArea(left, right, top, bottom);
        }

        private IArea<float> CreateRelativeArea(float left, float right, float bottom, float top)
        {
            return Vertical
                       ? AreasFactory.CreateRelativeArea(bottom, top, 1-right, 1-left)
                       : AreasFactory.CreateRelativeArea(left, right, 1-top, 1-bottom);
        }


        private void OnCursorChanged()
        {
            CursorPositionChanged(GetExtension<RefPositionCursor>().AbsolutePosition);
            
        }
    }
}
