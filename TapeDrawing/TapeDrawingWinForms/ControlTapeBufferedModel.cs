
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TapeDrawing.Core.Engine;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinForms
{
    public class ControlTapeBufferedModel
    {
        public ControlTapeBufferedModel()
        {
            Engine = new DrawingEngine
                          {
                              GraphicContext = _graphicContext,
                              Area = default(Rectangle<float>)
                          };
        }
        

        private Control _control;

        public DrawingEngine Engine { get; private set; }

        private readonly GraphicContext _graphicContext = new GraphicContext();

        private BufferedGraphics _graphx;
        
        private void Connect(Control control)
        {
            Engine.Area = new Rectangle<float>
            {
                Right = control.ClientSize.Width,
                Bottom = control.ClientSize.Height
            };

            InitGraphics(control);

            control.Paint += ControlPaint;
            control.Resize += ControlResize;
            control.MouseMove += ControlMouseMove;
            control.MouseLeave += ControlMouseLeave;
            control.MouseUp += ControlMouseUp;
            control.MouseDown += ControlMouseDown;
            control.MouseWheel += ControlMouseWheel;
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
        }

        void ControlMouseLeave(object sender, System.EventArgs e)
        {
            Engine.OnMouseLeave();
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

        void ControlMouseMove(object sender, MouseEventArgs e)
        {
            Engine.OnMouseMove(new Point<float> { X = e.X, Y = e.Y });
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
            if (_graphx == null)
                return;

            _graphicContext.Graphics = _graphx.Graphics;
            _graphicContext.Graphics.SmoothingMode = SmoothingMode.None;
            _graphicContext.Graphics.CompositingQuality = CompositingQuality.AssumeLinear;
            _graphicContext.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            var control = (Control)sender;

            Engine.Draw();

            _graphx.Render(Graphics.FromHwnd(control.Handle));
        }

        private void ControlResize(object sender, System.EventArgs e)
        {
            var control = (Control)sender;

            Engine.Area = new Rectangle<float>
            {
                Right = control.Width,
                Bottom = control.Height
            };

            InitGraphics(control);
        }

        private void InitGraphics(Control control)
        {
            if (_graphx != null)
            {
                _graphx.Dispose();
                _graphx = null;
            }

            if (control.Width <= 0 || control.Height <= 0)
                return;

            _graphx = BufferedGraphicsManager.Current
                .Allocate(control.CreateGraphics(), new Rectangle(0, 0, control.Width, control.Height));
        }
    }
}
