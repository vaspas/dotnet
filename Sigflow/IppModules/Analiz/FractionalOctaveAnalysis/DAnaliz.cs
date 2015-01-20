using System;
using System.Collections.Generic;
using System.Text;
using IppWrapper;

namespace IppModules.Analiz.FractionalOctaveAnalysis
{
    internal unsafe sealed class DAnaliz: DAnalizBase
    {
        /// <summary>
        /// Подготовка к работе. Следует вызывать перед началом работы.
        /// </summary>
        /// <param name="BlockSize">Размер блока данных сигнала.</param>
        /// <param name="Freq">Частота квантования.</param>
        /// <param name="QX">Множитель.</param>
        /// <param name="Ripple">Пульсация.</param>
        /// <param name="NIirOct">Кол-во октав.</param>
        /// <param name="NFperOct">Кол-во фильтров на октаву.</param>
        /// <param name="NZV">Порядок / 2</param>
        public void Prepare(int BlockSize, double Freq, double QX,
            float Ripple, int NIirOct, int NFperOct, int NZV)
        {
            //рассчитываем кол-во фильтров
            int nf = NIirOct * NFperOct;

            //рассчитываем порядок
            int s = BlockSize;
            int order = 0;
            while ((s >>= 1) > 0)
                order++;

            //запоминаем изменение значений
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

            //запоминаем сами значения
            m_blockSize = BlockSize;
            m_fqu = Freq;
            m_qx = QX;
            m_ripple = Ripple;
            m_IirOctCount = NIirOct;
            m_filtersPerOct = NFperOct;
            m_nzv = NZV;
            m_FiltersCount = nf;
            m_order = order;

            //создаем массив коэфф. при необходимости
            if (nfChanged || m_taps == null)
            {
                m_taps = new float[6 * 10 * m_FiltersCount]; // на каждую октаву свои коэфф. при десятичной сетке
                tapsChanged = true;
            }

            //инициализируем массив коеффицментов при необходимости
            if (tapsChanged || fquChanged || qxChanged || n_IirOct_Changed || nf_per_oct_Changed ||
                nzv_Changed || rippleChanged)
            {
                base.InitTerzTaps();
                tapsChanged = true;
            }

            //создаем массивы
            if (nfChanged)
                m_spData = new float[m_FiltersCount];
            if (blockSizeChanged)
            {
                m_wData = new float[m_blockSize];                
                m_sData = new float[m_blockSize];                
            }

            //создаем FIR структуры
            //пересоздаем каждый раз!!!, 
            //т.к. если фильтры работали с данными, а затем на них подать нули, то происходит заргузка ЦП
            //это связано только с IPP
            //if (m_FirSts == null || n_IirOct_Changed || orderChanged)
            {
                //очищаем неуправляемую память
                base.FreeFirSts(m_FirSts);
                //создаем новый массив
                m_FirSts = new ipp.IppsFIRState_32f*[m_IirOctCount];
                //инициализируем массив
                base.InitFirSts(m_FirSts);
            }

            //создаем IIR структуры
            //пересоздаем каждый раз!!!, 
            //т.к. если фильтры работали с данными, а затем на них подать нули, то происходит заргузка ЦП
            //это связано только с IPP
            //if (m_IirSts == null || nfChanged || tapsChanged)
            {
                //очищаем неуправляемую память
                base.FreeIirSts(m_IirSts);
                //создаем новый массив
                m_IirSts = new ipp.IppsIIRState_32f*[m_FiltersCount];
                //инициализируем массив
                base.InitIirSts(m_IirSts);
            }
        }

        #region ///// private fields /////

        private ipp.IppsFIRState_32f*[] m_FirSts;
        private ipp.IppsIIRState_32f*[] m_IirSts;
        /// <summary>
        /// Вспомогательный массив для функции Power
        /// </summary>
        private float[] m_wData = new float[0];
        /// <summary>
        /// Вспомогательный массив для рассчета
        /// </summary>
        private float[] m_sData = new float[0];

        private int m_cnt_blo = 0;

        #endregion

        
        /// <summary>
        /// Рассчитывает значение для спекра.
        /// </summary>
        /// <param name="p_IirSts"></param>
        /// <param name="p_srcData">Исходные данные.</param>
        /// <param name="p_wrkData">Рабочий вспомогательный массив.</param>
        /// <param name="p_osize">Размер данных.</param>
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
        /// Рассчитывает спектр сигнала.
        /// Переданный массив используется для рассчетов, т.е. его данные меняются.
        /// </summary>
        /// <param name="p_signalArr"></param>
        public float[] Calculate(float[] p_signalArr)
        {
            //флаг для смены массивов и ссылки на массивы
            bool ping = true;
            float[] inData;
            float[] outData;

            // счетчик октав
            int oct = 0;
            // флаг продолжения
            bool flag = true;
            do
            {
                // Идут фильтровые октавы
                int iir_oct = oct;
                // Имеем новый блок размером
                int osize = m_blockSize >> iir_oct;
                flag = (osize > 0) || ((1 & (m_cnt_blo >> (oct - m_order - 1))) == 0);
                //устанавливаем ссылки на массивы
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

                //рассчитываем данные
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
        /// Очистка ресурсов.
        /// </summary>
        /// <param name="disposing">true для очистки управляемых ресурсов</param>
        protected override void Dispose(bool disposing)
        {
            //проверим что ресурсы еще не очищены
            if (!m_disposed)
            {
                //очищаем структуры в неуправляемой памяти
                base.FreeFirSts(m_FirSts);
                base.FreeIirSts(m_IirSts);
            }

            base.Dispose(disposing);
        }

    }
}
