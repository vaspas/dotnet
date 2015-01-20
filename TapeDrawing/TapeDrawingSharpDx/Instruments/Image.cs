using SharpDX.Direct3D9;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingSharpDx.Instruments
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

        #region Implementation of IDisposable
        public void Dispose()
        {
            // За очистку отвечает объект, который создал элемент
        }
        #endregion
    }
}
