using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Area
{
    /// <summary>
    /// Область с указанием отступов.
    /// </summary>
    public class MarginsArea : IArea<float>
    {
        /// <summary>
        /// Отступ слева или null если нет привязки.
        /// </summary>
        public float? Left { get; set; }
        /// <summary>
        /// Отступ справа или null если нет привязки.
        /// </summary>
        public float? Right { get; set; }
        /// <summary>
        /// Отступ снизу или null если нет привязки.
        /// </summary>
        public float? Bottom { get; set; }
        /// <summary>
        /// Отступ сверху или null если нет привязки.
        /// </summary>
        public float? Top { get; set; }

        /// <summary>
        /// Размер области. Если есть привязка с двух сторон то соответствующее значение не используется.
        /// </summary>
        public Size<float> Size { get; set; }


        /// <summary>
        /// Возвращает положение слоя.
        /// </summary>
        /// <param name="parentSize">Текущий размер родителя.</param>
        /// <returns></returns>
        public Rectangle<float> GetRectangle(Size<float> parentSize)
        {
            var result = new Rectangle<float>();

            if (Left != null)
            {
                result.Left = Left.Value;
                if(Right==null)
                    result.Right = result.Left+Size.Width;
            }
            if (Bottom != null)
            {
                result.Bottom = Bottom.Value;
                if (Top == null)
                    result.Top = result.Bottom + Size.Height;
            }
            if (Right != null)
            {
                result.Right = parentSize.Width- Right.Value;
                if (Left == null)
                    result.Left = result.Right - Size.Width;
            }
            if (Top != null)
            {
                result.Top = parentSize.Height-Top.Value;
                if (Bottom == null)
                    result.Bottom = result.Top - Size.Height;
            }

            if (Left == null && Right == null)
            {
                result.Left = parentSize.Width / 2 - Size.Width / 2;
                result.Right = parentSize.Width / 2 + Size.Width / 2;
            }
            if(Bottom==null && Top==null)
            {
                result.Bottom = parentSize.Height/2 - Size.Height/2;
                result.Top = parentSize.Height / 2 + Size.Height / 2;
            }

            return result;
        }

    }
}
