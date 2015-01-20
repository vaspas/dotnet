using System;
using System.Runtime.InteropServices;

namespace IncModules.Ink824
{
    /// <summary>
    /// �����-������� ��� ������� ���������� "��������".
    /// </summary>
    public static unsafe class Wrapper
    {
        /// <summary>
        /// �������� ����� ����������.
        /// </summary>
        public const string DLL_NAME = "Ink824.dll";

        #region ������� ������������� ���������� � ����������

        /// <summary>
        /// ������������� ���424.
        /// ������������ ����� ���, ����� ����������� ������� ������. 
        /// ��������������� ������������ ��������  ��� ���� �������  0 db.  
        /// ��������������� ������� 50 000 ��. 
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioInit(int nBoard);

        /// <summary>
        /// ��������� ������� ���424.
        /// ��������� MioClose ��������� ������� ��� (������ �������� 0 � ������� CMD), 
        /// ��������� ������� ���� ������� (������ �������� 0 � ������� PWDN).
        /// </summary>
        /// <param name="nBoard">����� ����������, ��� int</param>
        [DllImport(DLL_NAME)]
        public static extern void MioClose(int nBoard);

        /// <summary>
        /// ����� ���424.
        /// ��������� MioReset ���������� ����� ���. 
        /// ��������������� ������������ ��������  ��� ���� �������  0 db.  
        /// ��������������� ������� 50 000 ��.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioReset(int nBoard);

        /// <summary>
        /// �������� ������� ���424.
        /// ��������� MioLaodPrj ���������� �������� ������� � ���������� ������ ���424. 
        /// �������� ������� ������������ ������ ��� ��������� ������������� ������� �����. 
        /// ���� ����������� � ������ �����, ����� �������� ���������� ��������� ������������� (MioInit)
        /// </summary>
        /// <param name="filename">��� ����� �������</param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioLoadPrj(string filename, int nBoard);

        /// <summary>
        /// ����������� ������� ��������� ���424.
        /// </summary>
        /// <param name="nDev"></param>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetNumDev(ref int nDev);

        #endregion

        #region ������� ������ ���������������� ����������

        /// <summary>
        /// �������� ������� ��������� ���������� ���.
        /// ��������� ����������  �������� ������� ��������� ���������� ���, ��������� � ������������. 
        /// </summary>
        /// <param name="nBoard">����� ����������, ��� int</param>
        /// <returns>������� ��������� ����������, ����, ��� double</returns>
        [DllImport(DLL_NAME)]
        public static extern double MioGetQuartz(int nBoard);

        /// <summary>
        /// �������� ��� �������� ������� ���.
        /// ��������� ����������  �������� ���� �������� ������� ���, ��������� � ������������. 
        /// </summary>
        /// <param name="nBoard">����� ����������, ��� int</param>
        /// <returns>��� �������� �������, �����, ��� double</returns>
        [DllImport(DLL_NAME)]
        public static extern double MioGetAbsVolt(int nBoard);

        /// <summary>
        /// �������� ����������� ���.
        /// ��������� ����������  �������� ����������� ���, ��������� � ������������.
        /// </summary>
        /// <param name="nBoard">����� ����������, ��� int</param>
        /// <returns>����������� ���, ��� int</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetBitWide(int nBoard);

        #endregion
        
        #region ������� ��������� � ������ ���������� �����

        /// <summary>
        /// ������ ������� ������ �������.
        /// ��������� MioGetTab ���������� ������ ������� ������ ������� ���424.
        /// </summary>
        /// <param name="n">���������� �������</param>
        /// <param name="Ntab">������� ������ ������� </param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� BYTE ,  ���������� �������</returns>
        [DllImport(DLL_NAME)]
        public static extern byte MioGetTab(byte n,byte[] Ntab, int nBoard);

        /// <summary>
        /// ������ ������� ������ �������.
        /// ��������� MioSetTab ���������� ������ ������� ������ �������.
        /// </summary>
        /// <param name="n">���������� �������</param>
        /// <param name="Ntab">������� ������ ������� </param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetTab(byte n,byte[] Ntab, int nBoard);

        /// <summary>
        /// ������ ���� �������� ������.
        /// ��������� MioGetGain ���������� ������ �������� �������� ������ ���424.
        /// </summary>
        /// <param name="n">����� �������� �������� ������ </param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� BYTE ,  �������� ���� ��������</returns>
        [DllImport(DLL_NAME)]
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
        [DllImport(DLL_NAME)]
        public static extern void MioSetGain(byte n,byte gain, int nBoard);

