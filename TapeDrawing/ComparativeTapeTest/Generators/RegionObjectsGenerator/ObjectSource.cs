using System;
using System.Collections.Generic;
using ComparativeTapeTest.Tapes.Types;
using TapeImplement.ObjectRenderers;

namespace ComparativeTapeTest.Generators.RegionObjectsGenerator
{
    class ObjectSource<TData> : IObjectSource<TData>, ISourceId where TData : Region
    { 
        public ObjectSource()
        {
            Data = new List<Region>();
        }

        public string Id { get; set; }

        public List<Region> Data { get; private set; }

        public IEnumerable<TData> GetData(int from, int to)
        {
            return Data.FindAll(r => r is TData && r.From < to && r.To > from)
                .ConvertAll(r => (TData) r);
        }

        public IEnumerable<TData> GetData()
        {
            throw new NotImplementedException();
        }
    }
}
