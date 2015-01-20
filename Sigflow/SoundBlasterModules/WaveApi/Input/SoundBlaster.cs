using System;
using System.Runtime.InteropServices;

namespace SoundBlasterModules.WaveApi.Input
{
    /// <summary>
    /// Класс для работы с устройством.
    /// </summary>
    internal class SoundBlaster
    {
        /// <summary>
        /// Кол-во буферов SB.
        /// </summary>
        const int SB_BUFFERS_COUNT= 4;

        #region ///// private fields /////

        /// <summary>
        /// Номер устройства.
        /// </summary>
        private int _device;

        /// <summary>
        /// Идентификатор устройства.
        /// </summary>
        private IntPtr m_hwi;

        /// <summary>
        /// Массив буферов SB.
        /// </summary>
        private SBBuffer[] m_buffers;

        /// <summary>
        /// Массив данных.
        /// </summary>
        private short[] m_writearr=new short[0];

        /// <summary>
        /// Handle для того чтобы сборщик мусора не перемещал делегат
        /// (указатель на функцию отратного вызова) в памяти.
        /// </summary>
        private GCHandle m_wipdelHandle;

        /// <summary>
        /// Сюда записываем коды ошибок от функций API.
        /// </summary>
        //private int m_result;

        /// <summary>
        /// Состояние устройства.
        /// </summary>
        private bool m_started=false;

        /// <summary>
        /// Список входных линий мультиплексора (описание входных линий устройства).
        /// </summary>
        private WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] m_muxList=new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[0];

        /// <summary>
        /// Результат выполнения последней операции.
        /// </summary>
        private int m_lastResult;

        /// <summary>
        /// Сбрасываем результат.
        /// </summary>
        private void ResetResult()
        {
            m_lastResult = WaveAPI.NOERROR;
        }

