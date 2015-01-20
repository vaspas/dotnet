using System;
using System.Runtime.InteropServices;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules
{
    /// <summary>
    /// Для перевода данных из массива байт.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class FromByteArrayModule<T> : IExecuteModule
        where T:struct
    {
        public ISignalReader<byte> In { get; set; }

        public ISignalWriter<T> Out { get; set; }

        private T[] _data = new T[0];

        public bool? Execute()
        {
            if (In.Available == 0)
                return false;

            var buffer = In.Take();

            var dataLength = buffer.Length / Marshal.SizeOf(typeof(T));
            if (_data.Length != dataLength)
                _data = new T[dataLength];

            Buffer.BlockCopy(buffer, 0, _data, 0, dataLength * Marshal.SizeOf(typeof(T)));

            In.Put(buffer);

            Out.Write(_data);

            return true;
        }
    }
}
