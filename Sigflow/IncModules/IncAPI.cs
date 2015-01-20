using System.Runtime.InteropServices;

namespace IncModules
{
    /// <summary>
    /// Класс-обертка для функций библиотеки "Инкомтех".
    /// </summary>
    public static class IncAPI
    {
        /// <summary>
        /// Название файла библиотеки.
        /// </summary>
        public const string DLL_NAME = "IncApi.dll";

        /// <summary>
        /// Программа pinit открывает драйвер устройства, выделяет ресурсы, устанавливает значения параметров драйвера. 
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int pinit();

        /// <summary>
        /// Программа pclose закрывает драйвер устройства, освобождает выделенные ресурсы. 
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int pclose();


        /// <summary>
        /// Занять устройство.
        /// Программа LockDevice использует механизм именованных мьютексов для предоставления возможности организовать 
        /// синхронизацию доступа к устройствам программных процессов. 
        /// Программа открывает мьютекс указанного устройства и ожидает его переход в сигнальное состояние. 
        /// Ожидание выполняется с помощью функции WaitForSingleObject. 
        /// Время ожидания устанавливается функцией Pos_GetWaitDeviceTimeout, опрашивается -  Pos_GetWaitDeviceTimeout. 
        /// Значение таймаута ожидания освобождения устройства по умолчанию, установленное в библиотеке IncApi,   0.  
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>IncSUCCESS – устройство успешно присоединено,
        /// IncFAILED     - выход по таймауту или указан неверный номер устройства.
        ///</returns>
        [DllImport(DLL_NAME)]
        public static extern int LockDevice(int nBoard);

        /// <summary>
        /// Освободить устройство .
        /// Программа UnlockDevice освобождает мьютекс заданного устройства. Устройство может быть занято другим процессом.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>IncSUCCESS – устройство успешно освобождено,
        ///IncFAILED     - ошибка функции Releasemutex или указан неверный номер устройства.
        ///</returns>
        [DllImport(DLL_NAME)]
        public static extern int UnlockDevice(int nBoard);

        /// <summary>
        /// Получить количество устройств, указанных в конфигурации .
        /// Программа POS_GetNBoards возвращает количество устройств, указанных в конфигурации.
        /// </summary>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern void POS_GetNBoards(ref int nBoard);

        /// <summary>
        /// Получить код внешнего устройства, установленного на устройстве.
        /// Переменной cExtDev присваивается значение -  код внешнего устройства, установленного на устройстве 
        /// </summary>
        /// <param name="cExtDev">код внешнего устройства, установленного на устройстве</param>
        /// <param name="nBoard">номер устройства</param>
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
