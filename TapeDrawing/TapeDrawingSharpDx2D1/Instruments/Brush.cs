using SharpDX;
using TapeDrawing.Core.Instruments;

namespace TapeDrawingSharpDx2D1.Instruments
{
    /// <summary>
    /// Кисть для заливки областей
    /// </summary>
    class Brush : IBrush
    {
        /// <summary>
        /// Цвет кисти
        /// </summary>
        public Vector4 Argb { get; set; }
        
        #region Implementation of IDisposable
        public void Dispose()
        {
            // Очистка не требуется
        }
        #endregion
    }
}
