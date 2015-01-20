using System;
using System.Runtime.InteropServices;

namespace SoundBlasterModules.WaveApi.Input
{
    /// <summary>
    /// ����� ��� ������ � �����������.
    /// </summary>
    internal class SoundBlaster
    {
        /// <summary>
        /// ���-�� ������� SB.
        /// </summary>
        const int SB_BUFFERS_COUNT= 4;

        #region ///// private fields /////

        /// <summary>
        /// ����� ����������.
        /// </summary>
        private int _device;

        /// <summary>
        /// ������������� ����������.
        /// </summary>
        private IntPtr m_hwi;

        /// <summary>
        /// ������ ������� SB.
        /// </summary>
        private SBBuffer[] m_buffers;

        /// <summary>
        /// ������ ������.
        /// </summary>
        private short[] m_writearr=new short[0];

        /// <summary>
        /// Handle ��� ���� ����� ������� ������ �� ��������� �������
        /// (��������� �� ������� ��������� ������) � ������.
        /// </summary>
        private GCHandle m_wipdelHandle;

        /// <summary>
        /// ���� ���������� ���� ������ �� ������� API.
        /// </summary>
        //private int m_result;

        /// <summary>
        /// ��������� ����������.
        /// </summary>
        private bool m_started=false;

        /// <summary>
        /// ������ ������� ����� �������������� (�������� ������� ����� ����������).
        /// </summary>
        private WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] m_muxList=new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[0];

        /// <summary>
        /// ��������� ���������� ��������� ��������.
        /// </summary>
        private int m_lastResult;

        /// <summary>
        /// ���������� ���������.
        /// </summary>
        private void ResetResult()
        {
            m_lastResult = WaveAPI.NOERROR;
        }

        /// <summary>
        /// ������������� ���������, �� ������ � ������ ������.
        /// </summary>
        /// <returns>true ���� ������ �����������</returns>
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
        /// ���������� ��������� ���������� ��������� ��������.
        /// </summary>
        public int LastResult
        {
            get { return m_lastResult; }
        }

        #endregion

        #region ///// private methods /////
		
        /// <summary>
        /// ������� ��� ������/��������� ������.
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

                //�������� ������� ��������� ����� ������);
                if (NewDataReceived != null)
                    NewDataReceived();

