using System;
using IppWrapper;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.SignalResampling
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public unsafe class SignalResamplingModule : IExecuteModule, IDisposable
    {
        /// <summary>
        /// ����������.
        /// </summary>
        ~SignalResamplingModule()
        {
            Free();
        }


        private ipp.IppWinType _actialWinType;

        /// <summary>
        /// ���������� � ������������� ��� ����.
        /// </summary>
        public ipp.IppWinType WinType { get; set; }

        private int _actialFilterTapsLen;
        public int FilterTapsLen { get; set; }

        private int _actialDownFactor;
        private int _downFactor;
        public int DownFactor
        {
            get { return _downFactor; }
            set
            {
                if (value == _downFactor)
                    return;

                if(value<2)
                    throw new ArgumentOutOfRangeException();
                
                _downFactor = value;
            }
        }

        private int _actialBlockSize;
        public int BlockSize { get; set; }
        
        
        public ISignalReader<float> In { get; set; }

        public ISignalWriter<float> Out { get; set; }


        
        private float[] _readBuffer=new float[0];

        private float[] _resultBuffer = new float[0];

        private ipp.IppsFIRState64f_32f* _firSts;

        private void Init()
        {
            if (_firSts != null)
                return;

            var band = 0.5f/_actialDownFactor;

            //������� ������ � ��������������
            var taps = new double[_actialFilterTapsLen];

            _resultBuffer = new float[_actialBlockSize];

            //�������������� ������ ������ ������
            fixed (ipp.IppsFIRState64f_32f** pFirSts = &_firSts)
            fixed (double* pTaps = taps)
            {
                //�������������� ������ � ��������������
                IppHelper.Do(ipp.sp.ippsFIRGenLowpass_64f(band, pTaps, taps.Length, _actialWinType, ipp.IppBool.ippFalse));
                
                //�������������� FIR, ������������ �����������
                IppHelper.Do(ipp.sp.ippsFIRMRInitAlloc64f_32f(pFirSts, pTaps, taps.Length, 1, 0, _actialDownFactor, 0, null));
            }
        }

        /// <summary>
        /// ������� ��������.
        /// </summary>
        public void Free()
        {
            //�������� ��� ������� ��� �� �������
            if (_firSts == null)
                return;

            //�������
            ipp.sp.ippsFIRFree64f_32f(_firSts);
            _firSts = null;
        }

        private bool SetValues()
        {
            var changed = false;

            if(WinType!=_actialWinType)
            {
                _actialWinType = WinType;
                changed = true;
            }

            if (FilterTapsLen != _actialFilterTapsLen)
            {
                _actialFilterTapsLen = FilterTapsLen;
                changed = true;
            }

            if (DownFactor!=_actialDownFactor)
            {
                _actialDownFactor = DownFactor;
                changed = true;
            }

            if (BlockSize != _actialBlockSize)
            {
                _actialBlockSize = BlockSize;
                changed = true;
            }

            return changed;
        }

        public bool? Execute()
        {
            if(SetValues())
                Free();
            Init();

            var readBlockSize = _actialBlockSize * _actialDownFactor;

            if (_readBuffer.Length != readBlockSize)
                _readBuffer = new float[readBlockSize];

            if (!In.ReadTo(_readBuffer))
                return false;

            //������������ ��������� � ���������� ��� � ������
            fixed (float* pSrc = _readBuffer, pDst = _resultBuffer)
                ipp.sp.ippsFIR64f_32f(pSrc, pDst, _actialBlockSize, _firSts);

            Out.Write(_resultBuffer);

            return true;
        }
        

        /// <summary>
        /// ������� ��������.
        /// </summary>
        public void Dispose()
        {
            Free();
        }
    }
}
