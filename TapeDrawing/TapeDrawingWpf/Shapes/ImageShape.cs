using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingWpf.Instruments;

namespace TapeDrawingWpf.Shapes
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
	    /// Выравнивание
	    /// </summary>
	    public Alignment Alignment;
		/// <summary>
		/// Угол поворота, градусов
		/// </summary>
		public float Angle { get; set; }

		public void Render(Point<float> point, Size<int> roi)
		{
			Surface.Context.PushTransform(new System.Windows.Media.TranslateTransform(point.X,point.Y));
			Surface.Context.PushTransform(new System.Windows.Media.RotateTransform(Angle));

			
			Surface.Context.Pop();
			Surface.Context.Pop();
		}

		public void Render(Point<float> point)
		{
			Surface.Context.PushTransform(new System.Windows.Media.TranslateTransform(point.X, point.Y));
			Surface.Context.PushTransform(new System.Windows.Media.RotateTransform(Angle));

            if (Image.Roi.IsEmpty())
            {
                Surface.Context.DrawImage(Image.ConcreteInstrument,
                                          new System.Windows.Rect(CalculateShiftX(), CalculateShiftY(),
                                                                  Image.Width,
                                                                  Image.Height));
            }
            else
            {
                var cb = new System.Windows.Media.Imaging.CroppedBitmap(Image.ConcreteInstrument,
                                                                    new System.Windows.Int32Rect(Image.Roi.Left, (int)(Image.ConcreteInstrument.Height-Image.Roi.Top),
                                                                       (int)Image.Width, (int)Image.Height));

                Surface.Context.DrawImage(cb, new System.Windows.Rect(CalculateShiftX(), CalculateShiftY(), cb.Width, cb.Height));
            }

		    Surface.Context.Pop();
			Surface.Context.Pop();
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
