using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace WpfTest
{
    class FillRectangleRenderer:IRenderer
    {
        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            var pen = gr.Instruments.CreateSolidBrush(new Color{A=255, R=255});
            var shape = gr.Shapes.CreateFillRectangle(pen);

            shape.Render(rect);

            if(pen is IDisposable)
                (pen as IDisposable).Dispose();
            if (shape is IDisposable)
                (shape as IDisposable).Dispose();
        }
    }
}
