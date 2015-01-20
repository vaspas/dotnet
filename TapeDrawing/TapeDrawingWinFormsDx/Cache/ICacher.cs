using System;

namespace TapeDrawingWinFormsDx.Cache
{
    interface ICacher<out TData, TArgs> : IDisposable
    {
        TData Get(ref TArgs args);
    }
}
