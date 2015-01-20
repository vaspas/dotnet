using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx11.Sprites;
using Pen = TapeDrawingSharpDx11.Instruments.Pen;

namespace TapeDrawingSharpDx11.Shapes
{
	/// <summary>
	/// Фигура для рисования незакрашенный прямоугольников
	/// </summary>
	class DrawRectangleShape : BaseShape, IDrawRectangleShape
	{

        /// <summary>
        /// Карандаш для рисования линий
        /// </summary>
        public Pen Pen { get; set; }

	    public Sprite Sprite;

        public LineSprite LineSprite;

        public void Render(Rectangle<float> rectangle)
        {
            // Instantiate Vertex buiffer from vertex data
            var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      new Vector4(rectangle.Left, rectangle.Top, 0.5f, 1.0f), Pen.Argb,
                                      new Vector4(rectangle.Left, rectangle.Bottom, 0.5f, 1.0f), Pen.Argb,
                                      new Vector4(rectangle.Right, rectangle.Bottom, 0.5f, 1.0f), Pen.Argb,
                                      new Vector4(rectangle.Right, rectangle.Top, 0.5f, 1.0f), Pen.Argb,
                                      new Vector4(rectangle.Left, rectangle.Top, 0.5f, 1.0f), Pen.Argb
                                  });

            if (Pen.Width == 1 && (Pen.Dash1 + Pen.Dash2 + Pen.Dash3 + Pen.Dash4) == 0)
                Sprite.Begin();
            else
                LineSprite.Begin(Pen.Width, Pen.Dash1, Pen.Dash2, Pen.Dash3, Pen.Dash4);

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineStrip;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));
            
            Device.Context.Draw(5, 0);

            vertices.Dispose();
        }
	}
}
