using System;
using Sigflow.Module;

namespace ViewModules
{
    public class FpsSignalReadControllerModule : IExecuteModule
    {
        public ISignalReaderController SignalReaderController { get; set; }

        public bool? Execute()
        {
            if (SignalReaderController==null || !SignalReaderController.Readed || (DateTime.Now.Ticks - _ticks) < (1f / Fps) * 10000000)
                return false;
            
            _ticks = DateTime.Now.Ticks;
            OnRedraw();

            SignalReaderController.Readed = false;

            return false;
        }

        public float Fps { get; set; }
        
        private long _ticks;

        public Action OnRedraw { get; set; }

    }
}
