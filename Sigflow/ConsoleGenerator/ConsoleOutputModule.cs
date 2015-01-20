using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace ConsoleGenerator
{
    class ConsoleOutputModule<T>:IExecuteModule
        where T:struct 
    {
        public bool? Execute()
        {
            var data = In.Take();

            if (data == null)
                return false;
            
            WriteToConsole(data);

            In.Put(data);

            return true;
        }

        public ISignalReader<T> In { get; set; }

        private void WriteToConsole(T[] data)
        {
            Console.WriteLine();
            foreach (T t in data)
            {
                Console.Write(t);
                Console.Write(" ");
            }
        }
    }
}
