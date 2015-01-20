using System;
using Sigflow.Dataflow;

namespace Sigflow.Performance
{
    /// <summary>
    /// Источник импульсов с контролем заполнения буфера.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BufferReaderBeat<T>:ISignalReader<T>, IDecorator<ISignalReader<T>>,IBeat
        where T : struct
    {
        public BufferReaderBeat()
        {
            Load = 0.5f;
        }

        public ISignalReader<T> Internal { get; set; }

        public event Action Impulse = delegate { };

        public float Load { get; set; }

        private void Balance()
        {
            var bs = Internal as IBufferState;

            if(Load>((float)bs.Count/bs.MaxCapacity))
                Impulse();
        }


        /// <summary>
        /// Чтение данных в массив. После чтения данные исчезают из канала.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ReadTo(T[] data)
        {
            Balance();

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
                Balance();

                return Internal.Available;
            }
        }


        public int? NextBlockSize
        {
            get { return Internal.NextBlockSize; }
        }

        public T[] Take()
        {
            Balance();

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
