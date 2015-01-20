using System.Collections.Generic;
using System.Drawing;
using SharpDX;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using Pen = TapeDrawingSharpDx.Instruments.Pen;

namespace TapeDrawingSharpDx.Shapes
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

        public void Render(Rectangle<float> rectangle)
        {
            var points = new PointF[5];
            points[0] = new PointF(rectangle.Left, rectangle.Top);
            points[1] = new PointF(rectangle.Left, rectangle.Bottom);
            points[2] = new PointF(rectangle.Right, rectangle.Bottom);
            points[3] = new PointF(rectangle.Right, rectangle.Top);
            points[4] = new PointF(rectangle.Left, rectangle.Top);
            DrawLines(points);
        }

        private void DrawLines(IList<PointF> points)
        {
            var cnt = points.Count;

            var v = new Vector2[cnt];

            int i = 0;
            var viewport = Device.DxDevice.Viewport;
            foreach (var point in points)
                v[i++] = new Vector2(point.X - viewport.X, point.Y - viewport.Y);


            Pen.HLine.Begin();
            Pen.HLine.Draw(v, Pen.Argb);
            Pen.HLine.End();

            // Нарисуем точку в конце линии. DirectX почемуто ее не рисует
            /*var verts = new CustomVertex.TransformedColored[1];
            Device.DxDevice.VertexFormat = CustomVertex.TransformedColored.Format;
            verts[0] = new CustomVertex.TransformedColored(points[points.Count - 1].X, points[points.Count - 1].Y, 1.0f, 1.0f, Pen.Argb);
            Device.DxDevice.DrawUserPrimitives(PrimitiveType.PointList, 1, verts[0]);*/
        }
	}
}
