
using System;

namespace Sigflow.Dataflow
{
    public class NodeDecorator<T> : INode<T>
        where T : struct
    {
        public INode<T> Internal { get; set; }

        public Func<ISignalWriter<T>, ISignalWriter<T>> CreateDecorate { get; set; }

        public Func<ISignalWriter<T>, ISignalWriter<T>> ConnectDecorate { get; set; }

        public ISignalWriter<T> Create(string id)
        {
            return CreateDecorate(
                Internal.Create(id));
        }

        public void Connect(string id, ISignalWriter<T> writer)
        {
            Internal.Connect(id, ConnectDecorate(writer));
        }
    }
}
