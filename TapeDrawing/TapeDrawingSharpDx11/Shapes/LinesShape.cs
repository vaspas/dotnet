using System.Collections.Generic;
using System.Linq;
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
    /// Фигура рисует линии
    /// </summary>
    class LinesShape : BaseShape, ILinesShape
    {
        /// <summary>
        /// Карандаш для рисования линий
        /// </summary>
        public Pen Pen;

        public LineSprite LineSprite;

        public Sprite Sprite;

        public GpaaSprite GpaaSprite;

        public void Render(IEnumerable<Point<float>> points)
        {
            var count = points.Count();

            var verts = new Vector4[count*2];

            int i = 0;
            foreach (var p in points)
            {
                verts[i++] = new Vector4(p.X, p.Y, 0.5f, 1.0f);
                verts[i++] = Pen.Argb;
            }

            // Instantiate Vertex buiffer from vertex data
            var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, verts);

            if (Pen.Width == 1 && (Pen.Dash1 + Pen.Dash2 + Pen.Dash3 + Pen.Dash4) == 0)
                Sprite.Begin();
            else
                LineSprite.Begin(Pen.Width, Pen.Dash1, Pen.Dash2, Pen.Dash3, Pen.Dash4);

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineStrip;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));

            Device.Context.Draw(count, 0);

            vertices.Dispose();

            if (GpaaSprite == null)
                return;

            //gpaa
            var verts2 = new Vector4[(count - 1)*4];

            var i2 = 0;
            foreach (var p in points)
            {
                verts2[2*i2++] = new Vector4(p.X, p.Y, 0.5f, 1.0f);
                if (i2 > 1 && i2 < verts2.Length/2)
                    verts2[2*i2++] = new Vector4(p.X, p.Y, 0.5f, 1.0f);
            }
            for (var j = 0; j < verts2.Length; j += 4)
            {
                verts2[j + 1] = new Vector4(verts2[j + 2].X - verts2[j].X, verts2[j + 2].Y - verts2[j].Y, 0.5f, 1.0f);
                verts2[j + 3] = new Vector4(verts2[j + 2].X - verts2[j].X, verts2[j + 2].Y - verts2[j].Y, 0.5f, 1.0f);
            }

            var vertices2 = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, verts2);

            GpaaSprite.Begin();

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices2, 32, 0));

            Device.Context.Draw(verts2.Length/2, 0);

            for (var j = 0; j < verts2.Length; j += 4)
            {
                verts2[j + 1] = new Vector4(-verts2[j + 2].X + verts2[j].X, -verts2[j + 2].Y + verts2[j].Y, 0.5f,
                                            1.0f);
                verts2[j + 3] = new Vector4(-verts2[j + 2].X + verts2[j].X, -verts2[j + 2].Y + verts2[j].Y, 0.5f,
                                            1.0f);
            }
            var vertices3 = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, verts2);

            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices3, 32, 0));

            Device.Context.Draw(verts2.Length/2, 0);

            vertices2.Dispose();
        }
    }
}
