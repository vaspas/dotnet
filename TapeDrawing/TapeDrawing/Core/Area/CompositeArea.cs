using System.Collections.Generic;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Area
{
    public class CompositeArea<T> : IArea<T> where T:struct
    {
        private readonly List<IArea<T>> _internal=new List<IArea<T>>();

        public CompositeArea<T> Add(IArea<T> area)
        {
            _internal.Add(area);

            return this;
        }

        public static CompositeArea<T> Create()
        {
            return new CompositeArea<T>();
        }

        public Rectangle<T> GetRectangle(Size<T> parentArea)
        {
            var parent = new Rectangle<T>
                             {
                                 Left = default(T),
                                 Right = parentArea.Width,
                                 Bottom = default(T),
                                 Top = parentArea.Height
                             };

            foreach(var ar in _internal)
            {
                var r = ar.GetRectangle(
                    new Size<T>
                        {
                            Width = (dynamic)parent.Right - parent.Left,
                            Height = (dynamic)parent.Top - parent.Bottom,
                        });

                r.Left = (dynamic)r.Left + parent.Left;
                r.Right = (dynamic)r.Right + parent.Left;
                r.Bottom = (dynamic)r.Bottom + parent.Bottom;
                r.Top = (dynamic)r.Top + parent.Bottom;

                parent = r;
            }

            return parent;
        }
    }
}
