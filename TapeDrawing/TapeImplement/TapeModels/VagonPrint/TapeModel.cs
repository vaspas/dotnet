using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.ObjectRenderers;
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.VagonPrint.Sources;
using TapeImplement.TapeModels.VagonPrint.Table;
using TapeImplement.TapeModels.VagonPrint.Track;

namespace TapeImplement.TapeModels.VagonPrint
{
    public class TapeModel
    {
        public TapeModel()
        {
            Table=new TableModel{TapeModel = this, ScaleFactor = DistTrackScaleFactor};
            DistTrackScaleFactorChanged += () => Table.ScaleFactor = _distTrackScaleFactor;

            Settings=new TapeSettings();
        }

        public IScalePosition<int> TapePosition { get; set; }

        public IPrintSource PrintSource { get; set; }

        public ILayer MainLayer { get; set; }

        public TapeSettings Settings { get; private set; }

        public void BuildMainLayer()
        {
            MainLayer.Clear();

            MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0,0,0,0),
                Renderer = new FillAllRenderer(
                    new Color(255, 255, 255))
            });

            MainLayer.Add(new RendererLayer
                              {
                                  Area = AreasFactory.CreateMarginsArea(null, 0, 0, 0, Settings.RightInfoWidth,0),
                                  Renderer = new TextRenderer
                                                 {
                                                     Angle = 90,
                                                     Color = new Color(0,0,0),
                                                     FontName = Settings.FontName,
                                                     Size = 8,
                                                     Style = FontStyle.None,
                                                     GetText = () => PrintSource.GetRightInfo(TapePosition),
                                                     LayerAlignment = Alignment.Bottom,
                                                     TextAlignment = Alignment.Right
                                                 }
                              });

            //подпись внизу
            MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(null, Settings.RightInfoWidth, Settings.ScaleInfoHeigth, 0, Tracks.Sum(t => t.Size + Settings.GraphsSplitWidth) + Table.Size, 0),
                Renderer = new TextRenderer
                {
                    Angle = 0,
                    Color = new Color(0, 0, 0),
                    FontName = Settings.FontName,
                    Size = 8,
                    Style = FontStyle.None,
                    GetText = () => PrintSource.GetBottomInfoLine(TapePosition),
                    LayerAlignment = Alignment.Bottom | Alignment.Left,
                    TextAlignment = Alignment.Bottom | Alignment.Left
                }
            });

            //подпись по центру
            MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(null, Settings.RightInfoWidth, Settings.ScaleInfoHeigth, Settings.TopInfoHeight, Tracks.Sum(t => t.Size + Settings.GraphsSplitWidth) + Table.Size, 0),
                Renderer = new TextRenderer
                {
                    Angle = 0,
                    Color = new Color(0, 0, 0),
                    FontName = Settings.FontName,
                    Size = 20,
                    Style = FontStyle.None,
                    GetText = () => PrintSource.GetCenterInfo(),
                    LayerAlignment = Alignment.None,
                    TextAlignment = Alignment.None
                }
            });

            

            var mainBorder = new RendererLayer
                                 {
                                     Area = AreasFactory.CreateMarginsArea(null, Settings.RightInfoWidth, 0, null,
                                        Tracks.Sum(t => t.Size + Settings.GraphsSplitWidth) +  Table.Size, 
                                        Settings.Height),
                                     Renderer =
                                         new BorderRenderer
                                             {
                                                 Color = Settings.DefaultColor,
                                                 Right = true,
                                                 Top = true,
                                                 LineStyle = LineStyle.Solid,
                                                 LineWidth = Settings.DefaultLineWidth
                                             }
                                 };
            MainLayer.Add(mainBorder);

            //подпись вверху слева
            /*mainBorder.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new TextRenderer
                {
                    Angle = 0,
                    Color = new Color(0, 0, 0),
                    FontName = Settings.FontName,
                    Size = 8,
                    Style = FontStyle.None,
                    GetText = () => PrintSource.GetTopInfoLineLeft(TapePosition),
                    LayerAlignment = Alignment.Top | Alignment.Left,
                    TextAlignment = Alignment.Top | Alignment.Left
                }
            });*/

            var topInfo = new RendererLayer
                              {
                                  Area = AreasFactory.CreateMarginsArea(null, 0, null, 0,
                                  Tracks.Sum(t => t.Size + Settings.GraphsSplitWidth) - Settings.GraphsSplitWidth, Settings.TopInfoHeight),
                                  Renderer =
                                         new BorderRenderer
                                         {
                                             Color = Settings.DefaultColor,
                                             Left = true,
                                             Bottom = true,
                                             LineStyle = LineStyle.Solid,
                                             LineWidth = Settings.DefaultLineWidth
                                         }
                              };
            mainBorder.Add(topInfo);

            topInfo.Add(new RendererLayer
                            {
                                Area = AreasFactory.CreateRelativeArea(0,1,0.5f,1),
                                Renderer = new TextRenderer
                                               {
                                                   Angle = 0,
                                                   Color = new Color(0,0,0),
                                                   FontName = Settings.FontName,
                                                   LayerAlignment = Alignment.Right,
                                                   Size = 8,
                                                   Style = FontStyle.None,
                                                   GetText = () => PrintSource.GetTopInfoLine1(TapePosition),
                                                   TextAlignment = Alignment.Right
                                               }
                            });
            topInfo.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 0.5f),
                Renderer = new TextRenderer
                {
                    Angle = 0,
                    Color = new Color(0, 0, 0),
                    FontName = Settings.FontName,
                    LayerAlignment = Alignment.Right,
                    Size = 8,
                    Style = FontStyle.None,
                    GetText = () => PrintSource.GetTopInfoLine2(TapePosition),
                    TextAlignment = Alignment.Right
                }
            });

            topInfo.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new TextRenderer
                {
                    Angle = 0,
                    Color = new Color(0, 0, 0),
                    FontName = Settings.FontName,
                    Size = 8,
                    Style = FontStyle.None,
                    GetText = () => PrintSource.GetTopInfoLineLeft(TapePosition),
                    LayerAlignment = Alignment.Bottom | Alignment.Left,
                    TextAlignment = Alignment.Bottom | Alignment.Left
                }
            });

            

            var graphsBorderParent = new EmptyLayer
            {
                Area = AreasFactory.CreateMarginsArea(null, 0, Settings.ScaleInfoHeigth + Settings.ScaleHeigth, null,
                                                      Tracks.Sum(t => t.Size + Settings.GraphsSplitWidth) - Settings.GraphsSplitWidth, Settings.GraphsHeigth)
            };
            mainBorder.Add(graphsBorderParent);
            
            //границы для графиков сверху и снизу
            _graphsBorder = new RendererLayer
                               {
                                   Area = AreasFactory.CreateRelativeArea(0,1,0,DistTrackScaleFactor),
                                   Renderer =
                                          new BorderRenderer
                                          {
                                              Color = Settings.DefaultColor,
                                              Top = true,
                                              Bottom = true,
                                              LineStyle = LineStyle.Solid,
                                              LineWidth = Settings.DefaultLineWidth
                                          }
                               };
            DistTrackScaleFactorChanged +=
                () => (_graphsBorder.Area as RelativeArea).CurrentRelative.Top = _distTrackScaleFactor;
            graphsBorderParent.Add(_graphsBorder);

            BuildTracks(mainBorder);
            _buildAdditionalLayers.ForEach(a=>a());
        }

        private ILayer _graphsBorder;

        internal readonly List<TrackItem> Tracks = new List<TrackItem>();

        public TableModel Table { get; private set; }

        public T CreateTrack<T>(int size, string title) where T : BaseTrackModel, new()
        {
            var trackLayer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0) };
            var scalelayer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0, 0, 0, null, 0,Settings.ScaleHeigth) };
            var datalayerparent = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(0,0,Settings.ScaleHeigth, null, 0, Settings.GraphsHeigth) };

            trackLayer.Add(scalelayer);
            trackLayer.Add(datalayerparent);

            var datalayer = new EmptyLayer { Area = AreasFactory.CreateRelativeArea(0, 1, 0, DistTrackScaleFactor) };
            DistTrackScaleFactorChanged +=
                () => (datalayer.Area as RelativeArea).CurrentRelative.Top = _distTrackScaleFactor;
            datalayerparent.Add(datalayer);

            var newtrack = new T
            {
                TapeModel = this,
                DataLayer = datalayer,
                ScaleLayer = scalelayer
            };

            Tracks.Add(new TrackItem
            {
                Layer = trackLayer,
                Size = size,
                Model = newtrack,
                Title = title
            });

            return newtrack;
        }
        
        private void BuildTracks(ILayer parent)
        {
            //вставляем дорожки
            float currentValue = 0;
            foreach (var track in Tracks)
            {
                var localTrack = track;

                var layer = new EmptyLayer
                                {
                                    Area =
                                        AreasFactory.CreateMarginsArea(null, currentValue, Settings.ScaleInfoHeigth,
                                                                       null, track.Size,
                                                                       Settings.ScaleHeigth + Settings.GraphsHeigth)
                                };
                layer.Add(track.Layer);
                parent.Add(layer);

                var infoLayer = new EmptyLayer
                                    {
                                        Area =
                                            AreasFactory.CreateMarginsArea(null, currentValue, 0,
                                                                           null, track.Size,
                                                                           Settings.ScaleInfoHeigth)
                                    };
                infoLayer.Add(new RendererLayer
                                  {
                                      Area = AreasFactory.CreateMarginsArea(0,0,0,0),
                                      Renderer = new BorderRenderer
                                                     {
                                                         Bottom = true,
                                                         Top = true,
                                                         Left = true,
                                                         Right = true,
                                                         Color = Settings.DefaultColor,
                                                         LineStyle = LineStyle.Solid,
                                                         LineWidth = Settings.DefaultLineWidth
                                                     }
                                  });
                infoLayer.Add(new RendererLayer
                {
                    Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                    Renderer = new TextRenderer
                                   {
                                       Angle = 0,
                                       Color = Settings.DefaultColor,
                                       FontName = Settings.FontName,
                                       Size = 7,
                                       Style = FontStyle.None,
                                       GetText = () => localTrack.Title
                                   }
                });
                parent.Add(infoLayer);


                currentValue += track.Size + Settings.GraphsSplitWidth;
            }

            var tableLayer = new EmptyLayer
            {
                Area =
                    AreasFactory.CreateMarginsArea(null, currentValue, Settings.ScaleInfoHeigth+Settings.ScaleHeigth,
                                                   null, Table.Size,
                                                   Settings.GraphsHeigth)
            };
            parent.Add(tableLayer);
            Table.BuildLayer(tableLayer);

            var tableInfoLayer = new EmptyLayer
                                     {
                                         Area =
                                             AreasFactory.CreateMarginsArea(null, currentValue, 0, null,
                                                                            Table.Size, Settings.ScaleInfoHeigth)
                                     };
            parent.Add(tableInfoLayer);
            Table.BuildInfoLayer(tableInfoLayer);

            
        }


        private float _distTrackScaleFactor = 1;
        public float DistTrackScaleFactor
        {
            get { return _distTrackScaleFactor; }
            set 
            { 
                _distTrackScaleFactor = value; 

                DistTrackScaleFactorChanged();
            }
        }

        private event Action DistTrackScaleFactorChanged = delegate { };


        private readonly List<Action> _buildAdditionalLayers=new List<Action>();

        public void AddRecordTextRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> presentation)
        {
            _buildAdditionalLayers.Add(() => CreateRecordTextRenderer(source, getIndex, presentation));
        }

        private void CreateRecordTextRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> presentation)
        {
            var parent = new EmptyLayer
                             {
                                 Area = AreasFactory.CreateMarginsArea(null, Settings.RightInfoWidth,
                                                                     Settings.ScaleInfoHeigth + Settings.ScaleHeigth,
                                                                     null,
                                                                     //2 *
                                                                     (Tracks.Sum(t => t.Size + Settings.GraphsSplitWidth) -
                                                                      Settings.GraphsSplitWidth), Settings.GraphsHeigth)
                             };
            MainLayer.Add(parent);

            var l1=new RendererLayer
                              {
                                  Area = AreasFactory.CreateRelativeArea(-1,1,0,DistTrackScaleFactor),
                                  Renderer = new RecordTextRenderer<T>
                                                 {
                                                     Source = source,
                                                     GetIndex = getIndex,
                                                     Alignment = Alignment.Left|Alignment.Bottom,
                                                     Angle = 0,
                                                     Translator =
                                                         PointTranslatorConfigurator.CreateLinear().ChangeAxels().
                                                         Translator,
                                                     TapePosition = TapePosition,
                                                     FontColor = Settings.DefaultColor,
                                                     FontSize = 8,
                                                     FontType = Settings.FontName,
                                                     FontStyle = FontStyle.None,
                                                     ObjectPresentation = presentation
                                                 }
                              };
            DistTrackScaleFactorChanged += () => (l1.Area as RelativeArea).CurrentRelative.Top = _distTrackScaleFactor;
            parent.Add(l1);

            var l2 = new RendererLayer
                         {
                             Area = AreasFactory.CreateRelativeArea(0, 1, 0, DistTrackScaleFactor),
                             Renderer = new RecordLineRenderer<T>
                                            {
                                                Source = source,
                                                GetIndex = getIndex,
                                                Translator =
                                                    PointTranslatorConfigurator.CreateLinear().ChangeAxels().
                                                    Translator,
                                                TapePosition = TapePosition,
                                                LineColor = Settings.LightColor,
                                                LineWidth = Settings.DefaultLineWidth,
                                                LineStyle = LineStyle.Dot
                                            }
                         };
            DistTrackScaleFactorChanged += () => (l2.Area as RelativeArea).CurrentRelative.Top = _distTrackScaleFactor;
            parent.Add(l2);
        }
    }
}