                //��������� ����� ��� ������
                var h = (GCHandle)wavhdr.dwUser;
                var sb_buf = (SBBuffer)h.Target;
                lock (sb_buf)
                {
                    /*m_result =*/ WaveAPI.waveInAddBuffer(m_hwi, sb_buf.WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(wavhdr));
                }
            }
        }

        /// <summary>
        /// ������ ���������� ��� ������ ������.
        /// </summary>
        /// <param name="p_deviceNumber">����� ����������.</param>
        /// <param name="p_wipdel">������� callback �������.</param> 
        private void WaveInStart(int p_deviceNumber, WaveAPI.WaveProcDelegate p_wipdel)
        {
            ResetResult();
            int res = WaveAPI.NOERROR;

            //������� � �������������� ���������
            WaveAPI.WAVEFORMATEX wfex;
            wfex.wFormatTag = (short)WaveAPI.WAVE_FORMAT_PCM;
            wfex.nSamplesPerSec = Frequency;
            wfex.nAvgBytesPerSec = (wfex.nSamplesPerSec * sizeof(short) * ChannelsCount); // !
            wfex.nChannels = (short)ChannelsCount;
            wfex.nBlockAlign = (short)(sizeof(short) * ChannelsCount);
            wfex.wBitsPerSample = (short)(8 * sizeof(short));
            wfex.cbSize = 0;

            //�������� �����
            m_hwi = IntPtr.Zero;

            //��������� ������� � ������
            if(p_wipdel!=null)
                m_wipdelHandle = GCHandle.Alloc(p_wipdel);

            //��������� ����������, � �������� ��� �����
            res = WaveAPI.waveInOpen(out m_hwi, p_deviceNumber/*WaveInAPI.WAVE_MAPPER*/, ref wfex, p_wipdel, 0, WaveAPI.CALLBACK_FUNCTION);
            if (SetError(res))
                return;

            //�������� ������ ������ ���� ����� ��������� ������
            if (p_wipdel != null)
            {
                res = AllocateBuffers();
                if (SetError(res))
                    return;
            }

            //��������� ����������
            res = WaveAPI.waveInStart(m_hwi);
            if (SetError(res))
                return;

            m_started = true;
        }

        /// <summary>
        /// �������� �������� ������.
        /// </summary>
        /// <returns>������.</returns>
        private int AllocateBuffers()
        {
            int res = WaveAPI.NOERROR;

            //�������� ������, � ��������� �� ����������,
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
        /// ��������� ����������.
        /// </summary>
        private void WaveInStop()
        {
            //���������� ����
            m_started = false;

            //������������� ����������
            WaveAPI.waveInStop(m_hwi);
            //����������
            WaveAPI.waveInReset(m_hwi);

            //����������� ���������� ������
            if (m_buffers != null)
            {                
                for (var i = 0; i < SB_BUFFERS_COUNT; i++)
                {
                    WaveAPI.waveInUnprepareHeader(m_hwi, m_buffers[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(m_buffers[i].WaveHdr));
                    
                    m_buffers[i].Free();
                }
                m_buffers = null;
            }
            //��������� ����������
            WaveAPI.waveInClose(m_hwi);
            //�������� ���� ����������
            m_hwi = IntPtr.Zero;
            //����������� ����� ��������
            if (m_wipdelHandle.IsAllocated)
                m_wipdelHandle.Free();

        }
      
        /// <summary>
        /// ������������� ����� ������� ����� ����������.
        /// ���������� ������ ��� ���������� ����������.
        /// </summary>
        /// <param name="p_volValue">������� ��������� ������������� ����� �� 0 �� 1.</param>
        private void SetLineInternal(out float p_volValue)
        {
            ResetResult();
            int res = WaveAPI.NOERROR;

            //Handle for mixer
            IntPtr hmx = IntPtr.Zero;     
            p_volValue = 0;

            //��������� ������
            res = WaveAPI.mixerOpen(out hmx, (uint)m_hwi, IntPtr.Zero, IntPtr.Zero, WaveAPI.MIXER_OBJECTF_HWAVEIN);
            if (SetError(res))
                return;

            try
            {
                //��������� ��� ����� ����
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
            //��������� ������
            finally
            {
               res= WaveAPI.mixerClose(hmx);
               SetError(WaveIn.LastResult);
                   
            }
        }

        /// <summary>
        /// ������������� ������� ���������.
        /// ���������� ������ ��� ���������� ����������.
        /// </summary>
        /// <param name="p_level">������� �� 0 �� 1.</param>
        private void SetVolumeInternal(ref float p_level)
        {
            ResetResult();
            int res = WaveAPI.NOERROR;

            //Handle for mixer
            var hmx = IntPtr.Zero;     

            //��������� ������
            res = WaveAPI.mixerOpen(out hmx, (uint)m_hwi, IntPtr.Zero, IntPtr.Zero, WaveAPI.MIXER_OBJECTF_HWAVEIN);
            if (SetError(res))
                return;

            try
            {
                //������������� �����
                //WaveIn.SetLineType(hmx, LineID);
                //if (res != WaveAPI.NOERROR)
                //    return res;
                //�������� ������� ���������
                p_level = WaveIn.SetADCVolume(hmx, p_level, LineID);
                if (SetError(WaveIn.LastResult))
                    return;
            }
            finally
            {
                //��������� ������
                res = WaveAPI.mixerClose(hmx);
                SetError(res);
            }
        }

        /// <summary>
        /// ���������� ������ ������� ����� ��������������. ���������� ������ ��� ���������� ����������.
        /// </summary>
        /// <returns>������</returns>
        private WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] GetMixerListInternal()
        {
            ResetResult();
            int res = WaveAPI.NOERROR;

            var result = new WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[0];

            //Handle for mixer
            IntPtr hmx = IntPtr.Zero;     //Handle for mixer

            //��������� ������
            res = WaveAPI.mixerOpen(out hmx, (uint)m_hwi, IntPtr.Zero, IntPtr.Zero, WaveAPI.MIXER_OBJECTF_HWAVEIN);
            if (SetError(res))
                return result;
            try
            {
                WaveAPI.MIXERLINE mxlWI = WaveIn.GetWaveInLine(hmx);
                if (SetError(WaveIn.LastResult))
                    return result;
                WaveAPI.MIXERCONTROL mxcMux = WaveIn.GetMixerControl(hmx, mxlWI, WaveAPI.ControlType.MIXERCONTROL_CONTROLTYPE_MUX);
                //� ������ ���������� ��������������
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
                //��������� ������
                res=WaveAPI.mixerClose(hmx);
                SetError(res);
            }


            return result;
        }

        /// <summary>
        /// ���������� ������ ������� ����� ��������������.
        /// </summary>
        /// <returns>������</returns>
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
        /// �������� ���������� ������. ��������� ������ ������ ������ � �������� ������.
        /// </summary>
        private void CheckLock()
        {
            if (m_started)
                throw new MemberAccessException("Data was locked.");
        }
        
	    #endregion

        #region ///// public methods /////

        /// <summary>
        /// ��������� ���������� ��� ������ ������.
        /// </summary>
        public void Start()
        {
            //������� ������ ��� ������
            int length = BlockSize * ChannelsCount;
            if(m_writearr.Length!=length)
                m_writearr = new short[length];

            //���������
            WaveInStart(_device, WaveInProc);            
        }

        /// <summary>
        /// ������������� ����������.
        /// </summary>
        public void Stop()
        {
            WaveInStop();
        }

        /// <summary>
        /// ������������� ������� ��������� ��� ��������� �����.
        /// </summary>
        /// <param name="p_level">������� �� 0 �� 1.</param>
        /// <returns>������</returns>
        public void SetVolume(float p_level)
        {
            bool doStop = !m_started;
            if (!m_started)
                WaveInStart(_device, null);
            try
            {
                if (m_lastResult != WaveAPI.NOERROR)
                    return;

                //������������� �������
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
        /// ������������� ������� �����
        /// </summary>
        /// <param name="p_lineNumber">���������� ����� �����.</param>    
        public void SetLine(int p_lineNumber)
        {
            //���������� ����� �����
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
        /// ���������� � ������������� ����� ����������.
        /// </summary>
        /// <exception cref="MemberAccessException">��� ��������� � �������� ������.</exception>
        public int DeviceNumber
        {
            get { return _device; }
            set 
            {
                //��������� ������ � �������� ������
                CheckLock();

                //������������� ����� ����������
                _device = value;

                //��������� ������ �����
                m_muxList = GetMixerList();
                if (m_lastResult != WaveAPI.NOERROR)
                    return;
                //������������� �����
                if(LineNumber>=m_muxList.Length)
                    SetLine(m_muxList.Length-1);
                else
                    SetLine(LineNumber);
            }
        }

        /// <summary>
        /// ���������� �������� ����������.
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
        /// ���������� ���������� ����� �����.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// ���������� ������������� �����.
        /// </summary>
        public int LineID
        {
            get
            {
                return LineDetails.dwParam1;
            }
        }

        /// <summary>
        /// ���������� ��������� ��������� �����.
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
        /// ���������� � ������������� ���-�� �������. ����� ��������� ��������� ����������.
        /// </summary>
        public int ChannelsCount { get; set; }

        /// <summary>
        /// ���������� � ������������� ������� �����������. ����� ��������� ��������� ����������.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// ���������� � ������������� ������ �����. ����� ��������� ��������� ����������.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// ���������� ������ �� ������ �������� ������.
        /// </summary>
        public short[] DataArray
        {
            get { return m_writearr; }
        }

        /// <summary>
        /// ���������� ������ ������� ����� ��������������.
        /// </summary>
        public WaveAPI.MIXERCONTROLDETAILS_LISTTEXT[] LinesList
        {
            get { return m_muxList; }
        }

        /// <summary>
        /// ���������� ������� ���������.
        /// </summary>
        public float Volume { get; private set; }

        #endregion

        /// <summary>
        /// ������� ��������� ����� ������.
        /// </summary>
        public event Action NewDataReceived;
    }
}
