using System;
using System.Linq;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Vagon.Track;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class ObjectsNavigatorTrack<T>:IExtension<DataTrackModel>, IKeyProcess
        where T:class
    {
        private DataTrackModel _trackModel;

        public IObjectsNavigator<T> Navigator { get; set; }

        public Func<T, int> GetIndex { get; set; }
        
        public void Build(DataTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);
            
            _trackModel.DataLayer.Add(
                new KeyboardListenerLayer
                    {
                        Area = AreasFactory.CreateFullArea(),
                        KeyboardProcess = this
                    });
        }

        void IKeyProcess.OnKeyDown(KeyboardKey key)
        {
            var posExt = _trackModel.TapeModel.GetExtension<RefPositionCursor>();

            if (_trackModel.TapeModel.SelectedTrack != _trackModel)
                return;

            int newIndex;


            if(key==KeyboardKey.A)
            {
                var data = Navigator.GetNext(posExt.AbsolutePosition, 1);
                if (!data.Any())
                    return;

                newIndex = data.Min(GetIndex);
            }
            else if(key==KeyboardKey.Z)
            {
                var data = Navigator.GetPrevious(posExt.AbsolutePosition, 1);
                if (!data.Any())
                    return;

                newIndex = data.Max(GetIndex);
            }
            else
            {
                return;
            }

            var f = _trackModel.TapeModel.TapePosition.From;
            var t = _trackModel.TapeModel.TapePosition.To;
            var half = (t - f) / 2;

            _trackModel.TapeModel.TapePosition.Set(newIndex - half, newIndex + half);

            posExt.AbsolutePosition = newIndex;

            _trackModel.TapeModel.Redraw();
        }

        void IKeyProcess.OnKeyUp(KeyboardKey key)
        {

        }
    }
}
