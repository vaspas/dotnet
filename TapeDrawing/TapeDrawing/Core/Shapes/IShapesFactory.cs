using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Shapes
{
    public interface IShapesFactory
    {
        IDrawRectangleShape CreateDrawRectangle(IPen pen);
        IDrawRectangleAreaShape CreateDrawRectangleArea(IPen pen, Alignment alignment);
        IFillRectangleShape CreateFillRectangle(IBrush brush);
        IFillRectangleAreaShape CreateFillRectangleArea(IBrush brush, Alignment alignment);
        ITextShape CreateText(IFont font, Alignment alignment, float angle);
        ILinesShape CreateLines(IPen pen);
    	ILinesArrayShape CreateLinesArray(Color color);
        IPolygonShape CreatePolygon(IBrush brush);
        IFillAllShape CreateFillAll(Color color);
        IImageShape CreateImage(IImage image, Alignment alignment, float angle);
        //IPixelShape CreatePixel();
    }
}
