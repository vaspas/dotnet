using SharpDX;
using SharpDX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx2D1.Instruments;

namespace TapeDrawingSharpDx2D1.Shapes
{
	/// <summary>
	/// Фигура для рисования незакрашенный прямоугольников
	/// </summary>
	class DrawRectangleAreaShape : BaseShape, IDrawRectangleAreaShape
	{

        /// <summary>
        /// Карандаш для рисования линий
        /// </summary>
        public Pen Pen { get; set; }

        /// <summary>
        /// Выравнивание
        /// </summary>
        public Alignment Alignment { get; set; }


        public void Render(Point<float> point, Size<float> size)
        {
            Render(point, size, 0, 0);
        }

        public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
        {
            // - Горизонталь -
            float shiftX = 0;

            // Если выравнивание по центру
            if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
                || ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
            {
                shiftX += size.Width / 2.0f;
            }
            // Если выравнивание по правому краю
            else if ((Alignment & Alignment.Right) != 0)
            {
                shiftX += size.Width - marginX;
            }
            else
                shiftX += marginX;

            // Вертикаль
            float shiftY = 0;

            // Если выравнивание по центру
            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                shiftY += size.Height / 2.0f;
            }
            //  Если по верхнему краю
            else if ((Alignment & Alignment.Bottom) != 0)
            {
                shiftY += size.Height - marginY;
            }
            else
                shiftY += marginY;

            var l = point.X - shiftX;
            var r = point.X + size.Width - shiftX;
            var b = point.Y - shiftY;
            var t = point.Y + size.Height - shiftY;
            
           /* // Instantiate Vertex buiffer from vertex data
            var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      new Vector4(l, t, 0.5f, 1.0f), Pen.Argb,
                                      new Vector4(l, b, 0.5f, 1.0f), Pen.Argb,
                                      new Vector4(r, b, 0.5f, 1.0f), Pen.Argb,
                                      new Vector4(r, t, 0.5f, 1.0f), Pen.Argb,
                                      new Vector4(l, t, 0.5f, 1.0f), Pen.Argb
                                  });

            if (Pen.Width == 1 && (Pen.Dash1 + Pen.Dash2 + Pen.Dash3 + Pen.Dash4) == 0)
                Sprite.Begin();
            else
                LineSprite.Begin(Pen.Width, Pen.Dash1, Pen.Dash2, Pen.Dash3, Pen.Dash4);

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineStrip;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));

            Device.Context.Draw(5, 0);

            vertices.Dispose();*/
        }
	}
}
