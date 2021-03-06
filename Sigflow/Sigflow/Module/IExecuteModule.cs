﻿

namespace Sigflow.Module
{
    public interface IExecuteModule : IModule
    {
        /// <summary>
        /// Выполнение операции модулем. 
        /// Возвращает true, если он совершил полезную работу по обработке сигнала,
        /// false - в случае если такой работы нет, или модуль вообще никогда не обрабатывает сигнал,
        /// null - для модулей которые независимо ни от чего всегда выполняют работу (например генератор).
        /// </summary>
        /// <returns></returns>
        bool? Execute();
    }
}
