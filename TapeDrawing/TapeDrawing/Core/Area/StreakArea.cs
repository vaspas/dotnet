using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Area
{
    /// <summary>
    /// Область в виде полосы.
    /// </summary>
    public class StreakArea : IArea<float>
    {
        /// <summary>
        /// Относительное значение центра полосы.
        /// </summary>
        public float RelativeCenterPosition { get; set; }
        /// <summary>
        /// Ширина полосы.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Вертикальная
        /// </summary>
        public bool IsVertical { get; set; }

        /// <summary>
        /// Возвращает положение слоя.
        /// </summary>
        /// <param name="parentSize">Текущий размер родителя.</param>
        /// <returns></returns>
        public Rectangle<float> GetRectangle(Size<float> parentSize)
        {
            var p1 = RelativeCenterPosition*parentSize.Width - Width/2;
            var p2 = RelativeCenterPosition*parentSize.Width + Width/2;

            return new Rectangle<float>
                             {
                                 Left = IsVertical?p1:0,
                                 Right = IsVertical ? p2 : parentSize.Width,
                                 Bottom = IsVertical ? 0 : p1,
                                 Top = IsVertical ? parentSize.Height:p2
                             };
        }

    }
}
