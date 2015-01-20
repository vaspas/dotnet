using System;
using System.Drawing;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinForms
{
    class Clip:IClip
    {
        public Clip(Graphics gr)
        {
            _saved = gr.ClipBounds;
            _gr = gr;
        }

        private readonly RectangleF _saved;
        private readonly Graphics _gr;

        public void Set(Rectangle<float> rectangle)
        {
            _gr.SetClip(new RectangleF(rectangle.Left, rectangle.Top,
                                       Math.Abs(rectangle.Right - rectangle.Left),
                                       Math.Abs(rectangle.Top - rectangle.Bottom)));
        }

        public void Undo()
        {
            _gr.SetClip(_saved);
        }
    }
}
