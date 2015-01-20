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
	class FillRectangleShape : BaseShape, IFillRectangleShape
	{
	    /// <summary>
	    /// Кисть заливки прямоугольника
	    /// </summary>
	    public Brush Brush;

	    public Sprite Sprite;

        public void Render(Rectangle<float> rectangle)
        {
            // Instantiate Vertex buiffer from vertex data
            var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      new Vector4(rectangle.Left, rectangle.Top, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(rectangle.Right, rectangle.Top, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(rectangle.Left, rectangle.Bottom, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(rectangle.Left, rectangle.Bottom, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(rectangle.Right, rectangle.Top, 0.5f, 1.0f), Brush.Argb,
                                      new Vector4(rectangle.Right, rectangle.Bottom, 0.5f, 1.0f), Brush.Argb
                                  });

            Sprite.Begin();

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));


            Device.Context.Draw(6, 0);

            vertices.Dispose();
        }
	}
}
