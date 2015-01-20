using Sigflow.Dataflow;

namespace Sigflow.Schema
{
    interface ISignalWriterCollector
    {
        void Add(ISignalWriter writer);
    }
}
