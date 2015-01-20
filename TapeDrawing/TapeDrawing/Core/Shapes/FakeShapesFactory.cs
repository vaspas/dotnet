using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Shapes
{
    public class FakeShapesFactory: IShapesFactory
    {
        public IDrawRectangleShape CreateDrawRectangle(IPen pen)
        {
            return new FakeDrawRectangleShape();
        }

        public IDrawRectangleAreaShape CreateDrawRectangleArea(IPen pen, Alignment alignment)
        {
            return new FakeDrawRectangleAreaShape();
        }

        public IFillRectangleShape CreateFillRectangle(IBrush brush)
        {
            return new FakeFillRectangleShape();
        }

        public IFillRectangleAreaShape CreateFillRectangleArea(IBrush brush, Alignment alignment)
        {
            return new FakeFillRectangleAreaShape();
        }

        public ITextShape CreateText(IFont font, Alignment alignment, float angle)
        {
            return new FakeTextShape();
        }

        public ILinesShape CreateLines(IPen pen)
        {
            return new FakeLinesShape();
        }

        public ILinesArrayShape CreateLinesArray(Color color)
        {
            return new FakeLinesArrayShape();
        }

        public IPolygonShape CreatePolygon(IBrush brush)
        {
            return new FakePolygonShape();
        }

        public IFillAllShape CreateFillAll(Color color)
        {
            return new FakeFillAllShape();
        }

        public IImageShape CreateImage(IImage image, Alignment alignment, float angle)
        {
            return new FakeImageShape();
        }

        //IPixelShape CreatePixel();
    }
}
