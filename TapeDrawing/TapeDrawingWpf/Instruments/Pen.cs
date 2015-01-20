using TapeDrawing.Core.Instruments;

namespace TapeDrawingWpf.Instruments
{
	/// <summary>
	/// Карандаш для рисования линий
	/// </summary>
	class Pen : IPen
	{
		/// <summary>
		/// Карандаш Wpf
		/// </summary>
		public System.Windows.Media.Pen ConcreteInstrument { get; set; }

		#region Implementation of IDisposable
		public void Dispose()
		{}
		#endregion
	}
}
