using System;
using System.Runtime.InteropServices;

namespace IncModules.Ink824
{
    /// <summary>
    /// Класс-обертка для функций библиотеки "Инкомтех".
    /// </summary>
    public static unsafe class Wrapper
    {
        /// <summary>
        /// Название файла библиотеки.
        /// </summary>
        public const string DLL_NAME = "Ink824.dll";

        #region функции инициализации устройства и библиотеки

        /// <summary>
        /// Инициализация ИНК424.
        /// Производится сброс АЦП, сброс синтезатора частоты модуля. 
        /// Устанавливаются коэффициенты усиления  для всех каналов  0 db.  
        /// Устанавливается частота 50 000 Гц. 
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioInit(int nBoard);

        /// <summary>
        /// Выключить питание ИНК424.
        /// Программа MioClose выполняет останов АЦП (запись значения 0 в регистр CMD), 
        /// выключает питание всех каналов (запись значения 0 в регистр PWDN).
        /// </summary>
        /// <param name="nBoard">номер устройства, тип int</param>
        [DllImport(DLL_NAME)]
        public static extern void MioClose(int nBoard);

        /// <summary>
        /// Сброс ИНК424.
        /// Программа MioReset производит сброс АЦП. 
        /// Устанавливаются коэффициенты усиления  для всех каналов  0 db.  
        /// Устанавливается частота 50 000 Гц.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioReset(int nBoard);

        /// <summary>
        /// Загрузка проекта ИНК424.
        /// Программа MioLaodPrj производит загрузку проекта в контроллер модуля ИНК424. 
        /// Загрузка проекта производится обычно при начальной инициализации системы сбора. 
        /// Если выполняется в другое время, после загрузки необходимо выполнить инициализацию (MioInit)
        /// </summary>
        /// <param name="filename">имя файла проекта</param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioLoadPrj(string filename, int nBoard);

        /// <summary>
        /// Определение номеров устройств ИНК424.
        /// </summary>
        /// <param name="nDev"></param>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetNumDev(ref int nDev);

        #endregion

        #region Функции выдачи конфигурационных параметров

        /// <summary>
        /// Получить частоту задающего генератора АЦП.
        /// Программа возвращает  значение частоты задающего генератора АЦП, указанное в конфигурации. 
        /// </summary>
        /// <param name="nBoard">номер устройства, тип int</param>
        /// <returns>Частота задающего генератора, герц, тип double</returns>
        [DllImport(DLL_NAME)]
        public static extern double MioGetQuartz(int nBoard);

        /// <summary>
        /// Получить вес младшего разряда АЦП.
        /// Программа возвращает  значение веса младшего разряда АЦП, указанное в конфигурации. 
        /// </summary>
        /// <param name="nBoard">номер устройства, тип int</param>
        /// <returns>Вес младшего разряда, вольт, тип double</returns>
        [DllImport(DLL_NAME)]
        public static extern double MioGetAbsVolt(int nBoard);

        /// <summary>
        /// Получить разрядность АЦП.
        /// Программа возвращает  значение разрядности АЦП, указанное в конфигурации.
        /// </summary>
        /// <param name="nBoard">номер устройства, тип int</param>
        /// <returns>Разрядность АЦП, тип int</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetBitWide(int nBoard);

        #endregion
        
        #region функции установки и опроса параметров сбора

        /// <summary>
        /// Чтение таблицы выбора каналов.
        /// Программа MioGetTab производит чтение таблицы выбора каналов ИНК424.
        /// </summary>
        /// <param name="n">количество каналов</param>
        /// <param name="Ntab">таблица выбора каналов </param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>Тип BYTE ,  количество каналов</returns>
        [DllImport(DLL_NAME)]
        public static extern byte MioGetTab(byte n,byte[] Ntab, int nBoard);

        /// <summary>
        /// Запись таблицы выбора каналов.
        /// Программа MioSetTab производит запись таблицы выбора каналов.
        /// </summary>
        /// <param name="n">количество каналов</param>
        /// <param name="Ntab">таблица выбора каналов </param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetTab(byte n,byte[] Ntab, int nBoard);

        /// <summary>
        /// Чтение кода усиления канала.
        /// Программа MioGetGain производит чтение регистра усиления канала ИНК424.
        /// </summary>
        /// <param name="n">адрес регистра усиления канала </param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>Тип BYTE ,  значение кода усиления</returns>
        [DllImport(DLL_NAME)]
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
        [DllImport(DLL_NAME)]
        public static extern void MioSetGain(byte n,byte gain, int nBoard);

