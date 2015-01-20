
namespace Sigflow.Dataflow
{
    public interface INode<T>
        where T : struct
    {
        ISignalWriter<T> Create(string id);

        void Connect(string id, ISignalWriter<T> writer);
    }
}
