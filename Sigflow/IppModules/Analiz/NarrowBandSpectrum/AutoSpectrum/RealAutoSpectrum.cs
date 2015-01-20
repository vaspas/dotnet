using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.AutoSpectrum
{
    /// <summary>
    /// Класс для рассчета авто спектра вещественного сигнала.
    /// </summary>
    internal class RealAutoSpectrum:AutoSpectrum
    {
        public RealAutoSpectrum()
        {
            FFTransform = new FastFourierTransform.RealFastFourierTransform(ipp.IppsFFTNorm.ippFftNoDivByAny, ipp.IppHintAlgorithm.ippAlgHintNone);
        }

        /// <summary>
        /// Функция для рассчета спектра.
        /// </summary>
        /// <param name="pCountArr">Указатель на массив исх. данных (отсчетов), длина должна быть = block_size.</param>
        /// <param name="pSpectrArr">Указатель на массив спектр, длина должна быть такова, чтобы умещалось = block_size/2 значений.</param>
        public unsafe void CalculateAutoSpectrum(float* pCountArr, float* pSpectrArr)
        {
            //рассчет БПФ
            (FFTransform as FastFourierTransform.RealFastFourierTransform)
                .CalculateFFT(pCountArr);
            //рассчет авто спектра
            fixed (float*
                pre = FFTransform.ResultRe, pim = FFTransform.ResultIm)      //указатели на массивы реальной и мнимой частей  
            {
                //учитываем нормировочный коэффициент окна
                ipp.sp.ippsMulC_32f_I(k_norm_, pre, FFTransform.BlockSize / 2);
                ipp.sp.ippsMulC_32f_I(k_norm_, pim, FFTransform.BlockSize / 2);

                //рассчет квадрата (мощность)
                ipp.sp.ippsPowerSpectr_32f(pre, pim, pSpectrArr, FFTransform.BlockSize / 2);

                //рассчет плотности
                if (unit_ == SpectrumUnit.Psd || unit_ == SpectrumUnit.Rmssd)
                    ipp.sp.ippsDivC_32f_I(k_widthFr_, pSpectrArr, FFTransform.BlockSize / 2);

                //переводим из мощности в СКЗ
                if (unit_ == SpectrumUnit.Rms || unit_ == SpectrumUnit.Rmssd)
                    ipp.sp.ippsSqrt_32f_I(pSpectrArr, FFTransform.BlockSize / 2);
            }
        }
    }
}
