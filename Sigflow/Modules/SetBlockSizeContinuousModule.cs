using System;
using System.Runtime.InteropServices;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules
{
    /// <summary>
    /// Считывает блок указанного размера. Выдает блок каждый раз при получении данных.
    /// </summary>
    public class SetBlockSizeContinuousModule<T>:IExecuteModule
        where T:struct 
    {
        public int BlockSize { get; set; }

        private T[] _primaryData=new T[0];
        private T[] _secondaryData = new T[0];

        public ISignalReader<T> In { get; set; }
        public ISignalWriter<T> Out { get; set; }

        private readonly int _dataSize = Marshal.SizeOf(typeof (T));

        public bool? Execute()
        {
            var blockSize=BlockSize;

            if (_primaryData.Length != blockSize)
            {
                var newData=new T[blockSize];

                var toCopy = Math.Min(_primaryData.Length, blockSize);
                Buffer.BlockCopy(_primaryData, _dataSize * (_primaryData.Length - toCopy), newData, _dataSize * (blockSize - toCopy), _dataSize * toCopy);
                _primaryData = newData;
                _secondaryData = new T[blockSize];
            }

            if (In.NextBlockSize == null)
                return false;

            if (In.NextBlockSize >= blockSize)
            {
                if (!In.ReadTo(_primaryData))
                    return false;
            }
            else
            {
                var readedData = In.Take();

                if (readedData == null)
                    return false;

                //копируем со смещением
                Buffer.BlockCopy(readedData, 0, _secondaryData, _dataSize * (blockSize - readedData.Length), _dataSize * readedData.Length);
                Buffer.BlockCopy(_primaryData, _dataSize * readedData.Length, _secondaryData, 0, _dataSize * (_primaryData.Length - readedData.Length));

                //меняем буферы
                var temp = _primaryData;
                _primaryData = _secondaryData;
                _secondaryData = temp;

                In.Put(readedData);
            }

            Out.Write(_primaryData);

            return true;
        }
    }
}
