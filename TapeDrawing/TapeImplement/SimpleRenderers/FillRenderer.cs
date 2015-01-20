using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.SimpleRenderers
{
    /// <summary>
    /// Рендерер для заполнения слоя цветом
    /// </summary>
    public class FillRenderer : IRenderer
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="color">Цвет заливки фона</param>
        public FillRenderer(Color color)
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
            using (var brush = gr.Instruments.CreateSolidBrush(_color))
            using (var shape = gr.Shapes.CreateFillRectangle(brush))
                shape.Render(rect);
        }
    }
}
