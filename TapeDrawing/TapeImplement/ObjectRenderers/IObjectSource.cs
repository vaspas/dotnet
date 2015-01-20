using System.Collections.Generic;

namespace TapeImplement.ObjectRenderers
{
    /// <summary>
    /// Источник данных об объектах
    /// </summary>
    public interface IObjectSource<out T>
    {
        IEnumerable<T> GetData(int from, int to);

        IEnumerable<T> GetData();
    }
}
