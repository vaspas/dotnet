
namespace TapeDrawing.Core
{
    public interface IGraphicContext
    {
        Instruments.IInstrumentsFactory Instruments { get; }

        Shapes.IShapesFactory Shapes { get; }

        IClip CreateClip();
    }
}
