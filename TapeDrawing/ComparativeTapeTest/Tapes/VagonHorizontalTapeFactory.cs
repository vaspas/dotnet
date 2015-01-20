using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ComparativeTapeTest.Generators;
using ComparativeTapeTest.Generators.CoordsGenerator;
using ComparativeTapeTest.Generators.RegionObjectsGenerator;
using ComparativeTapeTest.Generators.SignalGenerator;
using ComparativeTapeTest.Tapes.CurvePanelLayers;
using ComparativeTapeTest.Tapes.Images;
using ComparativeTapeTest.Tapes.Types;
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
using TapeImplement.TapeModels.Vagon;
using TapeImplement.TapeModels.Vagon.Extensions;
using TapeImplement.TapeModels.Vagon.Track;
using FontStyle = TapeDrawing.Core.Primitives.FontStyle;

namespace ComparativeTapeTest.Tapes
{
    class VagonHorizontalTapeFactory : IMainLayerFactory
    {
        public IScalePosition<int> TapePosition { get; set; }

        public Player Player { get; set; }

        public List<object> DataSources { get; set; }
        

        public void Create(ILayer mainLayer)
        {
            var model = new TapeModel(TapePosition)
                            {
                                MainLayer = mainLayer,
                                ScaleSize = 40,
                                DistScaleSize = 60,
                                Redraw = Player.Redraw,
                                Vertical = false
                            };
            model.GetExtension<CoordinateInfo>().GetCursorPosition = i => string.Format("pos " + i);
            
            var dist1 = model.GetExtension<DistScale>();
            dist1.AddSource(DataSources.First(s => s is CoordSource) as ICoordinateSource,
                new FontSettings{Size = 8}, new FontSettings{Style = FontStyle.Bold,Size = 10});
            var regions = new RegionsSource();
            regions.Add(new Types.Region { From = 0, To = 250 });
            dist1.AddRegionObjectRenderer(regions.As<Types.Region>(),r=>r.From,r=>r.To, Provider.GetStream("kolobok"));
            var records = new RecordsSource();
            records.Add(new Types.Record { Index = 250 });
            dist1.AddRecordObjectRenderer(records.As<Types.Record>(), r=>r.Index, Provider.GetStream("arrow"));

            model.GetExtension<TrackObjects>()
                .AddRegionObjectRenderer(regions.As<Types.Region>(), r => r.From, r => r.To, Provider.GetStream("kolobok"));

            records.Add(new TextRecord { Index = 350, Text = "до 100/120, после 120/140" });
            model.GetExtension<TrackObjects>()
                .AddLeftTextRecordObjectRenderer(records.As<TextRecord>(), r=>r.Index, r => r.Text, Provider.GetStream("kolobok"));

            model.GetExtension<TapeImplement.TapeModels.Vagon.Extensions.CoordGrid>()
                .Color = new Color(255, 220, 220);
            model.GetExtension<TapeImplement.TapeModels.Vagon.Extensions.CoordGrid>()
                .Source = DataSources.First(s => s is CoordSource) as ICoordinateSource;

            var levelSignal = model.CreateTrack<DataTrackModel>(new TrackSizeRelative { Value = 1 });
            levelSignal.Diapazone.Min = -500;
            levelSignal.Diapazone.Max = 500;
            levelSignal.Diapazone.MinWidth = 10;
            levelSignal.Diapazone.MaxWidth = 1000;
            levelSignal.Position.Set(-200,200);
            
            
            new ScaleGrid()
            .AddGridLines(new Color(220, 220, 220))
            .Build(levelSignal);
            levelSignal.AddSignal(new IntegratedMinMaxSignalSource
                                      {
                                          Internal = DataSources.First(
                                              s => (s is ISignalSource) && (s as ISourceId).Id == "SignalLevel") as
                                                     ISignalSource
                                      },
                                  new LineSettings {Color = new Color(0, 255, 0)});
            levelSignal.AddSignal(DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevel") as ISignalPointSource,
                            new LineSettings { Width = 2, Style = LineStyle.Dash}, true);
            levelSignal.Init("Уровень");
            
            var trackSignal = model.CreateTrack<DataTrackModel>(new TrackSizeRelative { Value = 1 });
            trackSignal.GetValue = i => i.ToString();
            
            trackSignal.Diapazone.Min = 1400;
            trackSignal.Diapazone.Max = 1600;
            trackSignal.Diapazone.MinWidth = 10;
            trackSignal.Diapazone.MaxWidth = 300;
            trackSignal.Position.Set(1400, 1600);
            

            new ScaleGrid()
            .AddGridLines(new Color(220, 220, 220))
            .Build(trackSignal);
            trackSignal.AddSignal(new IntegratedMinMaxSignalSource
                                      {
                                          Internal = DataSources.First(
                                              s => (s is ISignalSource) && (s as ISourceId).Id == "SignalTrack") as
                                                     ISignalSource
                                      },
                                  new LineSettings {Color = new Color(255, 0, 0)});
            trackSignal.AddSignal(DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrack") as ISignalPointSource,
                            new LineSettings(), true);
            trackSignal.Init("Шаблон");
            

            model.BuildMainLayer();


        }

    }
}
