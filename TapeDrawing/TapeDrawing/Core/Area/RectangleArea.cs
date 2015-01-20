using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Area
{
    public class RectangleArea : IArea<float>
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public Alignment Alignment { get; set; }

        public Size<float> Size { get; set; }


        /// <summary>
        /// Возвращает положение слоя.
        /// </summary>
        /// <param name="parentSize">Текущий размер родителя.</param>
        /// <returns></returns>
        public Rectangle<float> GetRectangle(Size<float> parentSize)
        {
            var result = new Rectangle<float>();
            
            if ((Alignment&Alignment.Left)!=0)
                result.Left = X;
            else if ((Alignment & Alignment.Right) != 0)
                result.Left = X-Width;
            else
                result.Left = X - Width/2;
            result.Right = result.Left + Width;

            if ((Alignment & Alignment.Bottom) != 0)
                result.Bottom = Y;
            else if ((Alignment & Alignment.Top) != 0)
                result.Bottom = Y - Height;
            else
                result.Bottom = Y - Height / 2;
            result.Top = result.Bottom + Height;
            
            return result;
        }

    }
}
