using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace WinFormsTest
{
    class ImageRenderer : IRenderer
    {
        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            using (var image = gr.Instruments.CreateImagePortion(Properties.Resources.testImage, new Rectangle<float>{Left=0, Right=25, Bottom =0 , Top=25}))
            using (var shape = gr.Shapes.CreateImage(image, Alignment.Right | Alignment.Top, 45))
            {
                shape.Render(
                    new Point<float> {X = (rect.Left + rect.Right)/2, Y = (rect.Bottom + rect.Top)/2});
            }
        }
    }
}
