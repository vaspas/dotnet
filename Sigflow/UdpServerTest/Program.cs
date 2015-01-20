using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules;
using Modules.Generator;
using Modules.Network;
using Sigflow.Dataflow;
using Sigflow.Performance;

namespace UdpServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Номер порта клиента:");
            var port=int.Parse(Console.ReadLine());
            Console.WriteLine("Хост клиента:");
            var host=Console.ReadLine();
            Console.WriteLine("Размер блока данных:");
            var blockSize = int.Parse(Console.ReadLine());

            var p=new Performer();

            var masterFrequency = new MasterFrequencyModule
                                      {
                                          IntervalMilliseconds = 2
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

            var server = new UdpServerModule
                             {
                                 Host = host,
                                 Port = port,
                                 Ttl=1,
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

            p.Start();

            Console.ReadLine();
            p.Stop();
            Console.ReadLine();
        }
    }
}
