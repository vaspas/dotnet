using System;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.TapeModels.Kuges.LayerArea
{
    public class TwoPointsArea : IArea<float>
    {
        public Alignment Alignment;

        public Size<float> Size;

        public Point<float> FirstPoint;
        public Point<float> SecondPoint;

        public IPointTranslator Translator;

        /// <summary>
        /// Позиция ленты, может быть null.
        /// </summary>
        public IScalePosition<int> TapePosition;

        /// <summary>
        /// Границы отображения сигнала на ленте, может быть null.
        /// </summary>
        public IScalePosition<float> Diapazone;

        /// <summary>
        /// Возвращает положение слоя.
        /// </summary>
        /// <param name="parentSize">Текущий размер родителя.</param>
        /// <returns></returns>
        public Rectangle<float> GetRectangle(Size<float> parentSize)
        {
            Translator.Src = new Rectangle<float>
            {
                Left = TapePosition.From,
                Right = TapePosition.To,
                Bottom = Diapazone.From,
                Top = Diapazone.To
            };
            Translator.Dst = new Rectangle<float>
                                 {Left = 0, Right = parentSize.Width, Bottom = 0, Top = parentSize.Height};


            var result = new Rectangle<float>();

            var firstTranslated = Translator.Translate(FirstPoint);
            var secondTranslated = Translator.Translate(SecondPoint);

            if ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0)
            {
                var center = (firstTranslated.X + secondTranslated.X) / 2;
                result.Left = center - Size.Width / 2;
                result.Right = center + Size.Width / 2;
            }
            else
            {
                if ((Alignment & Alignment.Left) != 0)
                    result.Left = Math.Min(firstTranslated.X, secondTranslated.X);
                if ((Alignment & Alignment.Right) != 0)
                    result.Right = Math.Max(firstTranslated.X, secondTranslated.X);

                if ((Alignment & Alignment.Left) == 0)
                    result.Left = result.Right - Size.Width;
                if ((Alignment & Alignment.Right) == 0)
                    result.Right = result.Left + Size.Width;
            }

            if ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0)
            {
                var center = (firstTranslated.Y + secondTranslated.Y) / 2;
                result.Bottom = center - Size.Height / 2;
                result.Top = center + Size.Height / 2;
            }
            else
            {
                if ((Alignment & Alignment.Bottom) != 0)
                    result.Bottom = Math.Min(firstTranslated.Y, secondTranslated.Y);
                if ((Alignment & Alignment.Top) != 0)
                    result.Top = Math.Max(firstTranslated.Y, secondTranslated.Y);

                if ((Alignment & Alignment.Bottom) == 0)
                    result.Bottom = result.Top - Size.Height;
                if ((Alignment & Alignment.Top) == 0)
                    result.Top = result.Bottom + Size.Height;
            }
            
            return result;
        }

    }
}
