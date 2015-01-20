
using System;
using Modules.Network;
using Sigflow.Dataflow;
using Sigflow.Performance;

namespace UdpClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Номер порта:");
            var port = int.Parse(Console.ReadLine());
            Console.WriteLine("Хост сервера:");
            var host = (Console.ReadLine());

            var p = new Performer();

            var block = new Block<byte>();
            var sw = new DataWriterBeat<byte>
                         {
                             Internal = block
                         };
            p.Beat = sw;
            

            var client = new UdpClientModule
            {
                Port = port,
                Address = host,
                OnException = e => Console.WriteLine(e.Message),
                Out = sw
            };
            p.AddModule(client);

            var consoleOutput = new ConsoleOutputModule<byte>
                                    {
                                        In = block
                                    };
            p.AddModule(consoleOutput);
            
            p.Start();

            Console.ReadLine();
            p.Stop();
            Console.ReadLine();
        }
    }
}
