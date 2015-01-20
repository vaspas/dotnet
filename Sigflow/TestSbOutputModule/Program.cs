using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IppModules;
using Modules;
using Sigflow.Dataflow;
using Sigflow.Performance;

namespace TestSbOutputModule
{
    class Program
    {
        [STAThread] 
        static void Main(string[] args)
        {
            var p = new Performer();

            var block1 = new Block<float>();
            /*p.AddModule(new IppModules.Generator.WhiteNoiseGeneratorModuleFloat
            {
                BlockSize =1024,
                Value = 1,
                Out = block1
            });*/
            p.AddModule(new IppModules.Generator.CosinusGeneratorModuleFloat
            {
                BlockSize = 1024,
                Value = 2,
                RelativeFrequency = 0.005f,
                Out = block1
            });
            /*p.AddModule(new Modules.Generator.ZeroDataModuleFloat()
            {
                BlockSize = 1024,
                Out = block1
            });*/

            var block2 = new Block<float>();
            /*p.AddModule(new IppModules.Generator.WhiteNoiseGeneratorModuleFloat
            {
                BlockSize = 1024,
                Value = 1,
                Out = block2
            });*/
            p.AddModule(new IppModules.Generator.CosinusGeneratorModuleFloat
            {
                BlockSize = 1024,
                Value = 1,
                RelativeFrequency = 0.02f,
                Out = block2
            });
            /*p.AddModule(new Modules.Generator.ZeroDataModuleFloat()
            {
                BlockSize = 1024,
                Out = block2
            });*/

            var block3 = new Block<float>();
            var multiplexer = new MultiplexerModuleFloat
            {
                Out = block3
            };
            multiplexer.In.Add(block1);
            //multiplexer.In.Add(block2);
            p.AddModule(multiplexer);

            var queue = new ThreadSafeQueue<float> { MaxCapacity = 100 };
            p.AddModule(new SetBlockSizeModule<float>
            {
                BlockSize = 1024,
                In = block3,
                Out = queue
            });

            var beat = new BufferReaderBeat<float>
                           {   
                               Load = 0.5f,
                               Internal = queue
                           };


            var asio = new SoundBlasterModules.WaveApi.Output.WaveOutputModule
            {
                BufferSize = 1024,
                DriverNumber = 0,
                OnException = ex => Console.WriteLine(ex.Message),
                SampleRate = 32567,
                ChannelsCount = 1
            };
            asio.In=beat;
            p.AddModule(asio);

            p.Beat = beat;

            p.Start();

            Console.ReadLine();

            p.Stop();
        }
    }
}
