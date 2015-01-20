using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Translators
{
    public interface IPointTranslator
    {
        Rectangle<float> Src { get; set; }

        Rectangle<float> Dst { get; set; }

        Point<float> Translate(Point<float> val);
    }
}
