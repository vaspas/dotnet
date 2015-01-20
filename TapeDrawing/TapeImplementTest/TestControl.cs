using System.Windows.Forms;

namespace TapeImplementTest
{
    public class TestControl : Control
    {
        public TestControl()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }
    }
}
