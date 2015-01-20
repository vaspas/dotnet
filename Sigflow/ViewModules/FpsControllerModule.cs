using System;
using Sigflow.Module;

namespace ViewModules
{
    public class FpsControllerModule : IExecuteModule
    {
        public bool? Execute()
        {
            if ((DateTime.Now.Ticks - _ticks) < (1f / Fps) * 10000000)
                return false;
            
            _ticks = DateTime.Now.Ticks;
            OnRedraw();

            return false;
        }

        public float Fps { get; set; }
        
        private long _ticks;

        public Action OnRedraw { get; set; }

    }
}
