
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Area
{
    public static class AreasFactory
    {
        public static IArea<float> CreateMarginsArea(float? left, float? right, float? bottom, float? top, Size<float> size)
        {
            return new MarginsArea
                       {
                           Left = left,
                           Right = right,
                           Bottom = bottom,
                           Top = top,
                           Size = size
                       };
        }

        public static IArea<float> CreateMarginsArea(float? left, float? right, float? bottom, float? top, float width, float height)
        {
            return CreateMarginsArea(left, right, bottom, top,
                                     new Size<float>{Width=width,Height = height});
        }

        public static IArea<float> CreateMarginsArea(float left, float right, float bottom, float top)
        {
            return CreateMarginsArea(left, right, bottom, top, default(Size<float>));
        }

        public static IArea<float> CreateFullArea()
        {
            return CreateMarginsArea(0,0,0,0);
        }

        public static IArea<float> CreateRelativeArea(Rectangle<float> rectangle)
        {
            return new RelativeArea
            {
                CurrentRelative = rectangle
            };
        }

        public static IArea<float> CreateRelativeArea(float left, float right,float bottom,float top)
        {
            return new RelativeArea
            {
                CurrentRelative = new Rectangle<float>{Left=left,Right= right,Bottom= bottom,Top= top}
            };
        }

        public static IArea<float> CreateRectangleArea(float x, float y, float width, float height,Alignment alingment)
        {
            return new RectangleArea
                       {
                           X = x,
                           Y = y,
                           Width = width,
                           Height = height,
                           Alignment = alingment
                       };
        }

        public static IArea<float> CreateRectangleArea(float width, float height)
        {
            return new RectangleArea
            {
                X = 0,
                Y = 0,
                Width = width,
                Height = height,
                Alignment = Alignment.None
            };
        }

        public static IArea<float> CreateStreakArea(float position, float width, bool isVertical)
        {
            return new StreakArea
                       {
                           RelativeCenterPosition = position,
                           Width = width,
                           IsVertical = isVertical
                       };
        }
    }
}
