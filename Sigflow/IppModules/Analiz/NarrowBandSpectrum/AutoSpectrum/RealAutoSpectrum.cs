using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.AutoSpectrum
{
    /// <summary>
    /// ����� ��� �������� ���� ������� ������������� �������.
    /// </summary>
    internal class RealAutoSpectrum:AutoSpectrum
    {
        public RealAutoSpectrum()
        {
            FFTransform = new FastFourierTransform.RealFastFourierTransform(ipp.IppsFFTNorm.ippFftNoDivByAny, ipp.IppHintAlgorithm.ippAlgHintNone);
        }

        /// <summary>
        /// ������� ��� �������� �������.
        /// </summary>
        /// <param name="pCountArr">��������� �� ������ ���. ������ (��������), ����� ������ ���� = block_size.</param>
        /// <param name="pSpectrArr">��������� �� ������ ������, ����� ������ ���� ������, ����� ��������� = block_size/2 ��������.</param>
        public unsafe void CalculateAutoSpectrum(float* pCountArr, float* pSpectrArr)
        {
            //������� ���
            (FFTransform as FastFourierTransform.RealFastFourierTransform)
                .CalculateFFT(pCountArr);
            //������� ���� �������
            fixed (float*
                pre = FFTransform.ResultRe, pim = FFTransform.ResultIm)      //��������� �� ������� �������� � ������ ������  
            {
                //��������� ������������� ����������� ����
                ipp.sp.ippsMulC_32f_I(k_norm_, pre, FFTransform.BlockSize / 2);
                ipp.sp.ippsMulC_32f_I(k_norm_, pim, FFTransform.BlockSize / 2);

                //������� �������� (��������)
                ipp.sp.ippsPowerSpectr_32f(pre, pim, pSpectrArr, FFTransform.BlockSize / 2);

                //������� ���������
                if (unit_ == SpectrumUnit.Psd || unit_ == SpectrumUnit.Rmssd)
                    ipp.sp.ippsDivC_32f_I(k_widthFr_, pSpectrArr, FFTransform.BlockSize / 2);

                //��������� �� �������� � ���
                if (unit_ == SpectrumUnit.Rms || unit_ == SpectrumUnit.Rmssd)
                    ipp.sp.ippsSqrt_32f_I(pSpectrArr, FFTransform.BlockSize / 2);
            }
        }
    }
}
