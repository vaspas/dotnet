using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D9;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx.Instruments;

namespace TapeDrawingSharpDx.Shapes
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

            /*// Нарисуем точку в конце линии. DirectX почемуто ее не рисует
            var verts = new SharpDX.Direct3D.CustomVertex.TransformedColored[1];
            Device.VertexFormat = CustomVertex.TransformedColored.Format;
            verts[0] = new CustomVertex.TransformedColored(v[v.Length - 1].X, v[v.Length - 1].Y, 1.0f, 1.0f, Pen.Argb);
            Device.DrawUserPrimitives(PrimitiveType.PointList, 1, verts[0]);*/

            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
