using System;

namespace IppModules.ImpulseSid
{
    /// <summary>
    /// Класс для построения корреляционных функций из передаваемых ему разверток сигнала, содержащего импульсы СИД.
    /// </summary>
    public unsafe class ConcurrentWorker:IDisposable
    {
        /// <summary>
        /// Деструктор.
        /// </summary>
        ~ConcurrentWorker()
        {
            Dispose(false);
        }

        #region ///// private fields /////
        
        /// <summary>
        /// Рабочий массив для работы ipp библиотеки.
        /// </summary>
        private byte[] _fftWorkBuf;

        /// <summary>
        /// Указатель на структуру для работы ipp библиотеки.
        /// </summary>
        private ipp.IppsFFTSpec_C_32fc* _pSpec;

        /// <summary>
        /// Массив данных для сравнения с сигналом от баз.
        /// </summary>
        private ipp.Ipp32fc[] _hData;

        /// <summary>
        /// Массив для работы.
        /// </summary>
        private ipp.Ipp32fc[] _workBuf1;

        /// <summary>
        /// Массив для работы.
        /// </summary>
        private ipp.Ipp32fc[] _workBuf2;


        /// <summary>
        /// Размер блока БПФ.
        /// </summary>
        private ushort _fftLen;

        /// <summary>
        /// Массив длиной с развертку, для рассчета результата.
        /// </summary>
        private ipp.Ipp32fc[] _buffer;

        #endregion

        #region ///// private methods /////

        /// <summary>
        /// Подготавливает CFT от переходной характеристики.
        /// </summary>
        /// <param name="dstData">Массив куда поместить результат, размером fftLen.</param>
        /// <param name="rFreq">Относительная частота.</param>
        private void PrepareCft(ref ipp.Ipp32fc[] dstData, float rFreq)
        {
            float ph=0;
            //обнуляем массив
            ipp.sp.ippsZero_32fc(_workBuf1,_fftLen);
            //генерируем тон, но не на всем массиве, а только в начале, длиной с размером импульса
            ipp.sp.ippsTone_Direct_32fc(_workBuf1, ImpulseLength, 2 * (float)_fftLen / ImpulseLength,
                rFreq,&ph,ipp.IppHintAlgorithm.ippAlgHintNone);            
            //учитываем вид импульса колокольчик
            if(Cos2)
                ipp.sp.ippsWinHann_32fc_I(_workBuf1, ImpulseLength);
            //БПФ
            fixed(byte* pfftBuf=_fftWorkBuf)
            {
                ipp.sp.ippsFFTFwd_CToC_32fc(_workBuf1, dstData, _pSpec, pfftBuf);
            }
            ipp.sp.ippsConj_32fc_I(dstData, _fftLen);
        }

        private void Envelope(ipp.Ipp32fc[] data, ipp.Ipp32fc* pFixBuf, ipp.Ipp32fc* pFixWorkBuf1)
        {
            fixed (byte* pfftBuf = _fftWorkBuf)
            {
                //рассчитываем функцию блоками, со смещением 0.5 блока, пока не дойдем до конца массива исходных данных
                for (int i = 0; i < BlockSize; i += _fftLen / 2)
                {
                    ipp.sp.ippsZero_32fc(_workBuf1, _fftLen);
                    //Buffer.BlockCopy(buf, i * sizeof(ipp.Ipp32fc), workBuf1_, 0, Math.Min(fftLen_, sourceLength_ - i));
                    ipp.sp.ippsCopy_32f((float*)pFixBuf + i * 2, (float*)pFixWorkBuf1, Math.Min(_fftLen, BlockSize - i) * 2);
                    ipp.sp.ippsFFTFwd_CToC_32fc(_workBuf1, _workBuf2, _pSpec, pfftBuf);
                    ipp.sp.ippsMul_32fc_I(data, _workBuf2, _fftLen);
                    ipp.sp.ippsFFTInv_CToC_32fc(_workBuf2, _workBuf1,_pSpec, pfftBuf);
                    //Buffer.BlockCopy(workBuf1_, 0, buf, i * sizeof(ipp.Ipp32fc), Math.Min(fftLen_ / 2, sourceLength_ - i));
                    ipp.sp.ippsCopy_32f((float*)pFixWorkBuf1, (float*)pFixBuf + i * 2, Math.Min(_fftLen / 2, BlockSize - i) * 2);
                }
            }
        }
        
