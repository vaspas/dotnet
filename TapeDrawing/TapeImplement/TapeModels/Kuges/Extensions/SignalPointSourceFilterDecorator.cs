using System;
using TapeDrawing.Core.Primitives;
using TapeImplement.ObjectRenderers.Signals;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class SignalPointSourceFilterDecorator:ISignalPointSource
    {
        public ISignalPointSource Internal { get; set; }

        public Predicate<Point<float>> Filter { get; set; }

        public Predicate<Point<float>> StopSearch { get; set; }

        public Point<float>? GetNextPoint()
        {
            var p = Internal.GetNextPoint();

            while (p != null && Filter(p.Value) && !StopSearch(p.Value))
                p=Internal.GetNextPoint();

            return p;
        }

        public Point<float>? GetStartPoint(int fromindex, int toindex)
        {
            var p = Internal.GetStartPoint(fromindex, toindex);

            if (p == null || !Filter(p.Value))
                return p;

            return GetNextPoint();
        }
    }
}
