using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace WinFormsTest
{
    class FillAllRenderer : IRenderer
    {
        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            using (var shape = gr.Shapes.CreateFillAll(new Color(255,255,255)))
            {
                shape.Render();
            }
        }
    }
}
