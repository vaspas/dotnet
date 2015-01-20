namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Базовый клаас для рисовальщиков координатной сетки
    /// </summary>
    public abstract class BaseCoordGridRenderer
    {
        /// <summary>
        /// Источник данных координат
        /// </summary>
        public ICoordinateSource Source { get; set; }
        /// <summary>
        /// Диапазон рисования
        /// </summary>
        public IScalePosition<int> TapePosition { get; set; }
    }
}
