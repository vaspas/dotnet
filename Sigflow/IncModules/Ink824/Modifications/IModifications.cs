
namespace IncModules.Ink824.Modifications
{
    /// <summary>
    /// Источник поправок.
    /// Создан изза бардака с API драйвера АЦП.
    /// </summary>
    public interface IModifications
    {
        double Get(GainValues gain, int channel);

        double GetQuantumFreqCorrPpu();
    }
}
