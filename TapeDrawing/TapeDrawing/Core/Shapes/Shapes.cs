using System.Collections.Generic;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Shapes
{
    public interface IDrawRectangleShape : IShape
    {
        void Render(Rectangle<float> rectangle);
    }

    public interface IDrawRectangleAreaShape : IShape
    {
        void Render(Point<float> point, Size<float> size);
        void Render(Point<float> point, Size<float> size, float marginX, float marginY);
    }

    public interface IFillRectangleShape : IShape
    {
        void Render(Rectangle<float> rectangle);
    }

    public interface IFillRectangleAreaShape : IShape
    {
        void Render(Point<float> point, Size<float> size);
        void Render(Point<float> point, Size<float> size, float marginX, float marginY);
    }

    public interface ITextShape : IShape
    {
        void Render(string text, Point<float> point);
    	Size<float> Measure(string text);
    }

	/// <summary>
	/// Интерфейс объекта для рисования соединенных между собой линий
	/// </summary>
    public interface ILinesShape : IShape
    {
        void Render(IEnumerable<Point<float>> points);
    }

	/// <summary>
	/// Интерфейс объекта для рисования отдельных линий единичной толщины
	/// </summary>
	public interface ILinesArrayShape : IShape
	{
		void Render(IEnumerable<Point<float>> points);
	}

    public interface IPolygonShape : IShape
    {
        void Render(IList<Point<float>> points);
    }

    public interface IFillAllShape : IShape
    {
        void Render();
    }

    public interface IImageShape : IShape
    {
        void Render(Point<float> point);
    }

    /*public interface IPixelShape : IShape
    {
        void Render(IPoint<float> point, IColor color);
    }*/
}
