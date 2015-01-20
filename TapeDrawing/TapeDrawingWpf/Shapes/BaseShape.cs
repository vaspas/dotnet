using System;

namespace TapeDrawingWpf.Shapes
{
	/// <summary>
	/// Базовый класс для фигур
	/// </summary>
	abstract class BaseShape : IDisposable
	{
		/// <summary>
		/// Поверхность для рисования
		/// </summary>
		public DrawSurface Surface { get; set; }

		#region Implementation of IDisposable
		public virtual void Dispose()
		{ }
		#endregion
	}
}
