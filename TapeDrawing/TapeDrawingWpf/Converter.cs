using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWpf
{
	/// <summary>
	/// Класс помогает преобразовывать значения
	/// </summary>
	static class Converter
	{
		/// <summary>
		/// Преобразовывает цвет TapeDrawing в цвет wpf
		/// </summary>
		/// <param name="color">Цвет TapeDrawing</param>
		/// <returns>Цвет wpf</returns>
		public static System.Windows.Media.Color Convert(Color color)
		{
			return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Конвертирует стиль линии TapeDrawing в стиль wpf
		/// </summary>
		/// <param name="lineStyle">Стиль линии TapeDrawing</param>
		/// <returns>Стиль линии wpf</returns>
		public static System.Windows.Media.DashStyle Convert(LineStyle lineStyle)
		{
			switch (lineStyle)
			{
				case LineStyle.Dash:
					return System.Windows.Media.DashStyles.Dash;
				case LineStyle.Dot:
					return System.Windows.Media.DashStyles.Dot;
			}
			return System.Windows.Media.DashStyles.Solid;
		}

		/// <summary>
		/// Конвертирует стиль текста TD в стиль Wpf
		/// </summary>
		/// <param name="fontStyle">Стиль текста TD</param>
		/// <returns>Стиль текста wpf</returns>
		public static System.Windows.FontStyle ConvertStyle(FontStyle fontStyle)
		{
			return (fontStyle & FontStyle.Italic) == FontStyle.Italic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
		}

		/// <summary>
		/// Конвертирует стиль текста TD в начертание Wpf
		/// </summary>
		/// <param name="fontStyle">Стиль текста TD</param>
		/// <returns>Начертание wpf</returns>
		public static System.Windows.FontWeight ConvertWeight(FontStyle fontStyle)
		{
			return (fontStyle & FontStyle.Bold) == FontStyle.Bold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal;
		}

		/// <summary>
		/// Преобразовывает прямоугольник TD в прямоугольник wpf
		/// </summary>
		/// <param name="rectangle">Прямоугольник TD</param>
		/// <returns>Прямоугольник wpf</returns>
		public static System.Windows.Rect Convert(Rectangle<float> rectangle)
		{
			return new System.Windows.Rect(rectangle.Left, rectangle.Top, Math.Abs(rectangle.Right - rectangle.Left),
										   Math.Abs(rectangle.Top - rectangle.Bottom));
		}

		/// <summary>
		/// Преобразует массив точек в сегменты линий
		/// </summary>
		/// <param name="points">Набор точек</param>
		/// <returns>Набор сегментов</returns>
		public static IEnumerable<System.Windows.Media.LineSegment> Convert(IEnumerable<Point<float>> points,out System.Windows.Point startPoint)
		{
			var pts = points.ToList();
			startPoint = Convert(pts[0]);
			pts.RemoveAt(0);
			return pts.ConvertAll(p => new System.Windows.Media.LineSegment(Convert(p), true));
		}


		/// <summary>
		/// Конвертирует точку TD в точку wpf
		/// </summary>
		/// <param name="point">Точка TD</param>
		/// <returns>Точку wpf</returns>
		public static System.Windows.Point Convert(Point<float> point)
		{
			return new System.Windows.Point(point.X, point.Y);
		}

        /// <summary>
        /// Конвертирует кнопку мыши wpf в кнопку TapeDrawing
        /// </summary>
        /// <param name="b">Кнопка wpf</param>
        /// <returns>Кнопку TapeDrawing</returns>
        public static MouseButton Convert(System.Windows.Input.MouseButton b)
        {
            switch (b)
            {
                case System.Windows.Input.MouseButton.Left:
                    return MouseButton.Left;
                case System.Windows.Input.MouseButton.Middle:
                    return MouseButton.Center;
                case System.Windows.Input.MouseButton.Right:
                    return MouseButton.Right;
                default:
                    return MouseButton.None;
            }
        }
	}
}
