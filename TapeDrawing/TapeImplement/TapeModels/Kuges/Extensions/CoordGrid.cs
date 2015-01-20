using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.CoordGridRenderers;
using TapeImplement.TapeModels.Kuges.Track;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class CoordGrid:IExtension
    {
        public CoordGrid()
        {
            Color = new Color(255, 220, 220);
        }

        public ICoordinateSource Source { get; set; }
        public Color Color { get; set; }

        private DataTrackModel _trackModel;

        public void Build(DataTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);

            var largeUnitGridLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source = Source,
                    LineColor = Color,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 2,
                    TapePosition = _trackModel.TapeModel.TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 150,
                    PriorityRenderers = new CoordUnitBaseRenderer[] { }
                }
            };
            _trackModel.DataLayer.Add(largeUnitGridLayer);

            var mediumUnitGridLayer =
                new RendererLayer
                    {
                        Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                        Settings = new RendererLayerSettings {Clip = true},
                        Renderer = new CoordUnitGridRenderer
                                       {
                                           Source = Source,
                                           LineColor = Color,
                                           LineStyle = LineStyle.Solid,
                                           LineWidth = 1,
                                           TapePosition = _trackModel.TapeModel.TapePosition,
                                           Translator =
                                               TapeDrawing.Core.Translators.
                                               PointTranslatorConfigurator.
                                               CreateLinear().Translator,
                                           Mask = new[] {0.1f, 0.5f},
                                           MinPixelsDistance = 70,
                                           PriorityRenderers =
                                               new[]
                                                   {
                                                       largeUnitGridLayer.Renderer as
                                                       CoordUnitBaseRenderer
                                                   }
                                       }
                    };
            _trackModel.DataLayer.Add(mediumUnitGridLayer);

            var smallUnitGridLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source = Source,
                    LineColor = Color,
                    LineStyle = LineStyle.Dot,
                    LineWidth = 1,
                    TapePosition = _trackModel.TapeModel.TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new [] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 14,
                    PriorityRenderers =
                        new[]
                                                                        {
                                                                            largeUnitGridLayer.Renderer as
                                                                            CoordUnitBaseRenderer,
                                                                            mediumUnitGridLayer.Renderer as
                                                                            CoordUnitBaseRenderer
                                                                        }
                }
            };
            _trackModel.DataLayer.Add(smallUnitGridLayer);

            var interruptLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new CoordInterruptGridRenderer
                {
                    Source = Source,
                    LineColor = Color,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 3,
                    TapePosition = _trackModel.TapeModel.TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.PointTranslatorConfigurator
                        .CreateLinear().Translator,
                    Filter = ic => true
                }
            };
            _trackModel.DataLayer.Add(interruptLayer);

        }
    }
}
