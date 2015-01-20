
namespace TapeDrawing.Core.Instruments
{
    public interface IBrush : IInstrument
    {}

    public interface IFont : IInstrument
    {}

    public interface IPen : IInstrument
    {}

    public interface IImage : IInstrument
    {
        float Width { get; }
        float Height { get; }
    }
}
