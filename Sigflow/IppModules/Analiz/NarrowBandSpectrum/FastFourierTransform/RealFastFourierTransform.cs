#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace IppModules.Analiz.NarrowBandSpectrum.FastFourierTransform
{

    /// <summary>
    /// Класс для рассчета БПФ.
    /// </summary>
    internal unsafe class RealFastFourierTransform : IDisposable, IFastFourierTransform
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public RealFastFourierTransform()
        { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="norm">Тип нормирования.</param>
        /// <param name="hint">Точность.</param>
        public RealFastFourierTransform(ipp.IppsFFTNorm norm,ipp.IppHintAlgorithm hint)
        {
            FFTNorm = norm;
            FFTHint = hint;
        }

        /// <summary>
        /// Деструктор.
        /// </summary>
        ~RealFastFourierTransform()
        {
            Dispose(false);
        }

        #region ///// private members /////

        /// <summary>
        /// рабочий массив
        /// </summary>
        private float[] sData_;
        /// <summary>
        /// рабочий массив
        /// </summary>
        private float[] sData2_;

        /// <summary>
        /// указатель на структуру для работы ipp библиотеки
        /// </summary>
        private ipp.IppsFFTSpec_R_32f* pSpec_;
        /// <summary>
        /// буфер также для работы ipp библиотеки  
        /// </summary>
        private byte[] ippBuf_;

        /// <summary>
        /// Тип окна.
        /// </summary>
        private WindowType winType_;

        #endregion

        #region ///// protected fields /////

        /// <summary>
        /// Метод нормализации.
        /// </summary>
        protected ipp.IppsFFTNorm FFTNorm = ipp.IppsFFTNorm.ippFftNoDivByAny;

        /// <summary>
        /// Точность.
        /// </summary>
        protected ipp.IppHintAlgorithm FFTHint = ipp.IppHintAlgorithm.ippAlgHintNone;

        /// <summary>
        /// Массив для хранения реальнй части
        /// </summary>
        protected float[] reArr;
        /// <summary>
        /// Массив для хранения мнимой части.
        /// </summary>
        protected float[] imArr;

        /// <summary>
        /// Размер блока (степень 2).
        /// </summary>
        protected int blockSizePower2;
        /// <summary>
        /// Размер блока.
        /// </summary>
        protected int blockSize;

        /// <summary>
        /// массив значений окна
        /// </summary>
        protected float[] winArr;

        #endregion

        /// <summary>
        /// Подготавливает класс для работы.
        /// Клиенту следует вызывать эту функцию перед началом использования класса.
        /// </summary>
        /// <param name="block_size_power2">Размер блока как степень двойки.</param>
        /// <param name="winType">Тип окна.</param>
        /// <returns>true если произошла подготовка, false если в ней небыло нужды</returns>
        public bool Prepare(int block_size_power2, WindowType winType)
        {
            //если изменился размер блока или еще не инициализирована структура
            if (pSpec_ == null || block_size_power2 != blockSizePower2)
            {
                //запоминаем параметры
                if (block_size_power2 <= 0)
                    throw new ArgumentOutOfRangeException("block_size_power2", "block_size_power2<=0");
                blockSizePower2 = block_size_power2;
                blockSize = (int)Math.Pow(2, blockSizePower2);
                winType_ = winType;

                //очищаем выделенную неуправляемую память 
                if (pSpec_ != null)
                {   //очищаем
                    ipp.sp.ippsFFTFree_R_32f(pSpec_);
                    pSpec_ = null;
                }

                //инициализация
                InitStruct();
                InitWindow();

                return true;
            }
            //изменился только тип окна
            else if (winType_ != winType)
            {
                winType_ = winType;

                //инициализация
                InitWindow();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Инициализация массивов и структур.
        /// </summary>
        private void InitStruct()
        {

            //инициализируем вспомогательные массивы
            sData_ = new float[blockSize];
            sData2_ = new float[blockSize];
            //инициализируем массивы действ. и  мнимой частей
            reArr = new float[blockSize / 2];
            imArr = new float[blockSize / 2];

            //инициализируем ipp структуру
            ipp.IppsFFTSpec_R_32f* ptr;
            ipp.sp.ippsFFTInitAlloc_R_32f(&ptr, blockSizePower2, (int)FFTNorm, FFTHint);
            pSpec_ = ptr;
            int ippbufsize = 0;
            ipp.sp.ippsFFTGetBufSize_R_32f(pSpec_, &ippbufsize);
            //инициализируем массив ipp
            ippBuf_ = new byte[ippbufsize];
        }

        private void WinCos4(float[] win, double a1, double a2, double a3, double a4)
        {
            for (int i = 0; i < win.Length; i++)
            {
                double ph = 2*Math.PI*i/(win.Length - 1);
                win[i] = (float)(a1 + a2*Math.Cos(ph) + a3*Math.Cos(2*ph) + a4*Math.Cos(3*ph));
            }
        }

        /// <summary>
        /// Инициализирует окно
        /// </summary>
        private void InitWindow()
        {
            winArr = new float[blockSize + 1];
            fixed (float* pWinArr = winArr)
            {
                ipp.sp.ippsSet_32f(1f, pWinArr, winArr.Length);
                switch (winType_)
                {
                    case WindowType.Rectangular: break;
                    case WindowType.Hamming:
                        ipp.sp.ippsWinHamming_32f_I(pWinArr, winArr.Length);
                        break;
                    case WindowType.Hann:
                        ipp.sp.ippsWinHann_32f_I(pWinArr, winArr.Length);
                        break;
                    case WindowType.Kaiser30:
                        ipp.sp.ippsWinKaiser_32f_I(pWinArr, winArr.Length, (float)((float)2 / (blockSize) * Math.PI * 3));
                        break;
                    case WindowType.Bartlett:
                        ipp.sp.ippsWinBartlett_32f_I(pWinArr, winArr.Length);
                        break;
                    case WindowType.BlackmanOpt:
                        ipp.sp.ippsWinBlackmanOpt_32f_I(pWinArr, winArr.Length);
                        break;
                    case WindowType.BlackmanStd:
                        ipp.sp.ippsWinBlackmanStd_32f_I(pWinArr, winArr.Length);
                        break;
                    case WindowType.FlatTop:
                        WinCos4(winArr, 0.2810639, -0.5208972, 0.1980399, 0);
                        break;
                    case WindowType.BlackmanHarris90:
                        WinCos4(winArr, 0.35875, -0.48829, 0.14128, -0.01168);
                        break;
                }
            }
        }
        
        /// <summary>
        /// Функция для рассчета.
        /// </summary>
        /// <param name="pCountArr">Указатель на массив исх. данных (отсчетов), длина должна быть = block_size.</param>            
        public void CalculateFFT(float* pCountArr)
        {
            fixed (float* pwinArr = winArr,             //указатель на массив окна
                psData = sData_,                        //указатель на рабочий массив    
                psData2 = sData2_)                       //указатель на рабочий массив 2
            {
                //учитываем окно
                ipp.sp.ippsMul_32f(pwinArr, pCountArr, psData, blockSize);

                //БПФ
                fixed (byte* pippBuf = ippBuf_)
                    ipp.sp.ippsFFTFwd_RToPerm_32f(psData, psData2, pSpec_, pippBuf);

                sData2_[1] = 0;    //последнюю частоту size/2+1 - игнорирую

                int rel, iml;
                fixed (float* pre = reArr, pim = imArr)
                {
                    //получаем массивы вещественной и мнимой части
                    int phase = 0;
                    ipp.sp.ippsSampleDown_32f(psData2, sData2_.Length, pre, &rel, 2, &phase);
                    phase = 1;
                    ipp.sp.ippsSampleDown_32f(psData2, sData2_.Length, pim, &iml, 2, &phase);
                }
            }

        }

        #region ///// public properties /////

        /// <summary>
        /// Возвращает ссылку на массив значений окна.
        /// </summary>
        public float[] WinArr
        {
            get { return winArr; }
        }

        /// <summary>
        /// Возвращает размер блока.
        /// </summary>
        public int BlockSize
        {
            get { return blockSize; }
        }

        /// <summary>
        /// Возвращает размер блока результата.
        /// </summary>
        public int OutBlockSize
        {
            get { return blockSize/2; }
        }

        /// <summary>
        /// Возвращает ссылку на массив результат действ. значений, размером с половину блока.
        /// </summary>
        public float[] ResultRe
        {
            get { return reArr; }
        }

        /// <summary>
        /// Возвращает ссылку на массив результат мнимых. значений, размером с половину блока.
        /// </summary>
        public float[] ResultIm
        {
            get { return imArr; }
        }

        #endregion


        /// <summary>
        /// Явно очищает выделенные ресурсы.
        /// Клиенту может вызывать эту функцию, если объект более не будет использоваться.
        /// </summary>
        public void Dispose()
        {
            //поскольку очистка выполянеться явно, запретить сборщику мусора вызов метода Finalize
            GC.SuppressFinalize(this);
            //очистка
            Dispose(true);
        }

        /// <summary>
        /// Очистка ресурсов.
        /// </summary>
        /// <param name="disposing">true для очистки управляемых ресурсов</param>
        protected void Dispose(bool disposing)
        {
            //проверим что ресурсы еще не очищены
            if (pSpec_ != null)
            {
                //очищаем
                ipp.sp.ippsFFTFree_R_32f(pSpec_);
                pSpec_ = null;
            }
        }

    }

}
