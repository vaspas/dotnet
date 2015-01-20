using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.CoordGridRenderers;

namespace TapeImplement.TapeModels.Kuges.Track
{
    /// <summary>
    /// Модель дорожки шкалы дистанции.
    /// </summary>
    public class DistScaleTrackModel:BaseTrackModel
    {
        public void AddSource(ICoordinateSource source, FontSettings unitFont, FontSettings interruptFont, Color lineColor)
        {
            AddUnits(source, unitFont, lineColor);
            AddInterrupt(source, interruptFont, lineColor);

            DataLayer.Add(
                 new MouseListenerLayer
                 {
                     Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                     Settings = new MouseListenerLayerSettings { MouseMoveOutside = true },
                     MouseListener =
                         new MouseListenerLayers.LinearScale.ShiftDiapazoneMouseMoveListener<int>
                         {
                             Button = MouseButton.Left,
                             Diapazone = TapeModel.TapePosition,
                             OnChanged = TapeModel.Redraw,
                             Translator =
                                 PointTranslatorConfigurator.CreateLinear().Translator
                         }

                 });

            DataLayer.Add(
                new MouseListenerLayer
                {
                    Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                    Settings = new MouseListenerLayerSettings { MouseMoveOutside = true },
                    MouseListener =
                        new MouseListenerLayers.LinearScale.ZoomDiapazoneMouseWheelListener<int>
                        {
                            Diapazone = TapeModel.TapePosition,
                            OnChanged = TapeModel.Redraw,
                            Factor = 0.1f
                        }

                });
        }
        
        private void AddUnits(ICoordinateSource source, FontSettings font, Color lineColor)
        {
            var largeUnitTextLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0.3f, 0.6f),
                Renderer = new CoordUnitTextRenderer
                {
                    Source = source,
                    IncreaseAlignment = Alignment.None,
                    DecreaseAlignment = Alignment.None,
                    TapePosition = TapeModel.TapePosition,
                    Translator =
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 150,
                    PriorityRenderers = new CoordUnitBaseRenderer[] { },
                    Angle = font.Angle,
                    FontName = font.Name,
                    Color = font.Color,
                    FontSize = font.Size,
                    FontStyle = FontStyle.None,
                    TextFormatString = string.Empty
                }
            };
            DataLayer.Add(largeUnitTextLayer);

            var largeUnitGridLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 0.3f),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source = source,
                    LineColor = lineColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 3,
                    TapePosition = TapeModel.TapePosition,
                    Translator =
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 150,
                    PriorityRenderers = new CoordUnitBaseRenderer[] { }
                }
            };
            DataLayer.Add(largeUnitGridLayer);

            var mediumUnitGridLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 0.2f),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source = source,
                    LineColor = lineColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 2,
                    TapePosition = TapeModel.TapePosition,
                    Translator =
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 70,
                    PriorityRenderers = new[] { largeUnitGridLayer.Renderer as CoordUnitBaseRenderer }
                }
            };
            DataLayer.Add(mediumUnitGridLayer);

            var smallUnitGridLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 0.1f),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source = source,
                    LineColor = lineColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapeModel.TapePosition,
                    Translator =
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new [] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 14,
                    PriorityRenderers = new[] { largeUnitGridLayer.Renderer as CoordUnitBaseRenderer, mediumUnitGridLayer.Renderer as CoordUnitBaseRenderer }
                }
            };
            DataLayer.Add(smallUnitGridLayer);
        }

        private void AddInterrupt(ICoordinateSource source,FontSettings font, Color lineColor)
        {
            var interruptLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 0.5f),
                Renderer = new CoordInterruptGridRenderer
                {
                    Source =source,
                    LineColor = lineColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 3,
                    TapePosition = TapeModel.TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    Filter = ic => true
                }
            };
            DataLayer.Add(interruptLayer);

            var interruptATextLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0.5f, 1),
                Renderer = new CoordInterruptTextRenderer
                {
                    Source = source,
                    Alignment = Alignment.None,
                    TapePosition = TapeModel.TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    Filter = ic => true,
                    Angle = font.Angle,
                    Color = font.Color,
                    FontName = font.Name,
                    FontSize = font.Size,
                    FontStyle = font.Style,
                    MinPixelsDistance = 30,
                    PriorityFilter = ci => false,
                    TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                    TextFormatString = string.Empty
                }
            };
            DataLayer.Add(interruptATextLayer);
        }
    
    }
}
