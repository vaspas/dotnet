using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.ObjectRenderers
{
    public class RegionTextRenderer<T> : IRenderer
    {
        /// <summary>
        /// Цвет
        /// </summary>
        public Color FontColor;

        public string FontName;

        public int FontSize;

        public FontStyle FontStyle;

        public float Angle;

        public Func<T, string> ObjectPresentation;

        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        /// <summary>
        /// Источник данных для отображения
        /// </summary>
        public IObjectSource<T> Source;

        /// <summary>
        /// Объект для преобразования точек
        /// </summary>
        public IPointTranslator Translator;

        public Func<T, int> GetFrom;
        public Func<T, int> GetTo;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (TapePosition.From >= TapePosition.To)
                return;

            Translator.Src = new Rectangle<float>
                                 {Left = TapePosition.From, Right = TapePosition.To, Bottom = 0f, Top = 1f};
            Translator.Dst = rect;


            var shapes = TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator
                .For(gr.Shapes).Translate(Translator).Result;

            using (var font = gr.Instruments.CreateFont(FontName, FontSize, FontColor, FontStyle))
            using (var textShape = shapes.CreateText(font, Alignment.None, Angle))
            {
                foreach (var r in Source.GetData(TapePosition.From, TapePosition.To))
                {
                    var code = (Math.Max(TapePosition.From, GetFrom(r)) + Math.Min(TapePosition.To, GetTo(r))) / 2;

                    textShape.Render(ObjectPresentation(r), new Point<float> {X = code, Y = 0.5f});
                }
            }
        }
    }
}
