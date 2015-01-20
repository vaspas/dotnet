namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Реализация прерывания
    /// </summary>
    public class CoordInterrupt : ICoordInterrupt
    {
        /// <summary>
        /// Обозначение отметки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Положение на ленте в индексах
        /// </summary>
        public int Index { get; set; }
    }
}
