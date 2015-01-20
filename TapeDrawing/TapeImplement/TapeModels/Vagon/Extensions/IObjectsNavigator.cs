
using System.Collections.Generic;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public interface IObjectsNavigator<out T> where T:class
    {
        IEnumerable<T> GetNext(int from, int minCount);

        IEnumerable<T> GetPrevious(int from, int minCount);
    }
}
