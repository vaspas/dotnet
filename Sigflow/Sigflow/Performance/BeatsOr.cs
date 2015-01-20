using System;
using System.Collections;
using System.Collections.Generic;

namespace Sigflow.Performance
{
    /// <remarks>
    /// INSPECTED 30/04/2013
    /// </remarks>
    public class BeatsOr:IBeatCollection
    {
        private readonly List<IBeat> _beats = new List<IBeat>();

        public event Action Impulse = delegate { };

        public void Add(IBeat beat)
        {
            if (_beats.Contains(beat))
                return;

            beat.Impulse += OnImpulse;
            _beats.Add(beat);
        }

        public bool Remove(IBeat beat)
        {
            if (!_beats.Contains(beat))
                return false;

            beat.Impulse -= OnImpulse;

            _beats.Remove(beat);

            return true;
        }

        private void OnImpulse()
        {
            Impulse();
        }


        public bool Contains(IBeat beat)
        {
            return _beats.Contains(beat);
        }

        public void Clear()
        {
            _beats.ForEach(b => b.Impulse -= OnImpulse);

            _beats.Clear();
        }

        public int Count
        {
            get { return _beats.Count; }
        }


        bool ICollection<IBeat>.IsReadOnly
        {
            get { return false; }
        }

        void ICollection<IBeat>.CopyTo(IBeat[] array, int index)
        {
            var i = index;
            _beats.ForEach(b=>array[i++]=b);
        }

        IEnumerator<IBeat> IEnumerable<IBeat>.GetEnumerator()
        {
            return _beats.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _beats.GetEnumerator();
        }
    }
}
