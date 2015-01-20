using SharpDX;
using SharpDX.Direct3D9;
using TapeDrawing.Core.Instruments;

namespace TapeDrawingSharpDx.Instruments
{
    /// <summary>
    /// Шрифт для рисования текста
    /// </summary>
    class Font : IFont
    {
        #region Implementation of IDisposable
        public void Dispose()
        {
            // За очистку отвечает объект, который создал элемент
        }
        #endregion
    }
}
