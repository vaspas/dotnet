using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sigflow.Dataflow
{
    /// <summary>
    /// Канал для передачи блока данных между модулями через буффер,
    /// с возможностью получения указанного размера данных.
    /// Не потокобезопасный.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class Buffer<T> : IChannel<T>
        where T:struct
    {
        private readonly Queue<Item> _buffers = new Queue<Item>();
        
        class Item
        {
            public int Available
            {
                get { return Data.Length - From; }
            }

            public int From;
            public T[] Data;
        }

        public bool ReadTo(T[] data)
        {
            if (Available < data.Length)
                return false;
            
            var datasize = Marshal.SizeOf(typeof(T));

            var readed = 0;
            while (readed < data.Length)
            {
                var toRead = data.Length - readed;
                
                var nextBuffer = _buffers.Peek();

                if(toRead<_buffers.Peek().Available)
                {
                    Buffer.BlockCopy(nextBuffer.Data, nextBuffer.From * datasize, data, readed * datasize, toRead * datasize);
                    nextBuffer.From += toRead;
                    readed += toRead;
                }
                else
                {
                    Buffer.BlockCopy(nextBuffer.Data, nextBuffer.From * datasize, data, readed * datasize, nextBuffer.Available * datasize);
                    readed += nextBuffer.Available;
                    _pool.Add(_buffers.Dequeue());
                }
            }

            return true;
        }
         
        public int Available
        {
            get { return _buffers.Sum(p => p.Available); }
        }

        public int? NextBlockSize
        {
            get { return _buffers.Count == 0 ? default(int?) : _buffers.Peek().Available; }
        }

        private Item _taked;
        public T[] Take()
        {
            if (_buffers.Count == 0)
                return null;

            if(_buffers.Peek().From>0)
            {
                var newData = new T[_buffers.Peek().Available];
                ReadTo(newData);
                return newData;
            }

            _taked = _buffers.Dequeue();
             return _taked.Data;
        }

        public void Put(T[] data)
        {
            if(_taked.Data==data)
            {
                _pool.Add(_taked);
                _taked = null;
            }
        }

        public void Write(T[] data)
        {
            var item = GetItem(data.Length);

            Buffer.BlockCopy(data, 0, item.Data, 0, data.Length*Marshal.SizeOf(typeof (T)));

            item.From = 0;

            _buffers.Enqueue(item);
        }

        private readonly List<Item> _pool = new List<Item>();

        private Item GetItem(int length)
        {
            var fromCache = _pool.FirstOrDefault(a => a.Data.Length == length);

            if(fromCache!=null)
            {
                _pool.Remove(fromCache);
                return fromCache;
            }

            return new Item {Data = new T[length]};
        }

        public void TrySkip(int size)
        {
            var skipped = 0;
            while (skipped < size && _buffers.Count!=0)
            {
                var toSkip = size - skipped;

                if (toSkip < _buffers.Peek().Available)
                {
                    _buffers.Peek().From += toSkip;
                    skipped += toSkip;
                }
                else
                {
                    skipped += _buffers.Peek().Available;
                    _pool.Add(_buffers.Dequeue());
                }
            }
        }
    }
}
