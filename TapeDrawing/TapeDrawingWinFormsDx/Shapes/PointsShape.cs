using System.Collections.Generic;
using System.Linq;
using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using Pen = TapeDrawingWinFormsDx.Instruments.Pen;

namespace TapeDrawingWinFormsDx.Shapes
{
	/// <summary>
	/// Фигура рисует точки
	/// </summary>
	class PointsShape : BaseShape, IPointsShape
	{
		/// <summary>
		/// Карандаш для рисования линий
		/// </summary>
		public Pen Pen { get; set; }

		public void Render(IEnumerable<IPoint<float>> points,IEnumerable<IColor> colors)
		{
            var verts = new CustomVertex.TransformedColored[points.Count()];

		    int i = 0;
		    var colorEnum=colors.GetEnumerator();
            foreach (var p in points)
		    {
                colorEnum.MoveNext();
                verts[i++] = new CustomVertex.TransformedColored(p.X, p.Y, 0.5f, 1.0f, Converter.ConvertToInt(colorEnum.Current));
		    }
                
            Device.DxDevice.VertexFormat = CustomVertex.TransformedColored.Format;
            Device.DxDevice.DrawUserPrimitives(PrimitiveType.PointList, points.Count(), verts);
		}
	}
}
