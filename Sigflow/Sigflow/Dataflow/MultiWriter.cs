using System.Collections.Generic;

namespace Sigflow.Dataflow
{
    public class MultiWriter<T> : List<ISignalWriter<T>>, ISignalWriter<T>
        where T:struct 
    {
        public void Add(ISignalWriter writer)
        {
            base.Add((ISignalWriter<T>)writer);
        }

        public void Write(T[] data)
        {
            ForEach(w=>w.Write(data));
        }
    }
}
