using System.Collections.Generic;
using System.IO;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.SimpleRenderers
{
    /// <summary>
    /// Рендерер для отображения границ
    /// </summary>
    public class ImageRenderer<T> : IRenderer
    {
        /// <summary>
        /// Цвет линий границ
        /// </summary>
        public T Image { get; set; }

        public float Angle { get; set; }
        
        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            using (var im = gr.Instruments.CreateImage(Image))
            using (var shape = gr.Shapes.CreateImage(im, Alignment.None, Angle))
            {
                shape.Render(new Point<float>(rect.Left+(rect.Right - rect.Left) / 2,rect.Bottom+ (rect.Top - rect.Bottom) / 2));
            }
        }
    }
}
