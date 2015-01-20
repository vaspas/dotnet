using System.Collections.Generic;
using TapeImplement.CoordGridRenderers;
using TapeImplementTest.SourceImplement;

namespace TapeImplementTest.Factories
{
    /// <summary>
    /// Фабрика создает и конфигурирует источник данных
    /// </summary>
    static class SourceFactory
    {
        public static TestCoordinateSource Generate(TestParams testParams)
        {
            var iBegin = new CoordInterrupt { Index = 0, Title = "Начало" };
            var iEnd = new CoordInterrupt { Index = testParams.IndexLen, Title = "Конец" };

            var interrupts = new List<ICoordInterrupt> { iBegin };
            if (testParams.Interrupts > 0)
            {
                for (int i = 0; i < testParams.Interrupts; i++)
                {
                    var buf = new CoordInterrupt
                    {
                        Index = (int)((i + 1) * (testParams.IndexLen / (testParams.Interrupts + 1.0f))),
                        Title = (i + 1) + " \\ " + (testParams.Interrupts + 1)
                    };
                    interrupts.Add(buf);
                }
            }
            interrupts.Add(iEnd);

            var source = new TestCoordinateSource
            {
                Min = testParams.Min,
                Max = testParams.Max,
                CoordinateStep = (testParams.Max - testParams.Min) / ((float)iEnd.Index - iBegin.Index),
                Interrupts = interrupts.ToArray()
            };
            return source;
        }
    }
}
