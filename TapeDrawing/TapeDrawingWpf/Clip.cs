using System;
using System.Windows;
using System.Windows.Media;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWpf
{
    class Clip:IClip
    {
        public Clip(DrawSurface gr)
        {
            _gr = gr;
        }

        private readonly DrawSurface _gr;

        public void Set(Rectangle<float> rectangle)
        {
            _gr.Context.PushClip(new RectangleGeometry(new Rect(
                    rectangle.Left, rectangle.Top,
                    Math.Abs(rectangle.Right - rectangle.Left), Math.Abs(rectangle.Top - rectangle.Bottom))));
        }

        public void Undo()
        {
            _gr.Context.Pop();
        }
    }
}
