
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawing.Core.Translators;

namespace TapeDrawing.ShapesDecorators
{
    class AlignmentTranslatorShapesFactoryDecorator : IShapesFactory, IDecorator<IShapesFactory>, ITranslatorDecorator<IAlignmentTranslator>
    {
        public IAlignmentTranslator Translator { get; set; }

        public IShapesFactory Target { get; set; }


        public IDrawRectangleShape CreateDrawRectangle(IPen pen)
        {
            return Target.CreateDrawRectangle(pen);
        }

        public IDrawRectangleAreaShape CreateDrawRectangleArea(IPen pen, Alignment alignment)
        {
            return Target.CreateDrawRectangleArea(pen, Translator.Translate(alignment));
        }

        public IFillRectangleShape CreateFillRectangle(IBrush brush)
        {
            return Target.CreateFillRectangle(brush);
        }

        public IFillRectangleAreaShape CreateFillRectangleArea(IBrush brush, Alignment alignment)
        {
            return Target.CreateFillRectangleArea(brush, Translator.Translate(alignment));
        }

        public ITextShape CreateText(IFont font, Alignment alignment, float angle)
        {
            return  Target.CreateText(font,Translator.Translate(alignment), angle);
        }

        public ILinesShape CreateLines(IPen pen)
        {
            return Target.CreateLines(pen);
        }

    	public ILinesArrayShape CreateLinesArray(Color color)
    	{
    		return Target.CreateLinesArray(color);
    	}

    	public IPolygonShape CreatePolygon(IBrush brush)
        {
            return Target.CreatePolygon(brush);
        }

        public IFillAllShape CreateFillAll(Color color)
        {
            return Target.CreateFillAll(color);
        }

        public IImageShape CreateImage(IImage image, Alignment alignment, float angle)
        {
            return Target.CreateImage(image, Translator.Translate(alignment), angle);
        }

        /*public IPixelShape CreatePixel()
        {
            return Target.CreatePixel();
        }*/

    }
}
