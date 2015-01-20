using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules
{
    /// <summary>
    /// Считывает блок указанного размера. Выдает блок только в случае, когда он полностью заполнен данными.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class SetBlockSizeModule<T>:IExecuteModule
        where T:struct 
    {
        public int BlockSize { get; set; }

        private T[] _data;

        public ISignalReader<T> In { get; set; }
        public ISignalWriter<T> Out { get; set; }

        public bool? Execute()
        {
            var blockSize=BlockSize;

            if(_data==null || _data.Length!=blockSize)
                _data=new T[blockSize];

            if (!In.ReadTo(_data))
                return false;

            Out.Write(_data);

            return true;
        }
    }
}
