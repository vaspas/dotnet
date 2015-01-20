using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace WinFormsTest
{
    class DrawRectangleRenderer:IRenderer
    {
        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            var pen = gr.Instruments.CreatePen(new Color{A=255}, 1, LineStyle.Dot);
            var shape = gr.Shapes.CreateDrawRectangle(pen);

            shape.Render(rect);

            if(pen is IDisposable)
                (pen as IDisposable).Dispose();
            if (shape is IDisposable)
                (shape as IDisposable).Dispose();
        }
    }
}
