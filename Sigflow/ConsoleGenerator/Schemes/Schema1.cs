using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IppModules.Generator;
using Modules.Generator;
using Sigflow.Performance;
using Sigflow.Dataflow;

namespace ConsoleGenerator.Schemes
{
    class Schema1
    {
        public Performer Engine { get; set; }

        public void Build()
        {
            var master = new MasterFrequencyModule {IntervalMilliseconds = 100};
            Engine.AddModule(master);
            Engine.Beat = master;

            var buffer = new Buffer<float>();

            Engine.AddModule(new WhiteNoiseGeneratorModuleFloat
                                   {
                                       BlockSize = 16,
                                       Value = (float)Math.Pow(10, 0 / 20),
                                       Out = buffer
                                   });
            /*Engine.Modules.Add(new CosinusGeneratorModuleFloat
                                   {
                                       BlockSize = 16,
                                       Value = (float)(Math.Sqrt(2) * Math.Pow(10, 0 / 20)),
                                       Phase = 0,
                                       RelativeFrequency = 0.2f,
                                       Out = buffer
                                   });*/


            Engine.AddModule(new ConsoleOutputModule<float>
                                   {
                                       In = buffer
                                   });
        }
    }
}
