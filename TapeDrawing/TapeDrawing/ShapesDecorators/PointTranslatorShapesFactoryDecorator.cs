
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawing.Core.Translators;

namespace TapeDrawing.ShapesDecorators
{
    class PointTranslatorShapesFactoryDecorator : IShapesFactory, IDecorator<IShapesFactory>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public IShapesFactory Target { get; set; }


        public IDrawRectangleShape CreateDrawRectangle(IPen pen)
        {
            return DecoratorsFactory.CreateFor(
                Target.CreateDrawRectangle(pen), Translator);
        }

        public IDrawRectangleAreaShape CreateDrawRectangleArea(IPen pen, Alignment alignment)
        {
            return DecoratorsFactory.CreateFor(
                Target.CreateDrawRectangleArea(pen, alignment), Translator);
        }

        public IFillRectangleShape CreateFillRectangle(IBrush brush)
        {
            return DecoratorsFactory.CreateFor(
                Target.CreateFillRectangle(brush), Translator);
        }

        public IFillRectangleAreaShape CreateFillRectangleArea(IBrush brush, Alignment alignment)
        {
            return DecoratorsFactory.CreateFor(
                Target.CreateFillRectangleArea(brush, alignment), Translator);
        }

        public ITextShape CreateText(IFont font, Alignment alignment, float angle)
        {
            return DecoratorsFactory.CreateFor(
                Target.CreateText(font, alignment, angle), Translator);
        }

        public ILinesShape CreateLines(IPen pen)
        {
            return DecoratorsFactory.CreateFor(
                Target.CreateLines(pen), Translator);
        }

		public ILinesArrayShape CreateLinesArray(Color color)
		{
			return DecoratorsFactory.CreateFor(Target.CreateLinesArray(color), Translator);
		}

    	public IPolygonShape CreatePolygon(IBrush brush)
        {
            return DecoratorsFactory.CreateFor(
                Target.CreatePolygon(brush), Translator);
        }

        public IFillAllShape CreateFillAll(Color color)
        {
            return DecoratorsFactory.CreateFor(
                Target.CreateFillAll(color), Translator);
        }

        public IImageShape CreateImage(IImage image, Alignment alignment, float angle)
        {
            return DecoratorsFactory.CreateFor(
                Target.CreateImage(image, alignment, angle), Translator);
        }

        /*public IPixelShape CreatePixel()
        {
            return ShapesDecoratorsFactory.CreateFor(
                Target.CreatePixel(), Translator);
        }*/

    }
}
