
namespace IppModules.Analiz.NarrowBandSpectrum
{
    /// <summary>
    /// Тип взвешивающего окна.
    /// </summary>
    public enum WindowType
    {
        /// <summary>
        /// Прямоугольное
        /// </summary>
        Rectangular = 0,
        /// <summary>
        /// Ханна
        /// </summary>
        Hann = 10,
        /// <summary>
        /// Хэмминга
        /// </summary>
        Hamming = 20,
        /// <summary>
        /// Кайзера
        /// </summary>
        Kaiser30 = 30,
        /// <summary>
        /// Барлетта
        /// </summary>
        Bartlett = 40,
        /// <summary>
        /// Блэкмана
        /// </summary>
        Blackman = 50,
        /// <summary>
        /// Блэкмана оптимальное
        /// </summary>
        BlackmanOpt = 51,
        /// <summary>
        /// Блэкмана стандартное
        /// </summary>
        BlackmanStd = 52,
        /// <summary>
        /// Блэкмана-Харриса 90 дБ
        /// </summary>
        BlackmanHarris90 = 53,
        /// <summary>
        /// Плоская ЧХ
        /// </summary>
        FlatTop = 60
    };
}