        /// <summary>
        /// Установка частоты дискретизации, fFreq- частота в Гц/
        /// </summary>
        /// <param name="fFreq">значение частоты выборки в Гц, тип float</param>
        /// <param name="nBoard">номер устройства, тип int</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetFreq(float fFreq, int nBoard);

        /// <summary>
        /// Установка кода частоты дискретизации в синтезатор частот.
        /// Программа MioSetFreqi выполняет запись кода частоты в синтезатор частот АЦП. 
        /// Код частоты должен быть подготовлен программой MioGetCodFreq().                
        /// </summary>
        /// <param name="CodFreq">значение кода частоты, тип int </param>
        /// <param name="nBoard">номер устройства, тип int</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetFreqi(int CodFreq, int nBoard);

        /// <summary>
        /// Определение кода частоты.
        /// Программа MioGetCodFreq определяет код установки частоты синтезатора ИНК424.
        /// </summary>
        /// <param name="fFreq">частоты выборки в Гц, тип double  </param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>Тип int ,  значение кода установки частоты синтезатора частот</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetCodFreq(double fFreq, int nBoard);

        /// <summary>
        /// Установка времени срабатывания.
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minites"></param>
        /// <param name="seconds"></param>
        /// <param name="nBoard"></param>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int MioSetTime(int hours, int minites, int seconds, int nBoard);

        /// <summary>
        /// Установка кода времени срабатывания.
        /// Программа MioSetCodTime производит запись значения двоично-десятичного кода времени в BRAM
        /// </summary>
        /// <param name="time">время в двоично-десятичном коде  </param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetCodTime(int time, int nBoard);

        /// <summary>
        /// Определение кода времени срабатывания.
        /// Программа MioGetCodTime производит чтение значения двоично-десятичного кода времени из BRAM .
        /// </summary>
        /// <param name="time">время в двоично-десятичном коде  </param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioGetCodTime(ref int time, int nBoard);

        /// <summary>
        /// Чтение регистра метки времени.
        /// Программа MioGetCodTime производит чтение значения двоично-десятичного кода времени из регистра метки времени.
        /// </summary>
        /// <param name="time">время в двоично-десятичном коде  </param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioGetMarkTime(ref int time, int nBoard);

        /// <summary>
        /// Определение размера буфера драйвера.
        /// Программа MioGetBufferSize возвращает размер буфера сбора, выделенного при загрузке драйвера. 
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>размер буфера сбора в байтах</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetBufferSize( int nBoard);

        /// <summary>
        /// Определение максимального количества буферов сбора, заданного размера.
        /// Программа MioGetNumBuff возвращает количество блоков указанного размера, 
        /// которые могут быть размещены в буфере сбора. 
        /// Для вычисления количества блоков размер буфера сбора делится на сумму размера блока и размера дескриптора блока.
        /// </summary>
        /// <param name="size">размер блока в байтах</param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>количество блоков заданного размера</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetNumBuff(int size, int nBoard);
        
        #endregion

        #region Функции установки режимов сбора
        
        /// <summary>
        /// Установка режима внутренней синхронизации.
        /// Программа MioSetInternalSync производит установку режима внутренней синхронизации, 
        /// очисткой бита 2 регистра SWR. Функция должна быть вызвана до вызова функций, 
        /// устанавливающих частоту дискретизации. 
        /// При вычислении кода частоты принимается, что активен генератор тактовой частоты 80 МГц.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetInternalSync( int nBoard);

        /// <summary>
        /// Установка режима внешней синхронизации.
        /// Программа MioSetExternalSync производит установку режима внешней синхронизации, 
        /// установкой бита 2 регистра SWR. 
        /// Функция должна быть вызвана до вызова функций, устанавливающих частоту дискретизации. 
        /// При вычислении кода частоты принимается, что активен генератор тактовой частоты 50 МГц.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetExternalSync( int nBoard);

        /// <summary>
        /// установка режима безусловного запуска.
        /// Программа MioSetInternalSbor производит установку режима безусловного запуска, очистка бита 7 регистра SWR. 
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetInternalSbor( int nBoard);

        /// <summary>
        /// установка режима запуска о внешнему сигналу.
        /// Программа MioSetExternalSbor производит установку режима внешнего сбора, установка бита 7 регистра SWR. 
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetExternalSbor( int nBoard);

