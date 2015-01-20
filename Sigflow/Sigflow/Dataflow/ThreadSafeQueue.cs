using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sigflow.Dataflow
{
    /// <summary>
    /// Канал для потокобезопасной передачи блока данных между модулями.
    /// Потокобезопасность обеспечивается для режима чтения/записи.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class ThreadSafeQueue<T> : IBufferState, IChannel<T>
        where T:struct
    {
        public ThreadSafeQueue()
        {
            MaxCapacity = 100;
        }

        private readonly Queue<T[]> _buffers = new Queue<T[]>();

        public int MaxCapacity { get; set; }

        public bool IsOverflow { get; private set; }

        public bool ClearOnOverflow { get; set; }

        public void DropOverflow()
        {
            IsOverflow = false;
        }

        public int Count
        {
            get { return _count; }
        }

        private int _availableSize;

        private int _count;
        
        public bool ReadTo(T[] data)
        {
            if (_count == 0)
                return false;

            T[] src;

            lock (_buffers)
            {
                if (_count == 0)
                    return false;

                if (_buffers.Peek().Length != data.Length)
                    return false;

                Interlocked.Add(ref _availableSize, -_buffers.Peek().Length);
                Interlocked.Decrement(ref _count);

                src = _buffers.Dequeue();
            }

            Buffer.BlockCopy(src, 0, data, 0, src.Length*Marshal.SizeOf(typeof(T)));

            lock (_pool)
            {
                _pool.Add(src);
            }

            return true;

        }

        public int Available
        {
            get { return _availableSize; }
        }

        public int? NextBlockSize
        {
            get 
            {
                if (_count == 0)
                    return null;

                lock (_buffers)
                {
                    if (_count == 0)
                        return null;

                    return _buffers.Peek().Length;
                } 
            }
        }

        private T[] _taked;
        public T[] Take()
        {
            if (_count == 0)
                return null;

            lock (_buffers)
            {
                if (_count == 0)
                    return null;

                Interlocked.Add(ref _availableSize, -_buffers.Peek().Length);
                Interlocked.Decrement(ref _count);

                _taked = _buffers.Dequeue();

                return _taked;
            }
        }

        public void Put(T[] data)
        {
            if (data == null || _taked != data)
                return;

            lock (_pool)
                _pool.Add(_taked);
        }

        public void Write(T[] data)
        {
            if (_count >= MaxCapacity)
            {
                IsOverflow = true;
                OnOverflow();

                if(!ClearOnOverflow)
                    return;

                DropBuffer();
            }

            var buffer = GetFromPool(data.Length);

            Buffer.BlockCopy(data,0,buffer,0, data.Length*Marshal.SizeOf(typeof(T)));

            lock (_buffers)
            {
                _buffers.Enqueue(buffer);

                Interlocked.Increment(ref _count);
                Interlocked.Add(ref _availableSize, data.Length);
            }
        }

        private readonly List<T[]> _pool = new List<T[]>();

        private T[] GetFromPool(int length)
        {
            lock (_pool)
            {
                var fromCache = _pool.FirstOrDefault(a => a.Length == length);

                if (fromCache != null)
                {
                    _pool.Remove(fromCache);
                    return fromCache;
                }
            }

            return new T[length];
        }

        public void DropBuffer()
        {
            if (_count == 0)
                return;

            lock (_buffers)
            {
                if (_count == 0)
                    return;

                lock(_pool)
                    _pool.AddRange(_buffers);
                _buffers.Clear();
                
                _count=0;
                _availableSize = 0;
            }
        }

        public void TrySkip(int size)
        {
            if (_count == 0)
                return;

            lock (_buffers)
            {
                if (_count == 0)
                    return;

                var toSkip = size;
                var skipped = _buffers.Peek().Length;
                while (skipped <= toSkip)
                {
                    var buf = _buffers.Dequeue();
                    lock (_pool)
                        _pool.Add(buf);

                    toSkip -= skipped;
                    skipped = _buffers.Peek().Length;
                    
                    Interlocked.Decrement(ref _count);
                    Interlocked.Add(ref _availableSize, -skipped);
                }
            }
        }

        public event Action OnOverflow = delegate { };
    }
}
