#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace IppModules.Analiz.NarrowBandSpectrum.AutoSpectrum
{
    /// <summary>
    /// Базовый класс для рассчета авто спектра 1 блока данных.
    /// Наследникам необходимо создать объект для рассчета БПФ, и передать его этому классу. 
    /// И создать публичную функцию рассчета авто спектра.
    /// </summary>
    internal abstract unsafe class AutoSpectrum
    {        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public AutoSpectrum()
        { }

        #region ///// protected fields /////

        /// <summary>
        /// Объект для преобразования Фурье.
        /// </summary>
        protected FastFourierTransform.IFastFourierTransform FFTransform;

        /// <summary>
        /// Нормировочный коэффициент взвешивающего окна.
        /// </summary>
        protected float k_norm_;

        /// <summary>
        /// Единицы измерения спектра.
        /// </summary>
        protected SpectrumUnit unit_;

        /// <summary>
        /// Ширина полосы.
        /// </summary>
        protected float k_widthFr_;

        #endregion

        /// <summary>
        /// Реальная часть сигнала после преобразования.
        /// </summary>
        public float[] FftTransformRe
        {
            get { return FFTransform.ResultRe; }
        }

        /// <summary>
        /// Мнимая часть сигнала после преобразования.
        /// </summary>
        public float[] FftTransformIm
        {
            get { return FFTransform.ResultIm; }
        }

        /// <summary>
        /// Подготавливает класс для работы.
        /// Клиенту следует вызывать эту функцию перед началом использования класса.
        /// </summary>
        /// <param name="block_size_power2">Размер блока как степень двойки.</param>
        /// <param name="winType">Тип окна.</param>
        /// <param name="unit">Тип спектра.</param>
        /// <param name="fQu">Частота квантования.</param>
        public void PrepareAutoSpectrum(int block_size_power2, WindowType winType, SpectrumUnit unit, double fQu)
        {
            //сначала подготавливаем внутренний объект
            bool fftChanged = FFTransform.Prepare(block_size_power2, winType);

            //рассчитываем ширину полосы
            k_widthFr_ = (float)(fQu / FFTransform.BlockSize);

            //проверяем изменились ли какие нибудь значения
            if (fftChanged || unit != unit_)
            {
                //запоминаем тип спектра
                unit_ = unit;

                //рассчитываем нормировочный коэффициент
                k_norm_ = CalculateKNorm();
            }

        }

        /// <summary>
        /// Рассчитывает и возвращает нормировочный коэффициент.
        /// </summary>
        /// <returns></returns>
        private float CalculateKNorm()
        {
            //рассчитываем нормировочный коэффициент
            float s = 0;
            for (int i = 0; i < FFTransform.WinArr.Length; i++)
            {
                if (unit_ == SpectrumUnit.Psd || unit_ == SpectrumUnit.Rmssd)
                    //рассчет плотности
                    s += FFTransform.WinArr[i] * FFTransform.WinArr[i];
                else
                    s += FFTransform.WinArr[i];
            }
            return (float)Math.Sqrt(2) / s;
        }

    }
}
