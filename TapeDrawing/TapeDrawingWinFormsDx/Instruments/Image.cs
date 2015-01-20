using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinFormsDx.Instruments
{
	/// <summary>
	/// Рисунок для отображения
	/// </summary>
	class Image : IImage
	{
		/// <summary>
		/// Ширина изображения
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Высота изображения
		/// </summary>
		public float Height { get; set; }

	    /// <summary>
	    /// Спрайт для рисования изображений
	    /// </summary>
	    public Sprite ImageSprite;

	    /// <summary>
	    /// Текстура для отображения
	    /// </summary>
	    public Texture Texture;

	    public Rectangle<int> Roi;

		#region Implementation of IDisposable
		public void Dispose()
		{
			// За очистку отвечает объект, который создал элемент
		}
		#endregion
	}
}
