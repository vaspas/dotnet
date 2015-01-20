using System;
using System.Runtime.InteropServices;

namespace SoundBlasterModules.WaveApi.Output
{
    class SoundBlaster
    {
        #region ///// private const /////

        /// <summary>
        /// Кол-во буферов SB.
        /// </summary>
        private const int SBBUFFERSCOUNT = 4;
        
        #endregion

        #region ///// private fields /////

        /// <summary>
        /// Номер выбранного устройства.
        /// </summary>
        private int _deviceNumber;


        /// <summary>
        /// Размер блока данных.
        /// </summary>
        private int _blockSize = 1024;

        /// <summary>
        /// Идентификатор устройства.
        /// </summary>
        private IntPtr _hwo;

        /// <summary>
        /// Структура WaveFormatEx.
        /// </summary>
        private WaveAPI.WAVEFORMATEX _wfex;

        /// <summary>
        /// Массив буферов SB.
        /// </summary>
        private SBBuffer[] _buffersSb;

        /// <summary>
        /// Handle для того чтобы сборщик мусора не перемещал наш делегат(указатель на функцию) в памяти.
        /// </summary>
        private GCHandle _wopdelHandle;

        /// <summary>
        /// Данные для отправки.
        /// </summary>
        private Array _data;

        private readonly object _sync=new object();

        /// <summary>
        /// Состояние устройства.
        /// </summary>
        private bool _started;


        /// <summary>
        /// Уровень громкости от 0 до 1.
        /// </summary>
        private float _volume;

        #endregion
                
        #region ///// public properties /////

        /// <summary>
        /// Возвращает и устанавливает номер устройства.
        /// </summary>
        /// <exception cref="MemberAccessException">При изменении в процессе работы.</exception>
        public int DeviceNumber
        {
            get { return _deviceNumber; }
            set 
            {
                //запрещаем менять в процессе работы
                CheckLock();

                if (value < 0)
                    return;
                
                //запоминаем номер устройства
                _deviceNumber = value;

                //устанавливаем уровень громкости
                _volume=0;
                GetVolumeLevel();                
            }
        }

        /// <summary>
        /// Возвращает и устанавливает частоту квантования.
        /// После установки требуется перезапуск.
        /// </summary>
        public double Frequency { get; set; }

        /// <summary>
        /// Возвращает и устанавливает флаг типа данных для работы.        
        /// После установки требуется перезапуск.
        /// </summary>
        public bool FloatType { get; set; }

        /// <summary>
        /// Возвращает и устанавливает размер блока одного канала. 
        /// После установки требуется перезапуск.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// Возвращает уровень громкости.
        /// </summary>
        public float Volume
        {
            get { return _volume; }
        }

        public int ChannelsCount { get; set; }

        #endregion

        #region ///// private methods /////

