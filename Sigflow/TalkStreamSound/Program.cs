using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IppModules;
using Modules;
using Sigflow.Dataflow;
using Sigflow.Performance;
using SoundBlasterModules.Asio;
using TalkModules;

namespace TalkStreamSound
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("host");
            var host = Console.ReadLine();

            Console.WriteLine("port");
            var port = int.Parse(Console.ReadLine());

            Console.WriteLine("frequency");
            var fq = int.Parse(Console.ReadLine());

            Console.WriteLine("blocksize"); 
            var blocksize = int.Parse(Console.ReadLine());

            var performer = new Performer();
            
            var buffer1 = new ThreadSafeQueue<byte> { MaxCapacity =100};
            var buffer2 = new Block<float>();
            var buffer3 = new Buffer<int>();
            var buffer4 = new ThreadSafeQueue<int> { MaxCapacity = 100 };

            var buffer1Decorator = new DataWriterBeat<byte> {Internal = buffer1};
            performer.Beat = buffer1Decorator;

            int c = 0;
            buffer1Decorator.Impulse += () =>
                                            {
                                                Console.Clear();
                                                Console.Write(c++);
                                                Console.Write(" ");
                                                Console.Write(buffer1.Count*100f / buffer1.MaxCapacity);
                                                Console.Write("%");
                                                Console.Write(" ");
                                                Console.Write(buffer4.Count*100f / buffer4.MaxCapacity);
                                                Console.Write("%");
                                            };

            var client = new StreamClientModule
                             {
                                 AutoConnectPeriod = 1000,
                                 PingPeriod = 2000,
                                 Host = host,
                                 Port = port,
                                 OnException = e=>Console.WriteLine(e.ToString()),
                                 OnReconnect = v => { if (v) Console.WriteLine("reconnected"); },
                                 ReadBlockSize = blocksize*sizeof(float),
                                 Out = buffer1Decorator
                             };
            performer.AddModule(client);

            
            var converter1 = new FromByteArrayModule<float>
                                {
                                    In = buffer1,
                                    Out=buffer2
                                };
            performer.AddModule(converter1);

            
            /*var converter2 = new ConvertFloatToIntModule
                                 {
                                     In = buffer2,
                                     Out = buffer4,
                                     ScaleFactor = -30
                                 };
            performer.AddModule(converter2);*/

            
            /*var blockSizeBuilder = new SetBlockSizeModule<int>
                                       {
                                           In = buffer3,
                                           BlockSize = blocksize,
                                           Out = buffer4
                                       };
            performer.AddModule(blockSizeBuilder);*/
            
            /*var asio = new AsioOutputModule
                           {
                               BufferSize = blocksize,
                               DriverNumber = 0,
                               OnException = e => Console.WriteLine(e.ToString()),
                               SampleRate = fq
                           };
            asio.In.Add(buffer4);
            performer.AddModule(asio);*/


            var wav = new SoundBlasterModules.WaveApi.Output.WaveOutputModule
            {
                BufferSize = blocksize,
                DriverNumber = 0,
                OnException = e => Console.WriteLine(e.ToString()),
                SampleRate = fq,
                ChannelsCount = 1,
                In = buffer2
            };
            performer.AddModule(wav);



            performer.Start();

            Console.ReadLine();

            performer.Stop();

        }
    }
}
