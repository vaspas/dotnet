using System;
using Sigflow.Dataflow;

namespace Sigflow.Performance
{
    /// <summary>
    /// Источник импульсов реагирующий на запись данных.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataWriterBeat<T>:ISignalWriter<T>, IDecorator<ISignalWriter<T>>,IBeat
        where T : struct
    {
        public ISignalWriter<T> Internal { get; set; }

        public event Action Impulse = delegate { }; 

        public void Write(T[] data)
        {
            Internal.Write(data);

            Impulse();
        }
    }
}
