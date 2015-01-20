using SharpDX.Direct3D9;

namespace TapeDrawingSharpDx.Cache.FontCache
{
    /// <summary>
    /// Создает шрифт DirectX из шрифта WinForms
    /// </summary>
    class FontFromGdiCreator : ICacher<Font, System.Drawing.Font>
    {
        public DeviceDescriptor Device { get; set; }

        public Font Get(ref System.Drawing.Font gdiFont)
        {
            return new Font(Device.DxDevice, gdiFont);
        }

        public void Dispose()
        { }
    }
}
