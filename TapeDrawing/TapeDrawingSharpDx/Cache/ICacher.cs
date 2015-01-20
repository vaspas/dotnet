﻿using System;

namespace TapeDrawingSharpDx.Cache
{
    interface ICacher<out TData, TArgs> : IDisposable
    {
        TData Get(ref TArgs args);
    }
}
