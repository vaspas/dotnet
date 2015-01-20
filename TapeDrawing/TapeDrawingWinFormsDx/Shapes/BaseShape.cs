using System;

namespace TapeDrawingWinFormsDx.Shapes
{
	abstract class BaseShape : IDisposable
	{
		/// <summary>
		/// Устройство, на котором производится рисование
		/// </summary>
		public DeviceDescriptor Device { get; set; }

		#region Implementation of IDisposable
		public virtual void Dispose()
		{}
		#endregion
	}
}
