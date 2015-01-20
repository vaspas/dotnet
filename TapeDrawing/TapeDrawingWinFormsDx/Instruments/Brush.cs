using TapeDrawing.Core.Instruments;

namespace TapeDrawingWinFormsDx.Instruments
{
	/// <summary>
	/// Кисть для заливки областей
	/// </summary>
	class Brush : IBrush
	{
		/// <summary>
		/// Цвет кисти
		/// </summary>
		public int Argb { get; set; }

        /// <summary>
        /// Прозрачность отдельно
        /// </summary>
        public int A { get; set; }

		#region Implementation of IDisposable
		public void Dispose()
		{
			// Очистка не требуется
		}
		#endregion
	}
}
