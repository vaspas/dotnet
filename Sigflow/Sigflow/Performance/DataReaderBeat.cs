using System;
using Sigflow.Dataflow;

namespace Sigflow.Performance
{
    
    public class DataReaderBeat<T>:ISignalReader<T>, IDecorator<ISignalReader<T>>,IBeat
        where T : struct
    {
        public ISignalReader<T> Internal { get; set; }

        public event Action Impulse = delegate { };
        

        /// <summary>
        /// Чтение данных в массив. После чтения данные исчезают из канала.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ReadTo(T[] data)
        {
            Impulse();

            return Internal.ReadTo(data);
        }

        /// <summary>
        /// Кол-во хранящихся отсчетов.
        /// </summary>
        /// <returns></returns>
        public int Available 
        { 
            get
            {
                var res = Internal.Available;

                if (res==0)
                    Impulse();

                return res;
            }
        }


        public int? NextBlockSize
        {
            get
            {
                var res = Internal.NextBlockSize;
                if (res==null)
                    Impulse();
                return res; 
            }
        }

        public T[] Take()
        {
            Impulse();

            return Internal.Take();
        }

        public void Put(T[] data)
        {
            Internal.Put(data);
        }

        public void TrySkip(int size)
        {
            Internal.TrySkip(size);
        }
    }
}
