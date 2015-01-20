using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingWpf.Instruments;

namespace TapeDrawingWpf.Shapes
{
	/// <summary>
	/// Фабрика фигур
	/// </summary>
	class ShapesFactory : IShapesFactory
	{
		/// <summary>
		/// Поверхность для рисования
		/// </summary>
		public DrawSurface Surface { get; set; }

		public IDrawRectangleShape CreateDrawRectangle(IPen pen)
		{
			return new DrawRectangleShape { Surface = Surface, Pen = (Pen)pen };
		}

        public IDrawRectangleAreaShape CreateDrawRectangleArea(IPen pen, Alignment alignment)
        {
            return new DrawRectangleAreaShape { Surface = Surface, Pen = (Pen)pen, Alignment= alignment };
        }

	    public IFillRectangleShape CreateFillRectangle(IBrush brush)
		{
			return new FillRectangleShape { Surface = Surface, Brush = (Brush)brush };
		}

        public IFillRectangleAreaShape CreateFillRectangleArea(IBrush brush, Alignment alignment)
        {
            return new FillRectangleAreaShape { Surface = Surface, Brush = (Brush)brush, Alignment = alignment};
        }

	    public ITextShape CreateText(IFont font, Alignment alignment, float angle)
		{
			return new TextShape { Surface = Surface, Font = (Font)font, Alignment = alignment, Angle = angle };
		}

		public ILinesShape CreateLines(IPen pen)
		{
			return new LinesShape { Surface = Surface, Pen = (Pen)pen };
		}

		public ILinesArrayShape CreateLinesArray(Color color)
		{
			return new LinesArrayShape { Surface = Surface, Color = Converter.Convert(color) };
		}

        public IPolygonShape CreatePolygon(IBrush brush)
        {
            return new PolygonShape { Surface = Surface, Brush = (Brush)brush };
        }

		public IFillAllShape CreateFillAll(Color color)
		{
			return new FillAllShape { Surface = Surface, Color = Converter.Convert(color) };
		}

		public IImageShape CreateImage(IImage image, Alignment alignment, float angle)
		{
			return new ImageShape { Surface = Surface, Image = (Image)image, Alignment = alignment, Angle = angle };
		}

        /*public IPixelShape CreatePixel()
        {
            return null;
        }*/
	}
}
