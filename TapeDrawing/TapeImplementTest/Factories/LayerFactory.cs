using System;
using System.IO;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement;
using TapeImplement.CoordGridRenderers;
using TapeImplement.ObjectRenderers;
using TapeImplement.ObjectRenderers.Signals;
using TapeImplement.SimpleRenderers;
using TapeImplementTest.SourceImplement;

namespace TapeImplementTest.Factories
{
    /// <summary>
    /// Создает набор слоев для отображения в тесте
    /// </summary>
    public class LayerFactory : ILayerFactory
    {
        /// <summary>
        /// Источник данных координат
        /// </summary>
        public TestCoordinateSource Source { get; set; }
        /// <summary>
        /// Диапазон рисования
        /// </summary>
        public IScalePosition<int> TapePosition { get; set; }

        public TestParams TestParams { get; set; }

        public void Create(ILayer mainLayer)
        {
            mainLayer.Add(CreateBackgroundLayer());

            var coordGridLayer = CreateLayer(0.01f, 0.99f, 0.67f, 0.99f, LineStyle.Dash,
                                             new Color(100, 100, 255));
            mainLayer.Add(coordGridLayer);
            CreateCoordLayers(coordGridLayer);

            var signalLayer = CreateLayer(0.01f, 0.99f, 0.34f, 0.65f, LineStyle.Dash,
                                          new Color(100, 100, 255));
            mainLayer.Add(signalLayer);
			CreateSignalLayers(signalLayer);
			CreateScaleGridLayers(signalLayer);

            var objectLayer = CreateLayer(0.01f, 0.99f, 0.01f, 0.32f, LineStyle.Dash,
                                          new Color(100, 100, 255));
            mainLayer.Add(objectLayer);
            CreateObjectLayers(objectLayer);
        }

