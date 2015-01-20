
using System;

namespace TapeImplement
{
    /// <summary>
    /// Позиция шкала с установлеными граничными значениями.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BoundedScalePosition<T> : IScalePosition<T>, IScaleDiapazone<T>
        where T : struct,IComparable<T>,IEquatable<T>

{
    public T Min { get; set; }
    public T Max { get; set; }

    public T MinWidth { get; set; }
    public T MaxWidth { get; set; }

    public T From { private set; get; }
    public T To { private set; get; }

    public void Set(T from, T to)
    {
        var _max = Max;

        if ((dynamic) to - from > (dynamic) _max - Min)
        {
            From = Min;
            To = _max;
            PositionChanged();
            return;
        }

        if ((dynamic) to - from < MinWidth)
        {
            var v = (dynamic) MinWidth - ((dynamic) to - from);
            from -= v/2;
            to += v/2;
        }

        if ((dynamic)to - from > MaxWidth)
        {
            var v = ((dynamic) to - from) - (dynamic) MaxWidth;
            from += v/2;
            to -= v/2;
        }

        if ((dynamic)from < Min)
        {
            To = to + ((dynamic)Min - from);
            From = Min;
            PositionChanged();
            return;
        }

        if ((dynamic)to > _max)
        {
            From = from - ((dynamic)to - _max);
            To = _max;
            PositionChanged();
            return;
        }

        From = from;
        To = to;

        PositionChanged();
    }

    public event Action PositionChanged = delegate { };

}

}
