using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComparativeTapeTest
{
    interface IWindow
    {
        TapeDrawing.Core.Engine.DrawingEngine Engine { get; }

        void Open();

        void Redraw();
        
        event EventHandler Closed;

        string Title { get; set; }
    }
}
