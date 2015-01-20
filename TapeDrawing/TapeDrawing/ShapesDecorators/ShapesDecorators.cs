using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawing.Core.Translators;

namespace TapeDrawing.ShapesDecorators
{
    class DrawRectangleShapeDecorator : IDrawRectangleShape, IDecorator<IDrawRectangleShape>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public IDrawRectangleShape Target { get; set; }

        public void Render(Rectangle<float> rectangle)
        {
            var p1 = Translator.Translate(new Point<float> { X = rectangle.Left, Y = rectangle.Bottom });
            var p2 = Translator.Translate(new Point<float> { X = rectangle.Right, Y = rectangle.Top });

            Target.Render(new Rectangle<float>
            {
                Left = Math.Min(p1.X, p2.X),
                Right = Math.Max(p1.X, p2.X),
                Bottom = Math.Max(p1.Y, p2.Y),
                Top = Math.Min(p1.Y, p2.Y)
            });
        }

        public void Dispose()
        {
            if(Target!=null)
                Target.Dispose();
        }
    }

    class DrawRectangleAreaShapeDecorator : IDrawRectangleAreaShape, IDecorator<IDrawRectangleAreaShape>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public IDrawRectangleAreaShape Target { get; set; }

        public void Render(Point<float> point, Size<float> size)
        {
            Target.Render(Translator.Translate(point), size);
        }

        public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
        {
            Target.Render(Translator.Translate(point), size, marginX, marginY);
        }

        public void Dispose()
        {
            if (Target != null)
                Target.Dispose();
        }
    }

    class FillRectangleShapeDecorator : IFillRectangleShape, IDecorator<IFillRectangleShape>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public IFillRectangleShape Target { get; set; }

        public void Render(Rectangle<float> rectangle)
        {
            var p1 = Translator.Translate(new Point<float> {X = rectangle.Left, Y = rectangle.Bottom});
            var p2 = Translator.Translate(new Point<float> { X = rectangle.Right, Y = rectangle.Top });

            Target.Render(new Rectangle<float>
                              {
                                  Left = Math.Min(p1.X, p2.X),
                                  Right = Math.Max(p1.X, p2.X),
                                  Bottom = Math.Max(p1.Y, p2.Y),
                                  Top = Math.Min(p1.Y, p2.Y)
                              });
        }
        public void Dispose()
        {
            if (Target != null)
                Target.Dispose();
        }
    }

    class FillRectangleAreaShapeDecorator : IFillRectangleAreaShape, IDecorator<IFillRectangleAreaShape>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public IFillRectangleAreaShape Target { get; set; }

        public void Render(Point<float> point, Size<float> size)
        {
            Target.Render(Translator.Translate(point), size);
        }

        public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
        {
            Target.Render(Translator.Translate(point), size, marginX, marginY);
        }

        public void Dispose()
        {
            if (Target != null)
                Target.Dispose();
        }
    }

    class TextShapeDecorator : ITextShape, IDecorator<ITextShape>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public ITextShape Target { get; set; }

        public void Render(string text, Point<float> point)
        {
            Target.Render(text,
                Translator.Translate(point));
        }

    	public Size<float> Measure(string text)
    	{
    		return Target.Measure(text);
    	}

    	public void Dispose()
        {
            if (Target != null)
                Target.Dispose();
        }
    }

    class LinesShapeDecorator : ILinesShape, IDecorator<ILinesShape>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public ILinesShape Target { get; set; }

        public void Render(IEnumerable<Point<float>> points)
        {
            Target.Render(points.Select(p => Translator.Translate(p)));
        }

        public void Dispose()
        {
            if (Target != null)
                Target.Dispose();
        }
    }

    class PolygonShapeDecorator : IPolygonShape, IDecorator<IPolygonShape>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public IPolygonShape Target { get; set; }

        public void Render(IList<Point<float>> points)
        {
            Target.Render(points.ToList().ConvertAll(p => Translator.Translate(p)));
        }

        public void Dispose()
        {
            if (Target != null)
                Target.Dispose();
        }
    }

    class ImageShapeDecorator : IImageShape, IDecorator<IImageShape>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public IImageShape Target { get; set; }

        public void Render(Point<float> point)
        {
            Target.Render(Translator.Translate(point));
        }

        public void Dispose()
        {
            if (Target != null)
                Target.Dispose();
        }
    }

    /*class PixelShapeDecorator : IPixelShape, IDecorator<IPixelShape>, ITranslatorDecorator<IPointTranslator>
    {
        public IPointTranslator Translator { get; set; }

        public IPixelShape Target { get; set; }

        public void Render(IPoint<float> point, IColor color)
        {
            Target.Render(Translator.Translate(point),color);
        }

        public void Dispose()
        {
            if (Target != null)
                Target.Dispose();
        }
    }*/
}
