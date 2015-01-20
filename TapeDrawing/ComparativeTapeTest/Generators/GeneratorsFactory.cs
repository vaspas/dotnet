using System;
using ComparativeTapeTest.Generators.RegionObjectsGenerator;
using TapeImplement.CoordGridRenderers;
using TapeImplement.ObjectRenderers;
using TapeImplement.ObjectRenderers.Signals;

namespace ComparativeTapeTest.Generators
{
    class GeneratorsFactory
    {
        public int From { get; set; }

        public int To { get; set; }

        public Random Random { get; set; }


        public ICoordinateSource CreateCoordinateSource()
        {
            var coordSource = new CoordsGenerator.CoordSource();

            new CoordsGenerator.Generator
                {
                    CoordSource = coordSource,
                    From = From,
                    To = To,
                    Random = Random
                }.Generate();

            return coordSource;
        }

        public NullLinesGenerator.NullLinesSource CreateLevelNullLineSource(string id)
        {
            var nullLinesSource = new NullLinesGenerator.NullLinesSource
                                      {
                                          Id=id
                                      };

            new NullLinesGenerator.Generator
            {
                Source = nullLinesSource,
                From = From,
                To = To,
                Random = Random,
                Max=120,
                Min=-120,
                RegionSize = 800
            }.Generate();

            return nullLinesSource;
        }

        public NullLinesGenerator.NullLinesSource CreateTrackNullLineSource(string id)
        {
            var nullLinesSource = new NullLinesGenerator.NullLinesSource
            {
                Id = id
            };

            new NullLinesGenerator.Generator
            {
                Source = nullLinesSource,
                From = From,
                To = To,
                Random = Random,
                Max = 1540,
                Min = 1500,
                RegionSize = 700
            }.Generate();

            return nullLinesSource;
        }

        public ISignalPointSource CreateExtendValue(ISignalPointSource nulllines, float shift, string id)
        {
            return new NullLinesGenerator.NullLineShiftDecorator
                       {
                           Id = id,
                           Shift = shift,
                           Target = nulllines
                       };
        }

        public ISignalSource CreateLevelSignalSource(NullLinesGenerator.NullLinesSource nlsource, string  id)
        {
            var signalSource = new SignalGenerator.SignalSource()
            {
                Id = id
            };

            new SignalGenerator.Generator
            {
                NullLineSource = nlsource,
                SignalSource = signalSource,
                From = From,
                To = To,
                Random = Random,
                Amplitude = 5
            }.Generate();

            return signalSource;
        }

        public ISignalSource CreateTrackSignalSource(NullLinesGenerator.NullLinesSource nlsource, string id)
        {
            var signalSource = new SignalGenerator.SignalSource()
            {
                Id = id
            };

            new SignalGenerator.Generator
            {
                NullLineSource = nlsource,
                SignalSource = signalSource,
                From = From,
                To = To,
                Random = Random,
                Amplitude = 5
            }.Generate();

            return signalSource;
        }

        public IObjectSource<TextRegion> CreateRegionsSource(string id)
        {
            var objectSource = new RegionObjectsGenerator.ObjectSource<TextRegion>
            {
                Id = id
            };

            new RegionObjectsGenerator.Generator
            {
                From = From,
                To = To,
                Random = Random,
                ObjectSource = objectSource
            }.Generate();

            return objectSource;
        }
    }
}
