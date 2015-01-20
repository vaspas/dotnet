using System;
using Sigflow.Dataflow;
using Sigflow.Module;
using System.Runtime.InteropServices;

namespace Modules
{
    /// <summary>
    /// Выделяет из блока часть данных.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class SubBlockModule<T>:IExecuteModule
        where T:struct 
    {
        public int BlockSize { get; set; }

        public int StartIndex { get; set; }

        private readonly object _sync=new object();
        public void SetupThreadSafe(int blockSize, int startIndex)
        {
            lock (_sync)
            {
                BlockSize = blockSize;
                StartIndex = startIndex;
            }
        }

        private T[] _data=new T[0];

        public ISignalReader<T> In { get; set; }
        public ISignalWriter<T> Out { get; set; }

        public bool? Execute()
        {
            if (In.Available == 0)
                return false;

            int blockSize;
            int startIndex;

            lock (_sync)
            {
                blockSize = BlockSize;
                startIndex = StartIndex;
            }

            if(_data.Length!=blockSize)
                _data=new T[blockSize];

            var data = In.Take();

            if (data.Length - startIndex >= blockSize)
            {
                Buffer.BlockCopy(data, startIndex * Marshal.SizeOf(typeof(T)), _data, 0, blockSize * Marshal.SizeOf(typeof(T)));
                Out.Write(_data);
            }

            In.Put(data);

            return true;
        }
    }
}
