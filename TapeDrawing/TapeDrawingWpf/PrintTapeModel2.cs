using TapeDrawing.Core.Engine;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWpf
{
	/// <summary>
	/// Это примитив, на котором происходит рисование
	/// </summary>
	public class PrintTapeModel2
	{
        public PrintTapeModel2()
		{
			// Создадим графический контекст и движок
		    _graphicContext = new GraphicContext {Surface = new DrawSurface()};
		    Engine = new DrawingEngine {GraphicContext = _graphicContext};
		}
        
		/// <summary>
		/// Движок рисования
		/// </summary>
		public DrawingEngine Engine { private set; get; }
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
        /// Выполняет полную перерисовку содержимого
        /// </summary>
        public void Redraw()
        {
            if (_parentVisual == null) return;

            _parentVisual.InvalidateVisual();
            
        }

        /// <summary>
        /// Элемент, на который происходит отрисовка
        /// </summary>
        private TapeDrawingCanvas _parentVisual;
		/// <summary>
		/// Графический контекст
		/// </summary>
		private readonly GraphicContext _graphicContext;

        private void Connect(TapeDrawingCanvas visual)
        {
            visual.OnDraw = ds =>
            {
                _graphicContext.Surface.Context = ds;

                Engine.Draw();
            };

            visual.SizeChanged += VisualSizeChanged;

            VisualSizeChanged(visual, null);
        }
        

        void VisualSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var w = e == null ? (float) _parentVisual.ActualWidth : (float)e.NewSize.Width;
            var h = e == null ? (float)_parentVisual.ActualHeight : (float)e.NewSize.Height;

            _graphicContext.Surface.Width = w;
            _graphicContext.Surface.Height = h;
            Engine.Area = new Rectangle<float>
            {
                Right = w,
                Bottom = h
            };

            if (e != null) Redraw();
        }
        private void Disconnect(TapeDrawingCanvas visual)
        {
            visual.SizeChanged -= VisualSizeChanged;

            visual.OnDraw = ds => { };
        }
	}
}
