using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.SimpleRenderers
{
    /// <summary>
    /// Рендерер для отображения границ
    /// </summary>
    public class BorderRenderer : IRenderer
    {
        /// <summary>
        /// Цвет линий границ
        /// </summary>
        public Color Color;

        /// <summary>
        /// Рисовать границу слева
        /// </summary>
        public bool Left;

        /// <summary>
        /// Рисовать границу справа
        /// </summary>
        public bool Right;

        /// <summary>
        /// Рисовать границу снизу
        /// </summary>
        public bool Bottom;

        /// <summary>
        /// Рисовать границу сверху
        /// </summary>
        public bool Top;

        /// <summary>
        /// Стиль линии для рисования границы
        /// </summary>
        public LineStyle LineStyle;

        /// <summary>
        /// Толщина линии для рисования границы
        /// </summary>
        public float LineWidth;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            using (var pen = gr.Instruments.CreatePen(Color, LineWidth, LineStyle))
            using (var shape = gr.Shapes.CreateLines(pen))
            {
                if (Left)
                {
                    var pnts = new List<Point<float>>
                                   {
                                       new Point<float>{X=rect.Left, Y=rect.Bottom},
                                       new Point<float>{X=rect.Left, Y=rect.Top}
                                   };
                    shape.Render(pnts);
                }
                if (Right)
                {
                    var pnts = new List<Point<float>>
                                   {
                                       new Point<float>{X=rect.Right, Y=rect.Bottom},
                                       new Point<float>{X=rect.Right, Y=rect.Top}
                                   };
                    shape.Render(pnts);
                }
                if (Bottom)
                {
                    var pnts = new List<Point<float>>
                                   {
                                       new Point<float>{X=rect.Left, Y=rect.Bottom},
                                       new Point<float>{X=rect.Right, Y=rect.Bottom}
                                   };
                    shape.Render(pnts);
                }
                if (Top)
                {
                    var pnts = new List<Point<float>>
                                   {
                                       new Point<float>{X=rect.Left, Y=rect.Top},
                                       new Point<float>{X=rect.Right, Y=rect.Top}
                                   };
                    shape.Render(pnts);
                }
            }
        }
    }
}
