using TapeDrawing.Core.Area;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Kuges.Track;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class Border:IExtension
    {
        public Border()
        {
            Settings=new LineSettings();
        }
        
        public LineSettings Settings { get; set; }

        private BaseTrackModel _trackModel;

        public void Build(BaseTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);

            _trackModel.TrackLayer.Add(new RendererLayer
            {
                Area = _trackModel.DataLayer.Area,
                Renderer = new SimpleRenderers.BorderRenderer
                {
                    Bottom = true,
                    Color = Settings.Color,
                    Left = true,
                    LineStyle = Settings.Style,
                    Right = true,
                    Top = true,
                    LineWidth = Settings.Width
                }
            });

        }
    }
}
