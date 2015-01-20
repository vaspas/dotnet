using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using Image = TapeDrawingWinFormsDx.Instruments.Image;

namespace TapeDrawingWinFormsDx.Shapes
{
	/// <summary>
	/// Фигура для рисования изображений
	/// </summary>
	class ImageShape : BaseShape, IImageShape
	{
	    /// <summary>
	    /// Рисунок для отображения
	    /// </summary>
	    public Image Image;

	    /// <summary>
	    /// Выравнивание изображения
	    /// </summary>
	    public Alignment Alignment;

	    /// <summary>
	    /// Угол повотора изображения
	    /// </summary>
	    public float Angle;
        
		public void Render(Point<float> point)
		{
			var shiftX = (int)-CalculateShiftX();
			var shiftY = (int)-CalculateShiftY();

			// Нарисуем битмап
			Image.ImageSprite.Begin(SpriteFlags.SortTexture | SpriteFlags.AlphaBlend);

		    var src = new Rectangle(0, 0, (int) Image.Width, (int) Image.Height);
            if(!Image.Roi.IsEmpty())
            {
                src.X = Image.Roi.Left;
                src.Y = (int) (Image.Height - Image.Roi.Top);
                src.Width = Math.Abs(Image.Roi.Right - Image.Roi.Left);
                src.Height = Math.Abs(Image.Roi.Top - Image.Roi.Bottom);
            }

			Image.ImageSprite.Transform = Matrix.AffineTransformation2D(1.0f,
																 new Vector2(shiftX, shiftY),
                                                                 (float)(Angle * Math.PI) / 180.0f, new Vector2((int)point.X - shiftX, (int)point.Y - shiftY));
			Image.ImageSprite.Draw(Image.Texture, src, Vector3.Empty, Vector3.Empty,
							  System.Drawing.Color.White);

			Image.ImageSprite.End();
		}

		private float CalculateShiftX()
		{
            var w = Image.Roi.IsEmpty() ? Image.Width : Math.Abs(Image.Roi.Right - Image.Roi.Left);

			if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
				|| ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
			{
				return -w / 2;
			}

			if ((Alignment & Alignment.Right) != 0)
			{
				return -w;
			}

			return 0;
		}

		private float CalculateShiftY()
		{
            var h = Image.Roi.IsEmpty() ? Image.Height : Math.Abs(Image.Roi.Top - Image.Roi.Bottom);

			if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
				|| ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
			{
				return -h / 2;
			}
			if ((Alignment & Alignment.Bottom) != 0)
			{
				return -h;
			}

			return 0;
		}
	}
}
