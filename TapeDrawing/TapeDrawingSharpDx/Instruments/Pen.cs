using SharpDX;
using SharpDX.Direct3D9;
using TapeDrawing.Core.Instruments;

namespace TapeDrawingSharpDx.Instruments
{
    /// <summary>
    /// Карандаш для рисования линий
    /// </summary>
    class Pen : IPen
    {
        /// <summary>
        /// Буферная линия для рисования
        /// </summary>
        public Line HLine { get; set; }

        /// <summary>
        /// Цвет линии
        /// </summary>
        public ColorBGRA Argb { get; set; }

        #region Implementation of IDisposable

        public void Dispose()
        {
            // За очистку отвечает объект, который создал элемент
        }

        #endregion
    }
}
