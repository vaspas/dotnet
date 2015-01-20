

using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core
{
    /// <summary>
    /// Интерфейс для рисования.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        void Draw(IGraphicContext gr, Rectangle<float> rect);
    }
}
