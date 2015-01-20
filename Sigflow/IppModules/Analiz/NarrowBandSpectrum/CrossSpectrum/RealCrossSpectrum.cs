using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.CrossSpectrum
{
    /// <summary>
    /// Класс для рассчета автоспектра комплексного сигнала
    /// </summary>
    internal unsafe class RealCrossSpectrum:CrossSpectrum
    {
        public RealCrossSpectrum()
        {
            FFTransform = new FastFourierTransform.RealFastFourierTransform(ipp.IppsFFTNorm.ippFftNoDivByAny, ipp.IppHintAlgorithm.ippAlgHintNone);
        }

        /// <summary>
        /// Рассчет БПФ.
        /// </summary>
        /// <param name="chan">Номер канала данных начиная с 0.</param>
        /// <param name="pCountArr">Указатель на массив данных сигнала.</param>
        public void CalculateFFT(ushort chan, float* pCountArr)
        {
            //проверка корректности канала и расчет
            if (chan >= nchans_)
                return;

            //рассчитываем БПФ
            (FFTransform as FastFourierTransform.RealFastFourierTransform).CalculateFFT(pCountArr);

            //копируем полученные данные в хранилище
            fixed (float* pStorRe = dataStorageRe_, pStorIm = dataStorageIm_,pFFTRe=FFTransform.ResultRe,pFFTIm=FFTransform.ResultIm)
            {
                //учитываем нормировочный коэффициент окна                
                ipp.sp.ippsMulC_32f_I(k_norm_, pFFTRe, FFTransform.OutBlockSize);
                ipp.sp.ippsMulC_32f_I(k_norm_, pFFTIm, FFTransform.OutBlockSize);                

                ipp.sp.ippsCopy_32f(pFFTRe, pStorRe + chan * FFTransform.OutBlockSize, FFTransform.OutBlockSize);
                ipp.sp.ippsCopy_32f(pFFTIm, pStorIm + chan * FFTransform.OutBlockSize, FFTransform.OutBlockSize);
            }
        }

    }
}
