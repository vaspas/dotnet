using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.FastFourierTransform
{
    /// <summary>
    /// Интерфейс для классов преобразований Фурье.
    /// </summary>
    interface IFastFourierTransform
    {
        /// <summary>
        /// Подготавливает класс для работы.
        /// Клиенту следует вызывать эту функцию перед началом использования класса.
        /// </summary>
        /// <param name="block_size_power2">Размер блока как степень двойки.</param>
        /// <param name="winType">Тип окна.</param>
        /// <returns>true если произошла подготовка, false если в ней небыло нужды</returns>
        bool Prepare(int block_size_power2, WindowType winType);

        /// <summary>
        /// Возвращает ссылку на массив значений окна.
        /// </summary>
        float[] WinArr
        {
            get;
        }

        /// <summary>
        /// Возвращает размер блока анализа.
        /// </summary>
        int BlockSize
        {
            get;
        }

        /// <summary>
        /// Возвращает размер блока результата.
        /// </summary>
        int OutBlockSize
        {
            get;
        }

        /// <summary>
        /// Возвращает ссылку на массив результат действ. значений.
        /// </summary>
        float[] ResultRe
        {
            get;
        }

        /// <summary>
        /// Возвращает ссылку на массив результат мнимых. значений.
        /// </summary>
        float[] ResultIm
        {
            get;
        }
    }
}
