using System;
using System.Windows.Media.Imaging;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace WpfTest
{
    class ToolTipRenderer : IRenderer
    {
        public bool Enabled { get; set; }

        public BitmapImage Bitmap { get; set; }

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (!Enabled)
                return;

            using (var brush = gr.Instruments.CreateSolidBrush(new Color { A = 150, B = 255 }))
           using (var shape = gr.Shapes.CreateFillRectangle(brush))
           {
               shape.Render(rect);
           }

            using(var pen = gr.Instruments.CreatePen(new Color{A=255},1,LineStyle.Solid))
           using(var shape = gr.Shapes.CreateDrawRectangle(pen))
           {
               shape.Render(rect);
           }

           using (var font = gr.Instruments.CreateFont("Arial",10, new Color { A = 255,R=255,G=255,B=255},FontStyle.None))
           using (var shape = gr.Shapes.CreateText(font,Alignment.Right|Alignment.Bottom, 3))
           {
               shape.Render("This is summary layer.",
                            new Point<float> { X = rect.Right, Y = rect.Bottom });
           }

           using (var image = gr.Instruments.CreateImage(Bitmap))
           using (var shape = gr.Shapes.CreateImage(image, Alignment.Left | Alignment.Top, -10))
           {
               shape.Render(new Point<float> {X = rect.Left, Y = rect.Top});
           }
        }
    }
}
