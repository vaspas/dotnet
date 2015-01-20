using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules
{
    /// <summary>
    /// Модуль для задержки данных при их отсутствии.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelayerModule<T> : IExecuteModule
        where T : struct 
    {
        public ISignalReader<T> In { get; set; }

        public ISignalWriter<T> Out { get; set; }

        public int Delay { get; set; }
        
        private int _counter;

        public bool? Execute()
        {
            if(_counter>0)
            {
                _counter--;
                return null;
            }

            var data = In.Take();

            if (data == null)
                _counter = Delay;
            else
            {
                Out.Write(data);
                In.Put(data);
            }

            return null;
        }
    }
}
