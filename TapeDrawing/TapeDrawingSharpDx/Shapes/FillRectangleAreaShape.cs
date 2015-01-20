using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingSharpDx.Shapes
{
	/// <summary>
	/// Фигура рисует закрашенный прямоугольник
	/// </summary>
	class FillRectangleAreaShape : BaseShape, IFillRectangleAreaShape
	{

        public void Render(Point<float> point, Size<float> size)
        {
            Render(point, size, 0, 0);
        }

	    public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
		{
            
		}
	}
}
