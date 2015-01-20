using System;
using TapeDrawing.Core.Instruments;

namespace TapeDrawingWinForms.Instruments
{
    abstract class Instrument<T> : IInstrument
    {
        public T ConcreteInstrument { get; set; }

        public virtual void Dispose()
        {
            if(ConcreteInstrument is IDisposable)
                (ConcreteInstrument as IDisposable).Dispose();
        }
    }
}
