using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinFormsDx.Instruments
{
	/// <summary>
	/// Карандаш для рисования линий
	/// </summary>
	class Pen : IPen
	{
		/// <summary>
		/// Буферная линия для рисования
		/// </summary>
		public Line HLine { get; set; }

		/// <summary>
		/// Цвет линии
		/// </summary>
		public int Argb { get; set; }

		#region Implementation of IDisposable

		public void Dispose()
		{
			// За очистку отвечает объект, который создал элемент
		}

		#endregion
	}
}
