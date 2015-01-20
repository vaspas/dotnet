namespace TapeImplementTest.Factories
{
    /// <summary>
    /// Параметры теста
    /// </summary>
    public class TestParams
    {
        /// <summary>
        /// Количество отсчетов в источнике
        /// </summary>
        public int IndexLen { get; set; }
        /// <summary>
        /// Начальная координата
        /// </summary>
        public float Min { get; set; }
        /// <summary>
        /// Конечная координата
        /// </summary>
        public float Max { get; set; }
        /// <summary>
        /// Количество прерываний на ленте
        /// </summary>
        public int Interrupts { get; set; }
        /// <summary>
        /// Масштаб
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// Рисовать вертикально
        /// </summary>
        public bool Vertical { get; set; }
    }
}
