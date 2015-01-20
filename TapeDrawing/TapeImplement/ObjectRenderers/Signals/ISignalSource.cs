namespace TapeImplement.ObjectRenderers.Signals
{
    /// <summary>
    /// Источник сигнала для рисования графика
    /// </summary>
    public interface ISignalSource
    {
        float GetStartIndex(int fromIndex);
        float Step { get; }
        float GetNextValue();
        bool IsDataAvailable { get; }
    }
}
