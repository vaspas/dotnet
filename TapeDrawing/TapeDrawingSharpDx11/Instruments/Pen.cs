using SharpDX;
using TapeDrawing.Core.Instruments;

namespace TapeDrawingSharpDx11.Instruments
{
    /// <summary>
    /// Карандаш для рисования линий
    /// </summary>
    class Pen : IPen
    {
        /// <summary>
        /// Цвет кисти
        /// </summary>
        public Vector4 Argb { get; set; }

        public float Width;

        public float Dash1;
        public float Dash2;
        public float Dash3;
        public float Dash4;

        #region Implementation of IDisposable

        public void Dispose()
        {
            // За очистку отвечает объект, который создал элемент
        }

        #endregion
    }
}