        /// <summary>
        /// Запуск устройства для приема данных, или просто открытие.
        /// </summary>
        /// <param name="wopdel">Делегат callback функции для приема или null для открытия.</param>
        /// <param name="deviceNumber">Номер устройства.</param>
        private int WaveOutOpen(int deviceNumber, WaveAPI.WaveProcDelegate wopdel)
        {
            //размер типа данных
            var dataTypeSize = FloatType ? sizeof(float) : sizeof(short);

            //подготавливаем структуру WaveFormatEx
            _wfex.wFormatTag = FloatType ? (short)WaveAPI.WAVE_FORMAT_FLOAT : (short)WaveAPI.WAVE_FORMAT_PCM;
            _wfex.nSamplesPerSec = (int) (Frequency);// + 0.49);
            _wfex.nAvgBytesPerSec = (_wfex.nSamplesPerSec * dataTypeSize * ChannelsCount); // !
            _wfex.nChannels = (short)ChannelsCount;
            _wfex.nBlockAlign = (short)(dataTypeSize * ChannelsCount);
            _wfex.wBitsPerSample = (short)(8 * dataTypeSize);
            _wfex.cbSize = 0;

            _hwo = IntPtr.Zero;

            //создаем хэндл для делегата
            if (wopdel != null)
                _wopdelHandle = GCHandle.Alloc(wopdel);

            //открываем устройство
            var res = WaveAPI.waveOutOpen(out _hwo, deviceNumber/*WaveInAPI.WAVE_MAPPER*/, ref _wfex, wopdel, 0, WaveAPI.CALLBACK_FUNCTION);
            if (res != WaveAPI.NOERROR)
                return res;

            //если нам передали callback функцию, значит будем передавать данные,
            //подготовим буферы
            if (wopdel != null)
            {
                //выделяем буферы передачи
                _buffersSb = new SBBuffer[SBBUFFERSCOUNT];  //создаем массив буферов
                for (var i = 0; i < SBBUFFERSCOUNT; i++)
                {
                    _buffersSb[i] = new SBBuffer();  //создаем буфер
                    _buffersSb[i].Alloc(_blockSize * ChannelsCount, FloatType);  //выделяем память
                    //подготавливаем буфер
                    res = WaveAPI.waveOutPrepareHeader(_hwo, _buffersSb[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(_buffersSb[i].WaveHdr));
                    if (res != WaveAPI.NOERROR)
                        return res;
                    //записываем пустой буфер для запуска процесса
                    res = WaveAPI.waveOutWrite(_hwo, _buffersSb[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(_buffersSb[i].WaveHdr));
                    if (res != WaveAPI.NOERROR)
                        return res;
                }
            }

            //устанавливаем флаг запуска если нет ошибок
            if (res == WaveAPI.NOERROR)
                _started = true;

            return WaveAPI.NOERROR;
        }

        /// <summary>
        /// Остановка устройства для передачи данных.
        /// </summary>
        private void WaveOutClose()
        {
            //сбрасываем флаг
            _started = false;
            
            WaveAPI.waveOutReset(_hwo);

            if (_buffersSb != null)
            {
                //освобождаем выделенные буферы
                for (int i = 0; i < SBBUFFERSCOUNT; i++)
                {
                    WaveAPI.waveOutUnprepareHeader(_hwo, _buffersSb[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(_buffersSb[i].WaveHdr));
                    _buffersSb[i].Free();
                }
                _buffersSb = null;
            }

            //закрываем устройство
            WaveAPI.waveOutClose(_hwo);
            _hwo = IntPtr.Zero;
            //освобождаем хэндл делегата
            if (_wopdelHandle.IsAllocated)
                _wopdelHandle.Free();

        }

        /// <summary>
        /// Функция вызывается устройством после приема очередного блока данных.
        /// </summary>
        /// <param name="hdrvr"></param>
        /// <param name="uMsg"></param>
        /// <param name="dwUser"></param>
        /// <param name="wavhdr"></param>
        /// <param name="dwParam2"></param>
        private void WaveOutProc(IntPtr hdrvr, int uMsg, int dwUser, ref WaveAPI.WAVEHDR wavhdr, int dwParam2)
        {
            //выполняем функцию только если вернулось сообщение что буфер обработан
            if (uMsg != WaveAPI.WindowMessages.MM_WOM_DONE || !_started)
                return;

            //ждем данных
            //записываем в буфер данные из очереди или обнуляем его, если данных в очереди нет
            lock (_sync)
                if (_data != null)
                {
                    lock (_data)
                    {
                        //записываем в буфер данные                            
                        if (FloatType)
                            Marshal.Copy((float[]) _data, 0, wavhdr.lpData, BlockSize);
                        else
                            Marshal.Copy((short[]) _data, 0, wavhdr.lpData, BlockSize);
                    }
                    //сбрасываем данные
                    _data = null;
                }

            //Вызываем событие запроса данных
            DataRequered();

            var gch = (GCHandle)wavhdr.dwUser;
            var sbbuf = (SBBuffer)gch.Target;
            //передаем устройству заполненный новыми данными буфер
            WaveAPI.waveOutWrite(_hwo, sbbuf.WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(wavhdr));
        }

        /// <summary>
        /// Функция возвращает уровень громкости на регуляторе или SB.
        /// </summary>
        private int GetVolumeLevel()
        {
            var res = WaveAPI.NOERROR;

            var doStop = !_started;
            if (!_started)
                res = WaveOutOpen(_deviceNumber, null);
            try
            {
                if (res != WaveAPI.NOERROR)
                    return res;

                var volume = WaveOut.GetDacVolume(_hwo);
                if (WaveOut.LastResult != WaveAPI.NOERROR)
                    return WaveOut.LastResult;

                _volume = volume;
            }
            finally
            {
                if (doStop)
                    WaveOutClose();
            }

            return WaveAPI.NOERROR;
        }

        /// <summary>
        /// Проверка блокировки данных. Некоторые данные нельзя менять в процессе работы.
        /// </summary>
        private void CheckLock()
        {
            if (_started)
                throw new MemberAccessException("Data was locked.");
        }
                
        #endregion


        /// <summary>
        /// Запускает устройство для приема данных.
        /// </summary>
        /// <returns>ошибка</returns>
        public int Start()
        {
            //очищаем очередь
            _data=null;

            //запускаем
            return WaveOutOpen(_deviceNumber, WaveOutProc);
        }

        /// <summary>
        /// Останавливает устройство.
        /// </summary>
        public void Stop()
        {
            WaveOutClose();
        }

        /// <summary>
        /// Функция устанавливает уровень громкости.
        /// </summary>
        public int SetVolume(float level)
        {
            int res = WaveAPI.NOERROR;

            bool doStop = !_started;
            if (!_started)
                res = WaveOutOpen( _deviceNumber,null);
            try
            {
                if (res != WaveAPI.NOERROR)
                    return res;

                float volume = WaveOut.SetDacVolume(_hwo, level);
                if (WaveOut.LastResult != WaveAPI.NOERROR)
                    return WaveOut.LastResult;

                _volume = volume;
            }
            finally
            {
                if (doStop)
                    WaveOutClose();
            }

            return WaveAPI.NOERROR;
        }

        /// <summary>
        /// Добавляет новые данные в очередь.
        /// </summary>
        /// <param name="data">Ссылка на данные.</param>
        /// <returns>True если данные добавлены в очередь.</returns>
        public void SetData(Array data)
        {
            lock (_sync)
            {
                //проверяем параметр
                //...ссылка на ноль
                if (data == null)
                    throw new ArgumentNullException();
                //...проверяем тип объекта
                if (!(data is float[]) && !(data is short[]))
                    throw new ArgumentException("Invalid data type");
                if (data is float[] && !FloatType)
                    throw new ArgumentException("Invalid data type");
                //...проверяем размер
                if (data.Length != BlockSize)
                    throw new ArgumentOutOfRangeException();

                //устанавливаем данные
                _data = data;
            }
        }


        /// <summary>
        /// Событие возникает при отправке каждого блока данных.
        /// </summary>
        public event Action DataRequered=delegate {};
    }
}
