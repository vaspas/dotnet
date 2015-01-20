using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ComparativeTapeTest.Generators;
using ComparativeTapeTest.Generators.CoordsGenerator;
using ComparativeTapeTest.Tapes.Images;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement;
using TapeImplement.CoordGridRenderers;
using TapeImplement.MouseListenerLayers;
using TapeImplement.ObjectRenderers;
using TapeImplement.ObjectRenderers.Signals;
using Color = TapeDrawing.Core.Primitives.Color;
using FontStyle = TapeDrawing.Core.Primitives.FontStyle;

namespace ComparativeTapeTest.Tapes
{
    class HorizontalTapeFactory : IMainLayerFactory
    {
        public IScalePosition<int> TapePosition { get; set; }

        public Player Player { get; set; }

        public List<object> DataSources { get; set; }

        private HorizontalTapeSettings _settings=new HorizontalTapeSettings();


        public void Create(ILayer mainLayer)
        {
            var layer = new EmptyLayer
                            {
                                Area = AreasFactory.CreateMarginsArea(10,10,10,10)
                            };

            mainLayer.Add(CreateBackgroundLayer());

            mainLayer.Add(layer);

            layer.Add(CreateCoordRulerLayer());

            layer.Add(CreateObjectsLayer());

            layer.Add(CreateInfoLayer());

            layer.Add(CreateScaleLayer());

            layer.Add(CreateGraphsLayer());
        }

        private ILayer CreateBackgroundLayer()
        {
            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new TapeImplement.SimpleRenderers.FillAllRenderer(
                    new Color(255, 255, 255))
            };
        }


        #region CoordRuler

