using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum
{
    /// <summary>
    /// Единицы измерения спектров.
    /// </summary>
    public enum SpectrumUnit
    {
        /// <summary>
        /// Средне квадратичное значение
        /// </summary>
        Rms,
        /// <summary>
        /// Мощности
        /// </summary>
        Pwr,
        /// <summary>
        /// Плотности
        /// </summary>
        Rmssd,
        /// <summary>
        /// Плотность мощности
        /// </summary>
        Psd,
        /// <summary>
        /// 
        /// </summary>
        Esd
    }
}
