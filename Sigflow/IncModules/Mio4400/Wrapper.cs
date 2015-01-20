using System;
using System.Runtime.InteropServices;

namespace IncModules.Mio4400
{
    /// <summary>
    /// �����-������� ��� ������� ���������� "��������".
    /// </summary>
    public static unsafe class Wrapper
    {
        /// <summary>
        /// �������� ����� ����������.
        /// </summary>
        public const string DllName = "Mio4400.dll";

        #region ������� ������������� ���������� � ����������

        /// <summary>
        /// ������������� ���424.
        /// ������������ ����� ���, ����� ����������� ������� ������. 
        /// ��������������� ������������ ��������  ��� ���� �������  0 db.  
        /// ��������������� ������� 50 000 ��. 
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioInit(int nBoard);

        /// <summary>
        /// ��������� ������� ���424.
        /// ��������� MioClose ��������� ������� ��� (������ �������� 0 � ������� CMD), 
        /// ��������� ������� ���� ������� (������ �������� 0 � ������� PWDN).
        /// </summary>
        /// <param name="nBoard">����� ����������, ��� int</param>
        [DllImport(DllName)]
        public static extern void MioClose(int nBoard);

        /// <summary>
        /// ����� ���424.
        /// ��������� MioReset ���������� ����� ���. 
        /// ��������������� ������������ ��������  ��� ���� �������  0 db.  
        /// ��������������� ������� 50 000 ��.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioReset(int nBoard);

        /// <summary>
        /// �������� ������� ���424.
        /// ��������� MioLaodPrj ���������� �������� ������� � ���������� ������ ���424. 
        /// �������� ������� ������������ ������ ��� ��������� ������������� ������� �����. 
        /// ���� ����������� � ������ �����, ����� �������� ���������� ��������� ������������� (MioInit)
        /// </summary>
        /// <param name="filename">��� ����� �������</param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioLoadPrj(string filename, int nBoard);
        
        #endregion

        #region ������� ������ ���������������� ����������

        /// <summary>
        /// �������� ������� ��������� ���������� ���.
        /// ��������� ����������  �������� ������� ��������� ���������� ���, ��������� � ������������. 
        /// </summary>
        /// <param name="nBoard">����� ����������, ��� int</param>
        /// <returns>������� ��������� ����������, ����, ��� double</returns>
        [DllImport(DllName)]
        public static extern double MioGetQuartz(int nBoard);

        /// <summary>
        /// �������� ��� �������� ������� ���.
        /// ��������� ����������  �������� ���� �������� ������� ���, ��������� � ������������. 
        /// </summary>
        /// <param name="nBoard">����� ����������, ��� int</param>
        /// <returns>��� �������� �������, �����, ��� double</returns>
        [DllImport(DllName)]
        public static extern double MioGetAbsVolt(int nBoard);

        /// <summary>
        /// �������� ����������� ���.
        /// ��������� ����������  �������� ����������� ���, ��������� � ������������.
        /// </summary>
        /// <param name="nBoard">����� ����������, ��� int</param>
        /// <returns>����������� ���, ��� int</returns>
        [DllImport(DllName)]
        public static extern int MioGetBitWide(int nBoard);

        #endregion
        
        #region ������� ��������� � ������ ���������� �����

        /// <summary>
        /// ������ ������� ������ �������.
        /// ��������� MioGetTab ���������� ������ ������� ������ ������� ���424.
        /// </summary>
        /// <param name="n">���������� �������</param>
        /// <param name="ntab">������� ������ ������� </param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� BYTE ,  ���������� �������</returns>
        [DllImport(DllName)]
        public static extern byte MioGetTab(byte n,byte[] ntab, int nBoard);

        /// <summary>
        /// ������ ������� ������ �������.
        /// ��������� MioSetTab ���������� ������ ������� ������ �������.
        /// </summary>
        /// <param name="n">���������� �������</param>
        /// <param name="ntab">������� ������ ������� </param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioSetTab(byte n,byte[] ntab, int nBoard);

        /// <summary>
        /// ������ ���� �������� ������.
        /// ��������� MioGetGain ���������� ������ �������� �������� ������ ���424.
        /// </summary>
        /// <param name="n">����� �������� �������� ������ </param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� BYTE ,  �������� ���� ��������</returns>
        [DllImport(DllName)]
        public static extern byte MioGetGain(byte n, int nBoard);

        /// <summary>
        /// ������ ���� �������� ������.
        /// ��������� MioSetGain ���������� ������ ���� ��������  ������. 
        /// � �������� ���� �������� ������������ ���������.
        /// ��������� ����� �������� ������������ � ������������ �� ���������� ����� ��������, ���������� � ������������.
        /// </summary>
        /// <param name="n">����� �������� �������� ������</param>
        /// <param name="gain">�������� ���� �������� </param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioSetGain(byte n,byte gain, int nBoard);

        /// <summary>
        /// ��������� ������� �������������, fFreq- ������� � ��/
        /// </summary>
        /// <param name="dFreq">�������� ������� ������� � ��, ��� float</param>
        /// <param name="nBoard">����� ����������, ��� int</param>
        [DllImport(DllName)]
        public static extern void MioSetFilterFreq(double dFreq, int nBoard);
        
