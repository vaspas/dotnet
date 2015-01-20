
using System.Linq;

namespace IncModules.Ink824.Modifications
{
    public class Modifications : IModifications
    {
        public string Gain0 { get; set; }

        public string Gain10 { get; set; }

        public string Gain20 { get; set; }

        public string Gain30 { get; set; }

        public string QuantumFreqCorrPpu { get; set; }

        public double Get(GainValues gain, int channel)
        {
            string arrayStr = null;
            if (gain == GainValues.Gain_0)
                arrayStr = Gain0;
            if (gain == GainValues.Gain_10)
                arrayStr = Gain10;
            if (gain == GainValues.Gain_20)
                arrayStr = Gain20;
            if (gain == GainValues.Gain_30)
                arrayStr = Gain30;

            if (string.IsNullOrEmpty(arrayStr))
                return 1;

            try
            {
                var array = arrayStr.Split(new[] { ';' }).Select(double.Parse).ToArray();

                return array.Length > channel ? array[channel] : 1;
            }
            catch
            {
                return 1;
            }
        }

        public double GetQuantumFreqCorrPpu()
        {
            double res;

            double.TryParse(QuantumFreqCorrPpu, out res);

            return res;
        }
    }
}
