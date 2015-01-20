using Sigflow.Module;
using Sigflow.Performance;

namespace Sigflow.Schema
{
    public class SchemaContainerThreadSafe:SchemaContainer
    {
        private readonly object _sync=new object();

        internal override void Add(PerformerContainer container)
        {
            lock (_sync)
            {
                base.Add(container);
            }
        }

        public override T Get<T>(Performer performer, string id)
        {
            lock (_sync)
            {
                return base.Get<T>(performer, id);
            }
        }

        public override T Get<T>(string performerid, string id)
        {
            lock (_sync)
            {
                return base.Get<T>(performerid, id);
            }
        }

        public override T Get<T>(string id)
        {
            lock (_sync)
            {
                return base.Get<T>(id);
            }
        }

        public override void Exclude(IModule module)
        {
            lock (_sync)
            {
                base.Exclude(module);
            }
        }

        public override void Set<T>(Performer performer, T element, string id)
        {
            lock (_sync)
            {
                base.Set(performer, element, id);
            }
        }

        public override void ChangeModule(IModule from, IModule to)
        {
            lock (_sync)
            {
                base.ChangeModule(from, to);
            }
        }

        public override void ChangeModuleList<T>(T from, T to)
        {
            lock (_sync)
            {
                base.ChangeModuleList(from, to);
            }
        }

        public override bool Start()
        {
            lock (_sync)
            {
                return base.Start();
            }
        }

        public override void Stop()
        {
            lock (_sync)
            {
                base.Stop();
            }
        }
    }
}