        /// <summary>
        /// установка режима сбора по коду времени.
        /// Программа MioSetTimeSbor производит установку режима сбора по коду времени, очистка бита 7 регистра SWR, 
        /// установка бита 2 регистра SWR.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetTimeSbor( int nBoard);

        #endregion

        #region Функции запуска и останова сбора
        
        /// <summary>
        /// Cбор данных в режиме безусловного запуска.
        /// Программа MioRunInternalSbor производит запуск сбора данных в режиме chaining DMA, 
        /// безусловный запуск сбора. 
        /// </summary>
        /// <param name="SizeBl">размер блока сбора,  32-хразрядных слов, тип int </param>
        /// <param name="NumBl">количество блоков (размером SizeBl) в host буфере выгрузки, тип int</param>
        /// <param name="NumChan">количество каналов, тип int</param>
        /// <param name="SBufs">указатель на адрес буфера сбора, заполняется при выделении памяти, тип int </param>
        /// <param name="hEventBufferReady">событие – собрано SizeBlockInt блоков</param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>код ошибки, тип int</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunInternalSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// Cбор в режиме запуска по внешнему сигналу разрешения работы АЦП.
        /// Программа MioRunExternalSbor производит запуск сбора данных в режиме chaining DMA, режим внешнего старта сбора. 
        /// </summary>
        /// <param name="SizeBl">размер блока сбора,  32-хразрядных слов, тип int </param>
        /// <param name="NumBl">количество блоков (размером SizeBl) в host буфере выгрузки, тип int</param>
        /// <param name="NumChan">количество каналов, тип int</param>
        /// <param name="SBufs">указатель на адрес буфера сбора, заполняется при выделении памяти, тип int </param>
        /// <param name="hEventBufferReady">событие – собрано SizeBlockInt блоков</param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>код ошибки, тип int</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunExternalSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// Cбор в режиме запуска при поступлении заданного кода времени.
        /// Программа MioRunTimeSbor производит запуск сбора данных в режиме chaining DMA, 
        /// режим запуска сбора при поступлении заданного кода времени. 
        /// </summary>
        /// <param name="SizeBl">размер блока сбора,  32-хразрядных слов, тип int </param>
        /// <param name="NumBl">количество блоков (размером SizeBl) в host буфере выгрузки, тип int</param>
        /// <param name="NumChan">количество каналов, тип int</param>
        /// <param name="SBufs">указатель на адрес буфера сбора, заполняется при выделении памяти, тип int </param>
        /// <param name="hEventBufferReady">событие – собрано SizeBlockInt блоков</param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>код ошибки, тип int.</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunTimeSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// Останов сбора данных.
        /// Программа MioStopSbor производит останов сбора данных в режиме non-chaining DMA. 
        /// Выполняется останов  преобразования,  закрывается канал DMA.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioStopSbor( int nBoard);

        /// <summary>
        /// Запуск сбора данных. Block mode DMA.
        /// Программа MioRunSbor производит запуск сбора данных в режиме non-chaining DMA. 
        /// Выделяется память для буфера выгрузки, размер буфера сбора вычисляется:
        /// (SizeBl*NumBl*NumChan*4) байтов.
        /// Готовится канал DMA для выгрузки,  выполняется запуск преобразования. Канал DMA программируется для выполнения передачи  (SizeBl*NumBl*NumChan*4) байтов однократно. Программа используется в тестовом режиме. 
        /// Для выполнения бесконечного сбора используется программа MioRunChainSbor.
        /// </summary>
        /// <param name="SizeBl">размер блока сбора,  32-хразрядных слов, тип int </param>
        /// <param name="NumBl">количество блоков (размером SizeBl) в host буфере выгрузки, тип int</param>
        /// <param name="NumChan">количество каналов, тип int</param>
        /// <param name="SBufs">указатель на адрес буфера сбора, заполняется при выделении памяти, тип int </param>
        /// <param name="hEventBufferReady">событие – собрано SizeBlockInt блоков</param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, out IntPtr hEventBufferReady, int nBoard);

