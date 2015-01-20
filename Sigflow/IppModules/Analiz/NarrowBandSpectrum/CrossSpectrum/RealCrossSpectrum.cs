using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.CrossSpectrum
{
    /// <summary>
    /// ����� ��� �������� ����������� ������������ �������
    /// </summary>
    internal unsafe class RealCrossSpectrum:CrossSpectrum
    {
        public RealCrossSpectrum()
        {
            FFTransform = new FastFourierTransform.RealFastFourierTransform(ipp.IppsFFTNorm.ippFftNoDivByAny, ipp.IppHintAlgorithm.ippAlgHintNone);
        }

        /// <summary>
        /// ������� ���.
        /// </summary>
        /// <param name="chan">����� ������ ������ ������� � 0.</param>
        /// <param name="pCountArr">��������� �� ������ ������ �������.</param>
        public void CalculateFFT(ushort chan, float* pCountArr)
        {
            //�������� ������������ ������ � ������
            if (chan >= nchans_)
                return;

            //������������ ���
            (FFTransform as FastFourierTransform.RealFastFourierTransform).CalculateFFT(pCountArr);

            //�������� ���������� ������ � ���������
            fixed (float* pStorRe = dataStorageRe_, pStorIm = dataStorageIm_,pFFTRe=FFTransform.ResultRe,pFFTIm=FFTransform.ResultIm)
            {
                //��������� ������������� ����������� ����                
                ipp.sp.ippsMulC_32f_I(k_norm_, pFFTRe, FFTransform.OutBlockSize);
                ipp.sp.ippsMulC_32f_I(k_norm_, pFFTIm, FFTransform.OutBlockSize);                

                ipp.sp.ippsCopy_32f(pFFTRe, pStorRe + chan * FFTransform.OutBlockSize, FFTransform.OutBlockSize);
                ipp.sp.ippsCopy_32f(pFFTIm, pStorIm + chan * FFTransform.OutBlockSize, FFTransform.OutBlockSize);
            }
        }

    }
}
