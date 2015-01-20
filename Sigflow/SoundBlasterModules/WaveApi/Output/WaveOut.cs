using System;

using System.Runtime.InteropServices;

namespace SoundBlasterModules.WaveApi.Output
{
    /// <summary>
    /// ������������� ����� ������� ��� ������ �� �������� ������.
    /// </summary>
    internal static class WaveOut
    {
        /// <summary>
        /// ���������� ���������.
        /// </summary>
        private static void ResetResult()
        {
            LastResult = WaveAPI.NOERROR;
        }

        /// <summary>
        /// ������������� ���������, �� ������ � ������ ������.
        /// </summary>
        /// <returns>true ���� ������ �����������</returns>
        private static bool SetError(int error)
        {
            if (error != WaveAPI.NOERROR)
            {
                LastResult = error;
                return true;
            }
            return false;
        }

        /// <summary>
        /// ���������� ��������� ���������� ��������� ��������.
        /// </summary>
        public static int LastResult { get; private set; }

        /// <summary>
        /// ���������� ������� �����.
        /// </summary>
        /// <param name="hWaveOut"></param>
        /// <param name="set">True �������� ��� ����� ���������� �������.</param>
        /// <param name="position">�������� ������ ��� ��������� ������� � 0.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">���� ��� ��������� ������ �������� 
        /// ������� ������ 0 ��� ������/����� ���������� �������.</exception>
        private static float DacVolume(IntPtr hWaveOut, bool set, float position)
        {
            //���������� ���������
            ResetResult();

            //��������� ���������� �������
            int res;

            //��������� ��������� 
            if (hWaveOut == IntPtr.Zero)
                throw new ArgumentNullException("hWaveOut");

            if (set)
            {
                if (position < 0) throw new ArgumentOutOfRangeException("position", "must be >=0");
                if (position > 1) throw new ArgumentOutOfRangeException("position", "must be <=1");
            }

            //��������������� ��������
            var value = (ushort)(ushort.MaxValue * position);

            int setv = value;
            setv = setv << 16;
            setv = setv + value;
            // ���������� �������� �������� ���� �����
            if (set)
            {
                res = WaveAPI.waveOutSetVolume(hWaveOut, setv);
                SetError(res);
            }
            // ��������� �������� ����������
            if (!set)   //��� ������� ���������� ������ ��� ���� ����� � ����� ����������
            {
                int resval;
                res = WaveAPI.waveOutGetVolume(hWaveOut, out resval);
                SetError(res);
                value = (ushort)resval;
            }

            return (float)value / ushort.MaxValue;
        }


        public static float SetDacVolume(IntPtr hWaveOut, float position)
        {
            return DacVolume(hWaveOut, true, position);
        }

        public static float GetDacVolume(IntPtr hWaveOut)
        {
            return DacVolume(hWaveOut, false, 0);
        }

        /// <summary>
        /// �������� ������ ���������.
        /// </summary>
        public static WaveAPI.WAVEOUTCAPS[] GetDevices()
        {
            //�������� ���������� ��������� � �������
            var numDevs = WaveAPI.waveOutGetNumDevs();
            var result = new WaveAPI.WAVEOUTCAPS[numDevs];
            for (var i = 0; i < numDevs; i++)
            {
                var woc = new WaveAPI.WAVEOUTCAPS();
                WaveAPI.waveOutGetDevCaps(i, ref woc, Marshal.SizeOf(woc));
                /*
                string text = "Mid= " + wic.wMid.ToString() +
                            Environment.NewLine + "Pid= " + wic.wPid.ToString() +
                            Environment.NewLine + "Version= " + wic.vDriverVersionMajor.ToString() + "." + wic.vDriverVersionMinor.ToString() +
                            Environment.NewLine + "Name= " + wic.szPname +
                            Environment.NewLine + "Format= " + wic.dwFormats.ToString() +
                            Environment.NewLine + "Channels= " + wic.wChannels.ToString();
                //MessageBox.Show(text);
                */
                result[i] = woc;
            }

            return result;
        }
    }
}
