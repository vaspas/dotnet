using System;
using SharpDX;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx11.Sprites.TextSprite;

namespace TapeDrawingSharpDx11.Shapes
{
    /// <summary>
    /// Фигура для рисования текста
    /// </summary>
    class TextShape : BaseShape, ITextShape
    {
        public TextSprite Sprite;

        public Instruments.Font Font;


        /// <summary>
        /// Выравнивание текста
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Угол повотора текста в градусах
        /// </summary>
        public float Angle { get; set; }

        public void Render(string text, Point<float> point)
        {
            // Измерим строчку
            var textSize = Measure(text);

            // - Горизонталь -
            float shiftX = 0;

            // Если выравнивание по центру
            if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
                || ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
            {
                shiftX += textSize.Width / 2.0f;
            }
            // Если выравнивание по правому краю
            else if ((Alignment & Alignment.Right) != 0)
            {
                shiftX += textSize.Width;
            }

            // Вертикаль
            float shiftY = 0;

            // Если выравнивание по центру
            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                shiftY += textSize.Height / 2.0f;
            }
            //  Если по верхнему краю
            else if ((Alignment & Alignment.Bottom) != 0)
            {
                shiftY += textSize.Height;
            }


            Device.VertexConstant.Translate = Matrix.AffineTransformation2D(1.0f, new Vector2(shiftX, shiftY),
                                                                      (float)(Angle * Math.PI) / 180.0f,
                                                                     new Vector2(point.X - shiftX, point.Y - shiftY));
            Device.VertexConstant.Translate.Transpose();
            Device.Context.UpdateSubresource(ref Device.VertexConstant, Device.VertexConstantBuffer);

            Font.TextBlock.DrawString(text, Vector2.Zero, Font.Color);

            Sprite.Flush();

            Device.VertexConstant.Translate = Matrix.Identity;
            Device.VertexConstant.Translate.Transpose();
            Device.Context.UpdateSubresource(ref Device.VertexConstant, Device.VertexConstantBuffer);
        }

        /*public void Render(string text, Point<float> point)
        {
            //Device.VertexConstant.Translate = Matrix.AffineTransformation2D(1, Vector2.Zero, (float)(Math.PI * 10 / 180), Vector2.Zero);
            //Device.VertexConstant.Translate = Matrix.AffineTransformation2D(2,0, new Vector2(0,0));
            Device.VertexConstant.Translate = Matrix.Identity;
            Device.VertexConstant.Translate.Transpose();
            Device.Context.UpdateSubresource(ref Device.VertexConstant, Device.VertexConstantBuffer);

            Font.TextBlock.DrawString(text, new Vector2(point.X, point.Y), Font.Color);

            Sprite.Flush();

            Device.VertexConstant.Translate = Matrix.Identity;
            Device.VertexConstant.Translate.Transpose();
            Device.Context.UpdateSubresource(ref Device.VertexConstant, Device.VertexConstantBuffer);

            
        }*/

        public Size<float> Measure(string text)
        {
            var r = Font.TextBlock.MeasureString(text);

            return new Size<float>{Height = r.Size.Y, Width = r.Size.X};
        }

    }
}
