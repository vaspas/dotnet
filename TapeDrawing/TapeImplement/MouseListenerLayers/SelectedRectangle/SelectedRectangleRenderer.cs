using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.MouseListenerLayers.SelectedRectangle
{
    public class SelectedRectangleRenderer : IRenderer
    {

        public Rectangle<float> Position;

        /// <summary>
        /// Цвет
        /// </summary>
        public Color? FillColor;

        /// <summary>
        /// Цвет
        /// </summary>
        public Color? LineColor;

        /// <summary>
        /// Ширина линии
        /// </summary>
        public int LineWidth;

        /// <summary>
        /// Стиль линии
        /// </summary>
        public LineStyle LineStyle;

        /// <summary>
        /// Транслятор точек
        /// </summary>
        public IPointTranslator Translator;


        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Src = new Rectangle<float> { Left = 0f, Right = 1f, Bottom = 0f, Top = 1f };
            Translator.Dst = rect;

            var shapes=TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator
                .For(gr.Shapes).Translate(Translator).Result;
            
            if(FillColor!=null)
            {
                using (var brush = gr.Instruments.CreateSolidBrush(FillColor.Value))
                using (var fillRectShape = shapes.CreateFillRectangle(brush))
                    fillRectShape.Render(Position);
            }

            if (LineColor != null)
            {
                using (var pen = gr.Instruments.CreatePen(LineColor.Value, LineWidth, LineStyle))
                using (var drawRectShape = shapes.CreateDrawRectangle(pen))
                    drawRectShape.Render(Position);
            }
        }
    }
}
