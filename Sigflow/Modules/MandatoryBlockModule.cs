
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules
{
    /// <summary>
    /// Модуль обязательно выдающий блок данных при запросе.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MandatoryBlockModule<T>:IExecuteModule
        where T : struct 
    {
        public MandatoryBlockModule()
        {
            UseInputBlockSize = true;
        }

        public ISignalReader<T> In { get; set; }

        public ISignalWriter<T> Out { get; set; }

        /// <summary>
        /// Размер блока данных.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// Использовать размер последнего полученного блока данных.
        /// </summary>
        public bool UseInputBlockSize { get; set; }

        private T[] _data=new T[0];

        private int _lastBlockSize;

        public bool? Execute()
        {
            var data = In.Take();

            if(data!=null)
            {
                Out.Write(data);
                In.Put(data);
                _lastBlockSize = data.Length;
            }
            else if (UseInputBlockSize && _lastBlockSize != 0)
            {
                if(_lastBlockSize!=_data.Length)
                    _data = new T[_lastBlockSize];

                Out.Write(_data);
            }
            else if(BlockSize>0)
            {
                if (BlockSize != _data.Length)
                    _data = new T[BlockSize];

                Out.Write(_data);
            }

            return null;
        }
    }
}
