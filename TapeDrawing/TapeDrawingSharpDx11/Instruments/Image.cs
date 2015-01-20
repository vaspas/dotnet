
using SharpDX.Direct3D11;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingSharpDx11.Instruments
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

        public ShaderResourceView TextureShaderResourceView;

        public Rectangle<int> Roi;

        #region Implementation of IDisposable
        public void Dispose()
        {
            // За очистку отвечает объект, который создал элемент
            TextureShaderResourceView.Dispose();
        }
        #endregion
    }
}
