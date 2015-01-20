using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.AutoSpectrum
{
    /// <summary>
    /// Класс для рассчета автоспектра комплексного сигнала
    /// </summary>
    internal class ComplexAutoSpectrum:AutoSpectrum
    {
        public ComplexAutoSpectrum()
        {
            FFTransform = new FastFourierTransform.ComplexFastFourierTransform(ipp.IppsFFTNorm.ippFftNoDivByAny, ipp.IppHintAlgorithm.ippAlgHintNone);
            ExchangeHalfs = true;
        }

        public bool ExchangeHalfs { get; set; }

        /// <summary>
        /// Функция для рассчета спектра.
        /// </summary>
        /// <param name="pCountArrRe">Указатель на массив исх. данных (отсчетов) вещественная часть, длина должна быть = block_size.</param>
        /// <param name="pCountArrIm">Указатель на массив исх. данных (отсчетов) мнимая часть, длина должна быть = block_size.</param>
        /// <param name="pSpectrArr">указатель на массив спектр, длина должна быть такова, чтобы умещалось = block_size значений.</param>        
        public unsafe void CalculateAutoSpectrum(float* pCountArrRe, float* pCountArrIm, float* pSpectrArr)
        {
            
            //рассчет КБПФ
            (FFTransform as FastFourierTransform.ComplexFastFourierTransform)
                .CalculateFFT(pCountArrRe, pCountArrIm);
            //рассчет авто спектра
            //рассчет авто спектра
            fixed (float*
                pre = FFTransform.ResultRe, pim = FFTransform.ResultIm)      //указатели на массивы реальной и мнимой частей  
            {
                //учитываем нормировочный коэффициент окна
                ipp.sp.ippsMulC_32f_I(k_norm_, pre, FFTransform.BlockSize);
                ipp.sp.ippsMulC_32f_I(k_norm_, pim, FFTransform.BlockSize);
                
                //рассчет квадрата (мощность)
                if (ExchangeHalfs)
                {
                    //заодно переворачиваем положительные и отрицательные значения
                    //чтобы 0 был посередине
                    var halfSize = FFTransform.BlockSize/2;
                    ipp.sp.ippsPowerSpectr_32f(pre, pim, pSpectrArr + halfSize, halfSize);
                    ipp.sp.ippsPowerSpectr_32f(pre + halfSize, pim + halfSize, pSpectrArr, halfSize);
                }
                else
                {
                    ipp.sp.ippsPowerSpectr_32f(pre, pim, pSpectrArr, FFTransform.BlockSize);
                }

                //рассчет плотности
                if (unit_ == SpectrumUnit.Psd || unit_ == SpectrumUnit.Rmssd)
                    ipp.sp.ippsDivC_32f_I(k_widthFr_, pSpectrArr, FFTransform.BlockSize);

                //переводим из мощности в СКЗ
                if (unit_ == SpectrumUnit.Rms || unit_ == SpectrumUnit.Rmssd)
                    ipp.sp.ippsSqrt_32f_I(pSpectrArr, FFTransform.BlockSize);
            }
        }
    }
}
