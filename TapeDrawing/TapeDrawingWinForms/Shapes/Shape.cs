
using System;
using System.Drawing;

namespace TapeDrawingWinForms.Shapes
{
    abstract class Shape: IDisposable
    {
        public Graphics Graphics;

        public virtual void Dispose()
        {

        }
    }
}
