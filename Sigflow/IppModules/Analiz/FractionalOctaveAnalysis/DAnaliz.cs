using System;
using System.Collections.Generic;
using System.Text;
using IppWrapper;

namespace IppModules.Analiz.FractionalOctaveAnalysis
{
    internal unsafe sealed class DAnaliz: DAnalizBase
    {
        /// <summary>
        /// ���������� � ������. ������� �������� ����� ������� ������.
        /// </summary>
        /// <param name="BlockSize">������ ����� ������ �������.</param>
        /// <param name="Freq">������� �����������.</param>
        /// <param name="QX">���������.</param>
        /// <param name="Ripple">���������.</param>
        /// <param name="NIirOct">���-�� �����.</param>
        /// <param name="NFperOct">���-�� �������� �� ������.</param>
        /// <param name="NZV">������� / 2</param>
        public void Prepare(int BlockSize, double Freq, double QX,
            float Ripple, int NIirOct, int NFperOct, int NZV)
        {
            //������������ ���-�� ��������
            int nf = NIirOct * NFperOct;

            //������������ �������
            int s = BlockSize;
            int order = 0;
            while ((s >>= 1) > 0)
                order++;

            //���������� ��������� ��������
            bool blockSizeChanged = (m_blockSize != BlockSize);
            bool fquChanged = (m_fqu != Freq);
            bool qxChanged = (m_qx != QX);
            bool rippleChanged = (m_ripple != Ripple);
            bool n_IirOct_Changed = (m_IirOctCount != NIirOct);
            bool nf_per_oct_Changed = (m_filtersPerOct != NFperOct);
            bool nzv_Changed = (m_nzv != NZV);
            bool nfChanged = (m_FiltersCount != nf);
            bool orderChanged = (m_order != order);
            bool tapsChanged = false;

            //���������� ���� ��������
            m_blockSize = BlockSize;
            m_fqu = Freq;
            m_qx = QX;
            m_ripple = Ripple;
            m_IirOctCount = NIirOct;
            m_filtersPerOct = NFperOct;
            m_nzv = NZV;
            m_FiltersCount = nf;
            m_order = order;

            //������� ������ �����. ��� �������������
            if (nfChanged || m_taps == null)
            {
                m_taps = new float[6 * 10 * m_FiltersCount]; // �� ������ ������ ���� �����. ��� ���������� �����
                tapsChanged = true;
            }

            //�������������� ������ ������������� ��� �������������
            if (tapsChanged || fquChanged || qxChanged || n_IirOct_Changed || nf_per_oct_Changed ||
                nzv_Changed || rippleChanged)
            {
                base.InitTerzTaps();
                tapsChanged = true;
            }

            //������� �������
            if (nfChanged)
                m_spData = new float[m_FiltersCount];
            if (blockSizeChanged)
            {
                m_wData = new float[m_blockSize];                
                m_sData = new float[m_blockSize];                
            }

            //������� FIR ���������
            //����������� ������ ���!!!, 
            //�.�. ���� ������� �������� � �������, � ����� �� ��� ������ ����, �� ���������� �������� ��
            //��� ������� ������ � IPP
            //if (m_FirSts == null || n_IirOct_Changed || orderChanged)
            {
                //������� ������������� ������
                base.FreeFirSts(m_FirSts);
                //������� ����� ������
                m_FirSts = new ipp.IppsFIRState_32f*[m_IirOctCount];
                //�������������� ������
                base.InitFirSts(m_FirSts);
            }

            //������� IIR ���������
            //����������� ������ ���!!!, 
            //�.�. ���� ������� �������� � �������, � ����� �� ��� ������ ����, �� ���������� �������� ��
            //��� ������� ������ � IPP
            //if (m_IirSts == null || nfChanged || tapsChanged)
            {
                //������� ������������� ������
                base.FreeIirSts(m_IirSts);
                //������� ����� ������
                m_IirSts = new ipp.IppsIIRState_32f*[m_FiltersCount];
                //�������������� ������
                base.InitIirSts(m_IirSts);
            }
        }

        #region ///// private fields /////

        private ipp.IppsFIRState_32f*[] m_FirSts;
        private ipp.IppsIIRState_32f*[] m_IirSts;
        /// <summary>
        /// ��������������� ������ ��� ������� Power
        /// </summary>
        private float[] m_wData = new float[0];
        /// <summary>
        /// ��������������� ������ ��� ��������
        /// </summary>
        private float[] m_sData = new float[0];

        private int m_cnt_blo = 0;

