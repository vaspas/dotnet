using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using Image = TapeDrawingWinForms.Instruments.Image;

namespace TapeDrawingWinForms.Shapes
{
    class DrawRectangleShape : Shape, IDrawRectangleShape
    {
        public Pen Pen { get; set; }

        public void Render(Rectangle<float> rectangle)
        {
            Graphics.DrawRectangle(
                Pen,
                rectangle.Left, rectangle.Top,
                Math.Abs(rectangle.Right-rectangle.Left), 
                Math.Abs(rectangle.Top-rectangle.Bottom));
        }
    }

    class DrawRectangleAreaShape : Shape, IDrawRectangleAreaShape
    {
        public Pen Pen { get; set; }

        public Alignment Alignment { get; set; }

        public void Render(Point<float> point, Size<float> size)
        {
            Render(point, size, 0, 0);
        }

        public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
        {
            float shiftX = 0;
            if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
                || ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
            {
                shiftX += size.Width / 2;
            }
            else if ((Alignment & Alignment.Right) != 0)
            {
                shiftX += size.Width - marginX;
            }
            else
                shiftX += marginX;
            float shiftY = 0;
            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                shiftY += size.Height / 2;
            }
            else if ((Alignment & Alignment.Bottom) != 0)
            {
                shiftY += size.Height - marginY;
            }
            else
                shiftY += marginY;

            Graphics.DrawRectangle(
                Pen,
                point.X - shiftX, point.Y - shiftY,
                size.Width, size.Height);
        }
    }

    class FillRectangleShape : Shape, IFillRectangleShape
    {
        public Brush Brush { get; set; }

        public void Render(Rectangle<float> rectangle)
        {
            Graphics.FillRectangle(
                Brush,
                rectangle.Left, rectangle.Top,
                Math.Abs(rectangle.Right - rectangle.Left),
                Math.Abs(rectangle.Top - rectangle.Bottom));
        }
    }

    class FillRectangleAreaShape : Shape, IFillRectangleAreaShape
    {
        public Brush Brush { get; set; }

        public Alignment Alignment { get; set; }

        public void Render(Point<float> point, Size<float> size)
        {
            Render(point, size, 0, 0);
        }

        public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
        {
            float shiftX = 0;
            if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
                || ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
            {
                shiftX += size.Width / 2;
            }
            else if ((Alignment & Alignment.Right) != 0)
            {
                shiftX += size.Width - marginX;
            }
            else
                shiftX += marginX;
            float shiftY = 0;
            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                shiftY += size.Height / 2;
            }
            else if ((Alignment & Alignment.Bottom) != 0)
            {
                shiftY += size.Height - marginY;
            }
            else
                shiftY += marginY;

            Graphics.FillRectangle(
                Brush,
                point.X - shiftX, point.Y - shiftY,
                size.Width, size.Height);
        }
    }

    class TextShape : Shape, ITextShape
    {
        public Font Font { get; set; }

        public Brush Brush { get; set; }

        public Alignment Alignment { get; set; }

        public float Angle { get; set; }

        public void Render(string text, Point<float> point)
        {
            var textSize = Graphics.MeasureString(text, Font);

            float shiftX = 0;
            if(((Alignment & Alignment.Left)!=0 && (Alignment & Alignment.Right)!=0)
                || ((Alignment & Alignment.Left)==0 && (Alignment & Alignment.Right)==0))
            {
                shiftX -= textSize.Width / 2;
            }
            else if((Alignment & Alignment.Right)!=0)
            {
                shiftX -= textSize.Width;
            }
            float shiftY = 0;
            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                shiftY -= textSize.Height / 2;
            }
            else if ((Alignment & Alignment.Bottom) != 0)
            {
                shiftY -= textSize.Height;
            }


            Graphics.TranslateTransform(point.X, point.Y);
            Graphics.RotateTransform(Angle);

            Graphics.DrawString(
                text,
                Font,
                Brush,
                shiftX, shiftY);

            Graphics.RotateTransform(-Angle);
            Graphics.TranslateTransform(-point.X, -point.Y);
        }

    	public Size<float> Measure(string text)
    	{
			var textSize = Graphics.MeasureString(text, Font);
    		return new Size<float> {Width = textSize.Width, Height = textSize.Height};
    	}
    }

    class LinesShape : Shape, ILinesShape
    {
        public Pen Pen { get; set; }

        public void Render(IEnumerable<Point<float>> points)
        {
            var arr = points.Select(p => new PointF(p.X, p.Y)).ToArray();

			// ReSharper disable PossibleMultipleEnumeration)
            if (arr.Length < 2)
                return;

            Graphics.DrawLines(Pen, arr);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }

	class LinesArrayShape : Shape, ILinesArrayShape
	{
		public System.Drawing.Color Color { get; set; }

		public void Render(IEnumerable<Point<float>> points)
		{
			// ReSharper disable PossibleMultipleEnumeration
			if (points.Count() < 2)
				return;

			var pts = points.ToList().ConvertAll(p => new PointF(p.X, p.Y)).ToArray();

			using (var tmpPen = new Pen(Color))
			{
				for (var i = 0; i < pts.Length; i += 2)
					Graphics.DrawLine(tmpPen, pts[i], pts[i + 1]);
			}
			// ReSharper restore PossibleMultipleEnumeration
		}
	}

	class PolygonShape : Shape, IPolygonShape
    {
        public Brush Brush { get; set; }

        public void Render(IList<Point<float>> points)
        {

            Graphics.FillPolygon(
                Brush,
                points.ToList().ConvertAll(p => new PointF(p.X, p.Y)).ToArray());
        }
    }

    class FillAllShape : Shape, IFillAllShape
    {
        public System.Drawing.Color Color { get; set; }

        public void Render()
        {
            Graphics.Clear(Color);
        }
    }

    class ImageShape : Shape, IImageShape
    {
        public Image Image { get; set; }

        public Alignment Alignment { get; set; }

        public float Angle { get; set; }

        public void Render( Point<float> point)
        {
            var saved = Graphics.InterpolationMode;
            Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Graphics.TranslateTransform(point.X, point.Y);
            Graphics.RotateTransform(Angle);

            if (Image.Roi.IsEmpty())
                Graphics.DrawImageUnscaled(Image.ConcreteInstrument,
                    (int)CalculateShiftX(), (int)CalculateShiftY());
            else
                Graphics.DrawImage(Image.ConcreteInstrument, (int) CalculateShiftX(), (int) CalculateShiftY(),
                                   new Rectangle(Image.Roi.Left, Image.ConcreteInstrument.Height - Image.Roi.Top,
                                                 Math.Abs(Image.Roi.Right - Image.Roi.Left), Math.Abs(Image.Roi.Top - Image.Roi.Bottom)),
                                   GraphicsUnit.Pixel);

            Graphics.RotateTransform(-Angle);
            Graphics.TranslateTransform(-point.X, -point.Y);

            Graphics.InterpolationMode = saved;
        }

        private float CalculateShiftX()
        {
            if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
                || ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
            {
                return -Image.Width / 2;
            }
            
            if ((Alignment & Alignment.Right) != 0)
            {
                return -Image.Width;
            }

            return 0;
        }

        private float CalculateShiftY()
        {
            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                return -Image.Height / 2;
            }
            if ((Alignment & Alignment.Bottom) != 0)
            {
                return -Image.Height;
            }

            return 0;
        }
    }
}
