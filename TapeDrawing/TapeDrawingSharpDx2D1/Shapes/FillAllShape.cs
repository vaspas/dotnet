using SharpDX;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingSharpDx2D1.Shapes
{
	/// <summary>
	/// Фигура заливки графической поверхности
	/// </summary>
	class FillAllShape : BaseShape, IFillAllShape
	{
		/// <summary>
		/// Цвет заливки
		/// </summary>
		public Color4 Color { get; set; }

		public void Render()
		{
            Device.RenderTarget2D.Clear(Color);
		}
	}
}
