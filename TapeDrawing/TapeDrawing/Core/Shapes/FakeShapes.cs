using System.Collections.Generic;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Shapes
{
    public class FakeDisposedShape:IShape
    {
        public void Dispose()
        {
        }
    }

    public class FakeDrawRectangleShape : FakeDisposedShape, IDrawRectangleShape
    {
        public void Render(Rectangle<float> rectangle)
        {
            
        }
    }

    public class FakeDrawRectangleAreaShape : FakeDisposedShape, IDrawRectangleAreaShape
    {
        public void Render(Point<float> point, Size<float> size)
        {

        }

        public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
        {

        }
    }

    public class FakeFillRectangleShape : FakeDisposedShape, IFillRectangleShape
    {
        public void Render(Rectangle<float> rectangle)
        {
        }
    }

    public class FakeFillRectangleAreaShape : FakeDisposedShape, IFillRectangleAreaShape
    {
        public void Render(Point<float> point, Size<float> size)
        {
        }

        public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
        {

        }
    }

    public class FakeTextShape :  FakeDisposedShape,ITextShape
    {
        public  void Render(string text, Point<float> point)
        {
        }

    	public Size<float> Measure(string text)
    	{
    		return new Size<float>();
    	}
    }

	/// <summary>
	/// Интерфейс объекта для рисования соединенных между собой линий
	/// </summary>
    public class FakeLinesShape :  FakeDisposedShape,ILinesShape
    {
        public void Render(IEnumerable<Point<float>> points)
        {
        }
    }

	/// <summary>
	/// Интерфейс объекта для рисования отдельных линий единичной толщины
	/// </summary>
    public class FakeLinesArrayShape :  FakeDisposedShape,ILinesArrayShape
	{
		public void Render(IEnumerable<Point<float>> points)
		{
		}
	}

    public class FakePolygonShape :  FakeDisposedShape,IPolygonShape
    {
        public void Render(IList<Point<float>> points)
        {
        }
    }

    public class FakeFillAllShape :  FakeDisposedShape, IFillAllShape
    {
        public void Render()
        {
        }
    }

    public class FakeImageShape : FakeDisposedShape, IImageShape
    {
        public void Render(Point<float> point)
        {
        }
    }

    /*public class IPixelShape : IShape
    {
        void Render(IPoint<float> point, IColor color);
    }*/
}
