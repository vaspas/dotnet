using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Translators
{
    class FakeAlignmentTranslator : IAlignmentTranslator
    {
        public Alignment Translate(Alignment val)
        {
            return val;
        }
    }
}
