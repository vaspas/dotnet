using System;

namespace Sigflow.Dataflow
{
    public interface IBufferState
    {
        int MaxCapacity { get; }

        bool IsOverflow { get; }

        void DropOverflow();

        int Count { get; }

        event Action OnOverflow;
    }
}