        /// <summary>
        /// ��������� ������� �������������, fFreq- ������� � ��/
        /// </summary>
        /// <param name="fFreq">�������� ������� ������� � ��, ��� float</param>
        /// <param name="nBoard">����� ����������, ��� int</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetFreq(float fFreq, int nBoard);

        /// <summary>
        /// ��������� ���� ������� ������������� � ���������� ������.
        /// ��������� MioSetFreqi ��������� ������ ���� ������� � ���������� ������ ���. 
        /// ��� ������� ������ ���� ����������� ���������� MioGetCodFreq().                
        /// </summary>
        /// <param name="CodFreq">�������� ���� �������, ��� int </param>
        /// <param name="nBoard">����� ����������, ��� int</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetFreqi(int CodFreq, int nBoard);

        /// <summary>
        /// ����������� ���� �������.
        /// ��������� MioGetCodFreq ���������� ��� ��������� ������� ����������� ���424.
        /// </summary>
        /// <param name="fFreq">������� ������� � ��, ��� double  </param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� int ,  �������� ���� ��������� ������� ����������� ������</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetCodFreq(double fFreq, int nBoard);

        /// <summary>
        /// ��������� ������� ������������.
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minites"></param>
        /// <param name="seconds"></param>
        /// <param name="nBoard"></param>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int MioSetTime(int hours, int minites, int seconds, int nBoard);

        /// <summary>
        /// ��������� ���� ������� ������������.
        /// ��������� MioSetCodTime ���������� ������ �������� �������-����������� ���� ������� � BRAM
        /// </summary>
        /// <param name="time">����� � �������-���������� ����  </param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetCodTime(int time, int nBoard);

        /// <summary>
        /// ����������� ���� ������� ������������.
        /// ��������� MioGetCodTime ���������� ������ �������� �������-����������� ���� ������� �� BRAM .
        /// </summary>
        /// <param name="time">����� � �������-���������� ����  </param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioGetCodTime(ref int time, int nBoard);

        /// <summary>
        /// ������ �������� ����� �������.
        /// ��������� MioGetCodTime ���������� ������ �������� �������-����������� ���� ������� �� �������� ����� �������.
        /// </summary>
        /// <param name="time">����� � �������-���������� ����  </param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioGetMarkTime(ref int time, int nBoard);

        /// <summary>
        /// ����������� ������� ������ ��������.
        /// ��������� MioGetBufferSize ���������� ������ ������ �����, ����������� ��� �������� ��������. 
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>������ ������ ����� � ������</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetBufferSize( int nBoard);

        /// <summary>
        /// ����������� ������������� ���������� ������� �����, ��������� �������.
        /// ��������� MioGetNumBuff ���������� ���������� ������ ���������� �������, 
        /// ������� ����� ���� ��������� � ������ �����. 
        /// ��� ���������� ���������� ������ ������ ������ ����� ������� �� ����� ������� ����� � ������� ����������� �����.
        /// </summary>
        /// <param name="size">������ ����� � ������</param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>���������� ������ ��������� �������</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetNumBuff(int size, int nBoard);
        
        #endregion

        #region ������� ��������� ������� �����
        
        /// <summary>
        /// ��������� ������ ���������� �������������.
        /// ��������� MioSetInternalSync ���������� ��������� ������ ���������� �������������, 
        /// �������� ���� 2 �������� SWR. ������� ������ ���� ������� �� ������ �������, 
        /// ��������������� ������� �������������. 
        /// ��� ���������� ���� ������� �����������, ��� ������� ��������� �������� ������� 80 ���.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetInternalSync( int nBoard);

        /// <summary>
        /// ��������� ������ ������� �������������.
        /// ��������� MioSetExternalSync ���������� ��������� ������ ������� �������������, 
        /// ���������� ���� 2 �������� SWR. 
        /// ������� ������ ���� ������� �� ������ �������, ��������������� ������� �������������. 
        /// ��� ���������� ���� ������� �����������, ��� ������� ��������� �������� ������� 50 ���.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetExternalSync( int nBoard);

        /// <summary>
        /// ��������� ������ ������������ �������.
        /// ��������� MioSetInternalSbor ���������� ��������� ������ ������������ �������, ������� ���� 7 �������� SWR. 
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetInternalSbor( int nBoard);

