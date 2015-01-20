
namespace Sigflow.Dataflow
{
    public interface IChannel:ISignalReader,ISignalWriter
    {
    }

    public interface IChannel<T> : IChannel, ISignalReader<T>, ISignalWriter<T>
        where T:struct
    {
    }
}
