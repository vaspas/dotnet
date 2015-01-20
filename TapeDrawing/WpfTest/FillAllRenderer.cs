using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using Color = TapeDrawing.Core.Primitives.Color;

namespace WpfTest
{
    class FillAllRenderer : IRenderer
    {
		public float Angle { get; set; }

        public TestParameters TestParameters { get; set; }

		private readonly Random _rnd = new Random();


        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
        	var yellowColor = Colors.Yellow;
			var bmpImage = new BitmapImage();
			bmpImage.BeginInit();
			bmpImage.CacheOption = BitmapCacheOption.OnLoad;
			bmpImage.CreateOptions = BitmapCreateOptions.None;
			bmpImage.UriSource = new Uri(@"pack://application:,,,/testImage.bmp", UriKind.Absolute);
			bmpImage.EndInit();
			bmpImage.Freeze();

        	Angle += 1;

			using (var penBlack = gr.Instruments.CreatePen(new Color { A = 255 }, 1, LineStyle.Solid))
			using (var penRed = gr.Instruments.CreatePen(new Color { A = 255, R = 255 }, 1, LineStyle.Dash))
			using (var penGreen = gr.Instruments.CreatePen(new Color { A = 255, G = 255 }, 2, LineStyle.Dot))
			using (var penBlue = gr.Instruments.CreatePen(new Color { A = 100, B = 255 }, 4, LineStyle.Solid))
			using (var brushRed = gr.Instruments.CreateSolidBrush(new Color { A = 255, R = 255 }))
			using (var brushGreen = gr.Instruments.CreateSolidBrush(new Color { A = 255, G = 255 }))
			using (var brushBlue = gr.Instruments.CreateSolidBrush(new Color { A = 255, B = 255 }))
			using (var brushWhite = gr.Instruments.CreateSolidBrush(new Color { A = 255, R = yellowColor.R, G = yellowColor.G, B = yellowColor.B }))
			using (var penWhite = gr.Instruments.CreatePen(new Color { A = 255, R = 255, G = 255, B = 255 }, 1, LineStyle.Solid))
			using (var fillAllShape = gr.Shapes.CreateFillAll(new Color(200, 200, 200)))
			using (var linesGrid = gr.Shapes.CreateLines(penWhite))
			using (var linesBlack = gr.Shapes.CreateLines(penBlack))
			using (var linesRed = gr.Shapes.CreateLines(penRed))
			using (var linesGreen = gr.Shapes.CreateLines(penGreen))
			using (var linesBlue = gr.Shapes.CreateLines(penBlue))
			using (var rectRed = gr.Shapes.CreateDrawRectangle(penRed))
			using (var rectGreen = gr.Shapes.CreateDrawRectangle(penGreen))
			using (var rectBlue = gr.Shapes.CreateDrawRectangle(penBlue))
			using (var rectFillRed = gr.Shapes.CreateFillRectangle(brushRed))
			using (var rectFillGreen = gr.Shapes.CreateFillRectangle(brushGreen))
			using (var rectFillBlue = gr.Shapes.CreateFillRectangle(brushBlue))
			using (var rectFillWhite = gr.Shapes.CreateFillRectangle(brushWhite))
			using (var fontNone = gr.Instruments.CreateFont("Arial", 6, new Color { A = 255, R = 255 }, FontStyle.None))
			using (var fontItalic = gr.Instruments.CreateFont("Courier New", 10, new Color { A = 255, G = 255 }, FontStyle.Italic))
			using (var fontBold = gr.Instruments.CreateFont("Times New Roman", 16, new Color { A = 255, B = 255 }, FontStyle.Bold))
			using (var textNone = gr.Shapes.CreateText(fontNone, Alignment.Left, Angle))
			using (var textItalic = gr.Shapes.CreateText(fontItalic, Alignment.Right, Angle))
			using (var textBold = gr.Shapes.CreateText(fontBold, Alignment.Left | Alignment.Right, Angle))
			using (var image = gr.Instruments.CreateImage(bmpImage))
            using (var imagePart = gr.Instruments.CreateImagePortion(bmpImage, new Rectangle<float> { Left = 0f, Right = 30f, Bottom = 0f, Top = 30f }))
			using (var shapeLeftTop = gr.Shapes.CreateImage(image, Alignment.Left | Alignment.Top, Angle))
			using (var shapeCenter = gr.Shapes.CreateImage(image, Alignment.Left | Alignment.Top | Alignment.Right | Alignment.Bottom, Angle))
            using (var shapeRightBottom = gr.Shapes.CreateImage(image, Alignment.Right | Alignment.Bottom, Angle))
            using (var shapeLeftTopPart = gr.Shapes.CreateImage(imagePart, Alignment.Left | Alignment.Top, Angle))
            using (var shapeCenterPart = gr.Shapes.CreateImage(imagePart, Alignment.Left | Alignment.Top | Alignment.Right | Alignment.Bottom, Angle))
            using (var shapeRightBottomPart = gr.Shapes.CreateImage(imagePart, Alignment.Right | Alignment.Bottom, Angle))
			{
				var watch = new Stopwatch();

				watch.Start();

				// Очистить экран
				fillAllShape.Render();

				watch.Stop();
				Debug.WriteLine("clear=" + watch.ElapsedMilliseconds);
				watch.Reset();
				watch.Start();


				float l = Math.Min(rect.Left, rect.Right);
				float r = Math.Max(rect.Left, rect.Right);
				float t = Math.Max(rect.Bottom, rect.Top);
				float b = Math.Min(rect.Bottom, rect.Top);

				// - НАГРУЗКА -

				#region - Рандомные линии -

				{
					const int count = 100;
					var points = new List<Point<float>>(count);

                    int repeat = TestParameters.Difficult / 5 + 1;
                    if (TestParameters.Difficult == 0) repeat = 0;
					for (int j = 0; j < repeat; j++)
					{
						points.Clear();
						for (int i = 0; i < count / 2; i++)
						{
							points.Add(new Point<float> { X = _rnd.Next((int)l, (int)r), Y = _rnd.Next((int)b, (int)t) });
						}

						linesGreen.Render(points);
					}

                    repeat = TestParameters.Difficult;
					for (int j = 0; j < repeat; j++)
					{
						points.Clear();
						for (int i = 0; i < count; i++)
						{
							points.Add(new Point<float> { X = _rnd.Next((int)l, (int)r), Y = _rnd.Next((int)b, (int)t) });
						}

						linesBlack.Render(points);
					}
				}

				#endregion

				watch.Stop();
				Debug.WriteLine("rand=" + watch.ElapsedMilliseconds);
				watch.Reset();
				watch.Start();


				// - ТЕСТЫ ЛИНИЙ -

				#region // Нарисовать сетку

				{
					float i;
					for (i = l; i < r; i += 30)
					{
						var points = new List<Point<float>>
							             	{
							             		new Point<float> {X = i, Y = b},
							             		new Point<float> {X = i, Y = t}
							             	};
						linesGrid.Render(points);
					}
					for (i = b; i < t; i += 30)
					{
						var points = new List<Point<float>>
							             	{
							             		new Point<float> {X = l, Y = i},
							             		new Point<float> {X = r, Y = i}
							             	};
						linesGrid.Render(points);
					}
				}

				#endregion

				#region // Нарисовать маленькие линии и диагональную линию

				{
					var points = new List<Point<float>>
					             	{
					             		new Point<float> {X = 1, Y = 1},
					             		new Point<float> {X = 10, Y = 1}
					             	};
					linesGrid.Render(points);
					points = new List<Point<float>>
					         	{
					         		new Point<float> {X = 1, Y = 3},
					         		new Point<float> {X = 11, Y = 3},
					         	};
					linesGrid.Render(points);
					points = new List<Point<float>>
					         	{
					         		new Point<float> {X = 1, Y = 5},
					         		new Point<float> {X = 12, Y = 5}
					         	};
					linesGrid.Render(points);

					points = new List<Point<float>>
					         	{
					         		new Point<float> {X = 10, Y = 10},
					         		new Point<float> {X = 100, Y = 100},
					         		new Point<float> {X = 190, Y = 150}
					         	};
					linesGrid.Render(points);
				}

				#endregion

				#region // Нарисовать цветные линии разных типов

				{
					// Red
					var points = new List<Point<float>> { new Point<float> { X = 5, Y = 55 }, new Point<float> { X = 150, Y = 55 } };
					linesRed.Render(points);
					points = new List<Point<float>> { new Point<float> { X = 7, Y = 54 }, new Point<float> { X = 152, Y = 54 } };
					linesRed.Render(points);
					points = new List<Point<float>> { new Point<float> { X = 5, Y = 55 }, new Point<float> { X = 150, Y = 155 } };
					linesRed.Render(points);

					// Green
					points = new List<Point<float>> { new Point<float> { X = 5, Y = 65 }, new Point<float> { X = 150, Y = 65 } };
					linesGreen.Render(points);
					points = new List<Point<float>> { new Point<float> { X = 6, Y = 64 }, new Point<float> { X = 151, Y = 64 } };
					linesGreen.Render(points);
					points = new List<Point<float>> { new Point<float> { X = 5, Y = 65 }, new Point<float> { X = 150, Y = 165 } };
					linesGreen.Render(points);

					// Blue
					points = new List<Point<float>> { new Point<float> { X = 5, Y = 75 }, new Point<float> { X = 150, Y = 75 } };
					linesBlue.Render(points);
					points = new List<Point<float>> { new Point<float> { X = 5, Y = 75 }, new Point<float> { X = 150, Y = 175 } };
					linesBlue.Render(points);
				}

				#endregion

				watch.Stop();
				Debug.WriteLine("lines=" + watch.ElapsedMilliseconds);
				watch.Reset();
				watch.Start();

				// - ТЕСТЫ ПРЯМОУГОЛЬНИКОВ -

				#region - Незакрашенные прямоугольники -

				{
					float rL = 200;
					float rR = 270;
					float rT = 100;
					float rB = 150;
					const float dt = 17;

					rectRed.Render(new Rectangle<float> { Left = rL, Right = rR, Top = rT, Bottom = rB });
					rL += dt;
					rR += dt;
					rT += dt;
					rB += dt;
					rectGreen.Render(new Rectangle<float> { Left = rL, Right = rR, Top = rT, Bottom = rB });
					rL += dt;
					rR += dt;
					rT += dt;
					rB += dt;
					rectBlue.Render(new Rectangle<float> { Left = rL, Right = rR, Top = rT, Bottom = rB });
				}

				#endregion

				#region - Закрашенные прямоугольники -

				{
					float rL = 0;
					float rR = 10;
					float rT = 30;
					float rB = 20;


					rL += 200;
					rR += 200;

					rectFillRed.Render(new Rectangle<float> { Left = rL, Right = rR, Top = rT, Bottom = rB });
					rL += 15;
					rR += 25;
					rT += 5;
					rB -= 5;

					rectFillGreen.Render(new Rectangle<float> { Left = rL, Right = rR, Top = rT, Bottom = rB });
					rL += 25;
					rR += 35;
					rT += 5;
					rB -= 5;

					rectFillBlue.Render(new Rectangle<float> { Left = rL, Right = rR, Top = rT, Bottom = rB });
				}

				#endregion

				watch.Stop();
				Debug.WriteLine("rect=" + watch.ElapsedMilliseconds);
				watch.Reset();
				watch.Start();

				// - ТЕСТЫ ВЫВОДА ТЕКСТА -

				#region - Текст под разными углами -
				{
					float textX = l;
					float textY = t - 20;

					const float delta = 2;
					textNone.Render("Arial size 6", new Point<float> { X = textX, Y = textY });
					rectFillWhite.Render(new Rectangle<float>
					{
						Left = textX - delta,
						Right = textX + delta,
						Top = textY + delta,
						Bottom = textY - delta
					});

					textX += 250;
					textItalic.Render("Courier new size 10", new Point<float> { X = textX, Y = textY });
					rectFillWhite.Render(new Rectangle<float>
					{
						Left = textX - delta,
						Right = textX + delta,
						Top = textY + delta,
						Bottom = textY - delta
					});


					textX -= 50;
					textY -= 150;
					textBold.Render("Times New Roman 16", new Point<float> { X = textX, Y = textY });
					rectFillWhite.Render(new Rectangle<float>
					{
						Left = textX - delta,
						Right = textX + delta,
						Top = textY + delta,
						Bottom = textY - delta
					});
				}

				#endregion

				watch.Stop();
				Debug.WriteLine("text=" + watch.ElapsedMilliseconds);
				watch.Reset();
				watch.Start();

				// - ТЕСТЫ ИЗОБРАЖЕНИЙ -

				#region - Вращающиеся изображения с разным выравниванием -
				{
					var x = 50;
					const int y = 250;
					const float delta = 2;

					// Левое верхнее выравнивание
					shapeLeftTop.Render(new Point<float> { X = x, Y = y });
					rectFillWhite.Render(new Rectangle<float>
					{
						Left = x - delta,
						Right = x + delta,
						Top = y + delta,
						Bottom = y - delta
					});

					x += 100;

					// Центральное выравнивание
					shapeCenter.Render(new Point<float> { X = x, Y = y });
					rectFillWhite.Render(new Rectangle<float>
					{
						Left = x - delta,
						Right = x + delta,
						Top = y + delta,
						Bottom = y - delta
					});

					x += 100;

					// Центральное выравнивание, часть изображения
					shapeCenterPart.Render(new Point<float> { X = x, Y = y });
					rectFillWhite.Render(new Rectangle<float>
					{
						Left = x - delta,
						Right = x + delta,
						Top = y + delta,
						Bottom = y - delta
					});

					x += 100;

					// Центральное выравнивание, часть изображения
					shapeRightBottomPart.Render(new Point<float> { X = x, Y = y });
					rectFillWhite.Render(new Rectangle<float>
					{
						Left = x - delta,
						Right = x + delta,
						Top = y + delta,
						Bottom = y - delta
					});
				}
				#endregion

				watch.Stop();
				Debug.WriteLine("images=" + watch.ElapsedMilliseconds);
				watch.Reset();

			}
        }
    }
}
