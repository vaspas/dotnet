using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.ObjectRenderers.LinearScale;
using TapeImplement.TapeModels.Kuges.Track;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class ScaleGrid:IExtension
    {
        public ScaleGrid()
        {
            Color = new Color(255, 220, 220);
        }

        public Color Color { get; set; }

        private DataTrackModel _trackModel;

        public void Build(DataTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);

            _trackModel.DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new ScaleLinesRenderer
                {
                    Diapazone = _trackModel.Position,
                    LineColor = Color,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    Mask = new[] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 30,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                }
            });

            _trackModel.DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new ScaleLinesRenderer
                {
                    Diapazone = _trackModel.Position,
                    LineColor = Color,
                    LineStyle = LineStyle.Dot,
                    LineWidth = 1,
                    Mask = new[] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 6,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                }
            });

        }
    }
}
