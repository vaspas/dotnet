using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.CoordGridRenderers;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class CoordGrid:IExtension<TapeModel>
    {
        internal CoordGrid()
        {
            Color = new Color(255, 220, 220);
        }

        public ICoordinateSource Source { get; set; }
        public Color Color { get; set; }

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
                Area = CreateMarginsArea(_tapeModel.ScaleSize, 0, tapeModel.DistScaleSize, 0 )
            };
            _tapeModel.MainLayer.Add(positionLayer);

            var translator = tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

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
                    TapePosition = _tapeModel.TapePosition,
                    Translator = translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 150,
                    PriorityRenderers = new CoordUnitBaseRenderer[] { }
                }
            };
            positionLayer.Add(largeUnitGridLayer);

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
                                           TapePosition = _tapeModel.TapePosition,
                                           Translator = translator,
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
            positionLayer.Add(mediumUnitGridLayer);

            var smallUnitGridLayer = new RendererLayer
                                         {
                                             Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                                             Settings = new RendererLayerSettings {Clip = true},
                                             Renderer = new CoordUnitGridRenderer
                                                            {
                                                                Source = Source,
                                                                LineColor = Color,
                                                                LineStyle = LineStyle.Dot,
                                                                LineWidth = 1,
                                                                TapePosition = _tapeModel.TapePosition,
                                                                Translator = translator,
                                                                Mask = new[] {0.1f, 0.2f, 0.5f},
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
            positionLayer.Add(smallUnitGridLayer);

            var interruptLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new CoordInterruptGridRenderer
                {
                    Source = Source,
                    LineColor = Color,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 3,
                    TapePosition = _tapeModel.TapePosition,
                    Translator = translator,
                    Filter = ic => true
                }
            };
            positionLayer.Add(interruptLayer);


        }
    }
}
