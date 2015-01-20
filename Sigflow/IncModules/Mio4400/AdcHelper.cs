using System;

namespace IncModules.Mio4400
{
    public sealed class AdcHelper
    {
        public Mio4400ModuleInt Module { get; set; }
        

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
        
    }
}
