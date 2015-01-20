
namespace IppModules.Analiz.FractionalOctaveAnalysis
{
    internal unsafe sealed class DCrossAnaliz : DAnalizBase
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
                m_wData1 = new float[m_blockSize];
                m_sData1 = new float[m_blockSize];
                m_wData2 = new float[m_blockSize];
                m_sData2 = new float[m_blockSize];
            }

            //������� FIR ���������
            //����������� ������ ���!!!, 
            //�.�. ���� ������� �������� � �������, � ����� �� ��� ������ ����, �� ���������� �������� ��
            //��� ������� ������ � IPP
            //if (m_FirSts == null || n_IirOct_Changed || orderChanged)
            {
                //������� ������������� ������
                base.FreeFirSts(m_FirSts1);
                base.FreeFirSts(m_FirSts2);
                //������� ����� ������
                m_FirSts1 = new ipp.IppsFIRState_32f*[m_IirOctCount];
                m_FirSts2 = new ipp.IppsFIRState_32f*[m_IirOctCount];
                //�������������� ������
                base.InitFirSts(m_FirSts1);
                base.InitFirSts(m_FirSts2);
            }

            //������� IIR ���������
            //����������� ������ ���!!!, 
            //�.�. ���� ������� �������� � �������, � ����� �� ��� ������ ����, �� ���������� �������� ��
            //��� ������� ������ � IPP
            //if (m_IirSts == null || nfChanged || tapsChanged)
            {
                //������� ������������� ������
                base.FreeIirSts(m_IirSts1);
                base.FreeIirSts(m_IirSts2);
                //������� ����� ������
                m_IirSts1 = new ipp.IppsIIRState_32f*[m_FiltersCount];
                m_IirSts2 = new ipp.IppsIIRState_32f*[m_FiltersCount];
                //�������������� ������
                base.InitIirSts(m_IirSts1);
                base.InitIirSts(m_IirSts2);
            }
        }

        #region ///// private fields /////

        private ipp.IppsFIRState_32f*[] m_FirSts1;
        private ipp.IppsIIRState_32f*[] m_IirSts1;
        private ipp.IppsFIRState_32f*[] m_FirSts2;
        private ipp.IppsIIRState_32f*[] m_IirSts2;
        /// <summary>
        /// ��������������� ������ ��� ������� Power
        /// </summary>
        private float[] m_wData1 = new float[0];
        /// <summary>
        /// ��������������� ������ ��� ������� Power
        /// </summary>
        private float[] m_wData2 = new float[0];
        /// <summary>
        /// ��������������� ������ ��� ��������
        /// </summary>
        private float[] m_sData1 = new float[0];
        /// <summary>
        /// ��������������� ������ ��� ��������
        /// </summary>
        private float[] m_sData2 = new float[0];

        private int m_cnt_blo = 0;

        #endregion

        /// <summary>
        /// ������������ �������� ��� ��������� �������.
        /// </summary>
        /// <param name="p_IirSts1"></param>
        /// <param name="p_IirSts2"></param>
        /// <param name="p_srcData1">�������� ������ ������� �������.</param>
        /// <param name="p_wrkData1">������� ��������������� ������.</param>
        /// <param name="p_srcData2">�������� ������ ������� �������.</param>
        /// <param name="p_wrkData2">������� ��������������� ������.</param>
        /// <param name="p_osize">������ ������.</param>
        /// <returns></returns>
        private static float CrossPower(ipp.IppsIIRState_32f* p_IirSts1, ipp.IppsIIRState_32f* p_IirSts2, float* p_srcData1, float* p_wrkData1, float* p_srcData2, float* p_wrkData2, int p_osize)
        {
            if (p_osize > 1)
            {
                float mean = 1;

                ipp.sp.ippsIIR_32f(p_srcData1, p_wrkData1, p_osize, p_IirSts1);
                ipp.sp.ippsIIR_32f(p_srcData2, p_wrkData2, p_osize, p_IirSts2);
                ipp.sp.ippsMul_32f_I(p_wrkData1, p_wrkData2, p_osize);
                ipp.sp.ippsMean_32f(p_wrkData2, p_osize, &mean, ipp.IppHintAlgorithm.ippAlgHintNone);

                return mean;
            }
            else
            {

                ipp.sp.ippsIIROne_32f(p_srcData1[0], p_wrkData1, p_IirSts1);
                ipp.sp.ippsIIROne_32f(p_srcData2[0], p_wrkData2, p_IirSts2);

                return p_wrkData1[0] * p_wrkData2[0];
            }
        }

        /// <summary>
        /// ������������ ������� ������ �������.
        /// ���������� ������� ������������ ��� ���������, �.�. �� ������ ��������.
        /// </summary>
        /// <param name="p_signalArr1">���� ������� 1.</param>
        /// <param name="p_signalArr2">���� ������� 2.</param>
        public float[] Calculate(float[] p_signalArr1, float[] p_signalArr2)
        {
            //���� ��� ����� �������� � ������ �� �������
            bool ping = true;
            float[] inData1, inData2;
            float[] outData1, outData2;

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
                    inData1 = p_signalArr1;
                    inData2 = p_signalArr2;
                    outData1 = m_sData1;
                    outData2 = m_sData2;
                }
                else
                {
                    inData1 = m_sData1;
                    inData2 = m_sData2;
                    outData1 = p_signalArr1;
                    outData2 = p_signalArr2;
                }
                ping = !ping;

                //������������ ������
                fixed (float* pIn1 = inData1, pIn2 = inData2, pOut1 = outData1, pOut2 = outData2, pwData1 = m_wData1, pwData2 = m_wData2)
                {
                    for (int k = 0; k < m_filtersPerOct; k++)
                    {
                        if ((osize > 0) || flag)
                            m_spData[(m_IirOctCount - 1 - iir_oct) * m_filtersPerOct + k] = CrossPower(m_IirSts1[iir_oct * m_filtersPerOct + k],m_IirSts2[iir_oct * m_filtersPerOct + k], pIn1, pwData1, pIn2, pwData2, osize);
                    }

                    if ((osize / 2) > 0)
                    {
                        ipp.sp.ippsFIR_32f(pIn1, pOut1, osize / 2, m_FirSts1[oct]);
                        ipp.sp.ippsFIR_32f(pIn2, pOut2, osize / 2, m_FirSts2[oct]);
                    }
                    else if (flag)
                    {
                        ipp.sp.ippsFIROne_32f(pIn1[0], pOut1, m_FirSts1[oct]);
                        ipp.sp.ippsFIROne_32f(pIn2[0], pOut2, m_FirSts2[oct]);
                    }
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
                base.FreeFirSts(m_FirSts1);
                base.FreeIirSts(m_IirSts1);
                base.FreeFirSts(m_FirSts2);
                base.FreeIirSts(m_IirSts2);
            }

            base.Dispose(disposing);
        }

    }
}
