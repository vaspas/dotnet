using System;
using System.Collections.Generic;
using System.Linq;
using TapeImplement.ObjectRenderers;

namespace ComparativeTapeTest.Tapes.Types
{
    class RegionsSource
    {
        private readonly List<Region> _regions=new List<Region>();

        public void Add(Region region)
        {
            _regions.Add(region);
        }

        public IObjectSource<T> As<T>() where T : class
        {
            return new Source<T> { Src = this };
        }

        class Source<TData> : IObjectSource<TData> where TData : class
        {
            public RegionsSource Src;

            public IEnumerable<TData> GetData(int from, int to)
            {
                return Src._regions.FindAll(r => r.From < to && r.To > from)
                    .OfType<TData>();
            }

            public IEnumerable<TData> GetData()
            {
                throw new NotImplementedException();
            }
        }
    }
}
