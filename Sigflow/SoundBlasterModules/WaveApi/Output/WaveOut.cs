using System;

using System.Runtime.InteropServices;

namespace SoundBlasterModules.WaveApi.Output
{
    /// <summary>
    /// ѕредоставл€ет набор функций дл€ работы со звуковой картой.
    /// </summary>
    internal static class WaveOut
    {
        /// <summary>
        /// —брасываем результат.
        /// </summary>
        private static void ResetResult()
        {
            LastResult = WaveAPI.NOERROR;
        }

        /// <summary>
        /// ”станавливает результат, но только в случае ошибки.
        /// </summary>
        /// <returns>true если ошибка установлена</returns>
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
        /// ¬озвращает результат выполнени€ последней операции.
        /// </summary>
        public static int LastResult { get; private set; }

        /// <summary>
        /// ¬озвращает уровень звука.
        /// </summary>
        /// <param name="hWaveOut"></param>
        /// <param name="set">True означает что будет установлен уровень.</param>
        /// <param name="position">«начение уровн€ дл€ установки начина€ с 0.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">≈сли при установке уровн€ значение 
        /// позиции меньше 0 или больше/равно количеству позиций.</exception>
        private static float DacVolume(IntPtr hWaveOut, bool set, float position)
        {
            //сбрасываем результат
            ResetResult();

            //результат выполнени€ функций
            int res;

            //провер€ем аргументы 
            if (hWaveOut == IntPtr.Zero)
                throw new ArgumentNullException("hWaveOut");

            if (set)
            {
                if (position < 0) throw new ArgumentOutOfRangeException("position", "must be >=0");
                if (position > 1) throw new ArgumentOutOfRangeException("position", "must be <=1");
            }

            //устанавливаемое значение
            var value = (ushort)(ushort.MaxValue * position);

            int setv = value;
            setv = setv << 16;
            setv = setv + value;
            // установить заданное значение если нужно
            if (set)
            {
                res = WaveAPI.waveOutSetVolume(hWaveOut, setv);
                SetError(res);
            }
            // прочитать значение регул€тора
            if (!set)   //это условие поставлено только дл€ того чтобы в висте заработало
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
        /// «агрузка списка устройств.
        /// </summary>
        public static WaveAPI.WAVEOUTCAPS[] GetDevices()
        {
            //получаем количество устройств в системе
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
