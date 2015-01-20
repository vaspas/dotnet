using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ComparativeTest2.Configuration;
using ComparativeTest2.Forms;
using ComparativeTest2.Models;
using ComparativeTest2.Models.Shapes;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			// Заполним список доступных фигур
			_shapeTypes.Add(new FillAllModel().GetShapeName(), typeof(FillAllModel));
			_shapeTypes.Add(new DrawRectangleModel().GetShapeName(), typeof (DrawRectangleModel));
            _shapeTypes.Add(new DrawRectangleAreaModel().GetShapeName(), typeof(DrawRectangleAreaModel));
			_shapeTypes.Add(new FillRectangleModel().GetShapeName(), typeof (FillRectangleModel));
            _shapeTypes.Add(new FillRectangleAreaModel().GetShapeName(), typeof(FillRectangleAreaModel));
			_shapeTypes.Add(new TextShapeModel().GetShapeName(), typeof(TextShapeModel));
			_shapeTypes.Add(new LinesModel().GetShapeName(), typeof(LinesModel));
			_shapeTypes.Add(new LinesArrayModel().GetShapeName(), typeof (LinesArrayModel));
			_shapeTypes.Add(new PolygonModel().GetShapeName(), typeof (PolygonModel));
			_shapeTypes.Add(new ImageModel().GetShapeName(), typeof (ImageModel));

			// Инициализируем контекстное меню добавления
			foreach (var key in _shapeTypes.Keys)
			{
				var item = contextMenuShapes.Items.Add(key);
				item.Click += ContextMenuItemClick;
			}

			// Заполним типы форм 
            _formTypes.Add("SharpDx11", typeof(WinFormsSharpDx11));
			_formTypes.Add("GDI+", typeof(GdiPlus));
			_formTypes.Add("DirectX", typeof (WinFormsDx));
			_formTypes.Add("WPF", typeof (WpfWindow));

			// Инициализируем меню открытия формы
			foreach (var key in _formTypes.Keys)
			{
				var item = mmLaunch.DropDownItems.Add(key);
				item.Click += LaunchFormItemClick;
				_windows.Add(new WindowInfo { MenuItem = (ToolStripMenuItem)item });
			}
		}

		private void LaunchFormItemClick(object sender, EventArgs e)
		{
			// Если окно этого типа уже открыто
			var menuItem = (ToolStripMenuItem) sender;
			if(menuItem.Checked)
			{
				var index = _windows.FindIndex(w => w.MenuItem == menuItem);
				_windows[index].Window.Close();
				_windows[index].Window = null;
				return;
			}

			// Откроем окно
			var form = (ITestWindow) Activator.CreateInstance(_formTypes[sender.ToString()]);
			form.Factory = new MainLayerFactory { Shapes = CreateShapes() };
			form.Closed += TestFormClosed;
			_windows.Find(w => w.MenuItem == menuItem).Window = form;
			menuItem.Checked = true;

			form.Open();
			form.Redraw();
		}

		private void MmLaunchAllClick(object sender, EventArgs e)
		{
			foreach (var windowInfo in _windows)
			{
				if(windowInfo.Window == null)
					LaunchFormItemClick(windowInfo.MenuItem, new EventArgs());
			}
			ArrangeWindows();
		}

		private List<BaseModel> CreateShapes()
		{
			var parser = new ObjectParser<List<BaseModel>>("");
			return parser.Copy(lvShapes.GetListObjects());
		}

		private void TestFormClosed(object sender, EventArgs e)
		{
			var index = _windows.FindIndex(w => w.Window == sender);
			_windows[index].MenuItem.Checked = false;
			_windows[index].Window = null;
		}

		private void ContextMenuItemClick(object sender, EventArgs e)
		{
			var shape = (BaseModel) Activator.CreateInstance(_shapeTypes[sender.ToString()]);
			lvShapes.AddListItem(shape);
		}

		private void MainFormLoad(object sender, EventArgs e)
		{
			var list = new List<BaseModel> {new FillAllModel()};

			lvShapes.ObjectList = list;
			lvShapes.UpdateList();

			EnableButtonsPropertyGrid(null);
		}

		private void LvShapesObjectSеlected(object obj)
		{
			propertyGrid.SelectedObject = obj;
			EnableButtonsPropertyGrid(obj);
		}

		private void PropertyGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			lvShapes.ChangeProperty((BaseModel) propertyGrid.SelectedObject);
		}

		private void PropertyGridSelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
		{
			lvShapes.ChangeProperty((BaseModel)propertyGrid.SelectedObject);
		}

		private void PropertyGridLeave(object sender, EventArgs e)
		{
			lvShapes.ChangeProperty((BaseModel)propertyGrid.SelectedObject);
		}

		private void EnableButtonsPropertyGrid(object obj)
		{
			if(obj == null)
			{
				tsMainDelete.Enabled = false;
				tsMainUp.Enabled = false;
				tsMainDown.Enabled = false;
				return;
			}

			tsMainDelete.Enabled = true;
			tsMainUp.Enabled = true;
			tsMainDown.Enabled = true;
		}

		#region - Кнопки -

		private void TsMainAddClick(object sender, EventArgs e)
		{
			var point = new Point(tsMainAdd.Bounds.Right, tsMainAdd.Bounds.Top);
			point = tsMainAdd.GetCurrentParent().PointToScreen(point);
			contextMenuShapes.Show(point);
		}

		private void TsMainUpClick(object sender, EventArgs e)
		{
			lvShapes.UpSelectedListItem();
		}

		private void TsMainDownClick(object sender, EventArgs e)
		{
			lvShapes.DownSelectedItem();
		}

		private void TsMainDeleteClick(object sender, EventArgs e)
		{
			lvShapes.RemoveSeletedItem();
		}

		private void TsFileLoadClick(object sender, EventArgs e)
		{
			if(openFileDialog.ShowDialog() != DialogResult.OK) return;

			try
			{
				var parser = new ObjectParser<List<BaseModel>>(openFileDialog.FileName);
				var data = parser.Object;
				lvShapes.ObjectList = data;
				lvShapes.UpdateList();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			TsFileRefreshClick(this, null);

		}

		private void TsFileSaveClick(object sender, EventArgs e)
		{
			if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

			var parser = new ObjectParser<List<BaseModel>>(saveFileDialog.FileName);
			parser.SetNewObject(lvShapes.GetListObjects());
			parser.Save();
		}

		private void TsFileRefreshClick(object sender, EventArgs e)
		{
			foreach (var windowInfo in _windows)
			{
				if (windowInfo.Window != null)
				{
					windowInfo.Window.Factory = new MainLayerFactory { Shapes = CreateShapes() };
					windowInfo.Window.Redraw();
				}
			}
		}

		private void ArrangeWindows()
		{
			// Нужно: выровнять размеры окон по максимальному
			// Расположить окна слева-направо друг возле друга по центру от клавного окна
			// По высоте сразу над главным окном

			// 1. Максимальный размер
			int maxW = 0;
			int maxH = 0;
			int windowsCount = 0;
			{
				foreach (var windowInfo in _windows)
				{
					if (windowInfo.Window == null) continue;
					windowsCount++;
					var size = windowInfo.Window.FormSize;
					if (maxW < size.Width) maxW = size.Width;
					if (maxH < size.Height) maxH = size.Height;
				}

				// Если нет открытых окно, вернем управление
				if (windowsCount == 0) return;

				// Окна, расположенные рядом друг с другом не должны вылезать за экран
				if ((maxW * windowsCount) > Screen.PrimaryScreen.Bounds.Width)
					maxW = Screen.PrimaryScreen.Bounds.Width / windowsCount;
			}

			// Положение окон по X
			int windowsAllWidth = maxW * windowsCount;
			int windowsLeft = Left + Width / 2 - windowsAllWidth / 2;
			// Нельзя выходить за левую границу экрана
			if (windowsLeft < 0) windowsLeft = 0;
			// Нельзя выходить за правую границу экрана
			if ((windowsLeft + windowsAllWidth) > Screen.PrimaryScreen.Bounds.Right)
				windowsLeft = Screen.PrimaryScreen.Bounds.Right - windowsAllWidth;

			// Положение окон по Y
			int windowsTop = Top - maxH;
			if (windowsTop < 0) windowsTop = 0;

			// Теперь распределим окна
			{
				int x = windowsLeft;
				int y = windowsTop;
				foreach (var windowInfo in _windows)
				{
					if (windowInfo.Window == null) continue;
					windowInfo.Window.FormSize = new Size(maxW, maxH);
					windowInfo.Window.FormLocation = new Point(x, y);
					windowInfo.Window.ActivateForm();
					x += maxW;
				}
			}
		}

		private void TsFileArrangeWindowsClick(object sender, EventArgs e)
		{
			ArrangeWindows();
		}

		#endregion

		private class WindowInfo
		{
			/// <summary>
			/// Форма
			/// </summary>
			public ITestWindow Window { get; set; }
			/// <summary>
			/// Связанный с окноп элемент меню
			/// </summary>
			public ToolStripMenuItem MenuItem { get; set; }
		}

		private readonly List<WindowInfo> _windows = new List<WindowInfo>();
		private readonly Dictionary<string, Type> _shapeTypes = new Dictionary<string, Type>();
		private readonly Dictionary<string, Type> _formTypes = new Dictionary<string, Type>();
	}
}
