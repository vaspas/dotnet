using SharpDX;
using SharpDX.Direct3D11;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingSharpDx11.Shapes
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
            //Device.Context.ClearDepthStencilView(Device.DepthView, DepthStencilClearFlags.Depth, 1.0f, 0);
            Device.Context.ClearRenderTargetView(Device.RenderView, Color);
		}
	}
}
