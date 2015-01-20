
using System;

namespace TapeImplement
{
    /// <summary>
    /// Позиция с промежуточным буфером.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BufferedScalePosition<T>:IScalePosition<T>
    {
        public IScalePosition<T> Buffer { get; set; }
        private IScalePosition<T> _result;
        public IScalePosition<T> Result
        {
            get { return _result; }
            set
            {
                if (_result != null)
                    _result.PositionChanged -= OnPositionChanged;

                _result = value;

                if (_result != null)
                    _result.PositionChanged += OnPositionChanged;
            }
        }

        private readonly object _sync = new object();
        private volatile bool _activated;

        public T From 
        {
            get { return _result.From; }
        }
        public T To 
        {
            get { return _result.To; }
        }

        public void Set(T from, T to)
        {
            lock (_sync)
            {
                Buffer.Set(from, to);
                _activated = false;
            }
            
        }

        public void Activate()
        {
            if (_activated)
                return;

            lock (_sync)
            {
                _result.Set(Buffer.From, Buffer.To);
            }

            _activated = true;
        }

        private void OnPositionChanged()
        {
            PositionChanged();
        }

        public event Action PositionChanged = delegate { };

    }

}
