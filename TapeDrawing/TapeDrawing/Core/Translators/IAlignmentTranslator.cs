
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Translators
{
    public interface IAlignmentTranslator
    {
        Alignment Translate(Alignment val);
    }
}
