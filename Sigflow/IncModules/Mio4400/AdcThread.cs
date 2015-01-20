using System;

using System.Runtime.InteropServices;
using System.Threading;

namespace IncModules.Mio4400
{
    /// <summary>
    /// Класс сбора данных с устройства.
    /// </summary>
    public sealed unsafe class AdcThread
    {
        public AdcThread()
        {
            _eventGcHandle = GCHandle.Alloc(new IntPtr(), GCHandleType.Pinned);
        }

        /// <summary>
        /// Деструктор.
        /// </summary>
        ~AdcThread()
        {
            _eventGcHandle.Free();
        }

        /// <summary>
        /// Событие получения новых данных.
        /// </summary>
        public Action<int[]> OnDataReceived { get; set; }

        public ThreadPriority ThreadPriority { get; set; }

        #region ///// private fields /////

        /// <summary>
        /// Событие поступления новых данных.
        /// </summary>
        private readonly ManualResetEvent _dataReady = new ManualResetEvent(false);


        private GCHandle _eventGcHandle;

        /// <summary>
        /// Указатель на собранные данные драйвера.
        /// </summary>
        private int* _driverData;
        
        /// <summary>
        /// Счетчик прерываний.
        /// </summary>
        private int _interruptsCounter;

        /// <summary>
        /// Буффер с данными.
        /// </summary>
        private int[] _dataBuffer=new int[0];

        /// <summary>
        /// Размер блока данных.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// Кол-во блоков данных в буфере.
        /// </summary>
        private int _blockCount;

        /// <summary>
        /// Кол-во каналов чтения.
        /// </summary>
        public byte ChannelsCount { get; set; }

        /// <summary>
        /// Номер устройства.
        /// </summary>
        public int BoardNumber { get; set; }

        /// <summary>
        /// Флаг работы потока.
        /// </summary>
        private bool _threadTerminated = true;

        /// <summary>
        /// Поток сбора данных.
        /// </summary>
        private Thread _thread;

        #endregion

        #region ///// private methods /////

        /// <summary>
        /// Запускает поток если он остановлен.
        /// </summary>
        private void StartThread()
        {
            //проверяем что поток остановлен
            if (!_threadTerminated)
                throw new InvalidOperationException("Поток сбора не остановлен!");

            //создаем поток
            _thread = new Thread(ThreadFunc);
            _thread.Priority = ThreadPriority;
            _thread.IsBackground = true;   //обязательно фоновый поток
            _thread.Name = GetType().ToString();

            //запускаем поток
            _threadTerminated = false;
            _thread.Start();
        }

        /// <summary>
        /// Останавливает поток если он запущен.
        /// </summary>
        private void StopThread()
        {
            //останавливаем поток если он запущен
            if (_threadTerminated)
                return;

            _threadTerminated = true;
            //NOTE: может быть вообще убрать отсюда Abort
            if (_thread.Join(5000))
                _thread.Abort();
            

            _thread = null;
        }

        /// <summary>
        /// Функция потока сбора данных.
        /// </summary>
        private void ThreadFunc()
        {
            //ждем когда появится хендл события
            while (!_threadTerminated && _dataReady.SafeWaitHandle.IsClosed)
                Thread.Sleep(10);

            while (!_threadTerminated)
            {
                //ожидаем поступление новых данных
                if (!_dataReady.WaitOne(100, true))
                    continue;


                //получаем значение счетчика прерываний устройства
                var deviceInterrupsCounter = Wrapper.MioGetReadyBuf(BoardNumber);

                //пока есть данные, принимаем
                while (deviceInterrupsCounter - _interruptsCounter > 0)
                {
                    //рассчитываем номер блока
                    var readBlockNumber = _interruptsCounter%_blockCount;

                    //получаем указатель на данные устройства
                    var dataPnt = new IntPtr(_driverData + BlockSize*ChannelsCount*readBlockNumber);
                    //копируем данные
                    Marshal.Copy(dataPnt, _dataBuffer, 0, BlockSize*ChannelsCount);
                    //генерируем событие
                    OnDataReceived(_dataBuffer);

                    //увеличиваем счетчик
                    _interruptsCounter++;
                    //обновляем значение счетчика прерываний устройства
                    //deviceInterrupsCounter = Wrapper.MioGetReadyBuf(m_boardNumber);
                }

                //сбрасываем событие
                Wrapper.MioResetIntr(_eventGcHandle.AddrOfPinnedObject(), BoardNumber);

            }
        }


        /// <summary>
        /// Рассчитывает количество блоков в буфере.
        /// </summary>
        /// <param name="blockSize">Размер блока в отчетах.</param>
        /// <param name="channelsCount">Количество каналов.</param>
        /// <param name="boardNumber">Номер устройства.</param>
        /// <returns>Кол-во блоков.</returns>
        private static int CalculateBlockCount(int blockSize, int channelsCount, int boardNumber)
        {
            //рассчитываем размер блока в байтах
            //int sizeBytes = blockSize * channelsCount * sizeof(int);

            //рассчитываем кол-во блоков в буфере            
            //return Wrapper.MioGetNumBuff(sizeBytes, p_boardNumber);
            var bufsize = 0;
            IncAPI.GetSizeDevBuf(boardNumber, ref bufsize);
            return (bufsize - 4096)/sizeof (int)/blockSize/channelsCount;
        }


        #endregion
        
        #region ///// public methods /////

        /// <summary>
        /// Запускает сбор в указанном режиме.
        /// </summary>
        /// <returns>Код ошибки  <see cref="RunSborErrors"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Размер блока меньше нуля.</exception>
        public int Run()
        {
            //проверяем кол-во блоков в буфере            
            _blockCount =CalculateBlockCount(BlockSize, ChannelsCount, BoardNumber);
            if (_blockCount <= 0)
                return (int)RunSborErrors.Failed;

            //сбрасываем счетчик прерываний
            _interruptsCounter = 0;
            //выделяем место для массива данных
            if(_dataBuffer.Length!=BlockSize*ChannelsCount)
                _dataBuffer = new int[BlockSize * ChannelsCount];

            StartThread();
            
            var res = Wrapper.MioRunSbor(BlockSize, _blockCount, -1, ChannelsCount, 1, (1<<ChannelsCount)-1, out _driverData, _eventGcHandle.AddrOfPinnedObject(), BoardNumber);
            
            //устанавливаем хендл события
            _dataReady.SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle((IntPtr)_eventGcHandle.Target, false);
            //при ошибке останавливаем процесс)
            if (res != (int)RunSborErrors.Success)
                Stop();

            //возвращаем результат
            return res;
        }

        /// <summary>
        /// Останавливает процесс сбора.
        /// </summary>
        public void Stop()
        {
            //останавливаем поток
            StopThread();

            //останавливаем сбор данных
            Wrapper.MioStopSbor(null, BoardNumber);
                        
            if(!_dataReady.SafeWaitHandle.IsClosed)
                _dataReady.SafeWaitHandle.Close();
        }

        #endregion

        
    }
}
