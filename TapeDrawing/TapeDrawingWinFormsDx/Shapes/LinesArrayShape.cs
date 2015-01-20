using System.Collections.Generic;
using System.Linq;
using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingWinFormsDx.Shapes
{
	/// <summary>
	/// Фигура для рисования отдельных линий
	/// </summary>
	class LinesArrayShape : BaseShape, ILinesArrayShape
	{
		public System.Drawing.Color Color { get; set; }

		public void Render(IEnumerable<Point<float>> points)
		{
			// ReSharper disable PossibleMultipleEnumeration

			if (points.Count() < 2) return;

			var verts = new CustomVertex.TransformedColored[points.Count()];
			int i = 0;
			foreach (var point in points)
				verts[i++] = new CustomVertex.TransformedColored(point.X, point.Y, 1.0f, 1.0f, Color.ToArgb());

			Device.DxDevice.VertexFormat = CustomVertex.TransformedColored.Format;
			Device.DxDevice.DrawUserPrimitives(PrimitiveType.LineList, points.Count() / 2, verts);

			// ReSharper restore PossibleMultipleEnumeration
		}
	}
}
