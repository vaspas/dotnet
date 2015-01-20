using System;
using System.Runtime.InteropServices;

namespace SoundBlasterModules.WaveApi.Output
{
    class SoundBlaster
    {
        #region ///// private const /////

        /// <summary>
        /// ���-�� ������� SB.
        /// </summary>
        private const int SBBUFFERSCOUNT = 4;
        
        #endregion

        #region ///// private fields /////

        /// <summary>
        /// ����� ���������� ����������.
        /// </summary>
        private int _deviceNumber;


        /// <summary>
        /// ������ ����� ������.
        /// </summary>
        private int _blockSize = 1024;

        /// <summary>
        /// ������������� ����������.
        /// </summary>
        private IntPtr _hwo;

        /// <summary>
        /// ��������� WaveFormatEx.
        /// </summary>
        private WaveAPI.WAVEFORMATEX _wfex;

        /// <summary>
        /// ������ ������� SB.
        /// </summary>
        private SBBuffer[] _buffersSb;

        /// <summary>
        /// Handle ��� ���� ����� ������� ������ �� ��������� ��� �������(��������� �� �������) � ������.
        /// </summary>
        private GCHandle _wopdelHandle;

        /// <summary>
        /// ������ ��� ��������.
        /// </summary>
        private Array _data;

        private readonly object _sync=new object();

        /// <summary>
        /// ��������� ����������.
        /// </summary>
        private bool _started;


        /// <summary>
        /// ������� ��������� �� 0 �� 1.
        /// </summary>
        private float _volume;

        #endregion
                
        #region ///// public properties /////

        /// <summary>
        /// ���������� � ������������� ����� ����������.
        /// </summary>
        /// <exception cref="MemberAccessException">��� ��������� � �������� ������.</exception>
        public int DeviceNumber
        {
            get { return _deviceNumber; }
            set 
            {
                //��������� ������ � �������� ������
                CheckLock();

                if (value < 0)
                    return;
                
                //���������� ����� ����������
                _deviceNumber = value;

                //������������� ������� ���������
                _volume=0;
                GetVolumeLevel();                
            }
        }

        /// <summary>
        /// ���������� � ������������� ������� �����������.
        /// ����� ��������� ��������� ����������.
        /// </summary>
        public double Frequency { get; set; }

        /// <summary>
        /// ���������� � ������������� ���� ���� ������ ��� ������.        
        /// ����� ��������� ��������� ����������.
        /// </summary>
        public bool FloatType { get; set; }

        /// <summary>
        /// ���������� � ������������� ������ ����� ������ ������. 
        /// ����� ��������� ��������� ����������.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// ���������� ������� ���������.
        /// </summary>
        public float Volume
        {
            get { return _volume; }
        }

        public int ChannelsCount { get; set; }

        #endregion

        #region ///// private methods /////

