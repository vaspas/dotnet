using System;

namespace IncModules.Mio4400
{
    public sealed class AdcHelper
    {
        public Mio4400ModuleInt Module { get; set; }
        

        /// <summary>
        /// ������������� ����� ����������. �������� ������ � ����������� ���������
        /// </summary>
        /// <returns>���������� true ���� ���������� �����������.</returns>
        public bool InitializeDevice()
        {
            //�������� ���-�� ���������
            var boardsCount=0;  
            IncAPI.POS_GetNBoards(ref boardsCount);

            //�������� ��� ����������, � ��������� ���-�� ���������
            var deviceCode = (int)IncDevices.NODEV;
            if (Module.BoardNumber < boardsCount)            
            {
                IncAPI.OpenBoard(Module.BoardNumber);
                IncAPI.POS_GetCExtDev(ref deviceCode, Module.BoardNumber);
            }

            Wrapper.MioInit(Module.BoardNumber);

            var maxChannelsCount = IncHelper.GetMaxChannelsCount(Module.DeviceType);

            //������������� ���� ������� �������
            var channelsTab = new byte[maxChannelsCount];
            for (byte i = 0; i < maxChannelsCount; i++)
                channelsTab[i] = i;
            Wrapper.MioSetTab(Module.ChannelsCount, channelsTab, Module.BoardNumber);

            //��������� ��� ��� ������������� ����������
            return deviceCode == (int)Module.DeviceType;
        }
        
    }
}
