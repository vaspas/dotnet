using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Font = Microsoft.DirectX.Direct3D.Font;

namespace TapeDrawingWinFormsDx.Cache.FontCache
{
    /// <summary>
    /// Создает шрифт DirectX из шрифта WinForms
    /// </summary>
    class FontFromGdiCreator : ICacher<Microsoft.DirectX.Direct3D.Font, System.Drawing.Font>
    {
        public DeviceDescriptor Device { get; set; }

        public bool Antialias { get; set; }

        public Microsoft.DirectX.Direct3D.Font Get(ref System.Drawing.Font gdiFont)
        {
            if (Antialias)
            {
                var fontDescriptor = new FontDescription
                                         {
                                             Height = gdiFont.Height,
                                             FaceName = gdiFont.Name,
                                             IsItalic = gdiFont.Italic,
                                             MipLevels = 0,
                                             OutputPrecision = Precision.Default,
                                             PitchAndFamily = PitchAndFamily.DefaultPitch,
                                             Quality = FontQuality.ClearType,
                                             Weight =
                                                 ((gdiFont.Style & FontStyle.Bold) != 0)
                                                     ? FontWeight.Bold
                                                     : FontWeight.Normal,
                                         };

                return new Font(Device.DxDevice, fontDescriptor);
            }

            return new Font(Device.DxDevice, gdiFont);
        }

        public void Dispose()
        { }
    }
}
