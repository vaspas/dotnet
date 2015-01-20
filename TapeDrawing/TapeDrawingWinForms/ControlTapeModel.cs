
using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TapeDrawing.Core.Engine;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinForms
{
    public class ControlTapeModel
    {
        public ControlTapeModel()
        {
            Engine = new DrawingEngine
                          {
                              GraphicContext = _graphicContext,
                              Area = default(Rectangle<float>)
                          };
        }

        private Control _control;

        public DrawingEngine Engine { private set; get; }

        private readonly GraphicContext _graphicContext = new GraphicContext();


        private void Connect(Control control)
        {
            Engine.Area = new Rectangle<float>
            {
                Right = control.ClientSize.Width,
                Bottom = control.ClientSize.Height
            };

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

        void ControlMouseLeave(object sender, System.EventArgs e)
        {
            Engine.OnMouseLeave();
        }

        void ControlMouseMove(object sender, MouseEventArgs e)
        {
            Engine.OnMouseMove(new Point<float>{X=e.X,Y=e.Y});
        }

        private void ControlResize(object sender, System.EventArgs e)
        {
            Engine.Area = new Rectangle<float>
                                  {
                                      Right = _control.ClientSize.Width,
                                      Bottom = _control.ClientSize.Height
                                  };
        }

        private void ControlMouseWheel(object sender, MouseEventArgs e)
        {
            Engine.OnMouseWheel(e.Delta / 120);
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
        }


        public Control Control
        {
            get { return _control; }
            set
            {
                if(_control==value)
                    return;

                if(_control!=null)
                    Disconnect(_control);

                _control = value;

                if (_control != null)
                    Connect(_control);
            }
        }


        private void ControlPaint(object sender, PaintEventArgs e)
        {
            _graphicContext.Graphics = e.Graphics;
            _graphicContext.Graphics.SmoothingMode = SmoothingMode.None;
            _graphicContext.Graphics.CompositingQuality = CompositingQuality.AssumeLinear;
            _graphicContext.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            
            Engine.Draw();
        }
    }
}
