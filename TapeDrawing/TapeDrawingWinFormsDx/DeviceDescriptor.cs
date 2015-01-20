using Microsoft.DirectX.Direct3D;

namespace TapeDrawingWinFormsDx
{
	/// <summary>
	/// Дескриптор устройства с доп. информацией
	/// </summary>
	public class DeviceDescriptor
	{
		/// <summary>
		/// Устройство DirectX
		/// </summary>
		public Device DxDevice { get; set; }
		/// <summary>
		/// Флаг запрета рисования
		/// </summary>
		public bool Pause { get; set; }
	}
}