        /// <summary>
        /// ��������� ������ ������� � �������� �������.
        /// ��������� MioSetExternalSbor ���������� ��������� ������ �������� �����, ��������� ���� 7 �������� SWR. 
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetExternalSbor( int nBoard);

        /// <summary>
        /// ��������� ������ ����� �� ���� �������.
        /// ��������� MioSetTimeSbor ���������� ��������� ������ ����� �� ���� �������, ������� ���� 7 �������� SWR, 
        /// ��������� ���� 2 �������� SWR.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetTimeSbor( int nBoard);

        #endregion

        #region ������� ������� � �������� �����
        
        /// <summary>
        /// C��� ������ � ������ ������������ �������.
        /// ��������� MioRunInternalSbor ���������� ������ ����� ������ � ������ chaining DMA, 
        /// ����������� ������ �����. 
        /// </summary>
        /// <param name="SizeBl">������ ����� �����,  32-���������� ����, ��� int </param>
        /// <param name="NumBl">���������� ������ (�������� SizeBl) � host ������ ��������, ��� int</param>
        /// <param name="NumChan">���������� �������, ��� int</param>
        /// <param name="SBufs">��������� �� ����� ������ �����, ����������� ��� ��������� ������, ��� int </param>
        /// <param name="hEventBufferReady">������� � ������� SizeBlockInt ������</param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� ������, ��� int</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunInternalSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// C��� � ������ ������� �� �������� ������� ���������� ������ ���.
        /// ��������� MioRunExternalSbor ���������� ������ ����� ������ � ������ chaining DMA, ����� �������� ������ �����. 
        /// </summary>
        /// <param name="SizeBl">������ ����� �����,  32-���������� ����, ��� int </param>
        /// <param name="NumBl">���������� ������ (�������� SizeBl) � host ������ ��������, ��� int</param>
        /// <param name="NumChan">���������� �������, ��� int</param>
        /// <param name="SBufs">��������� �� ����� ������ �����, ����������� ��� ��������� ������, ��� int </param>
        /// <param name="hEventBufferReady">������� � ������� SizeBlockInt ������</param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� ������, ��� int</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunExternalSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// C��� � ������ ������� ��� ����������� ��������� ���� �������.
        /// ��������� MioRunTimeSbor ���������� ������ ����� ������ � ������ chaining DMA, 
        /// ����� ������� ����� ��� ����������� ��������� ���� �������. 
        /// </summary>
        /// <param name="SizeBl">������ ����� �����,  32-���������� ����, ��� int </param>
        /// <param name="NumBl">���������� ������ (�������� SizeBl) � host ������ ��������, ��� int</param>
        /// <param name="NumChan">���������� �������, ��� int</param>
        /// <param name="SBufs">��������� �� ����� ������ �����, ����������� ��� ��������� ������, ��� int </param>
        /// <param name="hEventBufferReady">������� � ������� SizeBlockInt ������</param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� ������, ��� int.</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunTimeSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// ������� ����� ������.
        /// ��������� MioStopSbor ���������� ������� ����� ������ � ������ non-chaining DMA. 
        /// ����������� �������  ��������������,  ����������� ����� DMA.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioStopSbor( int nBoard);

        /// <summary>
        /// ������ ����� ������. Block mode DMA.
        /// ��������� MioRunSbor ���������� ������ ����� ������ � ������ non-chaining DMA. 
        /// ���������� ������ ��� ������ ��������, ������ ������ ����� �����������:
        /// (SizeBl*NumBl*NumChan*4) ������.
        /// ��������� ����� DMA ��� ��������,  ����������� ������ ��������������. ����� DMA ��������������� ��� ���������� ��������  (SizeBl*NumBl*NumChan*4) ������ ����������. ��������� ������������ � �������� ������. 
        /// ��� ���������� ������������ ����� ������������ ��������� MioRunChainSbor.
        /// </summary>
        /// <param name="SizeBl">������ ����� �����,  32-���������� ����, ��� int </param>
        /// <param name="NumBl">���������� ������ (�������� SizeBl) � host ������ ��������, ��� int</param>
        /// <param name="NumChan">���������� �������, ��� int</param>
        /// <param name="SBufs">��������� �� ����� ������ �����, ����������� ��� ��������� ������, ��� int </param>
        /// <param name="hEventBufferReady">������� � ������� SizeBlockInt ������</param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, out IntPtr hEventBufferReady, int nBoard);

