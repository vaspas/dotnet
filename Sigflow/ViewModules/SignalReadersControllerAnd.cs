using System;
using System.Collections.Generic;
using System.Linq;

namespace ViewModules
{
    /// <remarks>
    /// INSPECTED 02/05/2013
    /// </remarks>
    public class SignalReadersControllerAnd : ISignalReaderController
    {
        private readonly List<ISignalReaderController> _controllers = new List<ISignalReaderController>();

        public void Add(ISignalReaderController controller)
        {
            controller.OnReaded += OnReadedMethod;
            _controllers.Add(controller);
        }

        public void Remove(ISignalReaderController controller)
        {
            controller.OnReaded -= OnReadedMethod;
            _controllers.Remove(controller);
        }


        public bool Readed 
        {
            get { return _controllers.All(c => c.Readed); }
            set 
            {
                foreach (var c in _controllers)
                    c.Readed = value;
            }
        }

        private void OnReadedMethod()
        {
            if (!Readed)
                return;
            
            OnReaded();
        }

        public event Action OnReaded = delegate { };
    }
}