        /// <summary>
        /// Запуск сбора данных. Chain mode DMA.
        /// Программа MioRunChainSbor производит запуск сбора данных в режиме chaining DMA.
        /// Выделяется память для буфера выгрузки и таблицы дескрипторов. 
        /// Размер буфера сбора -(SizeBl*NumBl*NumChan*4) байтов. 
        /// Размер таблицы дескрипторов - (NumBl*4*4) байтов. 
        /// Последний элемент таблицы дескрипторов указывает на начальный, таким образом, организуется кольцевой буфер сбора. 
        /// Готовится канал DMA для выгрузки,  выполняется запуск АЦП. 
        /// После записи каждого блока в буфер происходит прерывание, 
        /// и событие hEventBufferReady переводится  в сигнальное состояние. 
        /// Данные выдаются в виде 32-хразрядных целых чисел.
        /// </summary>
        /// <param name="SizeBl">блока сбора,  32-хразрядных слов, тип int </param>
        /// <param name="NumBl">количество блоков (размером SizeBl) в host буфере выгрузки</param>
        /// <param name="NumChan">количество каналов, тип int</param>
        /// <param name="SBufs">указатель на адрес буфера сбора, заполняется при выделении памяти</param>
        /// <param name="hEventBufferReady">– событие, получено прерывание, очередной блок записан  в буфер</param>
        /// <param name="nBoard">номер устройства</param>
        /// <returns>код ошибки, тип int</returns>
        [DllImport(DLL_NAME)]
        public static extern int MioRunChainSbor(int SizeBl, int NumBl, int NumChan,
           out int* SBufs, out IntPtr hEventBufferReady, int nBoard);

        /// <summary>
        /// Останов сбора данных. Chain mode DMA.
        /// Программа MioStopSbor производит останов сбора данных в режиме chaining DMA. 
        /// Выполняется останов  преобразования, останов работы канала DMA, закрывается канал DMA.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioStopChainSbor( int nBoard);

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
        [DllImport(DLL_NAME)]
        public static extern int MioGetReadyBuf( int nBoard);

        #endregion

        #region Функции управления прерываниями

        /// <summary>
        /// Сброс прерывания, подготовка для приема следующего прерывания .
        /// Программа MioResetIntr должна быть вызвана при установке события  в сигнальное состояние.
        /// </summary>
        /// <param name="hEventBufferReady">событие</param>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioResetIntr(IntPtr phEventBufferReady, int nBoard);

        /// <summary>
        /// Разрешение прерываний по готовности буфера .
        /// Программа MioEnableIntr разрешает прерывания по готовности блока.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioEnableIntr ( int nBoard);

        /// <summary>
        /// Запрещение прерываний по готовности буфера .
        /// Программа MioDisableIntr запрещает прерывания по готовности блока.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioDisableIntr ( int nBoard);

        /// <summary>
        /// Установка режима подсчета прерываний.
        /// Программа MioSetModeCalcInterrupt устанавливает режим подсчета прерываний в драйвере PCI9080.sys. 
        /// Счетчик прерываний в драйвере обнуляется.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioSetModeCalcInterrupt( int nBoard);

        /// <summary>
        /// Сброс режима подсчета прерываний  .
        /// Программа MioSetModeCalcInterrupt сбрасывает режим подсчета прерываний в драйвере PCI9080.sys. 
        /// Счетчик прерываний в драйвере обнуляется.
        /// </summary>
        /// <param name="nBoard">номер устройства</param>
        [DllImport(DLL_NAME)]
        public static extern void MioResetModeCalcInterrupt( int nBoard);
        
        #endregion

        #region Вспомогательные функции 
        
        /// <summary>
        /// Получить код усиления по конфигурации.
        /// </summary>
        /// <param name="gain"></param>
        /// <param name="nBoard"></param>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern int MioGetCodGain(byte gain, int nBoard);

        /// <summary>
        /// Определение частоты внешнего источника тактирования.
        /// </summary>
        /// <param name="Freq"></param>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern void MioGetExternalFreq(ref double Freq, int nBoard);

        /// <summary>
        /// Определение частоты внутреннего источника тактирования.
        /// </summary>
        /// <param name="Freq"></param>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern void MioGetInternalFreq(ref double Freq, int nBoard);

        /// <summary>
        /// опрос готовности DMA.
        /// </summary>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern bool MioDmaReady( int nBoard);

        /// <summary>
        /// Стоп АЦП.
        /// </summary>
        /// <param name="nBoard"></param>
        [DllImport(DLL_NAME)]
        public static extern void MioStopADC( int nBoard);

        #endregion
    }

    /// <summary>
    /// коды ошибки функции MioRunSbor
    /// </summary>
    public enum RunSborErrors: int
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
