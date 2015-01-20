using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingWpf.Instruments;

namespace TapeDrawingWpf.Shapes
{
	/// <summary>
	/// Фигура рисует закрашенный прямоугольник
	/// </summary>
	class FillRectangleShape : BaseShape, IFillRectangleShape
	{
	    /// <summary>
	    /// Кисть заливки
	    /// </summary>
	    public Brush Brush;

		public void Render(Rectangle<float> rectangle)
		{
			Surface.Context.DrawRectangle(Brush.ConcreteInstrument, null, Converter.Convert(rectangle));
		}
	}
}
