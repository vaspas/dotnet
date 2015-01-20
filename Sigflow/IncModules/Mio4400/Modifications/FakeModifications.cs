
namespace IncModules.Mio4400.Modifications
{
    public class FakeModifications : IModifications
    {
        public double Get(GainValues gain, int channel)
        {
            return 1;
        }

        public double GetQuantumFreqCorrPpu()
        {
            return 0;
        }
    }
}
