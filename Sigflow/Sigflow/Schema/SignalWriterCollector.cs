using System.Collections.Generic;
using Sigflow.Dataflow;

namespace Sigflow.Schema
{
    public class SignalWriterCollector<T> : ISignalWriterCollector, ISignalWriter<T>
        where T:struct 
    {
        private readonly List<ISignalWriter<T>> _writers=new List<ISignalWriter<T>>();

        public void Add(ISignalWriter writer)
        {
            _writers.Add((ISignalWriter<T>)writer);
        }

        public void Write(T[] data)
        {
            _writers.ForEach(w => w.Write(data));
        }
    }
}
