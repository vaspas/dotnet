using TapeDrawing.Core.Primitives;

namespace TapeDrawingSharpDx.Cache.LineCache
{
    /// <summary>
    /// Параметры создания линий
    /// </summary>
    class LineCreatorArgs
    {
        /// <summary>
        /// Толщина линии
        /// </summary>
        public float Width { get; set; }
        /// <summary>
        /// Стиль линии
        /// </summary>
        public LineStyle Style { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is LineCreatorArgs)
            {
                return this == (LineCreatorArgs)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)Style * 1000 + (int)Width;
        }

        public static bool operator ==(LineCreatorArgs a, LineCreatorArgs b)
        {
            return (a.Style == b.Style) && (a.Width == b.Width);
        }

        public static bool operator !=(LineCreatorArgs a, LineCreatorArgs b)
        {
            return (a.Style != b.Style) || (a.Width != b.Width);
        }
    }
}
