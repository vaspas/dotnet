using TapeDrawing.Core.Shapes;

namespace TapeDrawingWinFormsDx.Shapes
{
	/// <summary>
	/// Фигура заливки графической поверхности
	/// </summary>
	class FillAllShape : BaseShape, IFillAllShape
	{
		/// <summary>
		/// Цвет заливки
		/// </summary>
		public System.Drawing.Color Color { get; set; }

		public void Render()
		{
			Device.DxDevice.Clear(Microsoft.DirectX.Direct3D.ClearFlags.Target, Color, 1.0f, 0);
		}
	}
}
