using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestIncAdc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start Mio4400? y,n");
            while (Console.ReadLine() == "y")
            {
                Console.WriteLine("board number:");
                int b = int.Parse(Console.ReadLine());

                var adc1 = new IncModules.Mio4400.Mio4400ModuleInt
                {
                    BoardNumber = b,
                    ChannelsCount = 4,
                    DeviceType = IncModules.IncDevices.INK1010,
                    Out = new Sigflow.Dataflow.Block<int>(),
                    BlockSize = 1024,
                    Frequency=400000,                    
                    GainValues = new[]
                {
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain0
                }
                };

                Console.WriteLine("initialize " + adc1.InitDevice());
                adc1.OnMessage = m => Console.WriteLine(m);
                adc1.Start();
                Console.WriteLine("started!");

                Console.WriteLine("start Mio4400? y,n");
            }

            Console.WriteLine("start INK1210 with mio? y,n");
            while (Console.ReadLine() == "y")
            {
                Console.WriteLine("board number:");
                int b = int.Parse(Console.ReadLine());

                var adc3 = new IncModules.Mio4400.Mio4400ModuleInt
                {
                    BoardNumber = b,
                    ChannelsCount = 4,
                    DeviceType = IncModules.IncDevices.INK1210,
                    Out = new Sigflow.Dataflow.Block<int>(),
                    BlockSize = 1024,
                    Frequency = 51200,
                    GainValues = new[]
                {
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain0
                }
                };

                Console.WriteLine("initialize " + adc3.InitDevice());
                adc3.OnMessage = m => Console.WriteLine(m);
                adc3.Start();
                Console.WriteLine("started!");

                Console.WriteLine("start INK1210 with mio? y,n");
            }

            Console.WriteLine("start 824 with mio? y,n");
            while (Console.ReadLine() == "y")
            {
                Console.WriteLine("board number:");
                int b = int.Parse(Console.ReadLine());

                var adc3 = new IncModules.Mio4400.Mio4400ModuleInt
                {
                    BoardNumber = b,
                    ChannelsCount = 4,
                    DeviceType = IncModules.IncDevices.INK824,
                    Out = new Sigflow.Dataflow.Block<int>(),
                    BlockSize = 1024,
                    Frequency = 51200,
                    GainValues = new[]
                {
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain0
                }
                };

                Console.WriteLine("initialize " + adc3.InitDevice());
                adc3.OnMessage = m => Console.WriteLine(m);
                adc3.Start();
                Console.WriteLine("started!");

                Console.WriteLine("start INK824 with mio? y,n");
            }

            Console.WriteLine("start INK1210 using Inc824? y,n");
            while(Console.ReadLine() == "y")
            {
                Console.WriteLine("board number:");
                int b = int.Parse(Console.ReadLine());

                var adc2 = new IncModules.Ink824.Ink824ModuleInt
                {
                    BoardNumber = b,
                    ChannelsCount = 8,
                    DeviceType = IncModules.IncDevices.INK1210,
                    Out = new Sigflow.Dataflow.Block<int>(),
                    BlockSize = 1024,
                    Frequency=51200,
                    ExternalSync=false,
                    StartMode=IncModules.StartMode.Internal,                    
                    GainValues = new[]
                {
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0
                }
                };

                Console.WriteLine("initialize " + adc2.InitDevice());
                adc2.OnMessage = m => Console.WriteLine(m);
                adc2.Start();
                Console.WriteLine("started!");

                Console.WriteLine("start INK1210 using Inc824? y,n");
            }

            Console.WriteLine("start Inc824? y,n");
            while (Console.ReadLine() == "y")
            {
                Console.WriteLine("board number:");
                int b = int.Parse(Console.ReadLine());

                var adc2 = new IncModules.Ink824.Ink824ModuleInt
                {
                    BoardNumber = b,
                    ChannelsCount = 8,
                    DeviceType = IncModules.IncDevices.INK824,
                    Out = new Sigflow.Dataflow.Block<int>(),
                    BlockSize = 1024,
                    Frequency = 51200,
                    ExternalSync = false,
                    StartMode = IncModules.StartMode.Internal,
                    GainValues = new[]
                {
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_0
                }
                };

                Console.WriteLine("initialize " + adc2.InitDevice());
                adc2.OnMessage = m => Console.WriteLine(m);
                adc2.Start();
                Console.WriteLine("started!");

                Console.WriteLine("start Inc824? y,n");
            }

            Console.ReadLine();
        }
    }
}
