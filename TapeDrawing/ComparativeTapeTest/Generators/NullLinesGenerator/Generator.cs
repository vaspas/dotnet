using System;
using TapeDrawing.Core.Primitives;

namespace ComparativeTapeTest.Generators.NullLinesGenerator
{
    class Generator
    {
        public NullLinesSource Source { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public float Min { get; set; }
        
        public float Max { get; set; }

        public Random Random { get; set; }

        public int RegionSize { get; set; }

        public void Generate()
        {
            Source.Points.Add(new Point<float> { X = From, Y = (Max + Min) / 2 });

            int currentIndex = From;
            while (currentIndex+2*RegionSize < To) 
            {
                currentIndex += RegionSize;
                Source.Points.Add(new Point<float> { X = currentIndex, Y = (Max + Min) / 2 });
                AddCurve(currentIndex, currentIndex + RegionSize);
                currentIndex += RegionSize;
                Source.Points.Add(new Point<float> { X = currentIndex, Y = (Max + Min) / 2 });
            }

            Source.Points.Add(new Point<float> { X = To, Y = (Max + Min) / 2 });
        }

        private void AddCurve(int from, int to)
        {
            var val = (float)(Min + Random.NextDouble()*(Max - Min));

            var pc = (int)((to - from)*0.2f);

            Source.Points.Add(new Point<float> { X = from+pc, Y = val });
            Source.Points.Add(new Point<float> {X = to - pc, Y = val});
        }
    }
}
