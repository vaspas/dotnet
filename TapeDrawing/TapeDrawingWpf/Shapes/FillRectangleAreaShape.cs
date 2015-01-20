using System.Windows;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingWpf.Instruments;

namespace TapeDrawingWpf.Shapes
{
	/// <summary>
	/// Фигура рисует закрашенный прямоугольник
	/// </summary>
	class FillRectangleAreaShape : BaseShape, IFillRectangleAreaShape
	{
	    /// <summary>
	    /// Кисть заливки
	    /// </summary>
	    public Brush Brush;

        public Alignment Alignment { get; set; }

		public void Render(Rectangle<float> rectangle)
		{
			Surface.Context.DrawRectangle(Brush.ConcreteInstrument, null, Converter.Convert(rectangle));
		}

        public void Render(Point<float> point, Size<float> size)
        {
            Render(point, size, 0, 0);
        }

	    public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
	    {
            float shiftX = 0;
            if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
                || ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
            {
                shiftX += size.Width / 2;
            }
            else if ((Alignment & Alignment.Right) != 0)
            {
                shiftX += size.Width - marginX;
            }
            else
                shiftX += marginX;
            float shiftY = 0;
            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                shiftY += size.Height / 2;
            }
            else if ((Alignment & Alignment.Bottom) != 0)
            {
                shiftY += size.Height - marginY;
            }
            else
                shiftY += marginY;

	        Surface.Context.DrawRectangle(Brush.ConcreteInstrument, null,
	                                      new Rect
	                                          (new Point(point.X - shiftX, point.Y - shiftY),
	                                           new Size(size.Width, size.Height)));
	    }
	}
}
