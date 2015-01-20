using System;
using System.Collections.Generic;
using System.Linq;
using ComparativeTapeTest.Generators;
using ComparativeTapeTest.Generators.CoordsGenerator;
using ComparativeTapeTest.Tapes.Images;
using ComparativeTapeTest.Tapes.Types;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeImplement;
using TapeImplement.ObjectRenderers.Signals;
using TapeImplement.TapeModels.VagonPrint;
using TapeImplement.TapeModels.VagonPrint.Track;

namespace ComparativeTapeTest.Tapes.VagonPrint
{
    class VagonPrintTapeFactory : IMainLayerFactory
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
                                PrintSource = new PrintSource
                                                  {
                                                      TapePosition = TapePosition
                                                  }
                            };
            
            var dist = model.CreateTrack<DistScaleTrackModel>(60, "Км");
            dist.AddSource(DataSources.First(s => s is CoordSource) as TapeImplement.CoordGridRenderers.ICoordinateSource);
            var regions = new RegionsSource();
            regions.Add(new Region { From = 0, To = 250 });
            dist.AddRegionObjectRenderer(regions.As<Region>(), r=>r.From, r=>r.To, Provider.GetStream("kolobok"));
            var records = new RecordsSource();
            records.Add(new Record { Index = 250 });
            dist.AddRecordObjectRenderer(records.As<Record>(), r=>r.Index, Provider.GetStream("arrow"));

            var prosRightSignal = model.CreateTrack<DataTrackModel>(50, "Пр. пр.");
            prosRightSignal.InitScale(new float[] { -10, 0, 10 }, new float[] { -10, 0, 10 }, 0, 12, -12);
            prosRightSignal.AddSignal(DataSources.First(
                s => (s is ISignalSource) && (s as ISourceId).Id == "SignalLevel") as ISignalSource,
                                  new LineSettings { Color = new Color(0, 0, 0) });
            prosRightSignal.AddSignal(DataSources.First(
                s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevel") as ISignalPointSource,
                                  new LineSettings { Width = 1, Style = LineStyle.Solid });

            var prosLeftSignal = model.CreateTrack<DataTrackModel>(50, "Пр. л.");
            prosLeftSignal.InitScale(new float[] { -10, 0, 10 }, new float[] { -10, 0, 10 }, 0, 12, -12);
            prosLeftSignal.AddSignal(DataSources.First(
                s => (s is ISignalSource) && (s as ISourceId).Id == "SignalLevel") as ISignalSource,
                                  new LineSettings { Color = new Color(0, 0, 0) });
            prosLeftSignal.AddSignal(DataSources.First(
                s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevel") as ISignalPointSource,
                                  new LineSettings { Width = 1, Style = LineStyle.Solid });

            var trackSignal = model.CreateTrack<DataTrackModel>(100, "Шаблон");
            trackSignal.InitScale(new float[] { 1512, 1520, 1528, 1536, 1542, 1548 },
                new float[] { 1510, 1512, 1516, 1520, 1528, 1536, 1542, 1546, 1548 }, 1520, 1550, 1510);
            trackSignal.AddSignal(DataSources.First(
                    s => (s is ISignalSource) && (s as ISourceId).Id == "SignalTrack") as ISignalSource,
                                  new LineSettings { Color = new Color(0, 0, 0) });
            trackSignal.AddSignal(DataSources.First(
                            s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineTrack") as ISignalPointSource,
                            new LineSettings());
            
            var rihtRightSignal = model.CreateTrack<DataTrackModel>(100, "Рихтовка п.");
            rihtRightSignal.InitScale(new float[] { 0, 3, 30 }, new float[] { 0, 3, 30 }, 0, 120, -3);
            rihtRightSignal.AddSignal(DataSources.First(
                s => (s is ISignalSource) && (s as ISourceId).Id == "SignalLevel") as ISignalSource,
                                  new LineSettings { Color = new Color(0, 0, 0) });
            rihtRightSignal.AddSignal(DataSources.First(
                s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevel") as ISignalPointSource,
                                  new LineSettings { Width = 1, Style = LineStyle.Solid });

            var rihtLeftSignal = model.CreateTrack<DataTrackModel>(100, "Рихтовка л.");
            rihtLeftSignal.InitScale(new float[] { 0, -3, -30 }, new float[] { 0, -3, -30 }, 0, 30, -90);
            rihtLeftSignal.AddSignal(DataSources.First(
                s => (s is ISignalSource) && (s as ISourceId).Id == "SignalLevel") as ISignalSource,
                                  new LineSettings { Color = new Color(0, 0, 0) });
            rihtLeftSignal.AddSignal(DataSources.First(
                s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevel") as ISignalPointSource,
                                  new LineSettings { Width = 1, Style = LineStyle.Solid });

            var levelSignal = model.CreateTrack<DataTrackModel>(150,"Уровень");
            levelSignal.InitScale(new float[] { -30, 0, 30 }, new float[] { -30, 0, 30 }, 0, 40, -90);
            levelSignal.AddSignal(DataSources.First(
                s => (s is ISignalSource) && (s as ISourceId).Id == "SignalLevel") as ISignalSource,
                                  new LineSettings {Color = new Color(0, 0, 0)});
            levelSignal.AddSignal(DataSources.First(
                s => (s is ISignalPointSource) && (s as ISourceId).Id == "NullLineLevel") as ISignalPointSource,
                                  new LineSettings {Width = 1, Style = LineStyle.Solid});

            var levelDeviations = new RegionsSource();
            levelDeviations.Add(new Region { From = 200, To = 250, Text = "qwe" + Environment.NewLine + Environment.NewLine + "sdff" });
            levelDeviations.Add(new Region { From = 300, To = 350, Text = "swdfsdf" });
            levelDeviations.Add(new Region { From = 500, To = 550, Text = "sdfasd" });
            levelSignal.AddDeviationsRenderer(levelDeviations.As<Region>(), r=>r.From,r=>r.To, new Color(150, 0, 0, 255), 15);

            model.Table.RowHeight = 20;
            model.Table.AddColumn(30,"м");
            model.Table.AddColumn(30, "Отст");
            model.Table.AddColumn(15, "Ст");
            model.Table.AddColumn(30, "Откл");
            model.Table.AddColumn(15, "Дл");
            model.Table.AddColumn(20, "Кол");
            model.Table.AddColumn(30, "Огр.ск");

            model.BuildMainLayer();
        }

    }
}
