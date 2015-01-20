using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Renderer для подписей шкалы значений сигнала
    /// Рисует подписи шкалы в соответствии с указанными в массиве значениями
    /// </summary>
    public class ScaleTextRenderer : IRenderer
    {
        public ScaleTextRenderer()
        {
            LayerAlignment = 0.5f;
        }

        /// <summary>
        /// Список значений в линейке
        /// </summary>
        public float[] Values;

        /// <summary>
        /// Нижняя граница шкалы
        /// </summary>
        public Func<float> GetMin;

        /// <summary>
        /// Верхняя граница шкалы
        /// </summary>
        public Func<float> GetMax;

        public Func<float, string> ValuePresentation;
        
        /// <summary>
        /// Транслятор точек
        /// </summary>
        public IPointTranslator Translator;

        /// <summary>
        /// Транслятор выравнивания текста
        /// </summary>
        public IAlignmentTranslator TextAlignmentTranslator;

        /// <summary>
        /// Название шрифта
        /// </summary>
        public string FontName;

        /// <summary>
        /// Размер шрифта
        /// </summary>
        public int FontSize;

        /// <summary>
        /// Стиль шрифта
        /// </summary>
        public FontStyle FontStyle;

        /// <summary>
        /// Выравнивание текста оносительно точки
        /// </summary>
        public Alignment Alignment;

        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color Color;

        /// <summary>
        /// Угол поворота текста
        /// </summary>
        public float Angle;

        public float LayerAlignment;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Src = new Rectangle<float> { Left = 0, Right = 1, Bottom = GetMin(), Top = GetMax() };
            Translator.Dst = rect;

            foreach (var value in Values) DrawText(gr, value);
        }

        private void DrawText(IGraphicContext context, float value)
        {
            using (var font = context.Instruments.CreateFont(FontName, FontSize, Color, FontStyle))
            using (var shape = context.Shapes.CreateText(font, TextAlignmentTranslator.Translate(Alignment), Angle))
            {
                shape.Render(ValuePresentation!=null?ValuePresentation(value):value.ToString(),
                             Translator.Translate(new Point<float>{X=LayerAlignment,Y= value}));
            }
        }
    }
}
