#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace IppModules.Analiz.NarrowBandSpectrum.CrossSpectrum
{
    /// <summary>
    /// Базовый класс для рассчета взаимных спектров 1 блока данных.
    /// Наследникам необходимо создать объект для рассчета БПФ, передать его этому классу.
    /// Также необходимо реализовать функцию рассчета и добавления в хранилища результатов БПФ для разных каналов.
    /// </summary>
    internal abstract unsafe class CrossSpectrum
    {        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public CrossSpectrum()
        { }

        #region ///// protected fields /////

        /// <summary>
        /// Объект для преобразования Фурье.
        /// </summary>
        protected FastFourierTransform.IFastFourierTransform FFTransform;

        /// <summary>
        /// Нормировочный коэффициент взвешивающего окна.
        /// </summary>
        protected float k_norm_;

        /// <summary>
        /// Единицы измерения спектра.
        /// </summary>
        protected SpectrumUnit unit_;

        /// <summary>
        /// Ширина полосы.
        /// </summary>
        protected float k_widthFr_;

        /// <summary>
        /// Массив для хранения действительных данных, данные храняться последовательно по каналам.
        /// </summary>
        protected float[] dataStorageRe_;

        /// <summary>
        /// Массив для хранения мнимых данных, данные храняться последовательно по каналам.
        /// </summary>
        protected float[] dataStorageIm_;

        /// <summary>
        /// Вспомогательный массив.
        /// </summary>
        private ipp.Ipp32fc[] workArr1_;

        /// <summary>
        /// Вспомогательный массив.
        /// </summary>
        private ipp.Ipp32fc[] workArr2_;

        /// <summary>
        /// Вспомогптельный массив.
        /// </summary>
        private float[] workArr_;

        /// <summary>
        /// Кол-во каналов.
        /// </summary>
        protected ushort nchans_;

        #endregion

        

        /// <summary>
        /// Подготавливает класс для работы.
        /// Клиенту следует вызывать эту функцию перед началом использования класса.
        /// </summary>
        /// <param name="block_size_power2">Размер блока как степень двойки.</param>
        /// <param name="winType">Тип окна.</param>
        /// <param name="unit">Тип спектра.</param>
        /// <param name="fQu">Частота квантования.</param>
        /// <param name="nchans">Кол-во каналов.</param>
        public void PrepareCrossSpectrum(int block_size_power2, WindowType winType, SpectrumUnit unit, double fQu,ushort nchans)
        {
            //сначала подготавливаем внутренний объект
            bool fftChanged = FFTransform.Prepare(block_size_power2, winType);

            //рассчитываем ширину полосы
            k_widthFr_ = (float)(fQu / FFTransform.BlockSize);

            //проверяем изменились ли какие нибудь значения
            if (fftChanged || unit != unit_)
            {
                //запоминаем тип спектра
                unit_ = unit;

                //рассчитываем нормировочный коэффициент
                k_norm_ = CalculateKNorm();
            }

            //запоминаем кол-во каналов
            nchans_ = nchans;

            //проверяем необходимость изменить хранилища
            int len=nchans*FFTransform.OutBlockSize;
            if (dataStorageRe_ == null || dataStorageRe_.Length != len)
                dataStorageRe_ = new float[len];
            if (dataStorageIm_ == null || dataStorageIm_.Length != len)
                dataStorageIm_ = new float[len];
            //проверяем необходимость изменить вспомогательные массивы
            len = FFTransform.OutBlockSize;
            if (workArr1_ == null || workArr1_.Length != len)
                workArr1_ = new ipp.Ipp32fc[len];
            if (workArr2_ == null || workArr2_.Length != len)
                workArr2_ = new ipp.Ipp32fc[len];
            if (workArr_ == null || workArr_.Length != len)
                workArr_ = new float[len];

        }

        /// <summary>
        /// Рассчитывает и возвращает нормировочный коэффициент.
        /// </summary>
        /// <returns></returns>
        private float CalculateKNorm()
        {
            //рассчитываем нормировочный коэффициент
            float s = 0;
            for (int i = 0; i < FFTransform.WinArr.Length; i++)
            {
                if (unit_ == SpectrumUnit.Psd || unit_ == SpectrumUnit.Rmssd)
                    //рассчет плотности
                    s += FFTransform.WinArr[i] * FFTransform.WinArr[i];
                else
                    s += FFTransform.WinArr[i];
            }
            return (float)Math.Sqrt(2) / s;
        }
        
        /// <summary>
        /// Рассчитывает взаимные спектры.
        /// </summary>
        /// <param name="ch1">Номер первого канала начиная с 0.</param>
        /// <param name="ch2">Номер второго канала начиная с 0.</param>
        /// <param name="destArr1">Указатель на массив, куда будет помещен результат, вещественная часть или модуль.</param>
        /// <param name="destArr2">Указатель на массив, куда будет помещен результат, мнимая часть или фаза.</param>
        public void CalculateCrossSpectrum(ushort ch1, ushort ch2, float* destArr1, float* destArr2)
        {
            //проверка корректности каналов
            //также проверка что каналы не равны
            if (ch1 >= nchans_ || ch2 >= nchans_ || ch1 == ch2)
                return;

            //копируем данные во вспомогательные массивы
            fixed (float* pStorRe = dataStorageRe_, pStorIm = dataStorageIm_)
            {
                ipp.sp.ippsRealToCplx_32f(pStorRe + FFTransform.OutBlockSize * ch1, pStorIm + FFTransform.OutBlockSize * ch1,
                    workArr1_, FFTransform.OutBlockSize);
                ipp.sp.ippsRealToCplx_32f(pStorRe + FFTransform.OutBlockSize * ch2, pStorIm + FFTransform.OutBlockSize * ch2,
                        workArr2_, FFTransform.OutBlockSize);
            }

            //рассчитываем комплексно сопряженное
            ipp.sp.ippsConj_32fc_I(workArr2_, FFTransform.OutBlockSize);

            //перемножаем комплексные числа
            ipp.sp.ippsMul_32fc_I(workArr1_, workArr2_, FFTransform.OutBlockSize);

            //возвращаем результат
            ipp.sp.ippsCplxToReal_32fc(workArr2_, destArr1, destArr2, FFTransform.OutBlockSize);

            //рассчет плотности
            if (unit_ == SpectrumUnit.Psd || unit_ == SpectrumUnit.Rmssd)
            {
                ipp.sp.ippsDivC_32f_I(k_widthFr_, destArr1, FFTransform.OutBlockSize);
                ipp.sp.ippsDivC_32f_I(k_widthFr_, destArr2, FFTransform.OutBlockSize);
            }

            //переводим из мощности в СКЗ
            if (unit_ == SpectrumUnit.Rms || unit_ == SpectrumUnit.Rmssd)
            {
                fixed(float* pWork=workArr_)
                {
                    //запоминаем знаки
                    ipp.sp.ippsThreshold_GTVal_32f(destArr1, pWork, FFTransform.OutBlockSize, 0, 1);
                    ipp.sp.ippsThreshold_LTVal_32f_I(pWork, FFTransform.OutBlockSize, 0, -1);
                    //рассчитываем модуль
                    ipp.sp.ippsAbs_32f_I(destArr1, FFTransform.OutBlockSize);
                    //вычисляем корень
                    ipp.sp.ippsSqrt_32f_I(destArr1, FFTransform.OutBlockSize);
                    //умножаем на знак
                    ipp.sp.ippsMul_32f_I(pWork, destArr1, FFTransform.OutBlockSize);

                    //запоминаем знаки
                    ipp.sp.ippsThreshold_GTVal_32f(destArr2, pWork, FFTransform.OutBlockSize, 0, 1);
                    ipp.sp.ippsThreshold_LTVal_32f_I(pWork, FFTransform.OutBlockSize, 0, -1);
                    //рассчитываем модуль
                    ipp.sp.ippsAbs_32f_I(destArr2, FFTransform.OutBlockSize);
                    //вычисляем корень
                    ipp.sp.ippsSqrt_32f_I(destArr2, FFTransform.OutBlockSize);
                    //умножаем на знак
                    ipp.sp.ippsMul_32f_I(pWork, destArr2, FFTransform.OutBlockSize);
                }
            }
            
        }

    }
}
