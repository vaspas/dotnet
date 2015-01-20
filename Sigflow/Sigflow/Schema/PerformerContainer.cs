using System.Collections.Generic;
using Sigflow.Performance;

namespace Sigflow.Schema
{
    internal class PerformerContainer : Dictionary<string, object>
    {
        public Performer Performer { get; set; }

        public string Id { get; set; }
    }
}
