

using System;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core
{
    /// <summary>
    /// Интерфейс для рисования.
    /// </summary>
    public class RendererDecorator: IRenderer
    {
        public IRenderer Internal { get; set; }

        public Action Before { get; set; }
        public Action After { get; set; }

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (Before != null)
                Before();

            Internal.Draw(gr, rect);

            if (After != null)
                After();
        }
    }
}
