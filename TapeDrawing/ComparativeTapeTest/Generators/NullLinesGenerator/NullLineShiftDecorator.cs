using TapeDrawing.Core.Primitives;
using TapeImplement.ObjectRenderers.Signals;

namespace ComparativeTapeTest.Generators.NullLinesGenerator
{
    class NullLineShiftDecorator: ISignalPointSource, ISourceId
    {
        public string Id { get; set; }

        public float Shift { get; set; }

        public ISignalPointSource Target { get; set; }

        public Point<float>? GetStartPoint(int fromIndex, int toIndex)
        {
            var point = Target.GetStartPoint(fromIndex, toIndex);

            if (point == null)
                return point;

            return new Point<float> { X = point.Value.X, Y = point.Value.Y + Shift };
        }

        public Point<float>? GetNextPoint()
        {
            var point = Target.GetNextPoint();

            if (point == null)
                return point;

            return new Point<float> { X = point.Value.X, Y = point.Value.Y + Shift }; 
        }
    }
}
