using System;

namespace Sigflow.Performance
{
    public interface IBeat
    {
        event Action Impulse;
    }
}
