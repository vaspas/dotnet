using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingWinForms.Instruments;

namespace TapeDrawingWinForms.Shapes
{
    class ShapesFactory : IShapesFactory
    {
        public GraphicContext GraphicContext { get; set; }

        public IDrawRectangleShape CreateDrawRectangle(IPen pen)
        {
            return new DrawRectangleShape
                       {
                           Graphics = GraphicContext.Graphics,
                           Pen = (pen as Pen).ConcreteInstrument
                       };
        }

        public IDrawRectangleAreaShape CreateDrawRectangleArea(IPen pen,Alignment alignment)
        {
            return new DrawRectangleAreaShape
            {
                Graphics = GraphicContext.Graphics,
                Pen = (pen as Pen).ConcreteInstrument,
                Alignment = alignment
            };
        }

        public IFillRectangleShape CreateFillRectangle(IBrush brush)
        {
            return new FillRectangleShape
            {
                Graphics = GraphicContext.Graphics,
                Brush = (brush as Brush).ConcreteInstrument
            };
        }

        public IFillRectangleAreaShape CreateFillRectangleArea(IBrush brush, Alignment alignment)
        {
            return new FillRectangleAreaShape
            {
                Graphics = GraphicContext.Graphics,
                Brush = (brush as Brush).ConcreteInstrument,
                Alignment = alignment
            };
        }

        public ITextShape CreateText(IFont font, Alignment alignment, float angle)
        {
            return new TextShape
            {
                Graphics = GraphicContext.Graphics,
                Font = (font as Font).ConcreteInstrument,
                Brush = (font as Font).Brush,
                Alignment = alignment,
                Angle = angle
            };
        }

        public ILinesShape CreateLines(IPen pen)
        {
            return new LinesShape
            {
                Graphics = GraphicContext.Graphics,
                Pen = (pen as Pen).ConcreteInstrument
            };
        }

		public ILinesArrayShape CreateLinesArray(Color color)
		{
			return new LinesArrayShape { Graphics = GraphicContext.Graphics, Color = Converter.Convert(color) };
		}

    	public IPolygonShape CreatePolygon(IBrush brush)
        {
            return new PolygonShape
            {
                Graphics = GraphicContext.Graphics,
                Brush = (brush as Brush).ConcreteInstrument
            };
        }

        public IFillAllShape CreateFillAll(Color color)
        {
            return new FillAllShape
            {
                Graphics = GraphicContext.Graphics,
                Color = Converter.Convert(color)
            };
        }

        public IImageShape CreateImage(IImage image, Alignment alignment, float angle)
        {
            return new ImageShape
            {
                Graphics = GraphicContext.Graphics,
                Image = image as Image,
                Alignment = alignment,
                Angle = angle
            };
        }

        /*public IPixelShape CreatePixel()
        {
            return null;
        }*/
    }
}
