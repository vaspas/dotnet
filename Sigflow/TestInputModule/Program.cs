using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules;
using Sigflow.Dataflow;
using Sigflow.Performance;

namespace TestInputModule
{
    class Program
    {
        //[STAThread]
        static void Main(string[] args)
        {
            var p = new Performer();

            var queue = new ThreadSafeQueue<int> { MaxCapacity = 100 };

            var sw = new DataWriterBeat<int>
            {
                Internal = queue
            };
            p.Beat = sw;

            var asio = new SoundBlasterModules.Asio.AsioInputModule
            {
                BufferSize = 2048,
                DriverNumber = 0,
                OnException = ex => Console.WriteLine(ex.Message),
                SampleRate = 192000
            };
            asio.Out.Add(sw);
            p.AddModule(asio);



            p.AddModule(new ConsoleOutputModule<int> { In = queue });
            
            p.Start();

            Console.ReadLine();

            p.Stop();
        }
    }
}
