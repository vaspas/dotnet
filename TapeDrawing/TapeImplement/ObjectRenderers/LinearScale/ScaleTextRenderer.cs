using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.ObjectRenderers.LinearScale
{
    public class ScaleTextRenderer : ScaleBase, IRenderer
    {
        /// <summary>
        /// Цвет
        /// </summary>
        public Color FontColor;

        public string FontName;

        public int FontSize;

        public FontStyle FontStyle;

        public float Angle;

        public Func<float, string> ScalePresentation;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Src = new Rectangle<float> { Left = Diapazone.From, Right = Diapazone.To, Bottom = 0, Top = 1 };
            Translator.Dst = rect;

            using (var font = gr.Instruments.CreateFont(FontName, FontSize, FontColor, FontStyle))
            using (var textShape = gr.Shapes.CreateText(font, Alignment.None, Angle))
            {
                foreach (var code in GetCodes())

                    textShape.Render(ScalePresentation(code), Translator.Translate(new Point<float> { X = code, Y = 0.5f }));
            }
        }
    }
}
