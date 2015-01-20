using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TapeDrawing.Core.Area;
using TapeDrawing.Layers;
using TapeDrawingWinForms;

namespace ComparativeTest2.Forms
{
	public partial class GdiPlus : Form, ITestWindow
	{
		public GdiPlus()
		{
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
		}

		/// <summary>
		/// Фабрика создания слоев
		/// </summary>
		public MainLayerFactory Factory
		{
			set
			{
				if(_factory != null)
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
			get { return Location; }
			set { Location = value; }
		}

		/// <summary>
		/// Размер окна
		/// </summary>
		public Size FormSize
		{
			get { return Size; }
			set { Size = value; }
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
			Invalidate();
		}

		public void ActivateForm()
		{
			Activate();
		}

		private void AssignToRenderer()
		{
			if(_model != null) _model.Control = null;
			_model = new ControlTapeModel
			         	{
			         		Control = this,
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
			_model.Control = null;

			base.OnClosing(e);
		}

		private MainLayerFactory _factory;

		private ControlTapeModel _model;
	}
}
