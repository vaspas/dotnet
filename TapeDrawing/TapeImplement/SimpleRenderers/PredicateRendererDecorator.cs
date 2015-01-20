using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.SimpleRenderers
{
    /// <summary>
    /// Рисует по условию.
    /// </summary>
    public class PredicateRendererDecorator : IRenderer
    {
        public IRenderer Internal;

        public Func<bool> Predicate;

        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (Predicate())
                Internal.Draw(gr, rect);
        }
    }
}
