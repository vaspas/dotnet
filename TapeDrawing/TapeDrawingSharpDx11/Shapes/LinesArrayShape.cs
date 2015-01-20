using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx11.Sprites;

namespace TapeDrawingSharpDx11.Shapes
{
	/// <summary>
	/// Фигура для рисования отдельных линий
	/// </summary>
	class LinesArrayShape : BaseShape, ILinesArrayShape
	{
        public Vector4 Color { get; set; }

        public Sprite Sprite;

        public void Render(IEnumerable<Point<float>> points)
        {
            var count = points.Count();

            var verts = new Vector4[count*2];

            int i = 0;
            foreach (var p in points)
            {
                verts[i++] = new Vector4(p.X, p.Y, 0.5f, 1.0f);
                verts[i++] = Color;
            }

            // Instantiate Vertex buiffer from vertex data
            var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, verts);

            Sprite.Begin();

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));

            Device.Context.Draw(count, 0);

            vertices.Dispose();
        }
	}
}
