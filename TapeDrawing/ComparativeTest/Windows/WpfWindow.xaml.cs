using System;
using System.Windows;
using TapeDrawing.Core.Area;
using TapeDrawing.Layers;
using TapeDrawingWpf;

namespace ComparativeTest.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class WpfWindow: Window, ITestWindow
	{
        public WpfWindow()
        {
            InitializeComponent();
        }

        public MainLayerFactory Factory { set; private get; }

        private readonly ControlTapeModel _model = new ControlTapeModel();

	    public void Open()
	    {
            _model.Engine.MainLayer = new EmptyLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1)
            };
            Factory.Create(_model.Engine.MainLayer);

	        _model.TapeDrawingCanvas = drawingCanvas;

            _model.Engine.AfterDraw += EngineAfterDraw;

            Show();
	    }

        private int _counter = 0;
        void EngineAfterDraw(object sender, EventArgs e)
        {
            _counter++;
        }

	    public void Redraw()
	    {
	        _model.Redraw();
	    }

	    public void ShowFps(float intervalSec)
	    {
            var counter = _counter;
            _counter = 0;
	        Title = "WPF FPS=" + (counter/intervalSec);
	    }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _model.TapeDrawingCanvas = null;

            base.OnClosing(e);
        }
	}
}
