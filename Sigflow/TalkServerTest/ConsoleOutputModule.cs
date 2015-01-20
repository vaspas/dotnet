using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace TalkServerTest
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

        private int count;
        private void WriteToConsole(T[] data)
        {
            count++;

            Console.SetCursorPosition(0,Console.CursorTop);

            Console.Write(count);
        }
    }
}
