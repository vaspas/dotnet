using System;

namespace IppModules.ImpulseSid
{
    /// <summary>
    /// ����� ��� ���������� �������������� ������� �� ������������ ��� ��������� �������, ����������� �������� ���.
    /// </summary>
    public unsafe class ConcurrentWorker:IDisposable
    {
        /// <summary>
        /// ����������.
        /// </summary>
        ~ConcurrentWorker()
        {
            Dispose(false);
        }

        #region ///// private fields /////
        
        /// <summary>
        /// ������� ������ ��� ������ ipp ����������.
        /// </summary>
        private byte[] _fftWorkBuf;

        /// <summary>
        /// ��������� �� ��������� ��� ������ ipp ����������.
        /// </summary>
        private ipp.IppsFFTSpec_C_32fc* _pSpec;

        /// <summary>
        /// ������ ������ ��� ��������� � �������� �� ���.
        /// </summary>
        private ipp.Ipp32fc[] _hData;

        /// <summary>
        /// ������ ��� ������.
        /// </summary>
        private ipp.Ipp32fc[] _workBuf1;

        /// <summary>
        /// ������ ��� ������.
        /// </summary>
        private ipp.Ipp32fc[] _workBuf2;


        /// <summary>
        /// ������ ����� ���.
        /// </summary>
        private ushort _fftLen;

        /// <summary>
        /// ������ ������ � ���������, ��� �������� ����������.
        /// </summary>
        private ipp.Ipp32fc[] _buffer;

        #endregion

        #region ///// private methods /////

        /// <summary>
        /// �������������� CFT �� ���������� ��������������.
        /// </summary>
        /// <param name="dstData">������ ���� ��������� ���������, �������� fftLen.</param>
        /// <param name="rFreq">������������� �������.</param>
        private void PrepareCft(ref ipp.Ipp32fc[] dstData, float rFreq)
        {
            float ph=0;
            //�������� ������
            ipp.sp.ippsZero_32fc(_workBuf1,_fftLen);
            //���������� ���, �� �� �� ���� �������, � ������ � ������, ������ � �������� ��������
            ipp.sp.ippsTone_Direct_32fc(_workBuf1, ImpulseLength, 2 * (float)_fftLen / ImpulseLength,
                rFreq,&ph,ipp.IppHintAlgorithm.ippAlgHintNone);            
            //��������� ��� �������� �����������
            if(Cos2)
                ipp.sp.ippsWinHann_32fc_I(_workBuf1, ImpulseLength);
            //���
            fixed(byte* pfftBuf=_fftWorkBuf)
            {
                ipp.sp.ippsFFTFwd_CToC_32fc(_workBuf1, dstData, _pSpec, pfftBuf);
            }
            ipp.sp.ippsConj_32fc_I(dstData, _fftLen);
        }

        private void Envelope(ipp.Ipp32fc[] data, ipp.Ipp32fc* pFixBuf, ipp.Ipp32fc* pFixWorkBuf1)
        {
            fixed (byte* pfftBuf = _fftWorkBuf)
            {
                //������������ ������� �������, �� ��������� 0.5 �����, ���� �� ������ �� ����� ������� �������� ������
                for (int i = 0; i < BlockSize; i += _fftLen / 2)
                {
                    ipp.sp.ippsZero_32fc(_workBuf1, _fftLen);
                    //Buffer.BlockCopy(buf, i * sizeof(ipp.Ipp32fc), workBuf1_, 0, Math.Min(fftLen_, sourceLength_ - i));
                    ipp.sp.ippsCopy_32f((float*)pFixBuf + i * 2, (float*)pFixWorkBuf1, Math.Min(_fftLen, BlockSize - i) * 2);
                    ipp.sp.ippsFFTFwd_CToC_32fc(_workBuf1, _workBuf2, _pSpec, pfftBuf);
                    ipp.sp.ippsMul_32fc_I(data, _workBuf2, _fftLen);
                    ipp.sp.ippsFFTInv_CToC_32fc(_workBuf2, _workBuf1,_pSpec, pfftBuf);
                    //Buffer.BlockCopy(workBuf1_, 0, buf, i * sizeof(ipp.Ipp32fc), Math.Min(fftLen_ / 2, sourceLength_ - i));
                    ipp.sp.ippsCopy_32f((float*)pFixWorkBuf1, (float*)pFixBuf + i * 2, Math.Min(_fftLen / 2, BlockSize - i) * 2);
                }
            }
        }
        
