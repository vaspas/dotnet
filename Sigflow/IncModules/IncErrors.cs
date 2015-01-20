
namespace IncModules
{
    /// <summary>
    /// Коды ошибок библиотеки "Инкомтех".
    /// </summary>
    public enum IncErrors
    {
        /// <summary>
        /// функция выполнена успешно
        /// </summary>
        IncSuccess = 0,
        /// <summary>
        /// ошибка при выполнении функции
        /// </summary>
        IncFailed = -1,

        /// <summary>
        /// ошибка открытия ключа реестра ...\configuration
        /// </summary>
        IncRegCfgOpen = 1,
        /// <summary>
        /// ошибка чтения  конфигурационного значения из реестра 
        /// </summary>
        IncRegCfgQuery = 2,
        /// <summary>
        /// конфигурационное значения, считанное из реестра вне диапазона
        /// </summary>
        IncRegCfgValue = 3,
        /// <summary>
        /// функция получила неверный параметр
        /// </summary>
        IncInvalidParam = 4,
        /// <summary>
        /// не найдено устройство PLX
        /// </summary>
        IncPlxDevFind = 5,
        /// <summary>
        /// не совпадает количество PLX на шине и в конфигурации 
        /// </summary>
        IncPlxInvalidNumber = 6,
        /// <summary>
        /// драйвер PLX не открыт
        /// </summary>
        IncPlxDrvOpen = 7,
        /// <summary>
        /// ошибка выделения памяти
        /// </summary>
        IncMemoryAllocation = 8,
        /// <summary>
        /// ошибка открытия файла
        /// </summary>
        IncFileOpen = 9,
        /// <summary>
        /// ошибка чтения из файла
        /// </summary>
        IncFileRead = 10,
        /// <summary>
        /// ошибка структуры файла, файл не микропрограмма
        /// </summary>
        IncFileStruct = 11,
        /// <summary>
        /// неверный код процессорного элемента
        /// </summary>
        IncInvalidCodPe = 12,
        /// <summary>
        /// Ошибка загрузки ини микропрограммы
        /// </summary>
        IncLoadIniMp = 13,
        /// <summary>
        /// ошибка открытия файла архитектуры
        /// </summary>
        IncFileAchOpen = 14,
        /// <summary>
        /// ошибка чтения из файла архитектуры
        /// </summary>
        IncFileAchRead = 15,
        /// <summary>
        /// незнакомое расширение имени файла микропрограммы
        /// </summary>
        IncExtFileMp = 16,
        /// <summary>
        /// ошибка чтения заголовка файла микропрограммы
        /// </summary>
        IncHeaderMpRead = 17,
        /// <summary>
        /// ошибка заголовка файла микропрограммы
        /// </summary>
        IncInvalidHeaderMp = 18,
        /// <summary>
        /// ошибка Pinit, программа инициализации не освободила ресурс
        /// </summary>
        IncCfgTimeOut = 19,
        /// <summary>
        /// таймаут ожидания устройства исчерпан
        /// </summary>
        IncWaitTimeOut = 20,
        /// <summary>
        /// ошибка освобождения мьютекса ожидания устройства
        /// </summary>
        IncReleaseWaitDev = 21,
        /// <summary>
        /// неверная контрольная сумма EEPROM 
        /// </summary>
        IncEepromCheckSum = 22,
        /// <summary>
        /// ошибка чтения  конфигурационного значения из EEPROM 
        /// </summary>
        IncEepromCfgQuery = 23,
        /// <summary>
        /// конфигурационное значения, считанное из EEPROM вне диапазона
        /// </summary>
        IncEepromCfgValue = 24,
        /// <summary>
        /// в библиотеке установлен немзвестный серийный номер(не 9080 и не9054)
        /// </summary>
        IncUnknownChip = 25,
        /// <summary>
        /// ошибка выделения dma буфера
        /// </summary>
        IncCommonBuffer = 26,
        /// <summary>
        /// PARAM EEPOM не готов, проект не загружен
        /// </summary>
        IncReadyParEeprom = 27,
        /// <summary>
        /// ошибка загрузки LDR микропрограммы
        /// </summary>
        IncLoadLdrMp = 28,
        /// <summary>
        /// неизвестный код подсистемы
        /// </summary>
        IncUnknownSubSystem = 29,
        /// <summary>
        /// выход по таймауту
        /// </summary>
        IncTimout = 30,
        /// <summary>
        /// внутренняя ошибка драйвера 1
        /// </summary>
        IncInternalError = 100
    }
}
