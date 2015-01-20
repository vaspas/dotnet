using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sigflow.Performance
{
    /// <summary>
    /// Непотокобезопасный.
    /// </summary>
    /// <remarks>
    /// INSPECTED 30/04/2013
    /// </remarks>
    public class BeatsAnd:IBeatCollection
    {
        private readonly List<Item> _items=new List<Item>();

        class Item
        {
            public IBeat Beat;
            public bool IsActive;
            public Action Impulse;
        }

        public event Action Impulse = delegate { };

        public int? CountToImpulse { get; set; }

        public void Add(IBeat beat)
        {
            if (_items.Any(i => i.Beat == beat))
                return;

            var item = new Item {Beat = beat};
            item.Impulse = () =>
                               {
                                   item.IsActive = true;
                                   Check();
                               };
            lock(_items)
                _items.Add(item);
            beat.Impulse += item.Impulse;
        }

        public bool Remove(IBeat beat)
        {
            var item = _items.FirstOrDefault(i => i.Beat == beat);

            if(item==null)
                return false;

            item.Beat.Impulse -= item.Impulse;

            lock (_items)
                _items.Remove(item);

            return true;
        }
        
        private void Check()
        {
            lock (_items)
            {
                if (CountToImpulse == null && !_items.All(i => i.IsActive))
                    return;

                if (CountToImpulse != null && _items.Count(i => i.IsActive) < CountToImpulse)
                    return;

                _items.ForEach(i => i.IsActive = false);
            }

            Impulse();
        }


        public bool Contains(IBeat beat)
        {
            return _items.FirstOrDefault(i => i.Beat == beat)!=null;
        }

        public void Clear()
        {
            _items.ConvertAll(i=>i.Beat).ToList()
                .ForEach(b=>Remove(b));
        }

        public int Count
        {
            get { return _items.Count; }
        }


        bool ICollection<IBeat>.IsReadOnly
        {
            get { return false; }
        }

        void ICollection<IBeat>.CopyTo(IBeat[] array, int index)
        {
            var i = index;
            _items.ForEach(e => array[i++] = e.Beat);
        }

        IEnumerator<IBeat> IEnumerable<IBeat>.GetEnumerator()
        {
            return _items.ConvertAll(i => i.Beat).ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.ConvertAll(i => i.Beat).ToList().GetEnumerator();
        }
    }
}
