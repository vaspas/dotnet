using TapeDrawing.Core.Primitives;

namespace TapeImplement.ObjectRenderers.Signals
{
    /// <summary>
    /// Источник сигнала для рисования графика
    /// </summary>
    public interface ISignalPointSource
    {
        Point<float>? GetStartPoint(int fromIndex, int toIndex);
        Point<float>? GetNextPoint();
    }
}
