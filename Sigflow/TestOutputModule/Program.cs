using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules;
using Sigflow.Dataflow;
using Sigflow.Performance;

namespace TestOutputModule
{
    class Program
    {
        [STAThread] 
        static void Main(string[] args)
        {
            var p=new Performer();

            var block = new Block<float>();
            p.AddModule(new IppModules.Generator.WhiteNoiseGeneratorModuleFloat
                                {
                                    BlockSize = 1000,
                                    Value = 1,
                                    Out = block
                                });

            var buffer = new Buffer<int>();
            p.AddModule(new IppModules.ConvertFloatToIntModule
                                {
                                    In = block,
                                    Out = buffer,
                                    ScaleFactor = -30
                                });

            var queue = new ThreadSafeQueue<int> {MaxCapacity = 100};
            p.AddModule(new SetBlockSizeModule<int>
                              {
                                  BlockSize = 2048,
                                  In=buffer,
                                  Out=queue
                              });
            
            

            var asio= new SoundBlasterModules.Asio.AsioOutputModule
                           {
                               BufferSize = 2048,
                               DriverNumber = 0,
                               OnException = ex=>Console.WriteLine(ex.Message),
                               SampleRate = 192000
                           };
            asio.In.Add(queue);
            p.AddModule(asio);

            var master = new Modules.Generator.MasterFrequencyModule { IntervalMilliseconds = 1 };
            p.Beat = master;
            p.AddModule(master);

            p.Start();

            Console.ReadLine();

            p.Stop();
        }
    }
}
