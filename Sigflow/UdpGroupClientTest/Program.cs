using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules.Network;
using Sigflow.Dataflow;
using Sigflow.Performance;

namespace UdpGroupClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Номер порта:");
            var port = int.Parse(Console.ReadLine());
            Console.WriteLine("Адрес группы:");
            var host = (Console.ReadLine());

            var p = new Performer();

            var block = new Block<byte>();
            var sw = new DataWriterBeat<byte>
            {
                Internal = block
            };
            p.Beat = sw;


            var client = new UdpGroupClientModule
            {
                Port = port,
                GroupAddress = host,
                SubscribeTtl = 1,
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
