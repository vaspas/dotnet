using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx11.Instruments;
using TapeDrawingSharpDx11.Sprites;

namespace TapeDrawingSharpDx11.Shapes
{
    /// <summary>
	/// Фигура рисует закрашенный прямоугольник
	/// </summary>
	class FillRectangleAreaShape : BaseShape, IFillRectangleAreaShape
	{
        /// <summary>
        /// Кисть заливки прямоугольника
        /// </summary>
        public Brush Brush { get; set; }

        public Sprite Sprite;

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
            var t = point.Y - shiftY;
            var b = point.Y + size.Height - shiftY;

            // Instantiate Vertex buiffer from vertex data
            var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      new Vector4(l, t, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(r, t, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(l, b, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(l, b, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(r, t, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(r, b, 0.5f, 1.0f), Brush.Argb
                                  });

            Sprite.Begin();

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));


            Device.Context.Draw(6, 0);

            vertices.Dispose();
		}
	}
}
