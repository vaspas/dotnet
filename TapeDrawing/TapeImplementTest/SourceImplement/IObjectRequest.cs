using System.Collections.Generic;
using TapeImplement;

namespace TapeImplementTest.SourceImplement
{
    // ReSharper disable UnusedTypeParameter

    /// <summary>
    /// Интерфейс объекта, который выполняет определенные виды запросов к источнику данных
    /// </summary>
    internal interface IObjectRequest
    { }
    internal interface IObjectRequest<TData> : IObjectRequest
    {
        List<TData> Get(int from, int to);
    }

    // ReSharper restore UnusedTypeParameter
}
