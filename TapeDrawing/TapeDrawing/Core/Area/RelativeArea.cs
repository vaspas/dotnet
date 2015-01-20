
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Area
{
    /// <summary>
    /// Объект для относительного расположения слоя на родителе.
    /// </summary>
    public class RelativeArea : IArea<float>
    {
        /// <summary>
        /// Возвращает и устнавливает текущее расположение слоя в относительных значениях.
        /// </summary>
        public Rectangle<float> CurrentRelative;

        /// <summary>
        /// Возвращает координаты слоя на родителе.
        /// </summary>
        /// <param name="parentSize">Текущие размеры родителя.</param>
        /// <returns></returns>
        public Rectangle<float> GetRectangle(Size<float> parentSize)
        {
            return new Rectangle<float>
                       {
                           Left = CurrentRelative.Left*parentSize.Width,
                           Right = CurrentRelative.Right*parentSize.Width,
                           Top = CurrentRelative.Top*parentSize.Height,
                           Bottom = CurrentRelative.Bottom*parentSize.Height
                       };
        }
    }
}
