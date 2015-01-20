using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Area
{
    public class VerticalScrollArea : IArea<float>
    {
        /// <summary>
        /// Высота области.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Относительное значение смещения, от 0 до 1.
        /// </summary>
        public float ScrollValue { get; set; }

        /// <summary>
        /// Возвращает значение относительного перекрытия области на родительском слое. >=1, область послоностью попадает на слой.
        /// </summary>
        public float Overlap { get; private set; }

        /// <summary>
        /// Фиксированное значение смещения, при условии что Overlap>1.
        /// </summary>
        public float? FixedScrollValueIfOverlap { get; set; }

        /// <summary>
        /// Возвращает положение слоя.
        /// </summary>
        /// <param name="parentSize">Текущий размер родителя.</param>
        /// <returns></returns>
        public Rectangle<float> GetRectangle(Size<float> parentSize)
        {
            Overlap = parentSize.Height/Height;

            if (parentSize.Height > Height)
            {
                var scroll = FixedScrollValueIfOverlap ?? ScrollValue;
                return new Rectangle<float>
                           {
                               Left = 0,
                               Right = parentSize.Width,

                               Bottom = (1 - scroll)*(parentSize.Height - Height),
                               Top = parentSize.Height + scroll * (Height - parentSize.Height)
                           };
            }

            return new Rectangle<float>
                       {
                           Left = 0,
                           Right = parentSize.Width,

                           Bottom = ScrollValue * (parentSize.Height - Height),
                           Top = Height + ScrollValue * (parentSize.Height - Height)
                       };
        }

    }
}
