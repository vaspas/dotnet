using SharpDX;
using TapeDrawing.Core.Instruments;

namespace TapeDrawingSharpDx11.Instruments
{
    /// <summary>
    /// Шрифт для рисования текста
    /// </summary>
    class Font : IFont
    {
        public Sprites.TextSprite.Font TextBlock;

        public Color4 Color;

        #region Implementation of IDisposable
        public void Dispose()
        {
            // За очистку отвечает объект, который создал элемент
        }
        #endregion
    }
}
