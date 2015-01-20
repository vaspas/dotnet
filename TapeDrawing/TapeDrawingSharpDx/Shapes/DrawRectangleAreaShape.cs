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
	class DrawRectangleAreaShape : BaseShape, IDrawRectangleAreaShape
	{

        /// <summary>
        /// Карандаш для рисования линий
        /// </summary>
        public Pen Pen { get; set; }

        /// <summary>
        /// Выравнивание
        /// </summary>
        public Alignment Alignment { get; set; }

        public void Render(Point<float> point, Size<float> size)
        {
            Render(point, size, 0, 0);
        }

        public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
        {
            // - Горизонталь -
            float shiftX = 0;

            // Если выравнивание по центру
            if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
                || ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
            {
                shiftX += size.Width / 2.0f;
            }
            // Если выравнивание по правому краю
            else if ((Alignment & Alignment.Right) != 0)
            {
                shiftX += size.Width - marginX;
            }
            else
                shiftX += marginX;

            // Вертикаль
            float shiftY = 0;

            // Если выравнивание по центру
            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                shiftY += size.Height / 2.0f;
            }
            //  Если по верхнему краю
            else if ((Alignment & Alignment.Bottom) != 0)
            {
                shiftY += size.Height - marginY;
            }
            else
                shiftY += marginY;

            var l = point.X - shiftX;
            var r = point.X + size.Width - shiftX;
            var b = point.Y - shiftY;
            var t = point.Y + size.Height - shiftY;

            var points = new PointF[5];
            points[0] = new PointF(l, t);
            points[1] = new PointF(l, b);
            points[2] = new PointF(r, b);
            points[3] = new PointF(r, t);
            points[4] = new PointF(l, t);
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
