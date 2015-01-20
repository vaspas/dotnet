
using System;

namespace TapeImplement
{
    public interface IScalePosition<T>
    {
        T From { get; }
        T To { get; }

        void Set(T from, T to);

        event Action PositionChanged;
    }
}
