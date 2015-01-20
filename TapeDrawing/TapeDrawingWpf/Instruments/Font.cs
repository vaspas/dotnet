using TapeDrawing.Core.Instruments;

namespace TapeDrawingWpf.Instruments
{
	/// <summary>
	/// Шрифт для рисования текста
	/// </summary>
	class Font : IFont
	{
		/// <summary>
		/// Информация о шрифте wpf
		/// </summary>
		public System.Windows.Media.Typeface ConcreteInstrument { get; set; }
		/// <summary>
		/// Кисть рисования шрифта. Хранит информацию о цвете
		/// </summary>
		public System.Windows.Media.Brush Brush { get; set; }
		/// <summary>
		/// Размер шрифта
		/// </summary>
		public float Size { get; set; }

		#region Implementation of IDisposable
		public void Dispose()
		{}
		#endregion
	}
}
