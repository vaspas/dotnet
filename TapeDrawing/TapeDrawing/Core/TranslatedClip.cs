using System;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeDrawing.Core
{
    public class TranslatedClip:IClip
    {
        public IClip Target { get; set; }

        public IPointTranslator Translator { get; set; }

        public void Set(Rectangle<float> rectangle)
        {
            var p1 = Translator.Translate(new Point<float> { X = rectangle.Left, Y = rectangle.Bottom });
            var p2 = Translator.Translate(new Point<float> { X = rectangle.Right, Y = rectangle.Top });

            Target.Set(new Rectangle<float>
                           {
                               Left = Math.Min(p1.X, p2.X),
                               Right = Math.Max(p1.X, p2.X),
                               Bottom = Math.Max(p1.Y, p2.Y),
                               Top = Math.Min(p1.Y, p2.Y)
                           });
        }

        public void Undo()
        {
            Target.Undo();
        }
    }
}
