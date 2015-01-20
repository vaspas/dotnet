using System;
using System.Windows;
using TapeDrawing.Core.Area;
using TapeDrawing.Layers;
using TapeDrawingWpf;

namespace ComparativeTapeTest.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class WpfWindow: Window, IWindow
	{
        public WpfWindow()
        {
            InitializeComponent();
        }

	    public TapeDrawing.Core.Engine.DrawingEngine Engine
	    {
            get { return _model.Engine; }
	    }

        private readonly ControlTapeModel _model = new ControlTapeModel();

	    public void Open()
	    {
            _model.Engine.MainLayer = new EmptyLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1)
            };

	        _model.TapeDrawingCanvas = drawingCanvas;

	        WindowState = WindowState.Maximized;
            
            Show();
	    }

	    public void Redraw()
	    {
	        _model.Redraw();
	    }
        
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _model.TapeDrawingCanvas = null;

            base.OnClosing(e);
        }
	}
}
