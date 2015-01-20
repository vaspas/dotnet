using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.CrossSpectrum
{
    /// <summary>
    /// ����� ��� �������� �������� �������� ������������ �������
    /// </summary>
    internal unsafe class ComplexCrossSpectrum : CrossSpectrum
    {
        public ComplexCrossSpectrum()
        {
            FFTransform = new FastFourierTransform.ComplexFastFourierTransform(ipp.IppsFFTNorm.ippFftNoDivByAny, ipp.IppHintAlgorithm.ippAlgHintNone);
        }

        /// <summary>
        /// ������� ���.
        /// </summary>
        /// <param name="chan">����� ������ ������ ������� � 0.</param>
        /// <param name="pCountArrRe">��������� �� ������ ������������ ����� �������.</param>
        /// <param name="pCountArrIm">��������� �� ������ ������ ����� �������.</param>
        public void CalculateFFT(int chan, float* pCountArrRe, float* pCountArrIm)
        {
            //�������� ������������ ������ � ������
            if (chan >= nchans_)
                return;

            //������������ ���
            (FFTransform as FastFourierTransform.ComplexFastFourierTransform).CalculateFFT(pCountArrRe, pCountArrIm);

            //�������� ���������� ������ � ���������
            fixed (float* pStorRe = dataStorageRe_, pStorIm = dataStorageIm_, pFFTRe = FFTransform.ResultRe, pFFTIm = FFTransform.ResultIm)
            {
                //��������� ������������� ����������� ����                
                ipp.sp.ippsMulC_32f_I(k_norm_, pFFTRe, FFTransform.OutBlockSize);
                ipp.sp.ippsMulC_32f_I(k_norm_, pFFTIm, FFTransform.OutBlockSize);

                //��������
                //������ �������������� ������������� � ������������� ��������
                //����� 0 ��� ����������
                ipp.sp.ippsCopy_32f(pFFTRe, pStorRe + chan * FFTransform.OutBlockSize + FFTransform.OutBlockSize/2, FFTransform.OutBlockSize/2);
                ipp.sp.ippsCopy_32f(pFFTRe + FFTransform.OutBlockSize / 2, pStorRe + chan * FFTransform.OutBlockSize, FFTransform.OutBlockSize / 2);
                ipp.sp.ippsCopy_32f(pFFTIm, pStorIm + chan * FFTransform.OutBlockSize + FFTransform.OutBlockSize / 2, FFTransform.OutBlockSize / 2);
                ipp.sp.ippsCopy_32f(pFFTIm + FFTransform.OutBlockSize / 2, pStorIm + chan * FFTransform.OutBlockSize, FFTransform.OutBlockSize / 2);
            }
        }
    }
}
