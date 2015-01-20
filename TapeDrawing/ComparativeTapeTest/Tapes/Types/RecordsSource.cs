using System;
using System.Collections.Generic;
using System.Linq;
using TapeImplement.ObjectRenderers;

namespace ComparativeTapeTest.Tapes.Types
{
    class RecordsSource
    {
        private readonly List<Record> _records = new List<Record>();

        public void Add(Record record)
        {
            _records.Add(record);
        }
        
        public IObjectSource<T> As<T>() where T:class
        {
            return new Source<T> {Src = this};
        }

        class Source<TData>: IObjectSource<TData> where TData:class
        {
            public RecordsSource Src;

            public IEnumerable<TData> GetData(int from, int to)
            {
                return Src._records.FindAll(r => r.Index < to && r.Index > from)
                    .OfType<TData>();
            }

            public IEnumerable<TData> GetData()
            {
                throw new NotImplementedException();
            }
        }
    }
}
