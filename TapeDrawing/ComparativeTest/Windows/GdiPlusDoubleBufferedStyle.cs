using System;
using System.Windows.Forms;
using TapeDrawing.Core.Area;
using TapeDrawing.Layers;
using TapeDrawingWinForms;

namespace ComparativeTest.Windows
{
    class GdiPlusDoubleBufferedStyle : Form, ITestWindow
    {
        public GdiPlusDoubleBufferedStyle()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        public MainLayerFactory Factory { set; private get; }

        private readonly ControlTapeModel _model=new ControlTapeModel();

        public void Open()
        {
            _model.Control = this;
            _model.Engine.MainLayer = new EmptyLayer
                                   {
                                       Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1)
                                   };
            Factory.Create(_model.Engine.MainLayer);

            _model.Engine.AfterDraw += EngineAfterDraw;

            Show();
        }

        private int _counter = 0;
        void EngineAfterDraw(object sender, EventArgs e)
        {
            System.Threading.Interlocked.Increment(ref _counter);
        }

        public void Redraw()
        {
            Invalidate();
        }

        public void ShowFps(float intervalSec)
        {
            var counter = _counter;
            _counter = 0;
            if (IsHandleCreated)
                BeginInvoke(new MethodInvoker(() =>
                                              Text = Title+" FPS =" + (counter / intervalSec).ToString()));
        }

        public string Title { get; set; }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _model.Engine.AfterDraw -= EngineAfterDraw;
            _model.Control = null;

            base.OnClosing(e);
        }
    }
}
