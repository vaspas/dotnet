using System;
using System.Runtime.InteropServices;

namespace IncModules.Mio4400
{
    /// <summary>
    /// Класс-обертка для функций библиотеки "Инкомтех".
    /// </summary>
    public static unsafe class Wrapper
    {
        /// <summary>
        /// Название файла библиотеки.
        /// </summary>
        public const string DllName = "Mio4400.dll";

        #region функции инициализации устройства и библиотеки

        /// <summary>
        /// Инициализация ИНК424.
        /// Производится сброс АЦП, сброс синтезатора частоты модуля. 
        /// Устанавливаются коэффициенты усиления  для всех каналов  0 db.  
        /// Устанавливается частота 50 000 Гц. 
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioInit(int nBoard);

        /// <summary>
        /// Выключить питание ИНК424.
        /// Программа MioClose выполняет останов АЦП (запись значения 0 в регистр CMD), 
        /// выключает питание всех каналов (запись значения 0 в регистр PWDN).
        /// </summary>
        /// <param name="nBoard">номер устройства, тип int</param>
        [DllImport(DllName)]
        public static extern void MioClose(int nBoard);

        /// <summary>
        /// Сброс ИНК424.
        /// Программа MioReset производит сброс АЦП. 
        /// Устанавливаются коэффициенты усиления  для всех каналов  0 db.  
        /// Устанавливается частота 50 000 Гц.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioReset(int nBoard);

        /// <summary>
        /// Загрузка проекта ИНК424.
        /// Программа MioLaodPrj производит загрузку проекта в контроллер модуля ИНК424. 
        /// Загрузка проекта производится обычно при начальной инициализации системы сбора. 
        /// Если выполняется в другое время, после загрузки необходимо выполнить инициализацию (MioInit)
        /// </summary>
        /// <param name="filename">имя файла проекта</param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioLoadPrj(string filename, int nBoard);
        
        #endregion

        #region Функции выдачи конфигурационных параметров

        /// <summary>
        /// Получить частоту задающего генератора АЦП.
        /// Программа возвращает  значение частоты задающего генератора АЦП, указанное в конфигурации. 
        /// </summary>
        /// <param name="nBoard">номер устройства, тип int</param>
        /// <returns>Частота задающего генератора, герц, тип double</returns>
        [DllImport(DllName)]
        public static extern double MioGetQuartz(int nBoard);

        /// <summary>
        /// Получить вес младшего разряда АЦП.
        /// Программа возвращает  значение веса младшего разряда АЦП, указанное в конфигурации. 
        /// </summary>
        /// <param name="nBoard">номер устройства, тип int</param>
        /// <returns>Вес младшего разряда, вольт, тип double</returns>
        [DllImport(DllName)]
        public static extern double MioGetAbsVolt(int nBoard);

        /// <summary>
        /// Получить разрядность АЦП.
        /// Программа возвращает  значение разрядности АЦП, указанное в конфигурации.
        /// </summary>
        /// <param name="nBoard">номер устройства, тип int</param>
        /// <returns>Разрядность АЦП, тип int</returns>
        [DllImport(DllName)]
        public static extern int MioGetBitWide(int nBoard);

        #endregion
        
        #region функции установки и опроса параметров сбора

        /// <summary>
        /// Чтение таблицы выбора каналов.
        /// Программа MioGetTab производит чтение таблицы выбора каналов ИНК424.
        /// </summary>
        /// <param name="n">количество каналов</param>
        /// <param name="ntab">таблица выбора каналов </param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>Тип BYTE ,  количество каналов</returns>
        [DllImport(DllName)]
        public static extern byte MioGetTab(byte n,byte[] ntab, int nBoard);

        /// <summary>
        /// Запись таблицы выбора каналов.
        /// Программа MioSetTab производит запись таблицы выбора каналов.
        /// </summary>
        /// <param name="n">количество каналов</param>
        /// <param name="ntab">таблица выбора каналов </param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioSetTab(byte n,byte[] ntab, int nBoard);

        /// <summary>
        /// Чтение кода усиления канала.
        /// Программа MioGetGain производит чтение регистра усиления канала ИНК424.
        /// </summary>
        /// <param name="n">адрес регистра усиления канала </param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>Тип BYTE ,  значение кода усиления</returns>
        [DllImport(DllName)]
        public static extern byte MioGetGain(byte n, int nBoard);

        /// <summary>
        /// Запись кода усиления канала.
        /// Программа MioSetGain производит запись кода усиления  канала. 
        /// В качестве кода усиления используются константы.
        /// Установка кодов усиления производится в соответствии со значениями кодов усиления, указанными в конфигурации.
        /// </summary>
        /// <param name="n">адрес регистра усиления канала</param>
        /// <param name="gain">значение кода усиления </param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioSetGain(byte n,byte gain, int nBoard);

