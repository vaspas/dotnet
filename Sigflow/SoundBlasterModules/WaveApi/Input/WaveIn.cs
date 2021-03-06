#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace SoundBlasterModules.WaveApi.Input
{
    /// <summary>
    /// Предоставляет набор функций для работы со звуковой картой.
    /// </summary>
    internal static class WaveIn
    {
        /// <summary>
        /// Результат выполнения последней операции.
        /// </summary>
        private static int m_lastResult;

        /// <summary>
        /// Сбрасываем результат.
        /// </summary>
        private static void ResetResult()
        {
            m_lastResult = WaveAPI.NOERROR;
        }

        /// <summary>
        /// Устанавливает результат, но только в случае ошибки.
        /// </summary>
        /// <returns>true если ошибка установлена</returns>
        private static bool SetError(int error)
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
        public static int LastResult
        {
            get { return m_lastResult; }
        }


        /// <summary>
        /// Возвращает уровень.
        /// </summary>
        /// <param name="hmx"></param>
        /// <param name="set"></param>
        /// <param name="position"></param>
        /// <param name="LineID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static float adc_volume(IntPtr hmx, bool set, float position, int LineID)
        {
            //сбрасываем результат
            ResetResult();
            //результат выполнения функций
            int res=0;

            if (hmx == IntPtr.Zero) 
                throw new ArgumentNullException("hmx");

            WaveAPI.MIXERLINE mxl;
            if(LineID!=0)
                mxl=GetComponentLine(hmx, LineID);
            else
                mxl = GetWaveInLine(hmx);
            if (m_lastResult != WaveAPI.NOERROR)
                return 0;
            WaveAPI.MIXERCONTROL mxc = GetMixerControl(hmx, mxl, WaveAPI.ControlType.MIXERCONTROL_CONTROLTYPE_VOLUME);
            if (m_lastResult != WaveAPI.NOERROR)
                return 0;
            
            if (set)
            {
                if (position < 0) throw new ArgumentOutOfRangeException("position", "must be >=0");
                if (position >1) throw new ArgumentOutOfRangeException("position", "must be <=1");
            }
            WaveAPI.MIXERCONTROLDETAILS_UNSIGNED MuxUnsigned = new WaveAPI.MIXERCONTROLDETAILS_UNSIGNED();
            if (set)
                MuxUnsigned.dwValue = (uint)((mxc.dwMaximum - mxc.dwMinimum)*position);

            WaveAPI.MIXERCONTROLDETAILS mxcd = new WaveAPI.MIXERCONTROLDETAILS(); ;
            mxcd.cbStruct = Marshal.SizeOf(mxcd);
            mxcd.dwControlID = mxc.dwControlID;
            mxcd.cChannels = 1;
            mxcd.Item = mxc.cMultipleItems;
            mxcd.cbDetails = Marshal.SizeOf(typeof(WaveAPI.MIXERCONTROLDETAILS_UNSIGNED)); ;
            mxcd.paDetails = Marshal.AllocHGlobal(mxcd.cbDetails);//volHandle.AddrOfPinnedObject();

            Marshal.StructureToPtr(MuxUnsigned, mxcd.paDetails, true);
            // установить заданное значение если нужно
            if (set)
            {
                res = WaveAPI.mixerSetControlDetails(hmx, ref mxcd, WaveAPI.MIXER_SETCONTROLDETAILSF_VALUE);
                SetError(res);
            }
            else
            {
                // прочитать значение регулятора
                res = WaveAPI.mixerGetControlDetails(hmx, ref mxcd, WaveAPI.MIXER_GETCONTROLDETAILSF_VALUE);
                SetError(res);
            }
            Marshal.PtrToStructure(mxcd.paDetails, MuxUnsigned);

            return ((float)(MuxUnsigned.dwValue - mxc.dwMinimum) / (mxc.dwMaximum - mxc.dwMinimum));
        }

        public static float SetADCVolume(IntPtr hmx, float position, int LineID)
        {
            return adc_volume(hmx, true, position, LineID);
        }

        public static float GetADCVolume(IntPtr hmx, int LineID)
        {
            return adc_volume(hmx, false, 0, LineID);
        }

        /// <summary>
        /// Возвращает структуру MIXERLINE для WaveIn.
        /// </summary>
        /// <param name="hmx">Хэндл.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static WaveAPI.MIXERLINE GetWaveInLine(IntPtr hmx)
        {
            //сбрасываем результат
            ResetResult();

            if (hmx == IntPtr.Zero) 
                throw new ArgumentNullException("hmx");
            
            WaveAPI.MIXERLINE mxl = new WaveAPI.MIXERLINE();
            mxl.cbStruct = Marshal.SizeOf(mxl);
            mxl.dwComponentType = WaveAPI.ComponentType.MIXERLINE_COMPONENTTYPE_DST_WAVEIN;
            // определим линию входа АЦП
            int res = WaveAPI.mixerGetLineInfo(hmx, ref mxl, WaveAPI.MIXER_GETLINEINFOF_COMPONENTTYPE);
            SetError(res);

            return mxl;
        }

        /// <summary>
        /// Возвращает структуру MIXERLINE для компонента.
        /// </summary>
        /// <param name="hmx">Хэндл.</param>
        /// <param name="LineID">Тип компонента.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static WaveAPI.MIXERLINE GetComponentLine(IntPtr hmx, int LineID)
        {
            //сбрасываем результат
            ResetResult();
            
            if (hmx == IntPtr.Zero) 
                throw new ArgumentNullException("hmx");

            WaveAPI.MIXERLINE mxl = new WaveAPI.MIXERLINE();
            mxl = GetWaveInLine(hmx);
            //проверяем результат
            if (m_lastResult != WaveAPI.NOERROR)
                return mxl;

            int conn = mxl.cConnections;
            for (int Source = 0; Source < conn; Source++)
            {
                mxl.dwSource = Source;
                int res = WaveAPI.mixerGetLineInfo(hmx, ref mxl, WaveAPI.MIXER_GETLINEINFOF_SOURCE);
                //устанавливаем ошибку
                //если ошибка произошла то выходим
                if (SetError(res))
                    break;
                //if (mxl.dwComponentType == ComponentType_src)
                if(mxl.dwLineID==LineID)// нашли нужную линию                
                    break;                
            }

            return mxl;
        }

        /// <summary>
        /// Возвращает структуру MIXERCONTROL для линии заданной структурой MIXERLINE.
        /// </summary>
        /// <param name="hmx">Хэндл.</param>
        /// <param name="mxl">Линия заданная структурой MIXERLINE.</param>
        /// <param name="ControlType">Тип контрола.</param>
        /// <returns>Структура MIXERCONTROL.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static WaveAPI.MIXERCONTROL GetMixerControl(IntPtr hmx, WaveAPI.MIXERLINE mxl, int ControlType)
        {
            //сбрасываем результат
            ResetResult();

            if (hmx == IntPtr.Zero) 
                throw new ArgumentNullException("hmx");

            WaveAPI.MIXERLINECONTROLS mxlc = new WaveAPI.MIXERLINECONTROLS();
            
            mxlc.cbStruct = Marshal.SizeOf(mxlc);
            mxlc.dwLineID = mxl.dwLineID; // выбранная входная линия
            mxlc.cControls = mxl.cControls;
            mxlc.cbmxctrl = Marshal.SizeOf(typeof(WaveAPI.MIXERCONTROL));
            mxlc.pamxctrl = Marshal.AllocHGlobal(mxlc.cbmxctrl);// mxcHandle.AddrOfPinnedObject();// &mxc;            
            mxlc.dwControlType = ControlType;                        
            int res = WaveAPI.mixerGetLineControls(hmx, ref mxlc, WaveAPI.MIXER_GETLINECONTROLSF_ONEBYTYPE);
            SetError(res);

            WaveAPI.MIXERCONTROL mxc = new WaveAPI.MIXERCONTROL();
            Marshal.PtrToStructure(mxlc.pamxctrl, (object)mxc);
            Marshal.FreeHGlobal(mxlc.pamxctrl);



            return mxc;

        }
  
        /// <summary>
        /// Устанавливает новый тип линии
        /// </summary>
        /// <param name="hmx">Хэндл.</param>
        /// <param name="LineID">Тип линии.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetLineType(IntPtr hmx, int LineID)
        {
            //сбрасываем результат
            ResetResult();

            if (hmx == IntPtr.Zero) 
                throw new ArgumentNullException("hmx");

            WaveAPI.MIXERLINE mxlWI;
            mxlWI = GetWaveInLine(hmx);
            if (m_lastResult != WaveAPI.NOERROR)
                return;

            WaveAPI.MIXERCONTROL mxcMux = GetMixerControl(hmx, mxlWI, WaveAPI.ControlType.MIXERCONTROL_CONTROLTYPE_MUX);
            if (m_lastResult != WaveAPI.NOERROR)
                return;

            WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] MuxList;
            MuxList = GetControlDetailsListText(hmx, mxcMux);
            if (m_lastResult != WaveAPI.NOERROR)
                return;
            

            int[] MuxVal = new int[MuxList.Length];
            GCHandle MuxValHandle = GCHandle.Alloc(MuxVal, GCHandleType.Pinned);
            // установить флаг выбора требуемой линии
            for (int i = 0; i < MuxList.Length; i++)
                MuxVal[i] = MuxList[i].dwParam1 == LineID ? 1 : 0;
            // установка входных линий
            WaveAPI.MIXERCONTROLDETAILS mxcd = new WaveAPI.MIXERCONTROLDETAILS(); ;
            mxcd.cbStruct = Marshal.SizeOf(mxcd);
            mxcd.dwControlID = mxcMux.dwControlID;
            mxcd.Item = mxcMux.cMultipleItems;
            mxcd.cChannels = 1;
            mxcd.cbDetails = 4; //size of int
            mxcd.paDetails = MuxValHandle.AddrOfPinnedObject();// MuxVal;
            int res = WaveAPI.mixerSetControlDetails(hmx, ref mxcd, WaveAPI.MIXER_SETCONTROLDETAILSF_VALUE);
            SetError(res);
            MuxValHandle.Free();            
        }

        /// <summary>
        /// Возвращает список входных линий мультиплексора.
        /// </summary>
        /// <param name="hmx"></param>
        /// <param name="mxc"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] GetControlDetailsListText(IntPtr hmx, WaveAPI.MIXERCONTROL mxc)
        {
            //сбрасываем результат
            ResetResult();

            if (hmx == IntPtr.Zero) 
                throw new ArgumentNullException("hmx");

            if (mxc == null) 
                throw new ArgumentNullException("hxc");

            WaveAPI.MIXERCONTROLDETAILS mxcd = new WaveAPI.MIXERCONTROLDETAILS(); ;
            mxcd.cbStruct = Marshal.SizeOf(mxcd);
            mxcd.dwControlID = mxc.dwControlID;
            mxcd.cChannels = 1;
            int mi = 1;
            // получить список входных линий мультиплексора
            mi = mxc.cMultipleItems > 64 ? 64 : mxc.cMultipleItems;
            if (mi < 0) 
                mi = 0;
            mxcd.Item = mi;
            mxcd.cbDetails = Marshal.SizeOf(typeof(WaveAPI.MIXERCONTROLDETAILS_LISTTEXT));
            mxcd.paDetails = Marshal.AllocHGlobal(mxcd.cbDetails * mi);// MuxListHandle.AddrOfPinnedObject();// MuxList;                
            int res = WaveAPI.mixerGetControlDetails(hmx, ref mxcd, WaveAPI.MIXER_GETCONTROLDETAILSF_LISTTEXT);
            if(SetError(res))
                return new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[0];

            WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] MuxList = new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[mi];
            for (int i = 0; i < mi; i++)
                MuxList[i] = new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT();
            for (int i = 0; i < mi; i++)
                Marshal.PtrToStructure((IntPtr)(mxcd.paDetails.ToInt32() + i * Marshal.SizeOf(typeof(WaveAPI.MIXERCONTROLDETAILS_LISTTEXT))), (object)MuxList[i]);

            Marshal.FreeHGlobal(mxcd.paDetails);

            return MuxList;
        }

        /// <summary>
        /// Возвращает список устройств.
        /// </summary>
        public static WaveAPI.WAVEINCAPS[] GetDevices()
        {
            int numDevs = WaveAPI.waveInGetNumDevs();
            WaveAPI.WAVEINCAPS[] result = new WaveAPI.WAVEINCAPS[numDevs];
            for (int i = 0; i < numDevs; i++)
            {
                WaveAPI.WAVEINCAPS wic = new WaveAPI.WAVEINCAPS();
                WaveAPI.waveInGetDevCaps(i, ref wic, Marshal.SizeOf(wic));
                /*
                string text = "Mid= " + wic.wMid.ToString() +
                            Environment.NewLine + "Pid= " + wic.wPid.ToString() +
                            Environment.NewLine + "Version= " + wic.vDriverVersionMajor.ToString() + "." + wic.vDriverVersionMinor.ToString() +
                            Environment.NewLine + "Name= " + wic.szPname +
                            Environment.NewLine + "Format= " + wic.dwFormats.ToString() +
                            Environment.NewLine + "Channels= " + wic.wChannels.ToString();
                //MessageBox.Show(text);
                */
                result[i] = wic;
            }

            return result;
        }

    }
}
