﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using Pen = TapeDrawingWinFormsDx.Instruments.Pen;

namespace TapeDrawingWinFormsDx.Shapes
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

		public void Render(IEnumerable<Point<float>> points)
		{
			// ReSharper disable PossibleMultipleEnumeration

            var viewport = Device.DxDevice.Viewport;
		    var v = points.Select(p => new Vector2(p.X - viewport.X, p.Y - viewport.Y)).ToArray();

            if (v.Length < 2)
                return;

			Pen.HLine.Begin();
			Pen.HLine.Draw(v, Pen.Argb);
			Pen.HLine.End();

			// Нарисуем точку в конце линии. DirectX почемуто ее не рисует
			var verts = new CustomVertex.TransformedColored[1];
			Device.DxDevice.VertexFormat = CustomVertex.TransformedColored.Format;
            verts[0] = new CustomVertex.TransformedColored(v[v.Length - 1].X, v[v.Length - 1].Y, 1.0f, 1.0f, Pen.Argb);
			Device.DxDevice.DrawUserPrimitives(PrimitiveType.PointList, 1, verts[0]);

			// ReSharper restore PossibleMultipleEnumeration
		}
	}
}
