using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComparativeTapeTest.Generators.NullLinesGenerator;

namespace ComparativeTapeTest.Generators.RegionObjectsGenerator
{
    class Generator
    {
        public ObjectSource<TextRegion> ObjectSource { get; set; }

        public int From { get; set; }

        public int To { get; set; }
        
        public Random Random { get; set; }

        public void Generate()
        {
            var start = From;

            while (start<To)
            {
                var length = Random.Next(500);
                ObjectSource.Data.Add(new TextRegion{From = start, To=start+length, Text = (Random.NextDouble()*50).ToString("N1")});
                start = start + length;
            }

        }
    }
}
