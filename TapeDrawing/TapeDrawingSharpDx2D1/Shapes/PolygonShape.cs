using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx2D1.Instruments;

namespace TapeDrawingSharpDx2D1.Shapes
{
	/// <summary>
	/// Фигура рисует
	/// </summary>
	class PolygonShape : BaseShape, IPolygonShape
	{
        /// <summary>
        /// Кисть заливки прямоугольника
        /// </summary>
        public Brush Brush;

	    public void Render(IList<Point<float>> points)
		{
		    var count = points.Count - 2;
            if (count <= 0)
                return;
		    var verts = new Vector4[count*3*2];

            
            

            var index1 = 0;
            var index2 = points.Count - 1;

            var step = true;

            var index = 0;

            while (index2 - index1 > 1)
            {
                if (step)
                {
                    verts[index++] = new Vector4(points[index1].X, points[index1].Y, 0.5f, 1.0f);
                    verts[index++] = Brush.Argb;
                    index1++;
                    verts[index++] = new Vector4(points[index1].X, points[index1].Y, 0.5f, 1.0f);
                    verts[index++] = Brush.Argb;
                    verts[index++] = new Vector4(points[index2].X, points[index2].Y, 0.5f, 1.0f);
                    verts[index++] = Brush.Argb;
                }
                else
                {
                    verts[index++] = new Vector4(points[index2].X, points[index2].Y, 0.5f, 1.0f);
                    verts[index++] = Brush.Argb;
                    verts[index++] = new Vector4(points[index1].X, points[index1].Y, 0.5f, 1.0f);
                    verts[index++] = Brush.Argb;
                    index2--;
                    verts[index++] = new Vector4(points[index2].X, points[index2].Y, 0.5f, 1.0f);
                    verts[index++] = Brush.Argb;
                }
                step = !step;
            }

            for (var i = 0; i < verts.Length; i += 6)
            {
                //(x2-x1)*(y3-y1)-(y2-y1)*(x3-x1)
                if ((verts[i + 2].X - verts[i].X) * (verts[i + 4].Y - verts[i].Y) -
                    (verts[i + 2].Y - verts[i].Y) * (verts[i + 4].X - verts[i].X) > 0)
                    continue;

                var t = verts[i];
                verts[i] = verts[i + 2];
                verts[i + 2] = t;
            }

            /*var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, verts);

            Sprite.Begin();

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));

            Device.Context.Draw(count*3, 0);

            vertices.Dispose();*/
		}
	}
}
