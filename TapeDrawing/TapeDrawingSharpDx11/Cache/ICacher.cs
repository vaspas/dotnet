using System;

namespace TapeDrawingSharpDx11.Cache
{
    interface ICacher<out TData, TArgs> : IDisposable
    {
        TData Get(ref TArgs args);
    }
}
