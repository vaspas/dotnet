using System;

using System.Runtime.InteropServices;
using System.Threading;

namespace IncModules.Mio4400
{
    /// <summary>
    /// ����� ����� ������ � ����������.
    /// </summary>
    public sealed unsafe class AdcThread
    {
        public AdcThread()
        {
            _eventGcHandle = GCHandle.Alloc(new IntPtr(), GCHandleType.Pinned);
        }

        /// <summary>
        /// ����������.
        /// </summary>
        ~AdcThread()
        {
            _eventGcHandle.Free();
        }

        /// <summary>
        /// ������� ��������� ����� ������.
        /// </summary>
        public Action<int[]> OnDataReceived { get; set; }

        public ThreadPriority ThreadPriority { get; set; }

        #region ///// private fields /////

        /// <summary>
        /// ������� ����������� ����� ������.
        /// </summary>
        private readonly ManualResetEvent _dataReady = new ManualResetEvent(false);


        private GCHandle _eventGcHandle;

        /// <summary>
        /// ��������� �� ��������� ������ ��������.
        /// </summary>
        private int* _driverData;
        
        /// <summary>
        /// ������� ����������.
        /// </summary>
        private int _interruptsCounter;

        /// <summary>
        /// ������ � �������.
        /// </summary>
        private int[] _dataBuffer=new int[0];

        /// <summary>
        /// ������ ����� ������.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// ���-�� ������ ������ � ������.
        /// </summary>
        private int _blockCount;

        /// <summary>
        /// ���-�� ������� ������.
        /// </summary>
        public byte ChannelsCount { get; set; }

        /// <summary>
        /// ����� ����������.
        /// </summary>
        public int BoardNumber { get; set; }

        /// <summary>
        /// ���� ������ ������.
        /// </summary>
        private bool _threadTerminated = true;

        /// <summary>
        /// ����� ����� ������.
        /// </summary>
        private Thread _thread;

        #endregion

        #region ///// private methods /////

        /// <summary>
        /// ��������� ����� ���� �� ����������.
        /// </summary>
        private void StartThread()
        {
            //��������� ��� ����� ����������
            if (!_threadTerminated)
                throw new InvalidOperationException("����� ����� �� ����������!");

            //������� �����
            _thread = new Thread(ThreadFunc);
            _thread.Priority = ThreadPriority;
            _thread.IsBackground = true;   //����������� ������� �����
            _thread.Name = GetType().ToString();

            //��������� �����
            _threadTerminated = false;
            _thread.Start();
        }

        /// <summary>
        /// ������������� ����� ���� �� �������.
        /// </summary>
        private void StopThread()
        {
            //������������� ����� ���� �� �������
            if (_threadTerminated)
                return;

            _threadTerminated = true;
            //NOTE: ����� ���� ������ ������ ������ Abort
            if (_thread.Join(5000))
                _thread.Abort();
            

            _thread = null;
        }

        /// <summary>
        /// ������� ������ ����� ������.
        /// </summary>
        private void ThreadFunc()
        {
            //���� ����� �������� ����� �������
            while (!_threadTerminated && _dataReady.SafeWaitHandle.IsClosed)
                Thread.Sleep(10);

            while (!_threadTerminated)
            {
                //������� ����������� ����� ������
                if (!_dataReady.WaitOne(100, true))
                    continue;


                //�������� �������� �������� ���������� ����������
                var deviceInterrupsCounter = Wrapper.MioGetReadyBuf(BoardNumber);

                //���� ���� ������, ���������
                while (deviceInterrupsCounter - _interruptsCounter > 0)
                {
                    //������������ ����� �����
                    var readBlockNumber = _interruptsCounter%_blockCount;

                    //�������� ��������� �� ������ ����������
                    var dataPnt = new IntPtr(_driverData + BlockSize*ChannelsCount*readBlockNumber);
                    //�������� ������
                    Marshal.Copy(dataPnt, _dataBuffer, 0, BlockSize*ChannelsCount);
                    //���������� �������
                    OnDataReceived(_dataBuffer);

                    //����������� �������
                    _interruptsCounter++;
                    //��������� �������� �������� ���������� ����������
                    //deviceInterrupsCounter = Wrapper.MioGetReadyBuf(m_boardNumber);
                }

                //���������� �������
                Wrapper.MioResetIntr(_eventGcHandle.AddrOfPinnedObject(), BoardNumber);

            }
        }


        /// <summary>
        /// ������������ ���������� ������ � ������.
        /// </summary>
        /// <param name="blockSize">������ ����� � �������.</param>
        /// <param name="channelsCount">���������� �������.</param>
        /// <param name="boardNumber">����� ����������.</param>
        /// <returns>���-�� ������.</returns>
        private static int CalculateBlockCount(int blockSize, int channelsCount, int boardNumber)
        {
            //������������ ������ ����� � ������
            //int sizeBytes = blockSize * channelsCount * sizeof(int);

            //������������ ���-�� ������ � ������            
            //return Wrapper.MioGetNumBuff(sizeBytes, p_boardNumber);
            var bufsize = 0;
            IncAPI.GetSizeDevBuf(boardNumber, ref bufsize);
            return (bufsize - 4096)/sizeof (int)/blockSize/channelsCount;
        }


        #endregion
        
        #region ///// public methods /////

        /// <summary>
        /// ��������� ���� � ��������� ������.
        /// </summary>
        /// <returns>��� ������  <see cref="RunSborErrors"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">������ ����� ������ ����.</exception>
        public int Run()
        {
            //��������� ���-�� ������ � ������            
            _blockCount =CalculateBlockCount(BlockSize, ChannelsCount, BoardNumber);
            if (_blockCount <= 0)
                return (int)RunSborErrors.Failed;

            //���������� ������� ����������
            _interruptsCounter = 0;
            //�������� ����� ��� ������� ������
            if(_dataBuffer.Length!=BlockSize*ChannelsCount)
                _dataBuffer = new int[BlockSize * ChannelsCount];

            StartThread();
            
            var res = Wrapper.MioRunSbor(BlockSize, _blockCount, -1, ChannelsCount, 1, (1<<ChannelsCount)-1, out _driverData, _eventGcHandle.AddrOfPinnedObject(), BoardNumber);
            
            //������������� ����� �������
            _dataReady.SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle((IntPtr)_eventGcHandle.Target, false);
            //��� ������ ������������� �������)
            if (res != (int)RunSborErrors.Success)
                Stop();

            //���������� ���������
            return res;
        }

        /// <summary>
        /// ������������� ������� �����.
        /// </summary>
        public void Stop()
        {
            //������������� �����
            StopThread();

            //������������� ���� ������
            Wrapper.MioStopSbor(null, BoardNumber);
                        
            if(!_dataReady.SafeWaitHandle.IsClosed)
                _dataReady.SafeWaitHandle.Close();
        }

        #endregion

        
    }
}
