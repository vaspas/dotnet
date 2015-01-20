using System;

namespace IncModules
{
    public class IncHelper
    {
        ~IncHelper()
        {
            if (_initCount>0)
                IncAPI.pclose();
        }

        private int _initCount;
        public void IncApiInit()
        {
            if(_initCount==0)
            {
                IncAPI.EnableWriteToLog();
                IncAPI.pinit();
            }

            _initCount++;
        }

        public void IncApiClose()
        {
            if(_initCount==0)
                throw new InvalidOperationException();

            if (_initCount == 1)
            {
                IncAPI.EnableWriteToLog();
                IncAPI.pclose();
            }

            _initCount--;
        }

        private static IncHelper _instance;
        public static IncHelper Instance
        {
            get { return _instance ?? (_instance = new IncHelper()); }
        }

        public static int GetMaxChannelsCount(IncDevices device)
        {
            if (device == IncDevices.INK424_v3)
                return 4;
            if (device == IncDevices.INK824)
                return 8;
            if (device == IncDevices.INK1010)
                return 4;
            if (device == IncDevices.INK1210)
                return 8;

            throw new ArgumentException("Устройство не поддерживается.");
        }

        public static byte[] GainAdresses = new byte[] { 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf };
    }
}
