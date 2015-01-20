
using System;
using ComparativeTapeTest.Tapes.Types;
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

namespace ComparativeTapeTest.Tapes.CurvePanelLayers
{
    class GraphLayerFactory
    {
        public int BottomAreaHeight { get; set; }
        public int TopAreaHeight { get; set; }

        public int TextSize { get; set; }

        public IScalePosition<int> TapePosition { get; set; }

        public IScalePosition<float> Diapazone { get; set; }
        

        public ILayer MainLayer { get; private set; }
        public GraphLayerFactory Start()
        {
            MainLayer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0) };

            //фон
            MainLayer.Add(new RendererLayer
            {
                Area =
                    AreasFactory.CreateRelativeArea(0f, 1f, 0f, 1f),
                Renderer = new FillRenderer(new Color(255, 255, 255))
            });

            //рамка
            MainLayer.Add(new RendererLayer
                              {
                                  Area =
                                      AreasFactory.CreateRelativeArea(0f, 1f, 0f, 1f),
                                  Renderer = new BorderRenderer
                                                 {
                                                     Bottom = true,
                                                     Top = true,
                                                     Left = true,
                                                     Right = true,
                                                     Color = new Color(0, 0, 0),
                                                     LineWidth = 1
                                                 }
                              });
            
            return this;
        }

        
        /// <summary>
        /// Создает слой с табличкой.
        /// </summary>
        /// <returns></returns>
        public GraphLayerFactory CreateDataTableLayer(string[,] tableData)
        {
            //область таблички
            var areaLayer = new EmptyLayer
                                {
                                    Area = AreasFactory.CreateMarginsArea(0, 0, null, 0, 0, TopAreaHeight)
                                };
            MainLayer.Add(areaLayer);

            //слой текста
            areaLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new TableTextRenderer
                {
                    TableData = tableData,
                    Color = new Color(0, 0, 0),
                    FontName = "Arial",
                    Size = TextSize
                }
            });

            //рамка
            var columnsCount = tableData.GetLength(1);
            for (var c = 0; c < columnsCount; c++)
                areaLayer.Add(new RendererLayer
                {
                    Area =
                        AreasFactory.CreateRelativeArea((float)c / columnsCount,
                                                        (float)(c + 1) / columnsCount, 0, 1),
                    Renderer = new BorderRenderer
                    {
                        Bottom = true,
                        Top = true,
                        Left = true,
                        Right = true,
                        Color = new Color(0, 0, 0),
                        LineWidth = 1
                    }
                });

            return this;
        }

        /// <summary>
        /// Создает слой с графиком.
        /// </summary>
        /// <returns></returns>
        public GraphLayerFactory CreateGraphLayer(IIntegratedSignalSource signalSource, string infoText)
        {
            //область графика
            var areaLayer = new EmptyLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, BottomAreaHeight,TopAreaHeight)
            };
            MainLayer.Add(areaLayer);
            
            //график с переключением рендерера при большом кол-ве точек на пиксель
            areaLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new SwitchRenderer<int>
                {
                    ScalePosition = TapePosition,
                    IsHorizontal = true,
                    SwitchValue = 1f,
                    OriginalStep = signalSource.Step,
                    BeforeDraw = signalSource.SetWindowSize,
                    LowDensityRenderer = new SignalRenderer
                    {
                        Source = signalSource,
                        Diapazone = Diapazone,
                        Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                        TapePosition = TapePosition,
                        LineColor = new Color(255,0,0),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 1
                    },
                    HighDensityRenderer = new SignalRenderer
                    {
                        Source = signalSource,
                        Diapazone = Diapazone,
                        Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                        TapePosition = TapePosition,
                        LineColor = new Color(255, 0, 0),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 1
                    }
                }
            });

            //информация о графике в правом верхнем углу
            areaLayer.Add(new RendererLayer
                              {
                                  Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                                  Renderer = new TextRenderer
                                                 {
                                                     Color = new Color(0, 0, 0),
                                                     Angle = 0,
                                                     FontName = "Arial",
                                                     LayerAlignment = Alignment.Right | Alignment.Top,
                                                     Size = TextSize,
                                                     Style = FontStyle.None,
                                                     Text = infoText,
                                                     TextAlignment = Alignment.Right | Alignment.Top
                                                 }
                              });

            return this;
        }

        /// <summary>
        /// Создает слой со шкалой.
        /// </summary>
        /// <returns></returns>
        public GraphLayerFactory CreateScaleLayer(float[] scaleValues)
        {
            //шкала
            MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(-50, null, BottomAreaHeight, TopAreaHeight, 95, 0),
                Renderer = new ScaleTextRenderer
                {
                    Alignment = Alignment.Right,
                    Angle = 0, 
                    Color = new Color(0,0,0),
                    FontSize = TextSize,
                    FontStyle = FontStyle.None,
                    FontName = "Arial",
                    Max = Diapazone.To,
                    Min = Diapazone.From,
                    TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                    TextFormatString = "",
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    Unit = "",
                    Values = scaleValues
                }
            });
            
            //горизонтальные линии сетки
            MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, BottomAreaHeight, TopAreaHeight),
                Renderer = new ScaleGridRenderer
                {
                    LineColor = new Color(220, 220, 220),
                    LineStyle = LineStyle.Dash,
                    LineWidth = 1,
                    GetMax = ()=> Diapazone.To,
                    GetMin = ()=>Diapazone.From,
                    Values = scaleValues,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator
                }
            });

            return this;
        }

        public GraphLayerFactory CreatePicketLayer<T>(IObjectSource<T> pickets, Func<T, string> presenter) 
            where T:Region
        {
            //область пикетов
            var areaLayer = new EmptyLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, null, 0, BottomAreaHeight)
            };
            MainLayer.Add(areaLayer);

            areaLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Settings = new RendererLayerSettings{Clip = true},
                Renderer = new RegionBoardsRenderer<T>
                {
                    Source = pickets,
                    LineColor = new Color(0,0,0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator 
                }
            });

            areaLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RegionTextRenderer<T>
                {
                    Source = pickets,
                    FontColor = new Color(0, 0, 0),
                    FontName = "Arial",
                    FontSize = TextSize,
                    FontStyle = FontStyle.None,
                    ObjectPresentation = presenter,
                    TapePosition = TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator
                }
            });

            //вертикальные линии сетки
            MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, BottomAreaHeight, TopAreaHeight),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RegionBoardsRenderer<T>
                {
                    Source = pickets,
                    LineColor = new Color(220, 220, 220),
                    LineStyle = LineStyle.Dot,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator
                }
            });

            return this;
        }
    }
}
