using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Modules;
using Modules.Generator;
using Sigflow.Dataflow;
using Sigflow.Performance;
using TalkModules;

namespace TalkServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Номер порта:");
            var port = int.Parse(Console.ReadLine());
            Console.WriteLine("Размер блока данных:");
            var blockSize = int.Parse(Console.ReadLine());
            Console.WriteLine("Период (мс):");
            var period = int.Parse(Console.ReadLine());

            var p = new Performer();

            var masterFrequency = new MasterFrequencyModule
            {
                IntervalMilliseconds = period
            };
            p.AddModule(masterFrequency);


            var writer = new MultiWriter<byte>();
            var block1 = new Block<byte>();
            writer.Add(block1);
            var block2 = new Block<byte>();
            writer.Add(block2);

            var genBlock = new Block<float>();
            var generator = new IppModules.Generator.CosinusGeneratorModuleFloat
            {
                BlockSize = blockSize,
                Out = genBlock,
                RelativeFrequency = 0.021f,
                Value = 2.514f
            };
            p.AddModule(generator);

            var converter = new ToByteArrayModule<float>
            {
                In = genBlock,
                Out = writer
            };
            p.AddModule(converter);

            var server = new StreamServerModule
            {
                IpAddress = "0.0.0.0",
                Port = port,
                OnException = e => Console.WriteLine(e.Message),
                In = block1
            };
            p.AddModule(server);

            var consoleOutput = new ConsoleOutputModule<byte>
            {
                In = block2
            };
            p.AddModule(consoleOutput);

            p.Beat = masterFrequency;

            var counter = 0;
            masterFrequency.Impulse += () => counter++;


            p.Start();

            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                { Console.Clear();

                    var cl = server.GetClientsCount();
                        Console.Write("clients " + cl);
                        Console.Write(" ");
                        Console.Write(counter);

                    Thread.Sleep(1000);
                }
            }, null);

            Console.ReadLine();
            p.Stop();
            Console.ReadLine();
        }
    }
}
