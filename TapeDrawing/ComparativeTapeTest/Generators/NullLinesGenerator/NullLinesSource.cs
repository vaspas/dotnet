using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Primitives;
using TapeImplement.ObjectRenderers.Signals;

namespace ComparativeTapeTest.Generators.NullLinesGenerator
{
    class NullLinesSource : ISignalPointSource, ISourceId
    {
        public NullLinesSource()
        {
            Points = new List<Point<float>>();
        }

        public string Id { get; set; }

        public List<Point<float>> Points { get; private set; }

        public Point<float>? GetStartPoint(int fromIndex, int toIndex)
        {
            _currentIndex = Points.FindLastIndex(p => p.X < fromIndex);
            if (_currentIndex < 0)
                _currentIndex = 0;

            return Points[_currentIndex++];
        }

        private int _currentIndex;

        public Point<float>? GetNextPoint()
        {
            if (_currentIndex >= Points.Count)
                return null;

            return Points[_currentIndex++];
        }

        public float GetValue(int index)
        {
            var pfrom = Points.Last(p => p.X <= index);
            var pto = Points.First(p => p.X >= index);

            if (pfrom.X == pto.X && pfrom.Y == pto.Y)
                return pfrom.Y;

            var k = (float) (index - pfrom.X)/(pto.X - pfrom.X);
            return pfrom.Y + k*(pto.Y - pfrom.Y);
        }
    }
}
