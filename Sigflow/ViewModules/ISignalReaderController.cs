
using System;

namespace ViewModules
{
    public interface ISignalReaderController
    {
        bool Readed { get; set; }

        event Action OnReaded;
    }
}
