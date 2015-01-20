using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.Vagon.Track;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class SelectedTrack:IExtension<DataTrackModel>
    {
        public SelectedTrack()
        {
            Color = new Color(255, 220, 220);
        }

        public Color Color { get; set; }

        private DataTrackModel _trackModel;

        public void Build(DataTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);

            var translator = trackModel.TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().Translator
                       : PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator;

             _trackModel.ScaleLayer.Insert(0,
                new RendererLayer
                {
                    Area = AreasFactory.CreateFullArea(),
                    Renderer =
                        new PredicateRendererDecorator
                        {
                            Internal = new FillRenderer(new Color(255, 165, 0)),
                            Predicate = () => _trackModel.TapeModel.SelectedTrack == _trackModel
                        }
                });

             _trackModel.ScaleLayer.Insert(0, new MouseListenerLayer
             {
                 Area = AreasFactory.CreateFullArea(),
                 Settings = new MouseListenerLayerSettings { MouseUpOutside = false },
                 MouseListener =
                 new MouseListenerLayers.PressedListener
                 {
                     Button = MouseButton.Left,
                     Shift = false,
                     Control = false,
                     Translator = translator,
                     PositionChanged =
                     (p1, p2) => { },
                     Completed =
                     (p1, p2) =>
                     {
                         if (p1 == null || p1.Value.X != p2.X || p1.Value.Y != p2.Y)
                             return false;

                         _trackModel.TapeModel.SelectedTrack = _trackModel;
                          _trackModel.TapeModel.Redraw();
                         return true;
                     }
                 }
             });
        }
    }
}
