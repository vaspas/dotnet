using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.CrossSpectrum
{
    /// <summary>
    /// Класс для рассчета взаимных спектров комплексного сигнала
    /// </summary>
    internal unsafe class ComplexCrossSpectrum : CrossSpectrum
    {
        public ComplexCrossSpectrum()
        {
            FFTransform = new FastFourierTransform.ComplexFastFourierTransform(ipp.IppsFFTNorm.ippFftNoDivByAny, ipp.IppHintAlgorithm.ippAlgHintNone);
        }

        /// <summary>
        /// Рассчет БПФ.
        /// </summary>
        /// <param name="chan">Номер канала данных начиная с 0.</param>
        /// <param name="pCountArrRe">Указатель на массив вещественной части сигнала.</param>
        /// <param name="pCountArrIm">Указатель на массив мнимой части сигнала.</param>
        public void CalculateFFT(int chan, float* pCountArrRe, float* pCountArrIm)
        {
            //проверка корректности канала и расчет
            if (chan >= nchans_)
                return;

            //рассчитываем БПФ
            (FFTransform as FastFourierTransform.ComplexFastFourierTransform).CalculateFFT(pCountArrRe, pCountArrIm);

            //копируем полученные данные в хранилище
            fixed (float* pStorRe = dataStorageRe_, pStorIm = dataStorageIm_, pFFTRe = FFTransform.ResultRe, pFFTIm = FFTransform.ResultIm)
            {
                //учитываем нормировочный коэффициент окна                
                ipp.sp.ippsMulC_32f_I(k_norm_, pFFTRe, FFTransform.OutBlockSize);
                ipp.sp.ippsMulC_32f_I(k_norm_, pFFTIm, FFTransform.OutBlockSize);

                //копируем
                //заодно переворачиваем положительные и отрицательные значения
                //чтобы 0 был посередине
                ipp.sp.ippsCopy_32f(pFFTRe, pStorRe + chan * FFTransform.OutBlockSize + FFTransform.OutBlockSize/2, FFTransform.OutBlockSize/2);
                ipp.sp.ippsCopy_32f(pFFTRe + FFTransform.OutBlockSize / 2, pStorRe + chan * FFTransform.OutBlockSize, FFTransform.OutBlockSize / 2);
                ipp.sp.ippsCopy_32f(pFFTIm, pStorIm + chan * FFTransform.OutBlockSize + FFTransform.OutBlockSize / 2, FFTransform.OutBlockSize / 2);
                ipp.sp.ippsCopy_32f(pFFTIm + FFTransform.OutBlockSize / 2, pStorIm + chan * FFTransform.OutBlockSize, FFTransform.OutBlockSize / 2);
            }
        }
    }
}
