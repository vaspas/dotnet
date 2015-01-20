using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.SimpleRenderers
{
    /// <summary>
    /// Рендерер для заполнения слоя цветом
    /// </summary>
    public class FillAllRenderer : IRenderer
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="color">Цвет заливки фона</param>
        public FillAllRenderer(Color color)
        {
            _color = color;
        }

        /// <summary>
        /// Цвет заливки фона
        /// </summary>
        private readonly Color _color;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            using (var shape = gr.Shapes.CreateFillAll(_color))
                shape.Render();
        }
    }
}
