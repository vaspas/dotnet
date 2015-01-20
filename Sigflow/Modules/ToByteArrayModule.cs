using System;
using System.Runtime.InteropServices;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules
{
    ///<summary>
    /// Для перевода данных в массив байт.
    /// </summary>
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class ToByteArrayModule<T>:IExecuteModule
        where T:struct
    {
        public ISignalReader<T> In { get; set; }

        public ISignalWriter<byte> Out { get; set; }

        private byte[] _buffer = new byte[0];

        public bool? Execute()
        {
            if (In.Available == 0)
                return false;

            var data = In.Take();

            if (_buffer.Length != data.Length * Marshal.SizeOf(typeof(T)))
                _buffer = new byte[data.Length * Marshal.SizeOf(typeof(T))];

            Buffer.BlockCopy(data, 0, _buffer, 0, _buffer.Length);

            In.Put(data);

            Out.Write(_buffer);

            return true;
        }
    }
}