        /// <summary>
        /// Установка частоты дискретизации, fFreq- частота в Гц/
        /// </summary>
        /// <param name="dFreq">значение частоты выборки в Гц, тип float</param>
        /// <param name="nBoard">номер устройства, тип int</param>
        [DllImport(DllName)]
        public static extern void MioSetFilterFreq(double dFreq, int nBoard);
        
        /// <summary>
        /// Определение кода частоты.
        /// Программа MioGetCodFreq определяет код установки частоты синтезатора ИНК424.
        /// </summary>
        /// <param name="fFreq">частоты выборки в Гц, тип double  </param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>Тип int ,  значение кода установки частоты синтезатора частот</returns>
        [DllImport(DllName)]
        public static extern int MioGetCodFreq(double fFreq, int nBoard);
        
        #endregion
        
        #region Функции запуска и останова сбора
        
        /// <summary>
        /// Cбор данных в режиме безусловного запуска.
        /// Программа MioRunInternalSbor производит запуск сбора данных в режиме chaining DMA, 
        /// безусловный запуск сбора. 
        /// </summary>
        /// <param name="sizeBl">размер блока сбора,  32-хразрядных слов, тип int </param>
        /// <param name="numBl">количество блоков (размером SizeBl) в host буфере выгрузки, тип int</param>
        /// <param name="numBuf"></param>
        /// <param name="numChan">количество каналов, тип int</param>
        /// <param name="sizeBlockInt"></param>
        /// <param name="posChan"></param>
        /// <param name="sBufs">указатель на адрес буфера сбора, заполняется при выделении памяти, тип int </param>
        /// <param name="phEventBufferReady">событие – собрано SizeBlockInt блоков</param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>код ошибки, тип int</returns>
        [DllImport(DllName)]
        public static extern int MioRunSbor(int sizeBl, int numBl, int numBuf, int numChan, int sizeBlockInt, int posChan,
           out int* sBufs, IntPtr phEventBufferReady, int nBoard);
        
        /// <summary>
        /// Останов сбора данных.
        /// </summary>
        /// <param name="sBufs"></param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioStopSbor(int* sBufs, int nBoard);

        /// <summary>
        /// Останов сбора данных.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioStopMpSbor( int nBoard);
        
        /// <summary>
        /// Останов сбора данных.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioStopADC( int nBoard);

        /// <summary>
        /// Функция возвращает количество полученных прерываний (собранных буферов).
        /// Программа MioGetReadyBuf  возвращает значение счетчика прерываний. 
        /// После загрузки драйвера режим подсчета прерываний запрещен, значение счетчика прерываний равно 0. 
        /// После установки режима подсчета прерываний (MioSetModeCalcInterrupt()), 
        /// счетчик будет инкрементироваться при получении драйвером прерывания, 
        /// пока режим подсчета прерываний не будет сброшен. 
        /// Инкремент счетчика прерываний производится в программе обработки прерываний драйвера PCI9080. 
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>Значение счетчика прерываний, тип int.</returns>
        [DllImport(DllName)]
        public static extern int MioGetReadyBuf( int nBoard);

        /// <summary>
        /// Функция возвращает количество выданных прерываний
        /// </summary>
        /// <param name="nBoard"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int MioGetReadyInt(int nBoard);

        #endregion

        #region Функции управления прерываниями


        /// <summary>
        /// Сброс прерывания, подготовка для приема следующего прерывания .
        /// Программа MioResetIntr должна быть вызвана при установке события  в сигнальное состояние.
        /// </summary>
        /// <param name="phEventBufferReady">событие</param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioResetIntr(IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// Разрешение прерываний по готовности буфера .
        /// Программа MioEnableIntr разрешает прерывания по готовности блока.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioEnableIntr ( int nBoard);

        /// <summary>
        /// Запрещение прерываний по готовности буфера .
        /// Программа MioDisableIntr запрещает прерывания по готовности блока.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DllName)]
        public static extern void MioDisableIntr ( int nBoard);
        
        #endregion

    }

    /// <summary>
    /// коды ошибки функции MioRunSbor
    /// </summary>
    public enum RunSborErrors
    {
        /// <summary>
        /// функция выполнена успешно
        /// </summary>
        Success = 0,
        /// <summary>
        /// ошибка при выполнении функции
        /// </summary>
        Failed = -1,
        /// <summary>
        /// ошибка выделения памяти для буферов сбора
        /// </summary>
        AllocMemory	= 100,
        /// <summary>
        /// ошибка отрытия драйвера PLX
        /// </summary>
        DriverPLX	=101,
        /// <summary>
        /// ошибка открытия канала DMA в драйвере PLX
        /// </summary>
        DMAPLX		=102	
    }


    /// <summary>
    /// значения коэффициентов усиления
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
