using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.ZonesView.Extensions;
using TapeImplement.TapeModels.ZonesView.Track;

namespace TapeImplement.TapeModels.ZonesView
{
    public class TapeModel
    {
        public TapeModel(IScalePosition<int> tapePosition)
        {
            Tracks=new List<TrackItem>();

            TapePosition = tapePosition;

            InitExtensions();
        }

        private void InitExtensions()
        {
            _extensions.Add(new RefPositionCursor());

            GetExtension<RefPositionCursor>().RefPositionCursorChanged += OnCursorChanged;
        }

        public IScalePosition<int> TapePosition { get; private set; }

        public ILayer MainLayer { get; set; }
        
        public Action Redraw { get; set; }

        public bool Vertical { get; set; }


        
        public event Action<int> CursorPositionChanged = delegate { };

        internal List<TrackItem> Tracks { get; private set; }

        private readonly List<IExtension> _extensions = new List<IExtension>();

        public T GetExtension<T>()
            where T : class,IExtension
        {
            return _extensions.FirstOrDefault(e => e is T) as T;
        }

        public void ClearTracks()
        {
            Tracks.Clear();
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
                    new Color(255, 255, 255))
            });
            
            BuildTracks(MainLayer, Tracks);
            
            GetExtension<RefPositionCursor>().Build(this);

            MainLayer.Add(
                new MouseListenerLayer
                {
                    Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                    Settings = new MouseListenerLayerSettings { MouseMoveOutside = true },
                    MouseListener =
                        new MouseListenerLayers.LinearScale.ZoomDiapazoneMouseWheelListener<int>
                        {
                            Diapazone = TapePosition,
                            OnChanged = Redraw,
                            Factor = 0.1f
                        }

                });

        }
        
        public T CreateTrack<T>(TrackSize size) where T : TrackModel, new()
        {
            return CreateTrack<T>(Tracks, size);
        }
        
        private T CreateTrack<T>(List<TrackItem> tracks, TrackSize size) where T : TrackModel, new()
        {
            var trackLayer = new EmptyLayer { Area = CreateMarginsArea(0, 0, 0, 0) };

            var newtrack = new T
            {
                TapeModel = this,
                DataLayer = trackLayer
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
            float currentValue = 0;
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
                    absoluteTracksGroup[0].Sum(t => t.Size.Value) / 2,
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