        /// <summary>
        /// Устанавливает результат, но только в случае ошибки.
        /// </summary>
        /// <returns>true если ошибка установлена</returns>
        private bool SetError(int error)
        {
            if (error != WaveAPI.NOERROR)
            {
                m_lastResult = error;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Возвращает результат выполнения последней операции.
        /// </summary>
        public int LastResult
        {
            get { return m_lastResult; }
        }

        #endregion

        #region ///// private methods /////
		
        /// <summary>
        /// Функция для приема/обработки данных.
        /// </summary>
        /// <param name="hdrvr"></param>
        /// <param name="uMsg"></param>
        /// <param name="dwUser"></param>
        /// <param name="wavhdr"></param>
        /// <param name="dwParam2"></param>
        private void WaveInProc(IntPtr hdrvr, int uMsg, int dwUser, ref WaveAPI.WAVEHDR wavhdr, int dwParam2)
        {
            if (uMsg == WaveAPI.WindowMessages.MM_WIM_DATA && m_started)
            {
                Marshal.Copy(wavhdr.lpData, m_writearr, 0, m_writearr.Length);

                //вызываем событие получения новых данных);
                if (NewDataReceived != null)
                    NewDataReceived();

                //добавляем буфер для приема
                var h = (GCHandle)wavhdr.dwUser;
                var sb_buf = (SBBuffer)h.Target;
                lock (sb_buf)
                {
                    /*m_result =*/ WaveAPI.waveInAddBuffer(m_hwi, sb_buf.WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(wavhdr));
                }
            }
        }

        /// <summary>
        /// Запуск устройства для приема данных.
        /// </summary>
        /// <param name="p_deviceNumber">Номер устройства.</param>
        /// <param name="p_wipdel">Делегат callback функции.</param> 
        private void WaveInStart(int p_deviceNumber, WaveAPI.WaveProcDelegate p_wipdel)
        {
            ResetResult();
            int res = WaveAPI.NOERROR;

            //создаем и инициализируем структуру
            WaveAPI.WAVEFORMATEX wfex;
            wfex.wFormatTag = (short)WaveAPI.WAVE_FORMAT_PCM;
            wfex.nSamplesPerSec = Frequency;
            wfex.nAvgBytesPerSec = (wfex.nSamplesPerSec * sizeof(short) * ChannelsCount); // !
            wfex.nChannels = (short)ChannelsCount;
            wfex.nBlockAlign = (short)(sizeof(short) * ChannelsCount);
            wfex.wBitsPerSample = (short)(8 * sizeof(short));
            wfex.cbSize = 0;

            //обнуляем хэндл
            m_hwi = IntPtr.Zero;

            //фиксируем делегат в памяти
            if(p_wipdel!=null)
                m_wipdelHandle = GCHandle.Alloc(p_wipdel);

            //открываем устройство, и получаем его хендл
            res = WaveAPI.waveInOpen(out m_hwi, p_deviceNumber/*WaveInAPI.WAVE_MAPPER*/, ref wfex, p_wipdel, 0, WaveAPI.CALLBACK_FUNCTION);
            if (SetError(res))
                return;

            //выделяем буферы только если будем принимать данные
            if (p_wipdel != null)
            {
                res = AllocateBuffers();
                if (SetError(res))
                    return;
            }

            //запускаем устройство
            res = WaveAPI.waveInStart(m_hwi);
            if (SetError(res))
                return;

            m_started = true;
        }

        /// <summary>
        /// Выделяет приемные буферы.
        /// </summary>
        /// <returns>Ошибка.</returns>
        private int AllocateBuffers()
        {
            int res = WaveAPI.NOERROR;

            //выделяем буферы, и указываем их устройству,
            m_buffers = new SBBuffer[SB_BUFFERS_COUNT];
            for (int i = 0; i < SB_BUFFERS_COUNT; i++)
            {
                m_buffers[i] = new SBBuffer();
                m_buffers[i].Alloc(BlockSize * ChannelsCount, false);

                res = WaveAPI.waveInPrepareHeader(m_hwi, m_buffers[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(m_buffers[i].WaveHdr));
                if (res != WaveAPI.NOERROR)
                    return res;

                res = WaveAPI.waveInAddBuffer(m_hwi, m_buffers[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(m_buffers[i].WaveHdr));
                if (res != WaveAPI.NOERROR)
                    return res;
            }

            return res;
        }

        /// <summary>
        /// Остановка устройства.
        /// </summary>
        private void WaveInStop()
        {
            //сбрасываем флаг
            m_started = false;

            //останавливаем устройство
            WaveAPI.waveInStop(m_hwi);
            //сбрасываем
            WaveAPI.waveInReset(m_hwi);

            //освобождаем выделенные буферы
            if (m_buffers != null)
            {                
                for (var i = 0; i < SB_BUFFERS_COUNT; i++)
                {
                    WaveAPI.waveInUnprepareHeader(m_hwi, m_buffers[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(m_buffers[i].WaveHdr));
                    
                    m_buffers[i].Free();
                }
                m_buffers = null;
            }
            //закрываем устройство
            WaveAPI.waveInClose(m_hwi);
            //обнуляем хэдл устройства
            m_hwi = IntPtr.Zero;
            //освобождаем хендл делегата
            if (m_wipdelHandle.IsAllocated)
                m_wipdelHandle.Free();

        }
      
        /// <summary>
        /// Устанавливает новую входную линию устройства.
        /// Вызывается только при включенном устройстве.
        /// </summary>
        /// <param name="p_volValue">Уровень громкости установленной линии от 0 до 1.</param>
        private void SetLineInternal(out float p_volValue)
        {
            ResetResult();
            int res = WaveAPI.NOERROR;

            //Handle for mixer
            IntPtr hmx = IntPtr.Zero;     
            p_volValue = 0;

            //открываем микшер
            res = WaveAPI.mixerOpen(out hmx, (uint)m_hwi, IntPtr.Zero, IntPtr.Zero, WaveAPI.MIXER_OBJECTF_HWAVEIN);
            if (SetError(res))
                return;

            try
            {
                //проверяем что линия есть
                if (LineID != 0)
                {
                    WaveIn.SetLineType(hmx, LineID);
                    if (SetError(WaveIn.LastResult))
                        return;
                }
                
                p_volValue = WaveIn.GetADCVolume(hmx, LineID);
                if (SetError(WaveIn.LastResult))
                    return;
            }
            //закрываем микшер
            finally
            {
               res= WaveAPI.mixerClose(hmx);
               SetError(WaveIn.LastResult);
                   
            }
        }

        /// <summary>
        /// Устанавливает уровень громкости.
        /// Вызывается только при включенном устройстве.
        /// </summary>
        /// <param name="p_level">Уровень от 0 до 1.</param>
        private void SetVolumeInternal(ref float p_level)
        {
            ResetResult();
            int res = WaveAPI.NOERROR;

            //Handle for mixer
            var hmx = IntPtr.Zero;     

            //открываем микшер
            res = WaveAPI.mixerOpen(out hmx, (uint)m_hwi, IntPtr.Zero, IntPtr.Zero, WaveAPI.MIXER_OBJECTF_HWAVEIN);
            if (SetError(res))
                return;

            try
            {
                //устанавливаем линию
                //WaveIn.SetLineType(hmx, LineID);
                //if (res != WaveAPI.NOERROR)
                //    return res;
                //получаем уровень громкости
                p_level = WaveIn.SetADCVolume(hmx, p_level, LineID);
                if (SetError(WaveIn.LastResult))
                    return;
            }
            finally
            {
                //закрываем микшер
                res = WaveAPI.mixerClose(hmx);
                SetError(res);
            }
        }

        /// <summary>
        /// Возвращает список входных линий мультиплексора. Вызывается только при включенном устройстве.
        /// </summary>
        /// <returns>список</returns>
        private WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] GetMixerListInternal()
        {
            ResetResult();
            int res = WaveAPI.NOERROR;

            var result = new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[0];

            //Handle for mixer
            IntPtr hmx = IntPtr.Zero;     //Handle for mixer

            //открываем микшер
            res = WaveAPI.mixerOpen(out hmx, (uint)m_hwi, IntPtr.Zero, IntPtr.Zero, WaveAPI.MIXER_OBJECTF_HWAVEIN);
            if (SetError(res))
                return result;
            try
            {
                WaveAPI.MIXERLINE mxlWI = WaveIn.GetWaveInLine(hmx);
                if (SetError(WaveIn.LastResult))
                    return result;
                WaveAPI.MIXERCONTROL mxcMux = WaveIn.GetMixerControl(hmx, mxlWI, WaveAPI.ControlType.MIXERCONTROL_CONTROLTYPE_MUX);
                //в случае отсутствия мультиплексора
                if (WaveIn.LastResult == (int)WaveAPI.Errors.MIXERR_INVALCONTROL)
                {
                    var lt = new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT();
                    lt.szName = mxlWI.szName;
                    return new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] { lt };
                }
                else if (SetError(WaveIn.LastResult))
                    return result;

                var outResult = new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[0];
                if (mxcMux != null)
                    outResult = WaveIn.GetControlDetailsListText(hmx, mxcMux);
                if (SetError(WaveIn.LastResult))
                    return result;
                result = outResult;
            }
            finally
            {
                //закрываем микшер
                res=WaveAPI.mixerClose(hmx);
                SetError(res);
            }


            return result;
        }

        /// <summary>
        /// Возвращает список входных линий мультиплексора.
        /// </summary>
        /// <returns>список</returns>
        private WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] GetMixerList()
        {
            ResetResult();

            bool doStop = !m_started;
            if (!m_started)
                WaveInStart(_device, null);
            try
            {
                if (m_lastResult != WaveAPI.NOERROR)
                    return new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[0];

                return GetMixerListInternal();
            }
            finally
            {
                if (doStop)
                    WaveInStop();
            }
        }
        
        /// <summary>
        /// Проверка блокировки данных. Некоторые данные нельзя менять в процессе работы.
        /// </summary>
        private void CheckLock()
        {
            if (m_started)
                throw new MemberAccessException("Data was locked.");
        }
        
	    #endregion

        #region ///// public methods /////

        /// <summary>
        /// Запускает устройство для приема данных.
        /// </summary>
        public void Start()
        {
            //создаем массив для чтения
            int length = BlockSize * ChannelsCount;
            if(m_writearr.Length!=length)
                m_writearr = new short[length];

            //запускаем
            WaveInStart(_device, WaveInProc);            
        }

        /// <summary>
        /// Останавливает устройство.
        /// </summary>
        public void Stop()
        {
            WaveInStop();
        }

        /// <summary>
        /// Устанавливает уровень громкости для выбранной линии.
        /// </summary>
        /// <param name="p_level">Уровень от 0 до 1.</param>
        /// <returns>ошибка</returns>
        public void SetVolume(float p_level)
        {
            bool doStop = !m_started;
            if (!m_started)
                WaveInStart(_device, null);
            try
            {
                if (m_lastResult != WaveAPI.NOERROR)
                    return;

                //устанавливаем уровень
                SetVolumeInternal(ref p_level);
                if (m_lastResult == WaveAPI.NOERROR)
                    Volume = p_level;
            }
            finally
            {
                if (doStop)
                    WaveInStop();
            }
        }

        /// <summary>
        /// Устанавливает входную линию
        /// </summary>
        /// <param name="p_lineNumber">Порядковый номер линии.</param>    
        public void SetLine(int p_lineNumber)
        {
            //запоминаем номер линии
            LineNumber = p_lineNumber;

            int res = WaveAPI.NOERROR;

            bool doStop = !m_started;
            if (!m_started)
                WaveInStart(_device, null);
            try
            {
                if (m_lastResult != WaveAPI.NOERROR)
                    return;

                float volume = Volume;
                SetLineInternal(out volume);
                if (m_lastResult == WaveAPI.NOERROR)
                    Volume = volume;
            }
            finally
            {
                if (doStop)
                    WaveInStop();
            }
        }

        #endregion

        #region ///// public properties /////

        /// <summary>
        /// Возвращает и устанавливает номер устройства.
        /// </summary>
        /// <exception cref="MemberAccessException">При изменении в процессе работы.</exception>
        public int DeviceNumber
        {
            get { return _device; }
            set 
            {
                //запрещаем менять в процессе работы
                CheckLock();

                //устанавливаем новое устройство
                _device = value;

                //загружаем список линий
                m_muxList = GetMixerList();
                if (m_lastResult != WaveAPI.NOERROR)
                    return;
                //устанавливаем линию
                if(LineNumber>=m_muxList.Length)
                    SetLine(m_muxList.Length-1);
                else
                    SetLine(LineNumber);
            }
        }

        /// <summary>
        /// Возвращает описание устройства.
        /// </summary>
        public WaveAPI.WAVEINCAPS DeviceCaps
        {
            get 
            {
                ResetResult();

                WaveAPI.WAVEINCAPS wic = new WaveAPI.WAVEINCAPS();
                int res=WaveAPI.waveInGetDevCaps(_device, ref wic, Marshal.SizeOf(wic));
                SetError(res);

                return wic;
            }
        }

        /// <summary>
        /// Возвращает порядковый номер линии.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Возвращает идентификатор линии.
        /// </summary>
        public int LineID
        {
            get
            {
                return LineDetails.dwParam1;
            }
        }

        /// <summary>
        /// Возвращает параметры выбранной линии.
        /// </summary>
        public WaveAPI.MIXERCONTROLDETAILS_LISTTEXT LineDetails
        {
            get 
            {
                if (LineNumber < m_muxList.Length)
                    return m_muxList[LineNumber];
                else
                    return new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT();
            }
        }

        /// <summary>
        /// Возвращает и устанавливает кол-во каналов. После установки требуется перезапуск.
        /// </summary>
        public int ChannelsCount { get; set; }

        /// <summary>
        /// Возвращает и устанавливает частоту квантования. После установки требуется перезапуск.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// Возвращает и устанавливает размер блока. После установки требуется перезапуск.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// Возвращает ссылку на массив принятых данных.
        /// </summary>
        public short[] DataArray
        {
            get { return m_writearr; }
        }

        /// <summary>
        /// Возвращает список входных линий мультиплексора.
        /// </summary>
        public WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] LinesList
        {
            get { return m_muxList; }
        }

        /// <summary>
        /// Возвращает уровень громкости.
        /// </summary>
        public float Volume { get; private set; }

        #endregion

        /// <summary>
        /// Событие получения новых данных.
        /// </summary>
        public event Action NewDataReceived;
    }
}