        #endregion

        
        /// <summary>
        /// ������������ �������� ��� ������.
        /// </summary>
        /// <param name="p_IirSts"></param>
        /// <param name="p_srcData">�������� ������.</param>
        /// <param name="p_wrkData">������� ��������������� ������.</param>
        /// <param name="p_osize">������ ������.</param>
        /// <returns></returns>
        private static float Power(ipp.IppsIIRState_32f* p_IirSts, float* p_srcData, float* p_wrkData, int p_osize)
        {
            if (p_osize > 2)
            {
                float mean = 1;

                IppHelper.Do(ipp.sp.ippsIIR_32f(p_srcData, p_wrkData, p_osize, p_IirSts));
                IppHelper.Do(ipp.sp.ippsMul_32f_I(p_wrkData, p_wrkData, p_osize));
                ipp.sp.ippsMean_32f(p_wrkData, p_osize, &mean, ipp.IppHintAlgorithm.ippAlgHintNone);

                // return _cida[0];
                return mean;
            }
                else if(p_osize>1)
                {
                    float mean = 1;

                    //IppHelper.Do(() => ipp.sp.ippsIIR_32f(p_srcData, p_wrkData, p_osize, p_IirSts));
                    IppHelper.Do(ipp.sp.ippsIIROne_32f(p_srcData[0], &p_wrkData[0], p_IirSts));
                    IppHelper.Do(ipp.sp.ippsIIROne_32f(p_srcData[1], &p_wrkData[1], p_IirSts));
                    //IppHelper.Do(() =>ipp.sp.ippsIIR_32f(&p_srcData[0], &p_wrkData[0], 1, p_IirSts));
                    //IppHelper.Do(() =>ipp.sp.ippsIIR_32f(&p_srcData[1], &p_wrkData[1], 1, p_IirSts));

                    IppHelper.Do(ipp.sp.ippsMul_32f_I(p_wrkData, p_wrkData, p_osize));
                    ipp.sp.ippsMean_32f(p_wrkData, p_osize, &mean, ipp.IppHintAlgorithm.ippAlgHintNone);

                    // return _cida[0];
                    return mean;
                }
                else
                {
                    IppHelper.Do(ipp.sp.ippsIIROne_32f(p_srcData[0], p_wrkData, p_IirSts));
                    //IppHelper.Do(() => ipp.sp.ippsIIR_32f(p_srcData, p_wrkData, p_osize, p_IirSts));

                return (float)Math.Pow(p_wrkData[0], 2);
            }
        }

        /// <summary>
        /// ������������ ������ �������.
        /// ���������� ������ ������������ ��� ���������, �.�. ��� ������ ��������.
        /// </summary>
        /// <param name="p_signalArr"></param>
        public float[] Calculate(float[] p_signalArr)
        {
            //���� ��� ����� �������� � ������ �� �������
            bool ping = true;
            float[] inData;
            float[] outData;

            // ������� �����
            int oct = 0;
            // ���� �����������
            bool flag = true;
            do
            {
                // ���� ���������� ������
                int iir_oct = oct;
                // ����� ����� ���� ��������
                int osize = m_blockSize >> iir_oct;
                flag = (osize > 0) || ((1 & (m_cnt_blo >> (oct - m_order - 1))) == 0);
                //������������� ������ �� �������
                if (ping)
                {
                    inData = p_signalArr;
                    outData = m_sData;
                }
                else
                {
                    inData = m_sData;
                    outData = p_signalArr;
                }
                ping = !ping;

                //������������ ������
                fixed (float* pIn = inData, pOut = outData, pwData = m_wData)
                {
                    for (int k = 0; k < m_filtersPerOct; k++)
                    {
                        if ((osize > 0) || flag)
                            m_spData[(m_IirOctCount - 1 - iir_oct) * m_filtersPerOct + k] = Power(m_IirSts[(m_IirOctCount - 1 - iir_oct) * m_filtersPerOct + k], pIn, pwData, osize);
                    }

                    if ((osize / 2) > 0)
                        ipp.sp.ippsFIR_32f(pIn, pOut, osize / 2, m_FirSts[oct]);
                    else if (flag)
                        ipp.sp.ippsFIROne_32f(pIn[0], pOut, m_FirSts[oct]);
                }

                //if ((osize == 0) && flag) cnts_avr[iir_oct]++;

                oct++;
                if (!flag)
                    oct = m_IirOctCount;
            } while (oct < m_IirOctCount);

            m_cnt_blo++;

            return m_spData;
        }

        /// <summary>
        /// ������� ��������.
        /// </summary>
        /// <param name="disposing">true ��� ������� ����������� ��������</param>
        protected override void Dispose(bool disposing)
        {
            //�������� ��� ������� ��� �� �������
            if (!m_disposed)
            {
                //������� ��������� � ������������� ������
                base.FreeFirSts(m_FirSts);
                base.FreeIirSts(m_IirSts);
            }

            base.Dispose(disposing);
        }

    }
}
