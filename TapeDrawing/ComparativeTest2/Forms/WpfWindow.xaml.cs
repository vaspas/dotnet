using System.ComponentModel;
using System.Drawing;
using TapeDrawing.Core.Area;
using TapeDrawing.Layers;
using TapeDrawingWpf;

namespace ComparativeTest2.Forms
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class WpfWindow: ITestWindow
	{
        public WpfWindow()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Фабрика создания слоев
		/// </summary>
		public MainLayerFactory Factory
		{
			set
			{
				if (_factory != null)
				{
					_factory = value;
					AssignToRenderer();
				}
				else _factory = value;
			}
			private get { return _factory; }
		}

		/// <summary>
		/// Расположение окна
		/// </summary>
		public Point FormLocation
		{
			get { return new Point((int)Left, (int)Top);}
			set 
			{
				Left = value.X;
				Top = value.Y;
			}
		}

		/// <summary>
		/// Размер окна
		/// </summary>
		public Size FormSize
		{
			get { return new Size((int)Width, (int)Height);}
			set 
			{
				Width = value.Width;
				Height = value.Height;	
			}
		}

		/// <summary>
		/// Открывает окно
		/// </summary>
		public void Open()
		{
			AssignToRenderer();

			Show();
		}

		/// <summary>
		/// Заставляет перерисовать окно
		/// </summary>
		public void Redraw()
		{
			_model.Redraw();
		}

		public void ActivateForm()
		{
			Activate();
		}

		private void AssignToRenderer()
		{
			if (_model != null) _model.TapeDrawingCanvas = null;
			_model = new ControlTapeModel
			{
				TapeDrawingCanvas = drawingCanvas,
				Engine =
				{
					MainLayer = new EmptyLayer
					{
						Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1)
					}
				}
			};

			Factory.Create(_model.Engine.MainLayer);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			_model.TapeDrawingCanvas = null;

			base.OnClosing(e);
		}

		private MainLayerFactory _factory;

		private ControlTapeModel _model;
	}
}
