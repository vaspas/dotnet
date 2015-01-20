using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.SimpleRenderers
{
    /// <summary>
    /// Рендерер для отображения текста
    /// </summary>
    public class TextRenderer : IRenderer
    {
        /// <summary>
        /// Текст для отображения
        /// </summary>
        public string Text;

        /// <summary>
        /// Или функция для получения текста.
        /// </summary>
        public Func<string> GetText;

        /// <summary>
        /// Название шрифта
        /// </summary>
        public string FontName;

        /// <summary>
        /// Размер текста
        /// </summary>
        public int Size;

        /// <summary>
        /// Стиль текста
        /// </summary>
        public FontStyle Style;

        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color Color;

        public Color? BackgroundColor;

        public float? BorderLineWidth;

        public float Margin;

        /// <summary>
        /// Угол поворота
        /// </summary>
        public float Angle;

        /// <summary>
        /// Выравнивание текста оносительно точки
        /// </summary>
        public Alignment TextAlignment;

        /// <summary>
        /// Выравнивание текста на слое
        /// </summary>
        public Alignment LayerAlignment;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            var txt = Text ?? GetText();
            var p = GetPoint(rect, LayerAlignment);

            using (var font = gr.Instruments.CreateFont(FontName, Size, Color, Style))
            using (var shape = gr.Shapes.CreateText(font, TextAlignment, Angle))
            {
                var s = shape.Measure(txt);
                s.Width += Margin*2;
                s.Height += Margin * 2;

                if (BackgroundColor != null)
                {
                    using (var brush = gr.Instruments.CreateSolidBrush(BackgroundColor.Value))
                    using (var shape2 = gr.Shapes.CreateFillRectangleArea(brush, TextAlignment))
                    {
                        shape2.Render(p, s, Margin,Margin);
                    }
                }

                shape.Render(txt, p);

                if(BorderLineWidth!=null)
                {
                    using (var pen = gr.Instruments.CreatePen(Color, BorderLineWidth.Value, LineStyle.Solid))
                    using (var shape3 = gr.Shapes.CreateDrawRectangleArea(pen, TextAlignment))
                    {
                        shape3.Render(p, s, Margin, Margin);
                    }
                }

            }


        }

        private Point<float> GetPoint(Rectangle<float> rectangle, Alignment alignment)
        {
            var point = new Point<float> { X = rectangle.Left + Margin, Y = rectangle.Bottom + Margin };
            var width = rectangle.Right - rectangle.Left;
            var height = rectangle.Top - rectangle.Bottom;

            // Если выравнивание по центру
            if (((alignment & Alignment.Left) != 0 && (alignment & Alignment.Right) != 0)
                || ((alignment & Alignment.Left) == 0 && (alignment & Alignment.Right) == 0))
            {
                point.X += width / 2.0f - Margin;
            }
            // Если выравнивание по правому краю
            else if ((alignment & Alignment.Right) != 0)
            {
                point.X += width- Margin*2;
            }

            // Если выравнивание по центру
            if (((alignment & Alignment.Bottom) != 0 && (alignment & Alignment.Top) != 0)
                || ((alignment & Alignment.Bottom) == 0 && (alignment & Alignment.Top) == 0))
            {
                point.Y += height/2.0f-Margin;
            }
                //  Если по верхнему краю
            else if ((alignment & Alignment.Top) != 0)
            {
                point.Y += height-Margin*2;
            }

            return point;
        }
    }
}
