using TapeDrawing.Core.Instruments;

namespace TapeDrawingWpf.Instruments
{
	/// <summary>
	/// Кисть для заливки областей
	/// </summary>
	class Brush : IBrush
	{
		/// <summary>
		/// Кисть Wpf
		/// </summary>
		public System.Windows.Media.Brush ConcreteInstrument { get; set; }

		#region Implementation of IDisposable
		public void Dispose()
		{}
		#endregion
	}
}
