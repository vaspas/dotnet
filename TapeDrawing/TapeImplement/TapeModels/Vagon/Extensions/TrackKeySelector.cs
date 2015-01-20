using System.Linq;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Vagon.Track;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class TrackKeySelector : IExtension<TapeModel>, IKeyProcess
    {
        private TapeModel _tapeModel;

        private IArea<float> CreateMarginsArea(float left, float right, float bottom, float top)
        {
            return _tapeModel.Vertical
                       ? AreasFactory.CreateMarginsArea(bottom, top, right, left)
                       : AreasFactory.CreateMarginsArea(left, right, top, bottom);
        }

        public void Build(TapeModel tapeModel)
        {
            _tapeModel = tapeModel;

            var positionLayer = new EmptyLayer
                                    {
                                        Area = CreateMarginsArea(_tapeModel.ScaleSize, 0, 0, 0)
                                    };
            _tapeModel.MainLayer.Add(positionLayer);
            
            positionLayer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess = this
            });
        }


        public void OnKeyDown(KeyboardKey key)
        {
            var dir = 0;

            if (key == (_tapeModel.Vertical ? KeyboardKey.Right : KeyboardKey.Up))
                dir = 1;
            if (key == (_tapeModel.Vertical ? KeyboardKey.Left : KeyboardKey.Down))
                dir = -1;

            if (dir == 0)
                return;

            var selected = _tapeModel.Tracks.FirstOrDefault(ti => ti.Model == _tapeModel.SelectedTrack);

            var index = _tapeModel.Tracks.IndexOf(selected);

            for (var i = index + dir; i != index; i += dir)
            {
                if (dir < 0 && i < 0)
                    i = _tapeModel.Tracks.Count - 1;
                if (dir > 0 && i == _tapeModel.Tracks.Count)
                    i = 0;

                var tm = _tapeModel.Tracks[i].Model as DataTrackModel;

                if (tm == null)
                    continue;

                if (tm.GetExtension<SelectedTrack>() == null)
                    continue;

                _tapeModel.SelectedTrack = tm;
                _tapeModel.Redraw();
                break;
            }

        }

        public void OnKeyUp(KeyboardKey key)
        {
        }
    }
}
