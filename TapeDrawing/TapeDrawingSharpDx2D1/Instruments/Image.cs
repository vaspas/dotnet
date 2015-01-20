
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingSharpDx2D1.Instruments
{
    /// <summary>
    /// Рисунок для отображения
    /// </summary>
    class Image : IImage
    {
        /// <summary>
        /// Ширина изображения
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Высота изображения
        /// </summary>
        public float Height { get; set; }
        
        public Rectangle<int> Roi;

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
