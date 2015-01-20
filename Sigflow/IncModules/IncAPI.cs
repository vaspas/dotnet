using System.Runtime.InteropServices;

namespace IncModules
{
    /// <summary>
    /// �����-������� ��� ������� ���������� "��������".
    /// </summary>
    public static class IncAPI
    {
        /// <summary>
        /// �������� ����� ����������.
        /// </summary>
        public const string DLL_NAME = "IncApi.dll";

        /// <summary>
        /// ��������� pinit ��������� ������� ����������, �������� �������, ������������� �������� ���������� ��������. 
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int pinit();

        /// <summary>
        /// ��������� pclose ��������� ������� ����������, ����������� ���������� �������. 
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int pclose();


        /// <summary>
        /// ������ ����������.
        /// ��������� LockDevice ���������� �������� ����������� ��������� ��� �������������� ����������� ������������ 
        /// ������������� ������� � ����������� ����������� ���������. 
        /// ��������� ��������� ������� ���������� ���������� � ������� ��� ������� � ���������� ���������. 
        /// �������� ����������� � ������� ������� WaitForSingleObject. 
        /// ����� �������� ��������������� �������� Pos_GetWaitDeviceTimeout, ������������ -  Pos_GetWaitDeviceTimeout. 
        /// �������� �������� �������� ������������ ���������� �� ���������, ������������� � ���������� IncApi,   0.  
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>IncSUCCESS � ���������� ������� ������������,
        /// IncFAILED     - ����� �� �������� ��� ������ �������� ����� ����������.
        ///</returns>
        [DllImport(DLL_NAME)]
        public static extern int LockDevice(int nBoard);

        /// <summary>
        /// ���������� ���������� .
        /// ��������� UnlockDevice ����������� ������� ��������� ����������. ���������� ����� ���� ������ ������ ���������.
        /// </summary>
        /// <param name="nBoard">����� ����������</param>
        /// <returns>IncSUCCESS � ���������� ������� �����������,
        ///IncFAILED     - ������ ������� Releasemutex ��� ������ �������� ����� ����������.
        ///</returns>
        [DllImport(DLL_NAME)]
        public static extern int UnlockDevice(int nBoard);

        /// <summary>
        /// �������� ���������� ���������, ��������� � ������������ .
        /// ��������� POS_GetNBoards ���������� ���������� ���������, ��������� � ������������.
        /// </summary>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern void POS_GetNBoards(ref int nBoard);

        /// <summary>
        /// �������� ��� �������� ����������, �������������� �� ����������.
        /// ���������� cExtDev ������������� �������� -  ��� �������� ����������, �������������� �� ���������� 
        /// </summary>
        /// <param name="cExtDev">��� �������� ����������, �������������� �� ����������</param>
        /// <param name="nBoard">����� ����������</param>
        [DllImport(DLL_NAME)]
        public static extern void POS_GetCExtDev(ref int cExtDev, int nBoard);

        [DllImport(DLL_NAME)]
        public static extern void EnableWriteToLog();

        [DllImport(DLL_NAME)]
        public static extern void OpenBoard(int nBoard);

        [DllImport(DLL_NAME)]
        public static extern void GetSizeDevBuf(int nBoard, ref int bufSize);

    }

    
    
}
