using TapeDrawing.Core.Primitives;
using TapeImplement.ObjectRenderers.Signals;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class SignalPointSourceChangeDecorator:ISignalPointSource
    {
        public ISignalPointSource Internal { get; set; }

        public Point<float>? FromPoint { get; set; }
        public Point<float>? ToPoint { get; set; }

        public Point<float>? GetNextPoint()
        {  
            return TryChange(Internal.GetNextPoint());
        }

        public Point<float>? GetStartPoint(int fromindex, int toindex)
        {
            return TryChange(Internal.GetStartPoint(fromindex, toindex));
        }

        private Point<float>? TryChange(Point<float>? point)
        {
            if (FromPoint == null || ToPoint == null || point==null)
                return point;

            if (point.Value.X == FromPoint.Value.X && point.Value.Y == FromPoint.Value.Y)
                return ToPoint.Value;

            return point;
        }
    }
}
