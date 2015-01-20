using TapeDrawing.Core.Engine;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWpf
{
    /// <summary>
    /// Модель объекта, который можно вывести на печать
    /// </summary>
    public class PrintTapeModel
    {
        public PrintTapeModel()
        {
            // Создадим графический контекст и движок
            _graphicContext = new GraphicContext { Surface = new DrawSurface() };
            Engine = new DrawingEngine { GraphicContext = _graphicContext };
        }

        /// <summary>
        /// Движок рисования
        /// </summary>
        public DrawingEngine Engine { private set; get; }

        /// <summary>
        /// Метод подготавливает страницу к печати
        /// </summary>
        /// <param name="pageWidth">Ширина печатной страницы</param>
        /// <param name="pageHeight">Высота печатной страницы</param>
        /// <param name="pageMargin">Отступ по краям</param>
        /// <returns>Элемент, который можно напечатать</returns>
        public System.Windows.Media.Visual GetPrintVisual(double pageWidth, double pageHeight, int pageMargin)
        {
            TapeDrawingCanvas.Margin = new System.Windows.Thickness(pageMargin);
            var size = new System.Windows.Size(pageWidth, pageHeight);
            TapeDrawingCanvas.Measure(size);
            TapeDrawingCanvas.Arrange(new System.Windows.Rect(0, 0, size.Width, size.Height));
            Redraw();
            return TapeDrawingCanvas;
        }

        /// <summary>
        /// Элемент пользовательского интерфейса, на который выводится графика
        /// </summary>
        public TapeDrawingCanvas TapeDrawingCanvas
        {
            get { return _parentVisual; }
            set
            {
                if (_parentVisual == value) return;

                if (_parentVisual != null) Disconnect(_parentVisual);
                _parentVisual = value;
                if (_parentVisual != null) Connect(_parentVisual);
            }
        }
        /// <summary>
        /// Элемент, на который происходит отрисовка
        /// </summary>
        private TapeDrawingCanvas _parentVisual;
        /// <summary>
        /// Визуальный объект, в котором происходит все рисование
        /// </summary>
        //private readonly System.Windows.Media.DrawingVisual _visual;
        /// <summary>
        /// Графический контекст
        /// </summary>
        private readonly GraphicContext _graphicContext;

        private void Connect(TapeDrawingCanvas visual)
        {
            visual.OnDraw = ds =>
            {
                _graphicContext.Surface.Context = ds;
                _graphicContext.Surface.Width = (float)_parentVisual.ActualWidth;
                _graphicContext.Surface.Height = (float)_parentVisual.ActualHeight;

                _graphicContext.Surface.Context.PushClip(
                    new System.Windows.Media.RectangleGeometry(new System.Windows.Rect(0, 0, _graphicContext.Surface.Width,
                                                                        _graphicContext.Surface.Height)));

                Engine.Area = new Rectangle<float> { Right = _graphicContext.Surface.Width, Bottom = _graphicContext.Surface.Height };
                Engine.Draw();

                _graphicContext.Surface.Context.Pop();
            };
        }
        private void Disconnect(TapeDrawingCanvas visual)
        {
            visual.OnDraw = dc => { };
        }
        /// <summary>
        /// Выполняет полную перерисовку содержимого
        /// </summary>
        private void Redraw()
        {
            if (_parentVisual == null) return;

            _parentVisual.InvalidateVisual();
        }
    }
}
