using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using Font = TapeDrawingWinFormsDx.Instruments.Font;

namespace TapeDrawingWinFormsDx.Shapes
{
	/// <summary>
	/// Фигура для рисования текста
	/// </summary>
	class TextShape : BaseShape, ITextShape
	{
		/// <summary>
		/// Шрифт текста
		/// </summary>
		public Font Font { get; set; }

		/// <summary>
		/// Выравнивание текста
		/// </summary>
		public Alignment Alignment { get; set; }

		/// <summary>
		/// Угол повотора текста в градусах
		/// </summary>
		public float Angle { get; set; }

		public void Render(string text, Point<float> point)
		{
			// Измерим строчку
			var textSize = Measure(text);

			// - Горизонталь -
			float shiftX = 0;

			// Если выравнивание по центру
			if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
				|| ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
			{
				shiftX += textSize.Width / 2.0f;
			}
			// Если выравнивание по правому краю
			else if ((Alignment & Alignment.Right) != 0)
			{
				shiftX += textSize.Width;
			}

			// Вертикаль
			float shiftY = 0;

			// Если выравнивание по центру
			if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
				|| ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
			{
				shiftY += textSize.Height / 2.0f;
			}
			//  Если по верхнему краю
			else if ((Alignment & Alignment.Bottom) != 0)
			{
				shiftY += textSize.Height;
			}

			// Будем текст рисовать на спрайте
			Font.TextSprite.Begin(SpriteFlags.SortTexture | SpriteFlags.AlphaBlend);

			// Выполним поворот текста
			Font.TextSprite.Transform = Matrix.AffineTransformation2D(1.0f, new Vector2(shiftX, shiftY),
			                                                          (float) (Angle*Math.PI)/180.0f,
			                                                          new Vector2(point.X - shiftX, point.Y - shiftY));
			// Отобразим текст
			Font.FontD3D.DrawText(Font.TextSprite, text, 0, 0, Font.Color);
			Font.TextSprite.End();
		}

		public Size<float> Measure(string text)
		{
			var textSize = MeasureString(text, Font.Color.ToArgb(), HorizontalAlignment.Left, VerticalAlignment.Top, true);
			return new Size<float> { Width = textSize.Width, Height = textSize.Height };
		}

		/// <summary>
		/// Позволяет получить размер текста
		/// </summary>
		/// <param name="text">Текст, который нужно отобразить</param>
		/// <param name="color">Цвет текста</param>
		/// <param name="horizontalAlignment">Выравнивание по горизонтали</param>
		/// <param name="verticalAlignment">Выравнивание по вертикали</param>
		/// <param name="wordWrap">Включен ли режим переноса по словам
		/// Если тест не помечается в размеры экрана, он будет перенесен на следующую строку</param>
		/// <returns>Размер текста</returns>
		private SizeF MeasureString(string text, int color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, bool wordWrap)
		{
			// Проверим можно ли рисовать
			if (Device.Pause) return new SizeF(1, 1);

		    //var linesCount = text.Split(new[] {Environment.NewLine}, StringSplitOptions.None).Length;

			var format = Converter.Convert(horizontalAlignment, verticalAlignment, wordWrap);
			var rect = Font.FontD3D.MeasureString(Font.TextSprite, text, format, color);
            return new SizeF(rect.Width, rect.Height);
		}
	}
}