        private void CreateCoordLayers(ILayer parentLayer)
        {
            const float left = 0.01f;
            const float right = 0.99f;

            // Большие штрихи
            var gridRendererBig = CreateCoordUnitGridRenderer(null, new[] { 0.5f }, 100, 2,
                                                              new Color(255, 0, 0));
            var textRendererBig = CreateCoordUnitTextRenderer(null, new[] { 0.5f }, 100, null, 9, FontStyle.Bold,
                                                              Alignment.None, 0, "м");
            parentLayer.Add(CreateLayer(gridRendererBig, left, right, 0.75f, 0.9f, LineStyle.Dot,
                                      new Color(100, 0, 255, 0)));
            parentLayer.Add(CreateLayer(textRendererBig, left, right, 0.71f, 0.73f, LineStyle.Dot,
                                      new Color(100, 0, 255, 0)));

            // Средние штрихи
            var gridRendererMid = CreateCoordUnitGridRenderer(new CoordUnitBaseRenderer[] { gridRendererBig },
                                                              new[] { 0.1f, 0.5f }, 100, 1,
                                                              new Color(0, 0, 0));
            var textRendererMid = CreateCoordUnitTextRenderer(new CoordUnitBaseRenderer[] { textRendererBig },
                                                              new[] { 0.1f, 0.5f }, 100, null, 8,
                                                              FontStyle.Bold, Alignment.None, 0, null);
            parentLayer.Add(CreateLayer(gridRendererMid, left, right, 0.75f, 0.9f, LineStyle.Dot,
                                      new Color(100, 0, 255, 0)));
            parentLayer.Add(CreateLayer(textRendererMid, left, right, 0.71f, 0.73f, LineStyle.Dot,
                                      new Color(100, 0, 255, 0)));

            // Маленькие штрихи
            var gridRendererSmall = CreateCoordUnitGridRenderer(new CoordUnitBaseRenderer[] { gridRendererBig, gridRendererMid },
                                                                new[] { 0.1f, 0.2f, 0.5f }, 20, 1,
                                                                new Color(0, 0, 0));
            var textRendererSmall = CreateCoordUnitTextRenderer(new CoordUnitBaseRenderer[] { textRendererBig, textRendererMid },
                                                                new[] { 0.1f, 0.2f, 0.5f }, 20, null, 5,
                                                                FontStyle.None, Alignment.Left, 90, null);
            parentLayer.Add(CreateLayer(gridRendererSmall, left, right, 0.75f, 0.9f, LineStyle.Dot,
                                      new Color(100, 0, 255, 0)));
            parentLayer.Add(CreateLayer(textRendererSmall, left, right, 0.73f, 0.75f, LineStyle.Dot,
                                      new Color(100, 0, 255, 0)));


            // Отметки начала и конца
            Predicate<ICoordInterrupt> filter = ci => (ci.Index == 0) || (ci.Index == TestParams.IndexLen);
            var interruptGrid1 = CreateCoordInterruptGridRenderer(filter, new Color(0, 0, 255));
            var interruotText1 = CreateCoordInterruptTextRenderer(1, filter, null,
                                                                  new Color(0, 0, 255));
            parentLayer.Add(CreateLayer(interruptGrid1, left, right, 0.35f, 0.9f, LineStyle.Dot,
                                      new Color(100, 255, 0, 0)));
            parentLayer.Add(CreateLayer(interruotText1, left, right, 0.34f, 0.35f, LineStyle.Dot,
                                      new Color(100, 255, 0, 0)));


            // Отметки по центру
            Predicate<ICoordInterrupt> filter2 = ci => (ci.Index > 0) && (ci.Index < TestParams.IndexLen);
            var interruptGrid2 = CreateCoordInterruptGridRenderer(filter2, new Color(30, 170, 255));
            var interruotText2 = CreateCoordInterruptTextRenderer(30, filter2, null,
                                                                  new Color(30, 170, 255));
            parentLayer.Add(CreateLayer(interruptGrid2, left, right, 0.35f, 0.9f, LineStyle.Dot,
                                      new Color(100, 255, 0, 0)));
            parentLayer.Add(CreateLayer(interruotText2, left, right, 0.34f, 0.35f, LineStyle.Dot,
                                      new Color(100, 255, 0, 0)));

            // Текст
            var text = CreateTextRenderer("--- Линейка ----", "Arial black", 14, FontStyle.Bold,
                                          new Color(100, 255, 30, 30), 0, Alignment.Bottom, Alignment.None);
            parentLayer.Add(CreateLayer(text, 0, 1, 0, 1, LineStyle.Solid, new Color(0, 0, 0, 0)));
        }
        private void CreateSignalLayers(ILayer parentLayer)
        {
            var signalRenderer = CreateSignalRenderer(1, new Color(200, 40, 180), LineStyle.Solid);
            var signalLayer = CreateLayer(signalRenderer, 0.01f, 0.99f, 0.66f, 0.99f, LineStyle.Dot,
                                          new Color(100, 255, 0, 0));
            parentLayer.Add(signalLayer);

            var signalPointRenderer = CreateSignalPointRenderer(1, new Color(200, 40, 180), LineStyle.Solid);
            var signalPointLayer = CreateLayer(signalPointRenderer, 0.01f, 0.99f, 0.33f, 0.65f, LineStyle.Dot,
                                          new Color(100, 255, 0, 0));
            parentLayer.Add(signalPointLayer);


            // Текст
            var text = CreateTextRenderer("~ СИГНАЛ ~", "Arial black", 14, FontStyle.Bold | FontStyle.Italic,
                                          new Color(100, 15, 100, 100), 0, Alignment.None, Alignment.None);
            parentLayer.Add(CreateLayer(text, 0, 1, 0, 1, LineStyle.Solid, new Color(0, 0, 0, 0)));
        }
        private void CreateObjectLayers(ILayer parentLayer)
        {
            var pointObjectRenderer = CreatePointObjectRenderer(Alignment.Left);
            var pointObjectLayer = CreateLayer(pointObjectRenderer, 0.01f, 0.99f, 0.51f, 0.99f, LineStyle.Dot,
                                          new Color(100, 255, 0, 0));
            parentLayer.Add(pointObjectLayer);

            var regionObjectRenderer = CreateRegionObjectRenderer();
            var regionObjectLayer = CreateLayer(regionObjectRenderer, 0.01f, 0.99f, 0.01f, 0.49f, LineStyle.Dot,
                                          new Color(100, 255, 0, 0));
            parentLayer.Add(regionObjectLayer);

            // Текст
            var text = CreateTextRenderer("** ОБЪЕКТЫ **", "Arial black", 14, FontStyle.Bold | FontStyle.Italic,
                                          new Color(100, 70, 200, 120), 0, Alignment.None, Alignment.None);
            parentLayer.Add(CreateLayer(text, 0, 1, 0, 1, LineStyle.Solid, new Color(0, 0, 0, 0)));
        }
		private static void CreateScaleGridLayers(ILayer parentLayer)
		{
			var scaleGridRenderer = CreateScaleGridRenderer(new float[] {-100, -50, 0, 50, 100}, -100, 100, 7,
			                                                new Color(150, 0, 0, 0), LineStyle.Solid);
			var scaleGridLayer = CreateLayer(scaleGridRenderer, 0.01f, 0.99f, 0.01f, 0.99f, LineStyle.Solid,
			                                 new Color(0, 0, 0, 0));
			parentLayer.Add(scaleGridLayer);

			var scaleTextRenderer = CreateScaleTextRenderer(new float[] {-100, -50, 0, 50, 100}, -100, 100, "Arial", 6,
			                                                FontStyle.Bold, Alignment.Left,
			                                                new Color(255, 255, 60), 0);
			var scaleTextLayer = CreateLayer(scaleTextRenderer, 0.01f, 0.02f, 0.01f, 0.99f, LineStyle.Solid,
											 new Color(0, 0, 0, 0));
			parentLayer.Add(scaleTextLayer);
		}

