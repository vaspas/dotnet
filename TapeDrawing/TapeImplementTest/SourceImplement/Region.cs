using TapeImplement.ObjectRenderers;

namespace TapeImplementTest.SourceImplement
{
    class Region
    {
        /// <summary>
        /// Начало участка протяженного объекта
        /// </summary>
        public int From { get; set; }
        /// <summary>
        /// Конец участка протяженного объекта
        /// </summary>
        public int To { get; set; }
    }

    /// <summary>
    /// Реализация протяженного объекта
    /// </summary>
    /// <typeparam name="T">Тип объекта, имеющего границы</typeparam>
    class Region<T> : Region
    {
        /// <summary>
        /// Собственно объект
        /// </summary>
        public T Target { get; set; }
    }
}
