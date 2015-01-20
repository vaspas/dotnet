using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Kuges.Track;

namespace TapeImplement.TapeModels.Kuges.TrackHost
{
    public class StackTrackHost:BaseTrackHost
    {
        public T CreateTrack<T>(TrackSizeAbsolute size) where T : BaseTrackModel, new()
        {
            return CreateTrack<T>(size, false);
        }

        public T CreateTrack<T>(TrackSize size, bool clip) where T : BaseTrackModel, new()
        {
            var trackLayer = new EmptyLayer{ Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0) };
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

        public T CreateHost<T>(TrackSize size) where T : BaseTrackHost, new()
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

        internal override void BuildTracks(ILayer parent)
        {
            //все дорожки с относительными размерами
            var relativeTracks = Tracks.ToList()
                .FindAll(t => t.Size is TrackSizeRelative);

            //группы дорожек с абсолютными размерами, между дорожками с относительными размерами
            var absoluteTracksGroup = new List<List<TrackItem>>();
            absoluteTracksGroup.Add(new List<TrackItem>());
            foreach (var item in Tracks)
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
                var layer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0, 0, currentValue, null, 0, track.Size.Value) };
                layer.Add(track.Layer);
                parent.Add(layer);
                currentValue += track.Size.Value;
            }

            //вставляем дорожки с абсолютными размерами в конце
            currentValue = 0;
            foreach (var track in absoluteTracksGroup.Last())
            {
                var layer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0, 0, null, currentValue, 0, track.Size.Value) };
                layer.Add(track.Layer);
                parent.Add(layer);
                currentValue += track.Size.Value;
            }

            var relativeTracksLayer = new EmptyLayer
            {
                Area = AreasFactory.CreateMarginsArea(
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

                var l1 = new EmptyLayer { Area = AreasFactory.CreateRelativeArea(0, 1, currentValue, currentValue + layerK) };
                relativeTracksLayer.Add(l1);
                var l2 = new EmptyLayer
                {
                    Area = AreasFactory.CreateMarginsArea(
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
            for (int i = 1; i < absoluteTracksGroup.Count - 1; i++)
            {
                var k = relativeTracks[i - 1].Size.Value / relativeTracks.Sum(t => t.Size.Value);

                var l1 = new EmptyLayer
                {
                    Area = AreasFactory.CreateRelativeArea(0, 1,
                        currentValue,
                        2 * k)
                };
                relativeTracksLayer.Add(l1);
                var l2 = new EmptyLayer
                {
                    Area = AreasFactory.CreateMarginsArea(0, 0, null, null,
                                                            0, absoluteTracksGroup[i].Sum(t => t.Size.Value))
                };
                l1.Add(l2);

                float v = 0;
                absoluteTracksGroup[i].ForEach(t =>
                {
                    var layer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0, 0, null, v, 0, t.Size.Value) };
                    layer.Add(t.Layer);
                    l2.Add(layer);
                    v += t.Size.Value;
                });

                currentValue += k;

            }
        }
    }
}
