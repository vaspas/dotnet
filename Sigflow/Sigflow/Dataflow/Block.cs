using System;
using System.Runtime.InteropServices;

namespace Sigflow.Dataflow
{
    /// <summary>
    /// Канал для передачи блока данных между модулями по ссылке.
    /// Не потокобезопасный.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class Block<T>:IChannel<T>
        where T:struct
    {
        private T[] _block;
        
        public bool ReadTo(T[] data)
        {
            if(Available!=data.Length)
                return false;

            Buffer.BlockCopy(_block, 0, data, 0, _block.Length * Marshal.SizeOf(typeof(T)));

            _block = null;

            return true;
        }


        public int Available
        {
            get { return _block == null ? 0 : _block.Length; }
        }

        public int? NextBlockSize
        {
            get { return _block==null ? default(int?): _block.Length; }
        }

        public T[] Take()
        {
            var t = _block;
            _block = null;
            return t;
        }

        public void Put(T[] data)
        {
        }

        public void Write(T[] data)
        {
            _block = data;
        }

        public void TrySkip(int size)
        {
            if (_block != null && _block.Length <= size)
                _block = null;
        }
    }
}
