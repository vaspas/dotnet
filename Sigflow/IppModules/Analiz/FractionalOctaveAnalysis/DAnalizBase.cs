
using System;

namespace IppModules.Analiz.FractionalOctaveAnalysis
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract unsafe class DAnalizBase
    {
        #region .ctor

        /// <summary>
        /// 
        /// </summary>
        public DAnalizBase()
        { }

        /// <summary>
        /// Деструктор.
        /// </summary>
        ~DAnalizBase()
        {
            //очистка
            Dispose(false);
        }

        #endregion


        #region ///// protected fields /////

        /// <summary>
        /// Коэффициенты фильтра.
        /// </summary>
        protected float[] m_taps;
        /// <summary>
        /// массив данных спектра
        /// </summary>
        protected float[] m_spData = new float[0];

        /// <summary>
        /// размер блока исходных данных
        /// </summary>
        protected int m_blockSize;

        /// <summary>
        /// пульсации в полосе пропускания (дБ)
        /// </summary>
        protected float m_ripple;
        /// <summary>
        /// число октав
        /// </summary>
        protected int m_IirOctCount;
        /// <summary>
        /// кол-во фильтров
        /// </summary>
        protected int m_FiltersCount;
        /// <summary>
        /// число фильтров 12, 3 или 1 на октаву
        /// </summary>
        protected int m_filtersPerOct;
        //int idx12Hz;
        protected int m_nzv;
        protected double m_qx; // 2^(1/nf_per_oct) или 10^(0.3/nf_per_oct) относительный сдвиг частоты
        /// <summary>
        /// частота квантования
        /// </summary>
        protected double m_fqu;

        protected int m_order = 0;

        /// <summary>
        /// Означеет что объект удален.
        /// </summary>
        protected bool m_disposed = false;

        #endregion ///// private fields /////

        #region ///// protected metods /////

        /// <summary>
        /// Инициализация коэффициентов фильтров.
        /// </summary>
        protected void InitTerzTaps()
        {
            int pow = 0;
            double f = CalculateFr(m_fqu, m_qx, ref pow);
            //double f0 = CalculateX0(f_qu, qx, nf, nf_per_oct);
            //idx12Hz = (int)Math.Floor(Math.Log(12.5 / f0) / Math.Log(qx) + 0.5);
            TerzTaps.CalcTerzOctTaps(ref m_taps, m_IirOctCount, m_filtersPerOct, m_nzv, m_ripple, f, m_qx);
            //CalcBandPassFiltersWrapper.CalcTerzOctTaps(m_taps,
            //        m_IirOctCount, m_filtersPerOct, m_nzv, m_ripple, f, m_qx);
            
        }

        /// <summary>
        /// Инициализация фильтров.
        /// </summary>
        /// <param name="p_FirSts"></param>
        protected void InitFirSts(ipp.IppsFIRState_32f*[] p_FirSts)
        {
            fixed (ipp.IppsFIRState_32f** pFirSts = p_FirSts)
            fixed (float* pTaps = DecFir.Decimate2a.Taps)
            {
                for (int i = 0; i < m_IirOctCount; i++)
                    if (i < m_order)
                        ipp.sp.ippsFIRMRInitAlloc_32f(pFirSts + i, pTaps, DecFir.Decimate2a.Len, 1, 0, DecFir.Decimate2a.DownFactor, 0, null);
                    else
                        ipp.sp.ippsFIRInitAlloc_32f(pFirSts + i, pTaps, DecFir.Decimate2a.Len, null);

            }
        }

        /// <summary>
        /// Инициализация фильтров.
        /// </summary>
        /// <param name="p_IirSts"></param>
        protected void InitIirSts(ipp.IppsIIRState_32f*[] p_IirSts)
        {
            fixed (ipp.IppsIIRState_32f** pIirSts = p_IirSts)
            fixed (float* ptaps = m_taps)
            {
                for (int i = 0; i < m_IirOctCount; i++)
                    for (int k = 0; k < m_filtersPerOct; k++)
                    {
                        ipp.IppStatus res = ipp.sp.ippsIIRInitAlloc_BiQuad_32f(pIirSts + i * m_filtersPerOct + k,
                                            ptaps + 6 * m_nzv * (k + m_filtersPerOct * i), m_nzv, null);
                    }
            }
        }

        /// <summary>
        /// Очистка неуправляемой памяти.
        /// </summary>
        /// <param name="p_FirSts"></param>
        protected void FreeFirSts(ipp.IppsFIRState_32f*[] p_FirSts)
        {
            //очищаем неуправляемую память
            if (p_FirSts != null)
            {
                for (int i = 0; i < p_FirSts.Length; i++)
                    ipp.sp.ippsFIRFree_32f(p_FirSts[i]);
            }
        }

        /// <summary>
        /// Очистка неуправляемой памяти.
        /// </summary>
        /// <param name="p_IirSts"></param>
        protected void FreeIirSts(ipp.IppsIIRState_32f*[] p_IirSts)
        {
            //очищаем неуправляемую память
            if (p_IirSts != null)
            {
                for (int i = 0; i < p_IirSts.Length; i++)
                    ipp.sp.ippsIIRFree_32f(p_IirSts[i]);
            }
        }

        /// <summary>
        /// Очистка ресурсов.
        /// </summary>
        /// <param name="disposing">true для очистки управляемых ресурсов</param>
        protected virtual void Dispose(bool disposing)
        {
            //устанавливаем флаг
            m_disposed = true;

        }

        #endregion

        #region ///// public metods /////

        /// <summary>
        /// Возвращает частоту самого нижнего фильтра анализатора
        /// </summary>
        /// <param name="FQu">Частота квантования.</param>
        /// <param name="QX">Множитель.</param>
        /// <param name="NF">Кол-во фильтров.</param>
        /// <param name="NFperOct">Кол-во фильтров на октаву.</param>
        /// <returns></returns>
        /*public static double CalculateF0(double FQu, double QX, int NF, int NFperOct)
        {
            int i = 0;
            // вернуть частоту самого нижнего фильтра анализатора
            return FQu * CalculateFr(FQu, QX, ref i) * Math.Pow(QX, -NF + NFperOct);
        }*/

        /// <summary>
        /// Степень для самого нижнего фильтра анализатора.
        /// </summary>
        /// <param name="FQu">Частота квантования.</param>
        /// <param name="QX">Множитель.</param>
        /// <param name="NF">Кол-во фильтров.</param>
        /// <param name="NFperOct">Кол-во фильтров на октаву.</param>
        /// <returns></returns>
        /*public static int CalculateX0(double FQu, double QX, int NF, int NFperOct)
        {
            int i = 0;
            CalculateFr(FQu, QX, ref i);
            return i - NF + NFperOct;
        }*/

        /// <summary>
        /// Расчет относительной частоты нижнего фильтра верхней октавы.
        /// </summary>
        /// <param name="FQu">Частота квантования.</param>
        /// <param name="QX">Множитель.</param>
        /// <param name="power">Степень для нижнего фильтра верхней октавы.</param>
        /// <returns></returns>
        private static double CalculateFr(double FQu, double QX, ref int power)
        {
            power = 0;
            var f = 1000 / FQu; // частота 1КГц относительная должна попадать в сетку
            // расчет относительной частоты нижнего фильтра верхней октавы
            // из расчета верхней частоты 0.4 (запас 5% по ФНЧ в других октавах)
            while (f * Math.Sqrt(QX) > 0.19) { f /= QX; power--; } // сначала спуститься по сетке вниз
            while (f * Math.Sqrt(QX) < 0.19) { f *= QX; power++; } // подняться вверх до границы октавы
            return f;
        }

        #endregion



        #region ///// IDisposable /////

        /// <summary>
        /// Явно очищает выделенные ресурсы.
        /// </summary>
        public void Dispose()
        {
            //поскольку очистка выполянеться явно, запретить сборщику мусора вызов метода Finalize
            GC.SuppressFinalize(this);
            //очистка
            Dispose(true);
        }

        #endregion

    }
}

