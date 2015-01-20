
namespace Sigflow.Dataflow
{
    public interface ISignalWriter
    {}

    public interface ISignalWriter<in T> : ISignalWriter
         where T : struct
    {
        void Write(T[] data);
    }
}
