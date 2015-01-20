using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComparativeTapeTest.Generators.NullLinesGenerator;

namespace ComparativeTapeTest.Generators.SignalGenerator
{
    class Generator
    {
        public NullLinesSource NullLineSource { get; set; }

        public SignalSource SignalSource { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public float Amplitude { get; set; }

        public Random Random { get; set; }

        public void Generate()
        {
            for(int i=From;i<To;i++)
            {
                var val = NullLineSource.GetValue(i) + (Random.NextDouble()*2*Amplitude - Amplitude);
                SignalSource.Data.Add((float)val);
            }
        }
    }
}
