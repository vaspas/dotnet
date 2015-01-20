using System;
using TapeDrawing.Core.Engine;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWpf
{
	/// <summary>
	/// Это примитив, на котором происходит рисование
	/// </summary>
	public class ControlTapeModel
	{
		public ControlTapeModel()
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
            if (_parentVisual == null) 
                return;

            _parentVisual.Dispatcher.BeginInvoke(
                new Action(_parentVisual.InvalidateVisual));
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



            visual.MouseMove += VisualMouseMove;
            visual.MouseLeave += VisualMouseLeave;
            visual.MouseUp += VisualMouseUp;
            visual.MouseDown += VisualMouseDown;
            visual.MouseWheel += VisualMouseWheel;
            visual.SizeChanged += VisualSizeChanged;

            VisualSizeChanged(visual, null);
        }
        
        void VisualMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Engine.OnMouseLeave();
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
        void VisualMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Engine.OnMouseWheel(e.Delta/120);
        }
	    void VisualMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
	    {
	        var button = Converter.Convert(e.ChangedButton);
            if (button == MouseButton.None) return;

            var el = (System.Windows.UIElement)sender;
	        el.CaptureMouse();

	        Engine.OnMouseButtonDown(button);
	    }
        void VisualMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var button = Converter.Convert(e.ChangedButton);
            if (button == MouseButton.None) return;

            Engine.OnMouseButtonUp(button);

            var el = (System.Windows.UIElement)sender;
            el.ReleaseMouseCapture();
        }
        void VisualMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var pos = e.GetPosition(_parentVisual);
            Engine.OnMouseMove(new Point<float> {X = (float) pos.X, Y = (float) pos.Y});
        }

        private void Disconnect(TapeDrawingCanvas visual)
        {
            visual.MouseMove -= VisualMouseMove;
            visual.MouseLeave -= VisualMouseLeave;
            visual.MouseUp -= VisualMouseUp;
            visual.MouseDown -= VisualMouseDown;
            visual.MouseWheel -= VisualMouseWheel;
            visual.SizeChanged -= VisualSizeChanged;

            visual.OnDraw = dc => { };
        }
	}
}