        /// <summary>
        /// ������ ���������� ��� ������ ������, ��� ������ ��������.
        /// </summary>
        /// <param name="wopdel">������� callback ������� ��� ������ ��� null ��� ��������.</param>
        /// <param name="deviceNumber">����� ����������.</param>
        private int WaveOutOpen(int deviceNumber, WaveAPI.WaveProcDelegate wopdel)
        {
            //������ ���� ������
            var dataTypeSize = FloatType ? sizeof(float) : sizeof(short);

            //�������������� ��������� WaveFormatEx
            _wfex.wFormatTag = FloatType ? (short)WaveAPI.WAVE_FORMAT_FLOAT : (short)WaveAPI.WAVE_FORMAT_PCM;
            _wfex.nSamplesPerSec = (int) (Frequency);// + 0.49);
            _wfex.nAvgBytesPerSec = (_wfex.nSamplesPerSec * dataTypeSize * ChannelsCount); // !
            _wfex.nChannels = (short)ChannelsCount;
            _wfex.nBlockAlign = (short)(dataTypeSize * ChannelsCount);
            _wfex.wBitsPerSample = (short)(8 * dataTypeSize);
            _wfex.cbSize = 0;

            _hwo = IntPtr.Zero;

            //������� ����� ��� ��������
            if (wopdel != null)
                _wopdelHandle = GCHandle.Alloc(wopdel);

            //��������� ����������
            var res = WaveAPI.waveOutOpen(out _hwo, deviceNumber/*WaveInAPI.WAVE_MAPPER*/, ref _wfex, wopdel, 0, WaveAPI.CALLBACK_FUNCTION);
            if (res != WaveAPI.NOERROR)
                return res;

            //���� ��� �������� callback �������, ������ ����� ���������� ������,
            //���������� ������
            if (wopdel != null)
            {
                //�������� ������ ��������
                _buffersSb = new SBBuffer[SBBUFFERSCOUNT];  //������� ������ �������
                for (var i = 0; i < SBBUFFERSCOUNT; i++)
                {
                    _buffersSb[i] = new SBBuffer();  //������� �����
                    _buffersSb[i].Alloc(_blockSize * ChannelsCount, FloatType);  //�������� ������
                    //�������������� �����
                    res = WaveAPI.waveOutPrepareHeader(_hwo, _buffersSb[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(_buffersSb[i].WaveHdr));
                    if (res != WaveAPI.NOERROR)
                        return res;
                    //���������� ������ ����� ��� ������� ��������
                    res = WaveAPI.waveOutWrite(_hwo, _buffersSb[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(_buffersSb[i].WaveHdr));
                    if (res != WaveAPI.NOERROR)
                        return res;
                }
            }

            //������������� ���� ������� ���� ��� ������
            if (res == WaveAPI.NOERROR)
                _started = true;

            return WaveAPI.NOERROR;
        }

        /// <summary>
        /// ��������� ���������� ��� �������� ������.
        /// </summary>
        private void WaveOutClose()
        {
            //���������� ����
            _started = false;
            
            WaveAPI.waveOutReset(_hwo);

            if (_buffersSb != null)
            {
                //����������� ���������� ������
                for (int i = 0; i < SBBUFFERSCOUNT; i++)
                {
                    WaveAPI.waveOutUnprepareHeader(_hwo, _buffersSb[i].WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(_buffersSb[i].WaveHdr));
                    _buffersSb[i].Free();
                }
                _buffersSb = null;
            }

            //��������� ����������
            WaveAPI.waveOutClose(_hwo);
            _hwo = IntPtr.Zero;
            //����������� ����� ��������
            if (_wopdelHandle.IsAllocated)
                _wopdelHandle.Free();

        }

        /// <summary>
        /// ������� ���������� ����������� ����� ������ ���������� ����� ������.
        /// </summary>
        /// <param name="hdrvr"></param>
        /// <param name="uMsg"></param>
        /// <param name="dwUser"></param>
        /// <param name="wavhdr"></param>
        /// <param name="dwParam2"></param>
        private void WaveOutProc(IntPtr hdrvr, int uMsg, int dwUser, ref WaveAPI.WAVEHDR wavhdr, int dwParam2)
        {
            //��������� ������� ������ ���� ��������� ��������� ��� ����� ���������
            if (uMsg != WaveAPI.WindowMessages.MM_WOM_DONE || !_started)
                return;

            //���� ������
            //���������� � ����� ������ �� ������� ��� �������� ���, ���� ������ � ������� ���
            lock (_sync)
                if (_data != null)
                {
                    lock (_data)
                    {
                        //���������� � ����� ������                            
                        if (FloatType)
                            Marshal.Copy((float[]) _data, 0, wavhdr.lpData, BlockSize);
                        else
                            Marshal.Copy((short[]) _data, 0, wavhdr.lpData, BlockSize);
                    }
                    //���������� ������
                    _data = null;
                }

            //�������� ������� ������� ������
            DataRequered();

            var gch = (GCHandle)wavhdr.dwUser;
            var sbbuf = (SBBuffer)gch.Target;
            //�������� ���������� ����������� ������ ������� �����
            WaveAPI.waveOutWrite(_hwo, sbbuf.WaveHdrHdl.AddrOfPinnedObject(), Marshal.SizeOf(wavhdr));
        }

        /// <summary>
        /// ������� ���������� ������� ��������� �� ���������� ��� SB.
        /// </summary>
        private int GetVolumeLevel()
        {
            var res = WaveAPI.NOERROR;

            var doStop = !_started;
            if (!_started)
                res = WaveOutOpen(_deviceNumber, null);
            try
            {
                if (res != WaveAPI.NOERROR)
                    return res;

                var volume = WaveOut.GetDacVolume(_hwo);
                if (WaveOut.LastResult != WaveAPI.NOERROR)
                    return WaveOut.LastResult;

                _volume = volume;
            }
            finally
            {
                if (doStop)
                    WaveOutClose();
            }

            return WaveAPI.NOERROR;
        }

        /// <summary>
        /// �������� ���������� ������. ��������� ������ ������ ������ � �������� ������.
        /// </summary>
        private void CheckLock()
        {
            if (_started)
                throw new MemberAccessException("Data was locked.");
        }
                
        #endregion


        /// <summary>
        /// ��������� ���������� ��� ������ ������.
        /// </summary>
        /// <returns>������</returns>
        public int Start()
        {
            //������� �������
            _data=null;

            //���������
            return WaveOutOpen(_deviceNumber, WaveOutProc);
        }

        /// <summary>
        /// ������������� ����������.
        /// </summary>
        public void Stop()
        {
            WaveOutClose();
        }

        /// <summary>
        /// ������� ������������� ������� ���������.
        /// </summary>
        public int SetVolume(float level)
        {
            int res = WaveAPI.NOERROR;

            bool doStop = !_started;
            if (!_started)
                res = WaveOutOpen( _deviceNumber,null);
            try
            {
                if (res != WaveAPI.NOERROR)
                    return res;

                float volume = WaveOut.SetDacVolume(_hwo, level);
                if (WaveOut.LastResult != WaveAPI.NOERROR)
                    return WaveOut.LastResult;

                _volume = volume;
            }
            finally
            {
                if (doStop)
                    WaveOutClose();
            }

            return WaveAPI.NOERROR;
        }

        /// <summary>
        /// ��������� ����� ������ � �������.
        /// </summary>
        /// <param name="data">������ �� ������.</param>
        /// <returns>True ���� ������ ��������� � �������.</returns>
        public void SetData(Array data)
        {
            lock (_sync)
            {
                //��������� ��������
                //...������ �� ����
                if (data == null)
                    throw new ArgumentNullException();
                //...��������� ��� �������
                if (!(data is float[]) && !(data is short[]))
                    throw new ArgumentException("Invalid data type");
                if (data is float[] && !FloatType)
                    throw new ArgumentException("Invalid data type");
                //...��������� ������
                if (data.Length != BlockSize)
                    throw new ArgumentOutOfRangeException();

                //������������� ������
                _data = data;
            }
        }


        /// <summary>
        /// ������� ��������� ��� �������� ������� ����� ������.
        /// </summary>
        public event Action DataRequered=delegate {};
    }
}
