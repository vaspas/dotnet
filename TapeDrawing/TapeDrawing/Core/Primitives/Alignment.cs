using System;

namespace TapeDrawing.Core.Primitives
{
    [Flags]
    public enum Alignment
    {
        None=0,
        Left=1,
        Right=2,
        Bottom=4,
        Top=8
    }
}
