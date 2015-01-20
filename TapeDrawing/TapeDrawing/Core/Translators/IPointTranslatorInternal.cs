using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Translators
{
    internal interface IPointTranslatorInternal:IPointTranslator
    {
        ILinearTranslator TranslatorX { get; set; }

        ILinearTranslator TranslatorY { get; set; }
    }
}
