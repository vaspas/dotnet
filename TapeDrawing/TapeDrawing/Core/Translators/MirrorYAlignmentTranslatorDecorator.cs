using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Translators
{
    class MirrorYAlignmentTranslatorDecorator : IAlignmentTranslator
    {
        public IAlignmentTranslator Internal { get; set; }

        public Alignment Translate(Alignment val)
        {
            var internalValue = Internal.Translate(val);

            var newValue = Alignment.None;

            if ((internalValue & Alignment.Left) != 0)
                newValue |= Alignment.Left;
            if ((internalValue & Alignment.Right) != 0)
                newValue |= Alignment.Right;
            if ((internalValue & Alignment.Bottom) != 0)
                newValue |= Alignment.Top;
            if ((internalValue & Alignment.Top) != 0)
                newValue |= Alignment.Bottom;

            return newValue;
        }
    }
}
