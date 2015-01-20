using System.Collections.Generic;
using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingWinFormsDx.Shapes
{
	/// <summary>
	/// Фигура рисует
	/// </summary>
	class PolygonShape : BaseShape, IPolygonShape
	{
        public Instruments.Brush Brush { get; set; }

		public void Render(IList<Point<float>> points)
		{
            if (points.Count - 2 <= 0)
                return;

			bool needRestoreAlpha = false;
            if (Brush.A != 255)
            {
                Device.DxDevice.SetRenderState(RenderStates.AlphaBlendEnable, true);
                Device.DxDevice.SetRenderState(RenderStates.SourceBlend, 14);
                Device.DxDevice.SetRenderState(RenderStates.DestinationBlend, 15);
                Device.DxDevice.SetRenderState(RenderStates.BlendFactor,
                                               System.Drawing.Color.FromArgb(Brush.A, Brush.A, Brush.A, Brush.A).ToArgb());
				needRestoreAlpha = true;
            }
            else
                Device.DxDevice.SetRenderState(RenderStates.AlphaBlendEnable, false);


            var verts = new CustomVertex.TransformedColored[(points.Count-2)*3];

		    var index1 = 0;
		    var index2 = points.Count - 1;

		    var step=true;

		    var index = 0;

            while(index2-index1>1)
            {
                if(step)
                {
                    verts[index++] = new CustomVertex.TransformedColored(points[index1].X, points[index1].Y, 1.0f, 1.0f,Brush.Argb);
                    index1++;
                    verts[index++] = new CustomVertex.TransformedColored(points[index1].X, points[index1].Y, 1.0f, 1.0f, Brush.Argb);
                    verts[index++] = new CustomVertex.TransformedColored(points[index2].X, points[index2].Y, 1.0f, 1.0f, Brush.Argb);
                }
                else
                {
                    verts[index++] = new CustomVertex.TransformedColored(points[index2].X, points[index2].Y, 1.0f, 1.0f, Brush.Argb);
                    verts[index++] = new CustomVertex.TransformedColored(points[index1].X, points[index1].Y, 1.0f, 1.0f, Brush.Argb);
                    index2--;
                    verts[index++] = new CustomVertex.TransformedColored(points[index2].X, points[index2].Y, 1.0f, 1.0f, Brush.Argb);
                }
                step = !step;
            }

            for (var i = 0; i < verts.Length; i += 3)
            {
                //(x2-x1)*(y3-y1)-(y2-y1)*(x3-x1)
                if ((verts[i + 1].X - verts[i].X)*(verts[i + 2].Y - verts[i].Y) -
                    (verts[i + 1].Y - verts[i].Y)*(verts[i + 2].X - verts[i].X) > 0)
                    continue;

                var t = verts[i];
                verts[i] = verts[i + 1];
                verts[i + 1] = t;
            }

		    Device.DxDevice.VertexFormat = CustomVertex.TransformedColored.Format;
            Device.DxDevice.DrawUserPrimitives(PrimitiveType.TriangleList, points.Count - 2, verts);

			// Восстановить статус прозрачности
			if (needRestoreAlpha) Device.DxDevice.SetRenderState(RenderStates.AlphaBlendEnable, false);
		}
	}
}
