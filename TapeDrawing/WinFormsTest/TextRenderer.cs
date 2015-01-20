using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace WinFormsTest
{
    class TextRenderer : IRenderer
    {
        public string SomeText;
        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            
            using (var f=gr.Instruments.CreateFont("Arial", 12,new Color(0,0,0),FontStyle.Bold))
            using (var shape = gr.Shapes.CreateText(f, Alignment.Left|Alignment.Bottom, 0))
            {
                shape.Render(SomeText, new Point<float>{X=rect.Left,Y= rect.Bottom});
            }
        }
    }
}
