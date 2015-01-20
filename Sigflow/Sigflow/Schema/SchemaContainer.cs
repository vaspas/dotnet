using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sigflow.Module;
using Sigflow.Performance;

namespace Sigflow.Schema
{
    /// <summary>
    /// Не потокобезопасный класс.
    /// </summary>
    public class SchemaContainer
    {
        private readonly List<PerformerContainer>  _containers = new List<PerformerContainer>();
        
        internal virtual void Add(PerformerContainer container)
        {
            _containers.Add(container);
        }

        public virtual T Get<T>(Performer performer, string id)
        {
            if (!_containers.First(c => c.Performer == performer).Any(o => o.Key == id && o.Value is T))
                return default(T);

            return (T) _containers.First(c => c.Performer == performer)
                           .FirstOrDefault(o => o.Key == id && o.Value is T).Value;
        }

        public virtual T Get<T>(string performerid, string id)
        {
            if (!_containers.First(c => c.Id == performerid).Any(o => o.Key == id && o.Value is T))
                return default(T);

            return (T)_containers.First(c => c.Id == performerid)
                           .FirstOrDefault(o => o.Key == id && o.Value is T).Value;
        }

        public virtual T Get<T>(string id)
        {
            var ids = id.Split(new[] {'.'});
            if (ids.Length > 1 && _containers.Any(c => c.Id == ids[0]))
                return (T) _containers.First(c => c.Id == ids[0])
                               .FirstOrDefault(o => o.Key == id.Remove(0, ids[0].Length + 1) && o.Value is T).Value;

            foreach (var p in _containers)
            {
                if (p.Any(o => o.Key == id && o.Value is T))
                    return (T)p.FirstOrDefault(o => o.Key == id && o.Value is T).Value;
            }

            if (_containers.Any(c => c.Id == id) && typeof(T) == typeof(Performer))
                return (T)(_containers.First(c => c.Id == id).Performer as object);

            return default(T);
        }

        public virtual void Exclude(IModule module)
        {
            foreach (var p in _containers)
            {
                if (!p.Any(o => o.Value == module))
                    continue;

                p.Performer.RemoveModule(module);
                p.Remove(p.First(o=>o.Value==module).Key);
            }
        }
        
        public virtual void Set<T>(Performer performer, T element, string id)
        {
            _containers.First(c => c.Performer == performer).Add(id, element);
        }

        public virtual void ChangeModule(IModule from, IModule to)
        {
            var performer = _containers.First(p => p.ContainsValue(from));

            //меняем свойства
            ChangeModulePropertiesHelper.Change(from, to);

            var pair = performer.First(p => p.Value == from);
            performer[pair.Key] = to;

            //меняем модули в исполнителе
            performer.Performer.ChangeModule(from, to);
        }

        public virtual void ChangeModuleList<T>(T from, T to) where T : class,IList
        {
            var performer = _containers.First(p => p.ContainsValue(from));

            //меняем свойства модулей
            for (var i = 0; i < from.Count; i++)
                ChangeModulePropertiesHelper.Change((IModule) from[i], (IModule) to[i]);

            var pair = performer.First(p => p.Value == from);
            performer[pair.Key] = to;

            //меняем модули в исполнителе
            for (var i = 0; i < from.Count; i++)
                performer.Performer.ChangeModule((IModule) from[i], (IModule) to[i]);
        }

        public virtual bool Start()
        {
            var started = true;
            foreach (var p in _containers)
            {
                if (!p.Performer.Start())
                    started = false;
            }

            return started;
        }

        public virtual void Stop()
        {
            _containers.ForEach(p => p.Performer.Stop());
        }
    }
}
