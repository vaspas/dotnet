using System;

namespace IncModules.Ink824
{
    public sealed class AdcHelper
    {
        public Ink824ModuleInt Module { get; set; }
        

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
        
        /// <summary>
        /// ������ ���.
        /// </summary>
        /// <returns>��� ������  <see cref="RunSborErrors"/>.</returns>
        /// <exception cref="INC824Exception">��� ���������� ��� ��� ����������� ����������.</exception>
        public void PrepareBeforeRun()
        {
            //�������������� ��� ��� ������ ������

            //��������� ������� �������            
            //���� ��� �� ��������� �������, �.�. ������� ���������� ������� ���������� ��������
            //Wrapper.MioGetTab( MAX_CHANNELS, channelsTab, m_boardNumber);
            
            //������������� ��� �������������
            if (Module.ExternalSync)
                Wrapper.MioSetExternalSync(Module.BoardNumber);
            else
            {
                Wrapper.MioSetInternalSync(Module.BoardNumber);
                //������������� ������� ����������� ���
                Wrapper.MioSetFreq((float)(Module.Frequency*(1+Module.Modifications.GetQuantumFreqCorrPpu()/1000000)), Module.BoardNumber);
            }

            //������������� ������� �������, ��� ������ �� �������
            if (Module.StartMode == StartMode.Time)
                Wrapper.MioSetTime(Module.StartTime.Hours, Module.StartTime.Minutes, Module.StartTime.Seconds, Module.BoardNumber);
        }

        public void SetupAllAmplifications()
        {
            //������������� ��������
            for (var i = 0; i < Module.ChannelsCount; i++)
                Wrapper.MioSetGain(IncHelper.GainAdresses[i], (byte)Module.GainValues[i], Module.BoardNumber);
        }

        public void SetupAmplification(int channel)
        {
            Wrapper.MioSetGain(IncHelper.GainAdresses[channel], (byte)Module.GainValues[channel], Module.BoardNumber);
        }


        /// <summary>
        /// ���������� ������� ����� ���.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetDeviceTime()
        {
            //�������� �������
            var time = 0;

            Wrapper.MioGetMarkTime(ref time, Module.BoardNumber);

            //�������� ��������
            var hour = GetHexDecValue((byte)(time >> 16));
            var minutes = GetHexDecValue((byte)(time >> 8));
            var seconds = GetHexDecValue((byte)(time));

            //���������� ��������
            return new TimeSpan(hour, minutes, seconds);
        }

        /// <summary>
        /// ���������� ��������, �������������� �������-���������� ��������.
        /// </summary>
        /// <param name="hexValue">�������������� ��������.</param>
        /// <returns></returns>
        private static int GetHexDecValue(byte hexValue)
        {
            //��������� �������� � ����������������� ������
            //� ����� � �����
            return int.Parse(hexValue.ToString("X"));
        }

    }
}
