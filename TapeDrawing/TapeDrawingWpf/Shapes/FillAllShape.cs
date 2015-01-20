using TapeDrawing.Core.Shapes;

namespace TapeDrawingWpf.Shapes
{
	/// <summary>
	/// Фигура заливки графической поверхности
	/// </summary>
	class FillAllShape : BaseShape, IFillAllShape
	{
		/// <summary>
		/// Цвет заливки
		/// </summary>
		public System.Windows.Media.Color Color { get; set; }

		public void Render()
		{
			Surface.Context.DrawRectangle( new System.Windows.Media.SolidColorBrush(Color), null,
			                              new System.Windows.Rect(0, 0, Surface.Width, Surface.Height));

            
		}
	}
}