        #endregion

        #region ///// public fields /////
        
        /// <summary>
        /// ����� �������� � �������� �������.
        /// </summary>
        public int ImpulseLength;
        /// <summary>
        /// ������������� ������� ���������� ��������� ������� �������.
        /// </summary>
        public float CarrierRelativeFrequency;
        /// <summary>
        /// ���� ����� �������� � ���� ������������.
        /// </summary>
        public bool Cos2;

        public int BlockSize;

        #endregion

        #region ///// public methods /////

        /// <summary>
        /// �������������� ������ ��� ������, ���������� ��������� ��� ��������� ����� ����������.
        /// </summary>
        public void Prepare()
        {
            //������������ ������ ����� ��� ��� ������� ������
            //������ �� ����� ����� ������ ������� ����� ��������
            byte order = 5; //��� 32
            while (2 * ImpulseLength > (1 << order))
                order++;
            //������ �������� ������ ������ ����� ���
            _fftLen = (ushort)(1 << order);

            //�������������� ���������
            var bsize = 0;
            ipp.IppsFFTSpec_C_32fc* ptr; 
            ipp.sp.ippsFFTInitAlloc_C_32fc(&ptr, order, (int)ipp.IppsFFTNorm.ippFftDivFwdByN, ipp.IppHintAlgorithm.ippAlgHintNone);
            _pSpec = ptr;
            ipp.sp.ippsFFTGetBufSize_C_32fc(_pSpec, &bsize);
            _fftWorkBuf = new byte[bsize];
            
            //�������� ��������������� �������
            _workBuf1=new ipp.Ipp32fc[_fftLen];
            _workBuf2 = new ipp.Ipp32fc[_fftLen];
            //�������� ������� ���������
            _hData=new ipp.Ipp32fc[_fftLen];
            //�������������� ������� ��� ������, c ������ ������� ��������
            PrepareCft(ref _hData, CarrierRelativeFrequency);
            //�������� ������ ������ � ���������
            _buffer = new ipp.Ipp32fc[BlockSize];
        }

        /// <summary>
        /// ���������� ������.
        /// </summary>
        public void Close()
        {
            //������� ������������� ��������
            //�������� ��� ������� ��� �� �������
            if (_pSpec != null)
            {
                //�������
                ipp.sp.ippsFFTFree_C_32fc(_pSpec);
                _pSpec = null;
            }

            //��������� ������ ������ ��������
            _buffer = null;
            _workBuf1 = null;
            _workBuf2 = null;
            _hData = null;
            _fftWorkBuf = null;

        }

        /// <summary>
        /// ��������� ������, ������� ������ � ���������� �������.
        /// </summary>
        /// <param name="pData">��������� �� ���������������������� ������ ������ �������� blockSize.</param>
        public void DoWork(float* pData)
        {
            //��������� ��� �������
            fixed (ipp.Ipp32fc* pBuffer = _buffer, pworkBuf1 = _workBuf1)
            {
                int dstLen;
                int ph;
                var pfBuf = (float*) pBuffer;
                ipp.sp.ippsSampleUp_32f(pData, BlockSize, pfBuf, &dstLen, 2, &ph);
                Envelope(_hData, pBuffer, pworkBuf1);
                ipp.sp.ippsMagnitude_32fc(_buffer, pData, BlockSize);
            }
        }

        #endregion

        #region ///// IDispose /////

        /// <summary>
        /// ���� ������� ���������� �������.
        /// </summary>
        public void Dispose()
        {
            //��������� ������� ������������ ����, ��������� �������� ������ ����� ������ Finalize
            GC.SuppressFinalize(this);
            //�������
            Dispose(true);
        }

        /// <summary>
        /// ������� ��������.
        /// </summary>
        /// <param name="disposing">true ��� ������� ����������� ��������</param>
        protected void Dispose(bool disposing)
        {
            Close();
        }

        #endregion
    }
}
