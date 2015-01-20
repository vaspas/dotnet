using SharpDX;
using TapeDrawing.Core.Instruments;

namespace TapeDrawingSharpDx2D1.Instruments
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
