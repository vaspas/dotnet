using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingWpf.Instruments;

namespace TapeDrawingWpf.Shapes
{
	/// <summary>
	/// Фигура для рисования незакрашенный прямоугольников
	/// </summary>
	class DrawRectangleShape : BaseShape, IDrawRectangleShape
	{
	    /// <summary>
	    /// Карандаш для рисования
	    /// </summary>
	    public Pen Pen;

		public void Render(Rectangle<float> rectangle)
		{
			Surface.Context.DrawRectangle(null, Pen.ConcreteInstrument, Converter.Convert(rectangle));
		}
	}
}
