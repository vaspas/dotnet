using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D9;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx.Instruments;

namespace TapeDrawingSharpDx.Shapes
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex
    {
        public Vector4 Position;
        public SharpDX.Color Color;
    }

	/// <summary>
	/// Фигура рисует закрашенный прямоугольник
	/// </summary>
	class FillRectangleShape : BaseShape, IFillRectangleShape
	{

        /// <summary>
        /// Кисть заливки прямоугольника
        /// </summary>
        public Brush Brush { get; set; }

        public void Render(Rectangle<float> rectangle)
        {
            var color = Brush.Argb;

            var vertices = new VertexBuffer(Device.DxDevice, 6 * 20, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            vertices.Lock(0, 0, LockFlags.None).WriteRange(new[] {
                new Vertex() { Color = color, Position = new Vector4(rectangle.Left, rectangle.Top, 1.0f, 1.0f) },
                new Vertex() { Color = color, Position = new Vector4(rectangle.Right, rectangle.Top, 1.0f, 1.0f) },
                new Vertex() { Color = color, Position = new Vector4(rectangle.Right, rectangle.Bottom, 1.0f, 1.0f) },
                new Vertex() { Color = color, Position = new Vector4(rectangle.Right, rectangle.Bottom, 1.0f, 1.0f) },
                new Vertex() { Color = color, Position = new Vector4(rectangle.Left, rectangle.Bottom, 1.0f, 1.0f) },
                new Vertex() { Color = color, Position = new Vector4(rectangle.Left, rectangle.Top, 1.0f, 1.0f) }
                //new Vertex() { Color =  SharpDX.Color.Red, Position = new Vector4(100.0f, 100.0f, 1f, 1.0f) },
                //new Vertex() { Color =  SharpDX.Color.Red, Position = new Vector4(350.0f, 500.0f, 1f, 1.0f) },
                //new Vertex() { Color =  SharpDX.Color.Red, Position = new Vector4(50.0f, 500.0f, 1f, 1.0f) }
            });
            vertices.Unlock();

            var vertexElems = new[] {
        		new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.PositionTransformed, 0),
        		new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
				VertexElement.VertexDeclarationEnd
        	};

            var vertexDecl = new VertexDeclaration(Device.DxDevice, vertexElems);

            Device.DxDevice.SetStreamSource(0, vertices, 0, 20);
            Device.DxDevice.VertexDeclaration = vertexDecl;
            Device.DxDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
        }
	}
}
