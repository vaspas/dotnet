using System.Globalization;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingWpf.Instruments;

namespace TapeDrawingWpf.Shapes
{
	/// <summary>
	/// Фигура для рисования текста
	/// </summary>
	class TextShape : BaseShape, ITextShape
	{
	    /// <summary>
	    /// Данные о шрифте текста
	    /// </summary>
	    public Font Font;
	    /// <summary>
	    /// Выравнивание
	    /// </summary>
	    public Alignment Alignment;
		/// <summary>
		/// Угол повотора текста (градусы)
		/// </summary>
		public float Angle { get; set; }

		public void Render(string text, Point<float> point)
		{
			var formattedText = new System.Windows.Media.FormattedText(text, CultureInfo.CurrentCulture,
			                                                           System.Windows.FlowDirection.LeftToRight,
			                                                           Font.ConcreteInstrument, Font.Size, Font.Brush);
			var textSize = new System.Windows.Size(formattedText.Width, formattedText.Height);

			float shiftX = 0;
			if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
				|| ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
			{
				shiftX -= (float)textSize.Width / 2.0f;
			}
			else if ((Alignment & Alignment.Right) != 0)
			{
				shiftX -= (float)textSize.Width;
			}
			float shiftY = 0;
			if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
				|| ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
			{
				shiftY -= (float)textSize.Height / 2.0f;
			}
			else if ((Alignment & Alignment.Bottom) != 0)
			{
				shiftY -= (float)textSize.Height;
			}

			Surface.Context.PushTransform(new System.Windows.Media.TranslateTransform(point.X, point.Y));
			Surface.Context.PushTransform(new System.Windows.Media.RotateTransform(Angle));

			Surface.Context.DrawText(formattedText, new System.Windows.Point(shiftX, shiftY));

			Surface.Context.Pop();
			Surface.Context.Pop();
		}

		public Size<float> Measure(string text)
		{
			var formattedText = new System.Windows.Media.FormattedText(text, CultureInfo.CurrentCulture,
																	   System.Windows.FlowDirection.LeftToRight,
																	   Font.ConcreteInstrument, Font.Size, Font.Brush);
			var textSize = new System.Windows.Size(formattedText.Width, formattedText.Height);
			return new Size<float> { Width = (float)textSize.Width, Height = (float)textSize.Height };
		}
	}
}
