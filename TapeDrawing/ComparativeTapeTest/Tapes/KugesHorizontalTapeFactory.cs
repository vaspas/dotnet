using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ComparativeTapeTest.Generators;
using ComparativeTapeTest.Generators.CoordsGenerator;
using ComparativeTapeTest.Generators.RegionObjectsGenerator;
using ComparativeTapeTest.Generators.SignalGenerator;
using ComparativeTapeTest.Tapes.CurvePanelLayers;
using TapeDrawing.Core;
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
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.Kuges;
using TapeImplement.TapeModels.Kuges.Track;
using TapeImplement.TapeModels.Kuges.TrackHost;
using FontStyle = TapeDrawing.Core.Primitives.FontStyle;

namespace ComparativeTapeTest.Tapes
{
    class KugesHorizontalTapeFactory : IMainLayerFactory
    {
        public IScalePosition<int> TapePosition { get; set; }

        public Player Player { get; set; }

        public List<object> DataSources { get; set; }
        

        public void Create(ILayer mainLayer)
        {
            var model = new TapeModel
                            {
                                MainLayer = mainLayer,
                                TapePosition = TapePosition,
                                ScaleSize = 40,
                                Redraw = Player.Redraw
                            };
            
            var dist1 = model.CreateTrack<DistScaleTrackModel>(new TrackSizeAbsolute { Value = 60 });
            dist1.AddSource(DataSources.First(s => s is CoordSource) as ICoordinateSource,
                new FontSettings(), new FontSettings{Style = FontStyle.Bold, Angle = 45}, new Color(0,0,0) );

            var scrollHost=model.CreateHost<ScrollTrackHost>(new TrackSizeRelative {Value = 1});
            

            var levelSignal = scrollHost.CreateTrack<DataTrackModel>(new TrackSizeAbsolute { Value = 200 }, true);
            new TapeImplement.TapeModels.Kuges.Extensions.CoordGrid
                {
                    Source = DataSources.First(s => s is CoordSource) as ICoordinateSource,
                    Color = new Color(255, 220, 220)
                }.Build(levelSignal);
            levelSignal.Diapazone.Min = -500;
            levelSignal.Diapazone.Max = 500;
            levelSignal.Diapazone.MinWidth = 10;
            levelSignal.Diapazone.MaxWidth = 1000;
            levelSignal.Position.Set(-200,200);
            levelSignal.InitScale(new FontSettings{Size = 8, Name = "Tahoma"},new Color(0,0,255) );
            new TapeImplement.TapeModels.Kuges.Extensions.TrackInfo()
                .Build(levelSignal)
                .Add("a123", new Color(0, 0, 255))
                .Add("b123", new Color(0, 255, 0))
                .Add("c123", new Color(255, 0, 0));
            new TapeImplement.TapeModels.Kuges.Extensions.ScaleGrid
                {
                    Color = new Color(220, 220, 220)
                }.Build(levelSignal);
            levelSignal.AddSignal(new IntegratedMinMaxSignalSource
                                      {
                                          Internal = DataSources.First(
                                              s => (s is ISignalSource) && (s as ISourceId).Id == "SignalLevel") as
                                                     ISignalSource
                                      },
                                  new LineSettings {Color = new Color(0, 255, 0)});
            levelSignal.AddSignal(DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevel") as ISignalPointSource,
                            new LineSettings { Width = 2, Style = LineStyle.Dash});
            new TapeImplement.TapeModels.Kuges.Extensions.Border
                {
                    Settings = new LineSettings()
                }.Build(levelSignal);
            new TapeImplement.TapeModels.Kuges.Extensions.NullLineChanger
                {
                    Source = DataSources.First(
                        s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevel") as ISignalPointSource,
                    
                    MarkImage = Images.Provider.GetStream("arrow"),
                    SelectedMarkImage = Images.Provider.GetStream("mark"),
                    ImageSize = new Size<int>{Width = 16,Height = 16},
                    ImageAlignment = Alignment.Bottom,
                    LineSettings = new LineSettings{Width = 5,Color=new Color(150, 200, 150,0)},
                    PointChanged = (p1, p2) => { MessageBox.Show("Good Job!"); }
                }
                .Build(levelSignal);
            new TapeImplement.TapeModels.Kuges.Extensions.RefPositionRectangle()
                .Build(levelSignal);
            //панели для карточек кривых
            var curvePanels = new TapeImplement.TapeModels.Kuges.Extensions.TwoPointsInfoPanels();
            curvePanels.Build(levelSignal);
            curvePanels.Register("1",
                new InfoTableLayerFactory 
                {
                    TableData = new[,] { { "1a", "2a", "3a" }, { "1b", "2b", "3b" }, { "1c", "", "3c" }, { "", "2d", "3d" }, { "1e", "", "e" } },
                    TextSize = 10
                }.Create());
            //показываем
            curvePanels.Show("1",
                //две точки между которыми отображается панель
                new Point<float> { X = 600, Y = 0 }, new Point<float> { X = 1100, Y = 0 },
                //высота панели
                new Size<float> { Width = 0, Height = 120 },
                //снизу не выравниваю, нижний край определяется высотой панели
                Alignment.Left|Alignment.Right|Alignment.Top);

            var dist = scrollHost.CreateTrack<DistScaleTrackModel>(new TrackSizeAbsolute { Value = 40 });
            dist.AddSource(DataSources.First(s => s is CoordSource) as ICoordinateSource,
                new FontSettings(), new FontSettings { Color = new Color(0,0,255),Angle = -45 }, new Color(0, 0, 0));

            var trackSignal = scrollHost.CreateTrack<DataTrackModel>(new TrackSizeAbsolute { Value = 200 });
            new TapeImplement.TapeModels.Kuges.Extensions.CoordGrid
                {
                    Source = DataSources.First(s => s is CoordSource) as ICoordinateSource,
                    Color = new Color(255, 220, 220)
                }
                .Build(trackSignal);
            
            trackSignal.Diapazone.Min = 1400;
            trackSignal.Diapazone.Max = 1600;
            trackSignal.Diapazone.MinWidth = 10;
            trackSignal.Diapazone.MaxWidth = 300;
            trackSignal.Position.Set(1400, 1600);
            trackSignal.InitScale(new FontSettings { Size = 8, Angle = 45, Name = "Tahoma" }, new Color(0, 0, 255));
            
            new TapeImplement.TapeModels.Kuges.Extensions.ScaleGrid
            {
                Color = new Color(220, 220, 220)
            }.Build(trackSignal);
            trackSignal.AddSignal(new IntegratedMinMaxSignalSource
                                      {
                                          Internal = DataSources.First(
                                              s => (s is ISignalSource) && (s as ISourceId).Id == "SignalTrack") as
                                                     ISignalSource
                                      },
                                  new LineSettings {Color = new Color(255, 0, 0)});
            trackSignal.AddSignal(DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrack") as ISignalPointSource,
                            new LineSettings());
            new TapeImplement.TapeModels.Kuges.Extensions.RectangleZoom
                {
                    Button = MouseButton.None,
                    Shift = true
                }
                .Build(trackSignal);
            new TapeImplement.TapeModels.Kuges.Extensions.TrackInfo
                {
                    Alignment = Alignment.Bottom,
                    Size = 10,
                    Height = 17,
                    Style = FontStyle.Italic
                }
                .Build(trackSignal)
                .Add("a123", new Color(0, 0, 255))
                .Add("b123", new Color(0, 255, 0))
                .Add("c123", new Color(255, 0, 0));
            new TapeImplement.TapeModels.Kuges.Extensions.Border
                {
                    Settings = new LineSettings()
                }.Build(trackSignal);

            var dist2 = model.CreateTrack<DistScaleTrackModel>(new TrackSizeAbsolute { Value = 30 });
            dist2.AddSource(DataSources.First(s => s is CoordSource) as ICoordinateSource,
                new FontSettings{Size = 8}, new FontSettings { Style = FontStyle.Italic, Angle = 0 }, new Color(0, 0, 0));

            //панели для карточек кривых
            var curvePanels2 = new TapeImplement.TapeModels.Kuges.Extensions.TwoPointsInfoPanels();
            curvePanels2.Build(trackSignal);
            //для теста беру сигнал уровня для отображения графика
            var ss = new IntegratedMinMaxSignalSource
                         {
                             Internal = DataSources.First(
                                 s => (s is ISignalSource) && (s as ISourceId).Id == "SignalLevel") as
                                        ISignalSource
                         };
            curvePanels2.Register("1",
                                //создаем слой для отображения на панели
                                  new GraphLayerFactory
                                      {
                                          TextSize = 9,
                                          BottomAreaHeight = 15,
                                          TopAreaHeight = 30,
                                          //диапазон значений
                                          Diapazone = new SimpleScalePosition<float> {From = -150f, To = 150f},
                                          //для теста беру участок ленты
                                          TapePosition = new SimpleScalePosition<int> {From = 1000, To = 2200}
                                      }
                                      .Start()
                                      //данные в таблице
                                      .CreateDataTableLayer(new[,] {{"1a", "2a", "3a"}, {"1b", "2b", "3b"}})
                                        //шкала
                                      .CreateScaleLayer(new[] {-70f, 0f, 90f})
                                       //пикеты, TextRegion надо заменить на чтото вроде PiketRegion 
                                       //и реализовать IObjectSource
                                      .CreatePicketLayer(
                                          DataSources.First(s => s is ObjectSource<TextRegion>) as IObjectSource<TextRegion>, r => r.Text)
                                        //график, сетка, подпись
                                      .CreateGraphLayer(ss, "Анп")
                                      .MainLayer);
            //показываем
            curvePanels2.Show("1", 
                //две точки между которыми отображается панель
                new Point<float> { X = 600, Y = 1520 }, new Point<float> { X = 1100, Y = 1520 },
                //высота панели
                new Size<float>{Width= 0,Height= 120}, 
                //снизу не выравниваю, нижний край определяется высотой панели
                Alignment.Left | Alignment.Right | Alignment.Top);

            model.BuildMainLayer();
            
            new TapeImplement.TapeModels.Kuges.Extensions.TapeArea
                {
                    Button = MouseButton.Left,
                    Control = true
                }
                .Build(model);
            new TapeImplement.TapeModels.Kuges.Extensions.TapeCursor()
                .Build(model);


            mainLayer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                KeyboardProcess = new ScrollKeyListener
                {
                    Host = scrollHost,
                    OnRedraw = Player.Redraw
                }
            });
        }

        class ScrollKeyListener:IKeyProcess
        {
            public ScrollTrackHost Host { get; set; }

            public Action OnRedraw { get; set; }

            public void OnKeyDown(KeyboardKey key)
            {
                if(key==KeyboardKey.A)
                {
                    Host.ScrollValue = Host.ScrollValue+0.01f;
                    if (Host.ScrollValue > 1)
                        Host.ScrollValue = 1;
                    OnRedraw();
                }

                if (key == KeyboardKey.Z)
                {
                    Host.ScrollValue = Host.ScrollValue - 0.01f;
                    if (Host.ScrollValue < 0)
                        Host.ScrollValue = 0;
                    OnRedraw();
                }
            }

            public void OnKeyUp(KeyboardKey key)
            {
            }
        }
    }
}
