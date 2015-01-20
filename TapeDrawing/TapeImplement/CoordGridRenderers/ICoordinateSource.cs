namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Источник данных для координат пути
    /// </summary>
    public interface ICoordinateSource
    {
        /// <summary>
        /// Позволяет получить все отметки для указанного диапазона ленты
        /// </summary>
        /// <param name="from">Начальный индекс диапазона</param>
        /// <param name="to">Конечный индекс диапазона</param>
        /// <returns>Отсортированный массив отметок (прерываний ленты)</returns>
        ICoordInterrupt[] GetCoordInterrupts(int from, int to);

        /// <summary>
        /// Возвращает значение координаты в единицах длины для указанного индекса
        /// </summary>
        /// <param name="index">Индекс сигнала</param>
        /// <returns>Координата на пути. Если индекс вне диапазона, то должен всегда возвращаться 0</returns>
        float GetUnitValue(int index);
    }
}
