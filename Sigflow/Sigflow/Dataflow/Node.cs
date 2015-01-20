
using System.Collections.Generic;

namespace Sigflow.Dataflow
{
    public class Node<T>: INode<T>
        where T : struct
    {
        private readonly Dictionary<string, MultiWriter<T>> _writers=new Dictionary<string, MultiWriter<T>>();

        public ISignalWriter<T> Create(string id)
        {
            var writer = new MultiWriter<T>();

            _writers.Add(id,writer);

            return writer;
        }

        public void Connect(string id, ISignalWriter<T> writer)
        {
            _writers[id].Add(writer);
        }
    }
}
