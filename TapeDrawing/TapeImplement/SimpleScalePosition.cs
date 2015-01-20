
using System;

namespace TapeImplement
{
    /// <summary>
    /// Позиция без ограничений.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleScalePosition<T> : IScalePosition<T>
    {
        public T From { set; get; }
        public T To { set; get; }

        public void Set(T from, T to)
        {
            if (Equals(From, from) && Equals(To, to))
                return;

            From = from;
            To = to;

            PositionChanged();
        }

        public event Action PositionChanged = delegate { };
    }

}