        /// <summary>
        /// ����������� ���� �������.
        /// ��������� MioGetCodFreq ���������� ��� ��������� ������� ����������� ���424.
        /// </summary>
        /// <param name="fFreq">������� ������� � ��, ��� double  </param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� int ,  �������� ���� ��������� ������� ����������� ������</returns>
        [DllImport(DllName)]
        public static extern int MioGetCodFreq(double fFreq, int nBoard);
        
        #endregion
        
        #region ������� ������� � �������� �����
        
        /// <summary>
        /// C��� ������ � ������ ������������ �������.
        /// ��������� MioRunInternalSbor ���������� ������ ����� ������ � ������ chaining DMA, 
        /// ����������� ������ �����. 
        /// </summary>
        /// <param name="sizeBl">������ ����� �����,  32-���������� ����, ��� int </param>
        /// <param name="numBl">���������� ������ (�������� SizeBl) � host ������ ��������, ��� int</param>
        /// <param name="numBuf"></param>
        /// <param name="numChan">���������� �������, ��� int</param>
        /// <param name="sizeBlockInt"></param>
        /// <param name="posChan"></param>
        /// <param name="sBufs">��������� �� ����� ������ �����, ����������� ��� ��������� ������, ��� int </param>
        /// <param name="phEventBufferReady">������� � ������� SizeBlockInt ������</param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� ������, ��� int</returns>
        [DllImport(DllName)]
        public static extern int MioRunSbor(int sizeBl, int numBl, int numBuf, int numChan, int sizeBlockInt, int posChan,
           out int* sBufs, IntPtr phEventBufferReady, int nBoard);
        
        /// <summary>
        /// ������� ����� ������.
        /// </summary>
        /// <param name="sBufs"></param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioStopSbor(int* sBufs, int nBoard);

        /// <summary>
        /// ������� ����� ������.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioStopMpSbor( int nBoard);
        
        /// <summary>
        /// ������� ����� ������.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioStopADC( int nBoard);

        /// <summary>
        /// ������� ���������� ���������� ���������� ���������� (��������� �������).
        /// ��������� MioGetReadyBuf  ���������� �������� �������� ����������. 
        /// ����� �������� �������� ����� �������� ���������� ��������, �������� �������� ���������� ����� 0. 
        /// ����� ��������� ������ �������� ���������� (MioSetModeCalcInterrupt()), 
        /// ������� ����� ������������������ ��� ��������� ��������� ����������, 
        /// ���� ����� �������� ���������� �� ����� �������. 
        /// ��������� �������� ���������� ������������ � ��������� ��������� ���������� �������� PCI9080. 
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>�������� �������� ����������, ��� int.</returns>
        [DllImport(DllName)]
        public static extern int MioGetReadyBuf( int nBoard);

        /// <summary>
        /// ������� ���������� ���������� �������� ����������
        /// </summary>
        /// <param name="nBoard"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int MioGetReadyInt(int nBoard);

        #endregion

        #region ������� ���������� ������������


        /// <summary>
        /// ����� ����������, ���������� ��� ������ ���������� ���������� .
        /// ��������� MioResetIntr ������ ���� ������� ��� ��������� �������  � ���������� ���������.
        /// </summary>
        /// <param name="phEventBufferReady">�������</param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioResetIntr(IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// ���������� ���������� �� ���������� ������ .
        /// ��������� MioEnableIntr ��������� ���������� �� ���������� �����.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioEnableIntr ( int nBoard);

        /// <summary>
        /// ���������� ���������� �� ���������� ������ .
        /// ��������� MioDisableIntr ��������� ���������� �� ���������� �����.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DllName)]
        public static extern void MioDisableIntr ( int nBoard);
        
        #endregion

    }

    /// <summary>
    /// ���� ������ ������� MioRunSbor
    /// </summary>
    public enum RunSborErrors
    {
        /// <summary>
        /// ������� ��������� �������
        /// </summary>
        Success = 0,
        /// <summary>
        /// ������ ��� ���������� �������
        /// </summary>
        Failed = -1,
        /// <summary>
        /// ������ ��������� ������ ��� ������� �����
        /// </summary>
        AllocMemory	= 100,
        /// <summary>
        /// ������ ������� �������� PLX
        /// </summary>
        DriverPLX	=101,
        /// <summary>
        /// ������ �������� ������ DMA � �������� PLX
        /// </summary>
        DMAPLX		=102	
    }


    /// <summary>
    /// �������� ������������� ��������
    /// </summary>
    public enum GainValues : byte
    {
        /// <summary>
        /// "0"	
        /// </summary>
        Gain0 = 0,
        /// <summary>
        /// "10"	
        /// </summary>
        Gain10 = 1,
        /// <summary>
        /// "20"
        /// </summary>
        Gain20 = 2,
        /// <summary>
        /// "30"
        /// </summary>
        Gain30 = 3	
    }
}
