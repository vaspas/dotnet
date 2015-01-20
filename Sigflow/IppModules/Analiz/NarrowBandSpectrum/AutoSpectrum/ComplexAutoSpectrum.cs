using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.AutoSpectrum
{
    /// <summary>
    /// ����� ��� �������� ����������� ������������ �������
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
        /// ������� ��� �������� �������.
        /// </summary>
        /// <param name="pCountArrRe">��������� �� ������ ���. ������ (��������) ������������ �����, ����� ������ ���� = block_size.</param>
        /// <param name="pCountArrIm">��������� �� ������ ���. ������ (��������) ������ �����, ����� ������ ���� = block_size.</param>
        /// <param name="pSpectrArr">��������� �� ������ ������, ����� ������ ���� ������, ����� ��������� = block_size ��������.</param>        
        public unsafe void CalculateAutoSpectrum(float* pCountArrRe, float* pCountArrIm, float* pSpectrArr)
        {
            
            //������� ����
            (FFTransform as FastFourierTransform.ComplexFastFourierTransform)
                .CalculateFFT(pCountArrRe, pCountArrIm);
            //������� ���� �������
            //������� ���� �������
            fixed (float*
                pre = FFTransform.ResultRe, pim = FFTransform.ResultIm)      //��������� �� ������� �������� � ������ ������  
            {
                //��������� ������������� ����������� ����
                ipp.sp.ippsMulC_32f_I(k_norm_, pre, FFTransform.BlockSize);
                ipp.sp.ippsMulC_32f_I(k_norm_, pim, FFTransform.BlockSize);
                
                //������� �������� (��������)
                if (ExchangeHalfs)
                {
                    //������ �������������� ������������� � ������������� ��������
                    //����� 0 ��� ����������
                    var halfSize = FFTransform.BlockSize/2;
                    ipp.sp.ippsPowerSpectr_32f(pre, pim, pSpectrArr + halfSize, halfSize);
                    ipp.sp.ippsPowerSpectr_32f(pre + halfSize, pim + halfSize, pSpectrArr, halfSize);
                }
                else
                {
                    ipp.sp.ippsPowerSpectr_32f(pre, pim, pSpectrArr, FFTransform.BlockSize);
                }

                //������� ���������
                if (unit_ == SpectrumUnit.Psd || unit_ == SpectrumUnit.Rmssd)
                    ipp.sp.ippsDivC_32f_I(k_widthFr_, pSpectrArr, FFTransform.BlockSize);

                //��������� �� �������� � ���
                if (unit_ == SpectrumUnit.Rms || unit_ == SpectrumUnit.Rmssd)
                    ipp.sp.ippsSqrt_32f_I(pSpectrArr, FFTransform.BlockSize);
            }
        }
    }
}
