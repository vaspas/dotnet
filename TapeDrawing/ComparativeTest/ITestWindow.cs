using System;

namespace ComparativeTest
{
    public interface ITestWindow
    {
        MainLayerFactory Factory { set; }

        void Open();

        void Redraw();

        void ShowFps(float intervalSec);

        event EventHandler Closed;

        string Title { get; set; }
    }
}
