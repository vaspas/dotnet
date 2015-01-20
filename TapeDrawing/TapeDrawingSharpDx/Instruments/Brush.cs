using SharpDX;
using TapeDrawing.Core.Instruments;

namespace TapeDrawingSharpDx.Instruments
{
    /// <summary>
    /// Кисть для заливки областей
    /// </summary>
    class Brush : IBrush
    {
        /// <summary>
        /// Цвет кисти
        /// </summary>
        public Color Argb { get; set; }
        
        #region Implementation of IDisposable
        public void Dispose()
        {
            // Очистка не требуется
        }
        #endregion
    }
}