        /// <summary>
        /// ������ ����� ������. Chain mode DMA.
        /// ��������� MioRunChainSbor ���������� ������ ����� ������ � ������ chaining DMA.
        /// ���������� ������ ��� ������ �������� � ������� ������������. 
        /// ������ ������ ����� -(SizeBl*NumBl*NumChan*4) ������. 
        /// ������ ������� ������������ - (NumBl*4*4) ������. 
        /// ��������� ������� ������� ������������ ��������� �� ���������, ����� �������, ������������ ��������� ����� �����. 
        /// ��������� ����� DMA ��� ��������,  ����������� ������ ���. 
        /// ����� ������ ������� ����� � ����� ���������� ����������, 
        /// � ������� hEventBufferReady �����������  � ���������� ���������. 
        /// ������ �������� � ���� 32-���������� ����� �����.
        /// </summary>
        /// <param name="SizeBl">����� �����,  32-���������� ����, ��� int </param>
        /// <param name="NumBl">���������� ������ (�������� SizeBl) � host ������ ��������</param>
        /// <param name="NumChan">���������� �������, ��� int</param>
        /// <param name="SBufs">��������� �� ����� ������ �����, ����������� ��� ��������� ������</param>
        /// <param name="hEventBufferReady">� �������, �������� ����������, ��������� ���� �������  � �����</param>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>��� ������, ��� int</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunChainSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, out IntPtr hEventBufferReady, int nBoard);

        /// <summary>
        /// ������� ����� ������. Chain mode DMA.
        /// ��������� MioStopSbor ���������� ������� ����� ������ � ������ chaining DMA. 
        /// ����������� �������  ��������������, ������� ������ ������ DMA, ����������� ����� DMA.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioStopChainSbor( int nBoard);

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
        [DllImport(DLL_NAME)]
        public static extern int MioGetReadyBuf( int nBoard);

        #endregion

        #region ������� ���������� ������������

        /// <summary>
        /// ����� ����������, ���������� ��� ������ ���������� ���������� .
        /// ��������� MioResetIntr ������ ���� ������� ��� ��������� �������  � ���������� ���������.
        /// </summary>
        /// <param name="hEventBufferReady">�������</param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioResetIntr(IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// ���������� ���������� �� ���������� ������ .
        /// ��������� MioEnableIntr ��������� ���������� �� ���������� �����.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioEnableIntr ( int nBoard);

        /// <summary>
        /// ���������� ���������� �� ���������� ������ .
        /// ��������� MioDisableIntr ��������� ���������� �� ���������� �����.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioDisableIntr ( int nBoard);

        /// <summary>
        /// ��������� ������ �������� ����������.
        /// ��������� MioSetModeCalcInterrupt ������������� ����� �������� ���������� � �������� PCI9080.sys. 
        /// ������� ���������� � �������� ����������.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetModeCalcInterrupt( int nBoard);

        /// <summary>
        /// ����� ������ �������� ����������  .
        /// ��������� MioSetModeCalcInterrupt ���������� ����� �������� ���������� � �������� PCI9080.sys. 
        /// ������� ���������� � �������� ����������.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void MioResetModeCalcInterrupt( int nBoard);
        
        #endregion

        #region ��������������� ������� 
        
        /// <summary>
        /// �������� ��� �������� �� ������������.
        /// </summary>
        /// <param name="gain"></param>
        /// <param name="nBoard"></param>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetCodGain(byte gain, int nBoard);

        /// <summary>
        /// ����������� ������� �������� ��������� ������������.
        /// </summary>
        /// <param name="Freq"></param>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern void MioGetExternalFreq(ref double Freq, int nBoard);

        /// <summary>
        /// ����������� ������� ����������� ��������� ������������.
        /// </summary>
        /// <param name="Freq"></param>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern void MioGetInternalFreq(ref double Freq, int nBoard);

        /// <summary>
        /// ����� ���������� DMA.
        /// </summary>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern bool MioDmaReady( int nBoard);

        /// <summary>
        /// ���� ���.
        /// </summary>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern void MioStopADC( int nBoard);

        #endregion
    }

    /// <summary>
    /// ���� ������ ������� MioRunSbor
    /// </summary>
    public enum RunSborErrors: int
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
        Gain_0 = 0,
        /// <summary>
        /// "10"	
        /// </summary>
        Gain_10 = 1,
        /// <summary>
        /// "20"
        /// </summary>
        Gain_20 = 2,
        /// <summary>
        /// "30"
        /// </summary>
        Gain_30 = 3	
    }
}
