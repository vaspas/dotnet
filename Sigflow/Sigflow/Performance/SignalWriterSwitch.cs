
using Sigflow.Dataflow;

namespace Sigflow.Performance
{
    /// <summary>
    /// Декоратор для вкл/отключения записи данных.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SignalWriterSwitch<T> : ISignalWriter<T>, IDecorator<ISignalWriter<T>>
        where T : struct
    {
        public ISignalWriter<T> Internal { get; set; }

        public bool Enable { get; set; }

        public void Write(T[] data)
        {
            if(Enable)
                Internal.Write(data);
        }
    }
}
