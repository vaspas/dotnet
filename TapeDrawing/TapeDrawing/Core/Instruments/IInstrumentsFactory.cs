
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Instruments
{
    public interface IInstrumentsFactory
    {
        IBrush CreateSolidBrush(Color color);

        IPen CreatePen(Color color, float width, LineStyle style);

        IFont CreateFont(string type, int size, Color color, FontStyle style);

        IImage CreateImage<T>(T data);

        IImage CreateImagePortion<T>(T data, Rectangle<float> roi);
    }
}
