using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TapeImplement.CoordGridRenderers;

namespace ComparativeTapeTest.Generators.CoordsGenerator
{
    class Generator
    {
        public CoordSource CoordSource { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public Random Random { get; set; }

        public void Generate()
        {
            for(int i=0;i<100;i++)
                GenerateInterrupt();

            CoordSource.Data.Add(new BeginEndInterrupt
                                     {
                                         Index = From,
                                         Title = "begin"
                                     });
            CoordSource.Data.Add(new BeginEndInterrupt
                                     {
                                         Index = To,
                                         Title = "end"
                                     });
            CoordSource.Data.OrderBy(o=> (o as ICoordInterrupt).Index);

            var cnt = CoordSource.Data.Count;
            for(int i=1;i<cnt;i++)
            {
                CoordSource.Data.Add(new UnitRegion
                                         {
                                             From = (CoordSource.Data[i - 1] as ICoordInterrupt).Index,
                                             To = (CoordSource.Data[i] as ICoordInterrupt).Index,
                                             ValueFrom = (int) (Random.NextDouble()*2000 - 1000),
                                             ValueTo = (int)(Random.NextDouble() * 2000 - 1000)
                                         });
            }
        }

        private void GenerateInterrupt()
        {
            var index = (int)(From + Random.NextDouble()*(To - From));

            if (CoordSource.Data.Any(o => o is ICoordInterrupt && (o as ICoordInterrupt).Index == index))
                return;

            if (Random.Next(0, 1) == 1)
                CoordSource.Data.Add(new InterruptA
                                         {
                                             Index = index,
                                             Title = index%100 + "A"
                                         });

            CoordSource.Data.Add(new InterruptB
                                     {
                                         Index = index,
                                         Title = index%100 + "B"
                                     });
        }

    }
}
