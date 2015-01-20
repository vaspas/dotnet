using System;

namespace IncModules.Ink824
{
    public sealed class AdcHelper
    {
        public Ink824ModuleInt Module { get; set; }
        

        /// <summary>
        /// Устанавливает номер устройства. Вызывать только в выключенном состоянии
        /// </summary>
        /// <returns>Возвращает true если устройство установлено.</returns>
        public bool InitializeDevice()
        {
            //получаем кол-во устройств
            var boardsCount=0;  
            IncAPI.POS_GetNBoards(ref boardsCount);

            //получаем код устройства, с проверкой кол-ва устройств
            var deviceCode = (int)IncDevices.NODEV;
            if (Module.BoardNumber < boardsCount)            
            {
                IncAPI.OpenBoard(Module.BoardNumber);
                IncAPI.POS_GetCExtDev(ref deviceCode, Module.BoardNumber);
            }

            Wrapper.MioInit(Module.BoardNumber);

            var maxChannelsCount = IncHelper.GetMaxChannelsCount(Module.DeviceType);

            //устанавливаем свою таблицу каналов
            var channelsTab = new byte[maxChannelsCount];
            for (byte i = 0; i < maxChannelsCount; i++)
                channelsTab[i] = i;
            Wrapper.MioSetTab(Module.ChannelsCount, channelsTab, Module.BoardNumber);

            //проверяем что код соответствует требуемому
            return deviceCode == (int)Module.DeviceType;
        }
        
        /// <summary>
        /// Запуск АЦП.
        /// </summary>
        /// <returns>Код ошибки  <see cref="RunSborErrors"/>.</returns>
        /// <exception cref="INC824Exception">При включенном АЦП или некоректных параметрах.</exception>
        public void PrepareBeforeRun()
        {
            //подготавливаем АЦП для начала работы

            //считываем таблицу каналов            
            //пока что не учитываем таблицу, т.к. функция возвращает какието непонятные значения
            //Wrapper.MioGetTab( MAX_CHANNELS, channelsTab, m_boardNumber);
            
            //устанавливаем тип синхронизации
            if (Module.ExternalSync)
                Wrapper.MioSetExternalSync(Module.BoardNumber);
            else
            {
                Wrapper.MioSetInternalSync(Module.BoardNumber);
                //устанавливаем частоту квантования АЦП
                Wrapper.MioSetFreq((float)(Module.Frequency*(1+Module.Modifications.GetQuantumFreqCorrPpu()/1000000)), Module.BoardNumber);
            }

            //устанавливаем времени запуска, для режима по времени
            if (Module.StartMode == StartMode.Time)
                Wrapper.MioSetTime(Module.StartTime.Hours, Module.StartTime.Minutes, Module.StartTime.Seconds, Module.BoardNumber);
        }

        public void SetupAllAmplifications()
        {
            //устанавливаем усиления
            for (var i = 0; i < Module.ChannelsCount; i++)
                Wrapper.MioSetGain(IncHelper.GainAdresses[i], (byte)Module.GainValues[i], Module.BoardNumber);
        }

        public void SetupAmplification(int channel)
        {
            Wrapper.MioSetGain(IncHelper.GainAdresses[channel], (byte)Module.GainValues[channel], Module.BoardNumber);
        }


        /// <summary>
        /// Возвращает текущее время АЦП.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetDeviceTime()
        {
            //значение времени
            var time = 0;

            Wrapper.MioGetMarkTime(ref time, Module.BoardNumber);

            //получаем значения
            var hour = GetHexDecValue((byte)(time >> 16));
            var minutes = GetHexDecValue((byte)(time >> 8));
            var seconds = GetHexDecValue((byte)(time));

            //возвращаем значение
            return new TimeSpan(hour, minutes, seconds);
        }

        /// <summary>
        /// Возвращает значение, закодированное двоично-десятиноым форматом.
        /// </summary>
        /// <param name="hexValue">Закодированное значение.</param>
        /// <returns></returns>
        private static int GetHexDecValue(byte hexValue)
        {
            //переводим значение в шестнадцатеричную строку
            //а потом в число
            return int.Parse(hexValue.ToString("X"));
        }

    }
}