    	private static ILayer CreateLayer(float left, float right, float bottom, float top, LineStyle borderLineStyle, Color borderColor)
        {
            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(left, right, bottom, top),
                Renderer =
                    new BorderRenderer
                    {
                        Color = borderColor,
                        Left = true,
                        Right = true,
                        Top = true,
                        Bottom = true,
                        LineStyle = borderLineStyle,
                        LineWidth = 1
                    }
            };
        }
        private static ILayer CreateLayer(IRenderer renderer, float left, float right, float bottom, float top, LineStyle borderLineStyle, Color borderColor)
        {
            var layer = CreateLayer(left, right, bottom, top, borderLineStyle, borderColor);
            layer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = renderer
            });
            return layer;
        }
        private static ILayer CreateBackgroundLayer()
        {
            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new FillAllRenderer(new Color(255, 255, 255))
            };
        }

        private static TextRenderer CreateTextRenderer(string text, string fontName, int size, FontStyle style, Color color, float angle, Alignment textAlignment, Alignment layerAlignment)
        {
            return new TextRenderer
            {
                Text = text,
                FontName = fontName,
                Size = size,
                Style = style,
                Color = color,
                Angle = angle,
                TextAlignment = textAlignment,
                LayerAlignment = layerAlignment
            };
        }
        private CoordUnitTextRenderer CreateCoordUnitTextRenderer(CoordUnitBaseRenderer[] hiRenderers, float[] mask, int minPixelsDistance, Predicate<float> filter, int size, FontStyle fontStyle, Alignment alignment, int angle, string unit)
        {
            return new CoordUnitTextRenderer
            {
                TapePosition = TapePosition,
                Source = Source,
                Mask = mask,
                MinPixelsDistance = minPixelsDistance,
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                FontName = "Arial",
                FontSize = size,
                FontStyle = fontStyle,
                DecreaseAlignment = alignment,
                IncreaseAlignment = alignment,
                Angle = angle,
                TextFormatString = "0.####",
                Color = new Color(0, 0, 0),
                ValueFilter = filter,
                PriorityRenderers = hiRenderers,
                Unit = unit
            };
        }
        private CoordUnitGridRenderer CreateCoordUnitGridRenderer(CoordUnitBaseRenderer[] hiRenderers, float[] mask, int minPixelsDistance, int lineWidth, Color lineColor)
        {
            return new CoordUnitGridRenderer
            {
                TapePosition = TapePosition,
                Source = Source,
                Mask = mask,
                LineColor = lineColor,
                LineStyle = LineStyle.Solid,
                LineWidth = lineWidth,
                MinPixelsDistance = minPixelsDistance,
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                PriorityRenderers = hiRenderers
            };
        }
        private CoordInterruptGridRenderer CreateCoordInterruptGridRenderer(Predicate<ICoordInterrupt> filter, Color color)
        {
            return
                new CoordInterruptGridRenderer
                {
                    TapePosition = TapePosition,
                    Source = Source,
                    LineColor = color,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    Filter = filter
                };
        }
        private CoordInterruptTextRenderer CreateCoordInterruptTextRenderer(int minPixelsDistance, Predicate<ICoordInterrupt> filter, Predicate<ICoordInterrupt> hiFilter, Color color)
        {
            return new CoordInterruptTextRenderer
            {
                TapePosition = TapePosition,
                Source = Source,
                MinPixelsDistance = minPixelsDistance,
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                FontName = "Arial",
                FontSize = 8,
                FontStyle = FontStyle.None,
                Alignment = Alignment.Left,
                Angle = 90,
                TextFormatString = "0.####",
                Color = color,
                Filter = filter,
                PriorityFilter = hiFilter
            };
        }
        private SignalRenderer CreateSignalRenderer(int lineWidth, Color lineColor, LineStyle lineStyle)
        {
            var scaleDiapazone = new SimpleScalePosition<float>();
            scaleDiapazone.Set(-100,100);
            

            return new SignalRenderer
            {
                TapePosition = TapePosition,
                Diapazone = scaleDiapazone,
                LineWidth = lineWidth,
                LineColor = lineColor,
                LineStyle = lineStyle,
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                Source = Source
            };
        }
        private SignalPointRenderer CreateSignalPointRenderer(int lineWidth, Color lineColor, LineStyle lineStyle)
        {
            var scaleDiapazone = new SimpleScalePosition<float>();
            scaleDiapazone.Set(-100, 100);

            return new SignalPointRenderer
            {
                TapePosition = TapePosition,
                Diapazone = scaleDiapazone,
                LineWidth = lineWidth,
                LineColor = lineColor,
                LineStyle = lineStyle,
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                Source = Source
            };
        }
        private PointObjectRenderer<PointObject> CreatePointObjectRenderer(Alignment alignment)
        {
            var ms = new MemoryStream();
            Resource1.flag_red.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return new PointObjectRenderer<PointObject>
            {
                Image = ms,
                Angle = 0,
                TapePosition = TapePosition,
                Source = Source.As<PointObject>(),
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                Alignment = alignment
            };
        }
        private RegionObjectRenderer<Region<RegionObject>> CreateRegionObjectRenderer()
        {
            var ms = new MemoryStream();
            Resource1.kolobok.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return new RegionObjectRenderer<Region<RegionObject>>
            {
                Image = ms,
                Angle = 0,
                Alignment = Alignment.Left,
                TapePosition = TapePosition,
                Source = Source.As<Region<RegionObject>>(),
                Translator = PointTranslatorConfigurator.CreateLinear().Translator
            };
        }
		private static ScaleGridRenderer CreateScaleGridRenderer(float[] values, float min, float max, int lineWidth, Color lineColor, LineStyle lineStyle)
		{
			return new ScaleGridRenderer
			       	{
			       		Translator = PointTranslatorConfigurator.CreateLinear().Translator,
			       		Values = values,
			       		GetMin = ()=> min,
			       		GetMax = ()=> max,
			       		LineWidth = lineWidth,
			       		LineColor = lineColor,
			       		LineStyle = lineStyle
			       	};
		}
		private static ScaleTextRenderer CreateScaleTextRenderer(float[] values, float min, float max, string fontName, int fontSize, FontStyle fontStyle, Alignment alignment, Color color, float angle)
		{
			return new ScaleTextRenderer
			       	{
			       		Translator = PointTranslatorConfigurator.CreateLinear().Translator,
			       		TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
			       		Values = values,
			       		Min = min,
			       		Max = max,
			       		FontName = fontName,
			       		FontSize = fontSize,
			       		FontStyle = fontStyle,
			       		Alignment = alignment,
			       		Color = color,
			       		Angle = angle,
			       	};
		}
    }
}
