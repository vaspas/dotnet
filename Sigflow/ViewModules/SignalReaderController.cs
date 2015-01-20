using System;
using Sigflow.Dataflow;

namespace ViewModules
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// INSPECTED 02/05/2013
    /// </remarks>
    public class SignalReaderController<T> : ISignalReaderController, IDecorator<ISignalReader<T>>, ISignalReader<T>
        where T:struct
    {
        public ISignalReader<T> Internal { get; set; }

        public bool Readed { get; set; }

        public event Action OnReaded = delegate { };

        private void OnReadedMethod()
        {
            if (!Readed)
                return;
            
            OnReaded();
        }

        /// <summary>
        /// Чтение данных в массив. После чтения данные исчезают из канала.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ReadTo(T[] data)
        {
            Readed = Internal.ReadTo(data);
            OnReadedMethod();
            return Readed;
        }

        /// <summary>
        /// Кол-во данных.
        /// </summary>
        /// <returns></returns>
        public int Available
        {
            get { return Internal.Available; }
        
        }


        /// <summary>
        /// Возвращает размер блока по умочанию, т.е. тот который записывается.
        /// </summary>
        public int? NextBlockSize
        {
            get { return Internal.NextBlockSize; }
        }

        /// <summary>
        /// Получение данных. После выполнения данные исчезают из канала.
        /// </summary>
        /// <returns></returns>
        public T[] Take()
        {
            var data = Internal.Take();
            Readed = data != null;
            OnReadedMethod();
            return data;
        }

        /// <summary>
        /// Положить обратно взятый массив данных.
        /// </summary>
        /// <returns></returns>
        public void Put(T[] data)
        {
            Internal.Put(data);
        }

        /// <summary>
        ///Пропустить данные.
        /// </summary>
        public void TrySkip(int size)
        {
            Internal.TrySkip(size);
        }

    }
}
