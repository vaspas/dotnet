using SharpDX;
using SharpDX.Direct3D9;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingSharpDx.Shapes
{
	/// <summary>
	/// Фигура заливки графической поверхности
	/// </summary>
	class FillAllShape : BaseShape, IFillAllShape
	{
		/// <summary>
		/// Цвет заливки
		/// </summary>
		public ColorBGRA Color { get; set; }

		public void Render()
		{
            Device.DxDevice.Clear(ClearFlags.Target, Color, 1.0f, 0); 
		}
	}
}