        private ILayer CreateCoordRulerLayer()
        {
            var layer = new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, _settings.ScalesSize+_settings.InfoSize, 
                null, 0, 
                0, _settings.CoordRulerSize),
                Renderer = new TapeImplement.SimpleRenderers.BorderRenderer
                {
                    Bottom = true,
                    Color = new Color(0, 0, 0),
                    Left = true,
                    LineStyle = LineStyle.Solid,
                    Right = true,
                    Top = true,
                    LineWidth = 1
                }
            };
            
            AddUnits(layer);

            AddInterrupt(layer);

            return layer;
        }

        private void AddUnits(ILayer parent)
        {
            var largeUnitTextLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0.3f, 0.6f),
                Renderer = new TapeImplement.CoordGridRenderers.CoordUnitTextRenderer
                {
                    Source =
                        DataSources.First(
                            s => s is Generators.CoordsGenerator.CoordSource) as
                        TapeImplement.CoordGridRenderers.ICoordinateSource,
                    DecreaseAlignment = Alignment.None,
                    IncreaseAlignment = Alignment.None,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 150,
                    PriorityRenderers = new CoordUnitBaseRenderer[] { },
                    Angle = 0,
                    FontName = "Arial",
                    Color = new Color(0, 0, 0),
                    FontSize = 12,
                    FontStyle = FontStyle.None,
                    TextFormatString = string.Empty
                }
            };
            parent.Add(largeUnitTextLayer);

            var largeUnitGridLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 0.3f),
                Settings = new RendererLayerSettings{Clip = true},
                Renderer = new CoordUnitGridRenderer
                {
                    Source =
                        DataSources.First(
                            s => s is Generators.CoordsGenerator.CoordSource) as
                        ICoordinateSource,
                    LineColor = new Color(0, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 3,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 150,
                    PriorityRenderers = new CoordUnitBaseRenderer[] { }
                }
            };
            parent.Add(largeUnitGridLayer);

            var mediumUnitGridLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 0.2f),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source =
                        DataSources.First(
                            s => s is Generators.CoordsGenerator.CoordSource) as
                        ICoordinateSource,
                    LineColor = new Color(0, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 2,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 70,
                    PriorityRenderers = new[] { largeUnitGridLayer.Renderer as CoordUnitBaseRenderer }
                }
            };
            parent.Add(mediumUnitGridLayer);

            var smallUnitGridLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 0.1f),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source =
                        DataSources.First(
                            s => s is Generators.CoordsGenerator.CoordSource) as
                        ICoordinateSource,
                    LineColor = new Color(0, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Mask = new float[] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 14,
                    PriorityRenderers = new[] { largeUnitGridLayer.Renderer as CoordUnitBaseRenderer, mediumUnitGridLayer.Renderer as CoordUnitBaseRenderer }
                }
            };
            parent.Add(smallUnitGridLayer);
        }

        private void AddInterrupt(ILayer parent)
        {
           var interruptLayer= new RendererLayer
           {
               Area = AreasFactory.CreateRelativeArea(0, 1, 0, 0.5f),
               Renderer = new CoordInterruptGridRenderer
               {
                   Source = DataSources.First(s => s is CoordSource) as ICoordinateSource,
                   LineColor = new Color(0,0,0),
                   LineStyle = LineStyle.Solid,
                   LineWidth = 3,
                   TapePosition = TapePosition,
                   Translator = TapeDrawing.Core.Translators.PointTranslatorConfigurator.CreateLinear().Translator,
                   Filter = ic=>true
               }
           };
           parent.Add(interruptLayer);
            
           var interruptATextLayer = new RendererLayer
           {
               Area = AreasFactory.CreateRelativeArea(0, 1, 0.5f, 1),
               Renderer = new CoordInterruptTextRenderer
               {
                   Source = DataSources.First(s => s is CoordSource) as ICoordinateSource,
                   Alignment = Alignment.None,
                   TapePosition = TapePosition,
                   Translator = TapeDrawing.Core.Translators.PointTranslatorConfigurator.CreateLinear().Translator,
                   Filter = ic => ic is InterruptA,
                   Angle = 0,
                   Color = new Color(255, 0, 0),
                   FontName = "Arial",
                   FontSize = 12,
                   FontStyle = FontStyle.Bold,
                   MinPixelsDistance = 30,
                   PriorityFilter = ci=>false,
                   TextAlignmentTranslator = TapeDrawing.Core.Translators.AlignmentTranslatorConfigurator.Create().Translator,
                   TextFormatString = string.Empty
               }
           };
           parent.Add(interruptATextLayer);
           
           var interruptBTextLayer = new RendererLayer
           {
               Area = AreasFactory.CreateRelativeArea(0, 1, 0.5f, 1),
               Renderer = new CoordInterruptTextRenderer
               {
                   Source = DataSources.First(s => s is CoordSource) as ICoordinateSource,
                   Alignment = Alignment.None,
                   TapePosition = TapePosition,
                   Translator = TapeDrawing.Core.Translators.PointTranslatorConfigurator.CreateLinear().Translator,
                   Filter = ic => ic is InterruptB,
                   Angle = 0,
                   Color = new Color(0, 0, 255),
                   FontName = "Arial",
                   FontSize = 12,
                   FontStyle = FontStyle.Bold,
                   MinPixelsDistance = 30,
                   PriorityFilter = ci => ci is InterruptA,
                   TextAlignmentTranslator = TapeDrawing.Core.Translators.AlignmentTranslatorConfigurator.Create().Translator,
                   TextFormatString = string.Empty
               }
           };
           parent.Add(interruptBTextLayer);
        }

        #endregion

        #region Info

        private ILayer CreateInfoLayer()
        {
            var layer = new RendererLayer
                            {
                                Area = AreasFactory.CreateMarginsArea(null, 0,
                                                                      0,
                                                                      _settings.CoordRulerSize + _settings.ObjectsSize,
                                                                      _settings.InfoSize, 0),
                                Renderer = new TapeImplement.SimpleRenderers.BorderRenderer
                                               {
                                                   Bottom = true,
                                                   Color = new Color(0, 0, 0),
                                                   Left = true,
                                                   LineStyle = LineStyle.Solid,
                                                   Right = true,
                                                   Top = true,
                                                   LineWidth = 1
                                               }
                            };

            AddGraphsInfo(layer);

            return layer;
        }

        private void AddGraphsInfo(ILayer parent)
        {
            var levelBorder = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0,1,1-_settings.LevelSize,1),
                Renderer = new TapeImplement.SimpleRenderers.BorderRenderer
                {
                    Bottom = true,
                    Color = new Color(0, 0, 0),
                    Left = true,
                    LineStyle = LineStyle.Solid,
                    Right = true,
                    Top = true,
                    LineWidth = 1
                }
            };
            parent.Add(levelBorder);

            var levelInfo = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 1 - _settings.LevelSize, 1),
                Renderer = new TapeImplement.SimpleRenderers.TextRenderer
                {
                    Angle = 90,
                    Color = new Color(0, 0, 0),
                    FontName = "Arial",
                    LayerAlignment = Alignment.None,
                    Size = 10,
                    Style = FontStyle.None,
                    Text = "Уровень(мм)",
                    TextAlignment = Alignment.None
                }
            };
            parent.Add(levelInfo);

            var trackBorder = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, _settings.TrackSize),
                Renderer = new TapeImplement.SimpleRenderers.BorderRenderer
                {
                    Bottom = true,
                    Color = new Color(0, 0, 0),
                    Left = true,
                    LineStyle = LineStyle.Solid,
                    Right = true,
                    Top = true,
                    LineWidth = 1
                }
            };
            parent.Add(trackBorder);

            var trackInfo = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, _settings.TrackSize),
                Renderer = new TapeImplement.SimpleRenderers.TextRenderer
                {
                    Angle = 90,
                    Color = new Color(0, 0, 0),
                    FontName = "Arial",
                    LayerAlignment = Alignment.None,
                    Size = 10,
                    Style = FontStyle.None,
                    Text = "Шаблон(мм)",
                    TextAlignment = Alignment.None
                }
            };
            parent.Add(trackInfo);
        }

        #endregion

        #region Scale

        private ILayer CreateScaleLayer()
        {
            var layer = new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(null, _settings.InfoSize,
                                                      0,
                                                      _settings.CoordRulerSize + _settings.ObjectsSize,
                                                      _settings.ScalesSize, 0),
                Renderer = new TapeImplement.SimpleRenderers.BorderRenderer
                {
                    Bottom = true,
                    Color = new Color(0, 0, 0),
                    Left = true,
                    LineStyle = LineStyle.Solid,
                    Right = true,
                    Top = true,
                    LineWidth = 1
                }
            };

            var levelLayer = new EmptyLayer
            {Area = AreasFactory.CreateRelativeArea(0, 1, 1 - _settings.LevelSize, 1)};
            layer.Add(levelLayer);

            var trackLayer = new EmptyLayer
            { Area = AreasFactory.CreateRelativeArea(0, 1, 0, _settings.TrackSize) };
            layer.Add(trackLayer);

            AddLevelScale(levelLayer);
            AddTrackScale(trackLayer);

            return layer;
        }

        private void AddLevelScale(ILayer parent)
        {
            var grid1Layer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new ScaleTextRenderer
                {
                    Max = _settings.LevelToValue,
                    Min = _settings.LevelFromValue,
                    Alignment = Alignment.None,
                    Angle = 90,
                    Color = new Color(0, 0, 0),
                    FontName = "Arial",
                    FontSize = 7,
                    FontStyle = FontStyle.None,
                    TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    Values = GetValues(_settings.LevelFromValue, _settings.LevelToValue, 50),
                    TextFormatString = "N0",
                    Unit = string.Empty
                }
            };
            parent.Add(grid1Layer);
        }

        private void AddTrackScale(ILayer parent)
        {
            var grid1Layer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new ScaleTextRenderer
                {
                    Max = _settings.TrackToValue,
                    Min = _settings.TrackFromValue,
                    Alignment = Alignment.None,
                    Angle = 90,
                    Color = new Color(0, 0, 0),
                    FontName = "Arial",
                    FontSize = 7,
                    FontStyle = FontStyle.None,
                    TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    Values = GetValues(_settings.TrackFromValue, _settings.TrackToValue,1520, 20),
                    TextFormatString = "N0",
                    Unit = string.Empty
                }
            };
            parent.Add(grid1Layer);
        }

        #endregion

        #region Objects

        private ILayer CreateObjectsLayer()
        {
            var layer = new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, _settings.InfoSize+_settings.ScalesSize,
                                                      null,
                                                      _settings.CoordRulerSize,
                                                      0, _settings.ObjectsSize),
                Renderer = new TapeImplement.SimpleRenderers.BorderRenderer
                {
                    Bottom = true,
                    Color = new Color(0, 0, 0),
                    Left = true,
                    LineStyle = LineStyle.Solid,
                    Right = true,
                    Top = true,
                    LineWidth = 1
                }
            };

            return layer;
        }

        #endregion

        #region Graphs

        private ILayer CreateGraphsLayer()
        {
            var layer = new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, _settings.InfoSize + _settings.ScalesSize,
                                                      0,
                                                      _settings.CoordRulerSize+_settings.ObjectsSize),
                Renderer = new TapeImplement.SimpleRenderers.BorderRenderer
                {
                    Bottom = true,
                    Color = new Color(0, 0, 0),
                    Left = true,
                    LineStyle = LineStyle.Solid,
                    Right = true,
                    Top = true,
                    LineWidth = 1
                }
            };

            AddTapeRefPositionCursor(layer);

            var levelLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0,1,1-_settings.LevelSize,1),
                Renderer = new TapeImplement.SimpleRenderers.BorderRenderer
                {
                    Bottom = true,
                    Color = new Color(0, 0, 0),
                    Left = true,
                    LineStyle = LineStyle.Solid,
                    Right = true,
                    Top = true,
                    LineWidth = 1
                }
            };
            layer.Add(levelLayer);

            var trackLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, _settings.TrackSize),
                Renderer = new TapeImplement.SimpleRenderers.BorderRenderer
                {
                    Bottom = true,
                    Color = new Color(0, 0, 0),
                    Left = true,
                    LineStyle = LineStyle.Solid,
                    Right = true,
                    Top = true,
                    LineWidth = 1
                }
            };
            layer.Add(trackLayer);

            AddCoordGrid(levelLayer);
            AddCoordGrid(trackLayer);

            AddLevelGrid(levelLayer);
            AddTrackGrid(trackLayer);

            AddLevelNullLines(levelLayer);
            AddTrackNullLines(trackLayer);

            AddLevelSignal(levelLayer);
            AddTrackSignal(trackLayer);

            return layer;
        }

        private void AddCoordGrid(ILayer parent)
        {
            var largeUnitGridLayer = new RendererLayer
                                         {
                                             Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                                             Settings = new RendererLayerSettings { Clip = true },
                                             Renderer = new CoordUnitGridRenderer
                                                            {
                                                                Source =
                                                                    DataSources.First(
                                                                        s => s is Generators.CoordsGenerator.CoordSource)
                                                                    as
                                                                    ICoordinateSource,
                                                                LineColor = new Color(220, 220, 220),
                                                                LineStyle = LineStyle.Solid,
                                                                LineWidth = 2,
                                                                TapePosition = TapePosition,
                                                                Translator =
                                                                    TapeDrawing.Core.Translators.
                                                                    PointTranslatorConfigurator.
                                                                    CreateLinear().Translator,
                                                                Mask = new[] {0.1f, 0.5f},
                                                                MinPixelsDistance = 150,
                                                                PriorityRenderers = new CoordUnitBaseRenderer[] {}
                                                            }
                                         };
            parent.Add(largeUnitGridLayer);

            var mediumUnitGridLayer = new RendererLayer
                                          {
                                              Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                                              Settings = new RendererLayerSettings { Clip = true },
                                              Renderer = new CoordUnitGridRenderer
                                                             {
                                                                 Source =
                                                                     DataSources.First(
                                                                         s =>
                                                                         s is CoordSource) as
                                                                     ICoordinateSource,
                                                                 LineColor =
                                                                     new Color(220, 220, 220),
                                                                 LineStyle = LineStyle.Solid,
                                                                 LineWidth = 1,
                                                                 TapePosition = TapePosition,
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
            parent.Add(mediumUnitGridLayer);

            var smallUnitGridLayer = new RendererLayer
                                         {
                                             Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                                             Settings = new RendererLayerSettings { Clip = true },
                                             Renderer = new CoordUnitGridRenderer
                                                            {
                                                                Source =
                                                                    DataSources.First(
                                                                        s => s is Generators.CoordsGenerator.CoordSource)
                                                                    as
                                                                    ICoordinateSource,
                                                                LineColor = new Color(220, 220, 220),
                                                                LineStyle = LineStyle.Dot,
                                                                LineWidth = 1,
                                                                TapePosition = TapePosition,
                                                                Translator =
                                                                    TapeDrawing.Core.Translators.
                                                                    PointTranslatorConfigurator.
                                                                    CreateLinear().Translator,
                                                                Mask = new float[] {0.1f, 0.2f, 0.5f},
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
            parent.Add(smallUnitGridLayer);

            var interruptLayer = new RendererLayer
                                     {
                                         Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                                         Renderer = new CoordInterruptGridRenderer
                                                        {
                                                            Source =
                                                                DataSources.First(s => s is CoordSource) as
                                                                ICoordinateSource,
                                                            LineColor = new Color(220, 220, 220),
                                                            LineStyle = LineStyle.Solid,
                                                            LineWidth = 3,
                                                            TapePosition = TapePosition,
                                                            Translator =
                                                                TapeDrawing.Core.Translators.PointTranslatorConfigurator
                                                                .CreateLinear().Translator,
                                                            Filter = ic => true
                                                        }
                                     };
            parent.Add(interruptLayer);
        }

        private void AddLevelNullLines(ILayer parent)
        {
            var scaleDiapazone = new SimpleScalePosition<float>();
            scaleDiapazone.Set(_settings.LevelFromValue, _settings.LevelToValue);

            var nullLineLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new TapeImplement.ObjectRenderers.Signals.SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id=="NullLineLevel")
                        as
                        ISignalPointSource,
                    LineColor = new Color(0, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                        Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayer);

            
            var nullLineMarksLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointMarksRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevel")
                        as
                        ISignalPointSource,
                    ImageStream = Provider.GetStream("mark"),
                    TapePosition = TapePosition,
                    ImageAlignment = Alignment.Right|Alignment.Top,
                    ImageAngle = 135,
                    Translator =
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineMarksLayer);

            var nullLineLayerExt4m = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevelExt4m")
                        as
                        ISignalPointSource,
                    LineColor = new Color(255, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt4m);

            var nullLineLayerExt3m = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevelExt3m")
                        as
                        ISignalPointSource,
                    LineColor = new Color(255, 0, 255),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt3m);

            var nullLineLayerExt2m = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevelExt2m")
                        as
                        ISignalPointSource,
                    LineColor = new Color(0, 255, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt2m);

            var nullLineLayerExt4p = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevelExt4p")
                        as
                        ISignalPointSource,
                    LineColor = new Color(255, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt4p);

            var nullLineLayerExt3p = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevelExt3p")
                        as
                        ISignalPointSource,
                    LineColor = new Color(255, 0, 255),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt3p);

            var nullLineLayerExt2p = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevelExt2p")
                        as
                        ISignalPointSource,
                    LineColor = new Color(0, 255, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt2p);
        }

        private void AddLevelSignal(ILayer parent)
        {
            var scaleDiapazone = new SimpleScalePosition<float>();
            scaleDiapazone.Set(_settings.LevelFromValue, _settings.LevelToValue);

            var signalLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalSource) && (s as ISourceId).Id == "SignalLevel")
                        as
                        ISignalSource,
                    LineColor = new Color(0, 0, 255),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(signalLayer);
        }

        private void AddLevelGrid(ILayer parent)
        {
            var grid1Layer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new ScaleGridRenderer()
                {
                    GetMax = ()=>_settings.LevelToValue,
                    GetMin = ()=>_settings.LevelFromValue,
                    LineColor = new Color(220,220,220),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Values = GetValues(_settings.LevelFromValue, _settings.LevelToValue, 50)
                }
            };
            parent.Add(grid1Layer);

            var grid2Layer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new ScaleGridRenderer
                {
                    GetMax = ()=>_settings.LevelToValue,
                    GetMin = ()=>_settings.LevelFromValue,
                    LineColor = new Color(220, 220, 220),
                    LineStyle = LineStyle.Dot,
                    LineWidth = 1,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Values = GetValues(_settings.LevelFromValue, _settings.LevelToValue, 10)
                }
            };
            parent.Add(grid2Layer);
        }

        private void AddTrackNullLines(ILayer parent)
        {
            var scaleDiapazone = new SimpleScalePosition<float>();
            scaleDiapazone.Set(_settings.TrackFromValue, _settings.TrackToValue);

            var nullLineLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrack")
                        as
                        ISignalPointSource,
                    LineColor = new Color(0, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                        Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayer);

            var nullLineLayerExt4m = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrackExt4m")
                        as
                        ISignalPointSource,
                    LineColor = new Color(255, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt4m);

            var nullLineLayerExt3m = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrackExt3m")
                        as
                        ISignalPointSource,
                    LineColor = new Color(255, 0, 255),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt3m);

            var nullLineLayerExt2m = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrackExt2m")
                        as
                        ISignalPointSource,
                    LineColor = new Color(0, 255, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt2m);

            var nullLineLayerExt4p = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrackExt4p")
                        as
                        ISignalPointSource,
                    LineColor = new Color(255, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt4p);

            var nullLineLayerExt3p = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrackExt3p")
                        as
                        ISignalPointSource,
                    LineColor = new Color(255, 0, 255),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt3p);

            var nullLineLayerExt2p = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalPointRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrackExt2p")
                        as
                        ISignalPointSource,
                    LineColor = new Color(0, 255, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(nullLineLayerExt2p);
        }

        private void AddTrackSignal(ILayer parent)
        {
            var scaleDiapazone = new SimpleScalePosition<float>();
            scaleDiapazone.Set(_settings.TrackFromValue, _settings.TrackToValue);

            var signalLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new SignalRenderer
                {
                    Source =
                        DataSources.First(
                            s => (s is ISignalSource) && (s as ISourceId).Id == "SignalTrack")
                        as
                        ISignalSource,
                    LineColor = new Color(0, 0, 255),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = TapePosition,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Diapazone = scaleDiapazone
                }
            };
            parent.Add(signalLayer);
        }

        private void AddTrackGrid(ILayer parent)
        {
            var grid1Layer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new ScaleGridRenderer()
                {
                    GetMax = ()=>_settings.TrackToValue,
                    GetMin = ()=>_settings.TrackFromValue,
                    LineColor = new Color(220, 220, 220),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Values = GetValues(_settings.TrackFromValue, _settings.TrackToValue,1520, 20)
                }
            };
            parent.Add(grid1Layer);

            var grid2Layer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new ScaleGridRenderer
                {
                    GetMax = ()=>_settings.TrackToValue,
                    GetMin = ()=>_settings.TrackFromValue,
                    LineColor = new Color(220, 220, 220),
                    LineStyle = LineStyle.Dot,
                    LineWidth = 1,
                    Translator =
                        TapeDrawing.Core.Translators.
                        PointTranslatorConfigurator.
                        CreateLinear().Translator,
                    Values = GetValues(_settings.TrackFromValue, _settings.TrackToValue, 1520, 10)
                }
            };
            parent.Add(grid2Layer);
        }

        private void AddTapeRefPositionCursor(ILayer parent)
        {
            var cursorRenderer = new TapeImplement.MouseListenerLayers.TapeCursor.TapeRefPositionCursorRenderer
            {
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                LineColor = new Color(255, 0, 0),
                LineWidth = 3,
                Position = 0.5f
            };

            parent.Add(new RendererLayer
                           {
                               Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                               Renderer = cursorRenderer
                           });
            /*parent.Add(new MouseListenerLayer
                           {
                               Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                               MouseListener =
                                   new TapeImplement.MouseListenerLayers.TapeRefPositionCursor.
                                   TapeRefPositionCursorMouseClickListener
                                       {
                                           Renderer = cursorRenderer,
                                           Button = MouseButton.Left,
                                           Translator = PointTranslatorConfigurator.Create().Translator,
                                           PositionChanged = () => Player.Redraw()
                                       }
                           });*/
            var pressedListener = new PressedListener
                                      {
                                          Button = MouseButton.Left,
                                          Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                          PositionChanged = (p1, p2) =>
                                                                {
                                                                    cursorRenderer.Position = p2.X;
                                                                    Player.Redraw();
                                                                }
                                      };
            parent.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener = pressedListener
            });
            parent.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener =
                    new TapeImplement.MouseListenerLayers.TapeCursor.
                    TapeRefPositionCursorMouseWheelListener
                    {
                        Renderer = cursorRenderer,
                        PositionChanged = () => Player.Redraw(),
                        Coeff = 1,
                        TapePosition = TapePosition
                    }
            });
            parent.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess =
                    new TapeImplement.MouseListenerLayers.TapeCursor.
                    TapeRefPositionCursorKeyboardKeyListener
                    {
                        Renderer = cursorRenderer,
                        PositionChanged = () => Player.Redraw(),
                        DownKey = KeyboardKey.Down,
                        UpKey = KeyboardKey.Up,
                        TapePosition = TapePosition
                    }
            });
        }

        #endregion

        private float[] GetValues(float from, float to, int step)
        {
            var values = new List<float>();
            var curVal = to - to % step;
            while (curVal > from)
            {
                values.Add(curVal);
                curVal -= step;
            }

            return values.ToArray();
        }

        private float[] GetValues(float from, float to, int start, int step)
        {
            var values = new List<float>();
            var curVal = start;
            while (curVal > from)
            {
                values.Add(curVal);
                curVal -= step;
            }
            curVal = start+step;
            while (curVal < to)
            {
                values.Add(curVal);
                curVal += step;
            }

            values.Sort();
            return values.ToArray();
        }
    }
}
