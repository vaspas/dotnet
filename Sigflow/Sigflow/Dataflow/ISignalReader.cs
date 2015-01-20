
namespace Sigflow.Dataflow
{
    public interface ISignalReader
    {}

    public interface ISignalReader<T> : ISignalReader
        where T : struct
    {
        /// <summary>
        /// Чтение данных в массив. После чтения данные исчезают из канала.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool ReadTo(T[] data);

        /// <summary>
        /// Кол-во хранящихся отсчетов.
        /// </summary>
        /// <returns></returns>
        int Available { get; }


        /// <summary>
        /// Возвращает размер блока для чтения, или null если нет блоков.
        /// </summary>
        int? NextBlockSize { get; }

        /// <summary>
        /// Получение блока данных. После выполнения данные исчезают из канала.
        /// </summary>
        /// <returns></returns>
        T[] Take();

        /// <summary>
        /// Положить обратно взятый массив данных.
        /// </summary>
        /// <returns></returns>
        void Put(T[] data);

        /// <summary>
        /// Пропустить данные, не обязательно будет пропущен указанный размер.
        /// </summary>
        void TrySkip(int size);
    }
}
