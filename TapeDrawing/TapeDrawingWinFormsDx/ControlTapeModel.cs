using System;
using System.Windows.Forms;
using TapeDrawing.Core.Engine;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinFormsDx
{
	public class ControlTapeModel
	{
		public ControlTapeModel()
		{
            Engine = new DrawingEngine
			{
				GraphicContext = null,
                Area = default(Rectangle<float>)
			};
		}

		private Control _control;

        public DrawingEngine Engine { get; private set; }

        public Action<Exception> OnException { get; set; }

        private GraphicContext _graphicContext;

        public bool Antialias { get; set; }

		private void Connect(Control control)
		{
            _graphicContext = new GraphicContext { Antialias = Antialias };
		    Engine.GraphicContext = _graphicContext;

            Engine.Area = new Rectangle<float>
            {
                Right = control.ClientSize.Width,
                Bottom = control.ClientSize.Height
            };

			_graphicContext.Graphics.InitGraphics(control);

			control.Paint += ControlPaint;
            control.Resize += ControlResize;
            control.MouseMove += ControlMouseMove;
            control.MouseLeave += ControlMouseLeave;
            control.MouseUp += ControlMouseUp;
            control.MouseDown += ControlMouseDown;
            control.MouseWheel += ControlMouseWheel;
            control.KeyDown += ControlKeyDown;
            control.KeyUp += ControlKeyUp;
            control.LostFocus += (s, e) => Engine.LostFocus();
		}

        void ControlKeyDown(object sender, KeyEventArgs e)
        {
            Engine.OnKeyboardKeyDown(KeyConverter.Convert(e.KeyValue));
        }

        void ControlKeyUp(object sender, KeyEventArgs e)
        {
            Engine.OnKeyboardKeyUp(KeyConverter.Convert(e.KeyValue));
        }

        private void ControlMouseWheel(object sender, MouseEventArgs e)
        {
            Engine.OnMouseWheel(e.Delta/120);
        }

        private void ControlMouseUp(object sender, MouseEventArgs e)
        {
            var b = Converter.Convert(e.Button);
            if (b == MouseButton.None)
                return;

            Engine.OnMouseButtonUp(b);
        }

        private void ControlMouseDown(object sender, MouseEventArgs e)
        {
            var b = Converter.Convert(e.Button);
            if (b == MouseButton.None)
                return;

            Engine.OnMouseButtonDown(b);
        }

        void ControlResize(object sender, EventArgs e)
        {
            Engine.Area = new Rectangle<float>
                                  {
                                      Right = _control.ClientSize.Width,
                                      Bottom = _control.ClientSize.Height
                                  };

            Resize();
        }

        void ControlMouseLeave(object sender, EventArgs e)
        {
            Engine.OnMouseLeave();
        }

        void ControlMouseMove(object sender, MouseEventArgs e)
        {
            Engine.OnMouseMove(new Point<float> {X = e.X, Y = e.Y});
        }


		private void Disconnect(Control control)
		{
			control.Paint -= ControlPaint;
            control.Resize -= ControlResize;
            control.MouseMove -= ControlMouseMove;
            control.MouseLeave -= ControlMouseLeave;
            control.MouseUp -= ControlMouseUp;
            control.MouseDown -= ControlMouseDown;
            control.MouseWheel -= ControlMouseWheel;
            control.KeyDown -= ControlKeyDown;
            control.KeyUp -= ControlKeyUp;

            Engine.GraphicContext = null;
			_graphicContext.Dispose();
		}

		public Control Control
		{
			get { return _control; }
			set
			{
				if (_control == value)
					return;

				if (_control != null)
					Disconnect(_control);

				_control = value;

				if (_control != null)
					Connect(_control);
			}
		}

	    private volatile bool _watchdog;
        private void ControlPaint(object sender, PaintEventArgs e)
        {
            try
            {
                // Не рисовать в режиме паузы
                if (_graphicContext.Graphics.Device.Pause) return;

                if(_watchdog)
                    return;
                _watchdog = true;

                _graphicContext.Graphics.StartDraw();

                Engine.Draw();

                _graphicContext.Graphics.EndDraw();
                _graphicContext.Graphics.Show();
                _watchdog = false;
            }
            catch (Exception ex)
            {
                if (OnException != null)
                    OnException(ex);
                throw;
            }
        }

	    public event Action Resize = delegate { };

	}
}
