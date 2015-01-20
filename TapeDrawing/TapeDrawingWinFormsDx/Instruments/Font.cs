using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Instruments;

namespace TapeDrawingWinFormsDx.Instruments
{
	/// <summary>
	/// Шрифт для рисования текста
	/// </summary>
	class Font : IFont
	{
		/// <summary>
		/// Шрифт DirectX
		/// </summary>
		public Microsoft.DirectX.Direct3D.Font FontD3D { get; set; }
		/// <summary>
		/// Спрайт для рисования текста
		/// </summary>
		public Sprite TextSprite { get; set; }
		/// <summary>
		/// Цвет текста
		/// </summary>
		public System.Drawing.Color Color { get; set; }

		#region Implementation of IDisposable
		public void Dispose()
		{
			// За очистку отвечает объект, который создал элемент
		}
		#endregion
	}
}
