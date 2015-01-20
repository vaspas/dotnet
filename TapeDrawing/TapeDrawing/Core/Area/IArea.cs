
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Area
{
    /// <summary>
    /// Для объектов позиционирования слоя.
    /// </summary>
    public interface IArea<T> where T:struct
    {
        /// <summary>
        /// Возвращает расположение слоя на родительском слое.
        /// </summary>
        /// <param name="parentArea">Размер родительского слоя.</param>
        /// <returns></returns>
        Rectangle<T> GetRectangle(Size<T> parentArea);
    }
}