        #endregion

        #region ///// public fields /////
        
        /// <summary>
        /// Длина импульса в отсчетах сигнала.
        /// </summary>
        public int ImpulseLength;
        /// <summary>
        /// Относительная частота заполнения импульсов сигнала объекта.
        /// </summary>
        public float CarrierRelativeFrequency;
        /// <summary>
        /// Флаг учета импульса в виде колокольчика.
        /// </summary>
        public bool Cos2;

        public int BlockSize;

        #endregion

        #region ///// public methods /////

        /// <summary>
        /// Подготавливает объект для работы, необходимо выполнять при изменении любых параметров.
        /// </summary>
        public void Prepare()
        {
            //рассчитываем размер блока БПФ как степень двойки
            //исходя из длина блока больше двойной длины импульса
            byte order = 5; //мин 32
            while (2 * ImpulseLength > (1 << order))
                order++;
            //теперь получаем просто размер блока БПФ
            _fftLen = (ushort)(1 << order);

            //инициализируем структуру
            var bsize = 0;
            ipp.IppsFFTSpec_C_32fc* ptr; 
            ipp.sp.ippsFFTInitAlloc_C_32fc(&ptr, order, (int)ipp.IppsFFTNorm.ippFftDivFwdByN, ipp.IppHintAlgorithm.ippAlgHintNone);
            _pSpec = ptr;
            ipp.sp.ippsFFTGetBufSize_C_32fc(_pSpec, &bsize);
            _fftWorkBuf = new byte[bsize];
            
            //выделяем вспомогательные массивы
            _workBuf1=new ipp.Ipp32fc[_fftLen];
            _workBuf2 = new ipp.Ipp32fc[_fftLen];
            //выделяем массивы сравнения
            _hData=new ipp.Ipp32fc[_fftLen];
            //подготавливаем массивы для работы, c учетом частоты импульса
            PrepareCft(ref _hData, CarrierRelativeFrequency);
            //выделяем массив длиной с развертку
            _buffer = new ipp.Ipp32fc[BlockSize];
        }

        /// <summary>
        /// Завершение работы.
        /// </summary>
        public void Close()
        {
            //очистка неуправляемых ресурсов
            //проверим что ресурсы еще не очищены
            if (_pSpec != null)
            {
                //очищаем
                ipp.sp.ippsFFTFree_C_32fc(_pSpec);
                _pSpec = null;
            }

            //остальные ссылки просто обнуляем
            _buffer = null;
            _workBuf1 = null;
            _workBuf2 = null;
            _hData = null;
            _fftWorkBuf = null;

        }

        /// <summary>
        /// Выполняет работу, изменяя данные в переданном массиве.
        /// </summary>
        /// <param name="pData">Указатель на демультиплексированный массив данных размером blockSize.</param>
        public void DoWork(float* pData)
        {
            //фиксируем все массивы
            fixed (ipp.Ipp32fc* pBuffer = _buffer, pworkBuf1 = _workBuf1)
            {
                int dstLen;
                int ph;
                var pfBuf = (float*) pBuffer;
                ipp.sp.ippsSampleUp_32f(pData, BlockSize, pfBuf, &dstLen, 2, &ph);
                Envelope(_hData, pBuffer, pworkBuf1);
                ipp.sp.ippsMagnitude_32fc(_buffer, pData, BlockSize);
            }
        }

        #endregion

        #region ///// IDispose /////

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

        /// <summary>
        /// Очистка ресурсов.
        /// </summary>
        /// <param name="disposing">true для очистки управляемых ресурсов</param>
        protected void Dispose(bool disposing)
        {
            Close();
        }

        #endregion
    }
}
