using System;
using System.Windows;
using System.Windows.Media.Imaging;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeDrawingWpf;
using System.Windows.Controls;

namespace WpfTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
        public MainWindow()
        {
            InitializeComponent();

            _tm = new ControlTapeModel();
            _allRenderer = new FillAllRenderer();

            // Основной слой отстает от краев на 10%
            var mainLayer = new RendererLayer
                                {
                                    Area = AreasFactory.CreateRelativeArea(0.1f, 0.9f, 0.1f, 0.9f),
                                    Renderer = _allRenderer
                                };

            // Обработчик мышки
            var ml = new MouseHoldListener
                         {
                             OnRedraw = () => _tm.Redraw()
                         };

            // Слой который закрашен красным цветом
            mainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(10, 10, 10, null, 60, 60),
                Renderer = new FillRectangleRenderer()
            });
            // Слой слушающий события мышки
            mainLayer.Add(new MouseListenerLayer
                              {
                                  Area = AreasFactory.CreateMarginsArea(10, 10, 10, null, 60, 60),
                                  MouseListener = ml
                              });
            // Закрашивает разным цветом в зависимости от колесика мышки
            mainLayer.Add(new RendererLayer
                              {
                                  Area = AreasFactory.CreateMarginsArea(10, 10, 10, null, 60, 60),
                                  Renderer = ml
                              });

            var bmpImage = new BitmapImage();
			bmpImage.BeginInit();
			bmpImage.CacheOption = BitmapCacheOption.OnLoad;
			bmpImage.CreateOptions = BitmapCreateOptions.None;
			bmpImage.UriSource = new Uri(@"pack://application:,,,/testImage.bmp", UriKind.Absolute);
			bmpImage.EndInit();
			bmpImage.Freeze();
            var toolTip = new RendererLayer
                              {
                                  Area = AreasFactory.CreateRectangleArea(0, 0, 120, 60, Alignment.Top | Alignment.Left),
                                  Renderer = new ToolTipRenderer
                                                 {
                                                     Bitmap = bmpImage
                                                 }
                              };

            var tooltipMl = new ToolTipListener()
            {
                OnRedraw = () => _tm.Redraw(),
                ToolTipLayerArea = toolTip.Area as RectangleArea,
                Renderer = toolTip.Renderer as ToolTipRenderer
            };

            // Слой слушающий события мышки
            var toolTipLayer = new EmptyLayer
                                   {
                                       Area = AreasFactory.CreateRelativeArea(0.2f, 0.8f, 0.2f, 0.8f)
                                   };
            mainLayer.Add(toolTipLayer);
            toolTipLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0,1, 0, 1),
                MouseListener = tooltipMl
            });
            toolTipLayer.Add(toolTip);

            _tm.Engine.MainLayer = mainLayer;

            // Подключим модель
            _tm.TapeDrawingCanvas = drawingCanvas;

            // Создадим модель для печати
            _ptm = new PrintTapeModel {Engine = {MainLayer = mainLayer}, TapeDrawingCanvas = new TapeDrawingCanvas()};
        }

	    public TestParameters TestParameters
        {
            set
            {
                _allRenderer.TestParameters = value;
                _tm.Redraw();
            }
        }

		protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseMove(e);
			_tm.Redraw();
		}

		private readonly ControlTapeModel _tm;
	    private readonly PrintTapeModel _ptm;
		private readonly FillAllRenderer _allRenderer;

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var printDialog = new PrintDialog();
			
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(
                    _ptm.GetPrintVisual(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight, 30), "Отчет");
            }
        }

	}
}
