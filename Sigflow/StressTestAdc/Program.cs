using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StressTestAdc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start Mio4400? y,n");
            if (Console.ReadLine() == "y")
            {
                var gains1 = new [] 
                { 
                    IncModules.Mio4400.GainValues.Gain0,
                    IncModules.Mio4400.GainValues.Gain10,
                    IncModules.Mio4400.GainValues.Gain20,
                    IncModules.Mio4400.GainValues.Gain30,
                };

                var adc1 = new IncModules.Mio4400.Mio4400ModuleInt
                {
                    BoardNumber = 0,
                    ChannelsCount = 4,
                    DeviceType = IncModules.IncDevices.INK1010,
                    Out = new Sigflow.Dataflow.Block<int>(),
                    BlockSize = 1024,
                    Frequency = 400000,
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

                var random=new Random((int)DateTime.Now.Ticks);
                var started = false;
                var th1 = new Thread(o => 
                {
                    while (true)
                    {
                        Thread.Sleep(random.Next(2000));

                        lock (adc1)
                        {
                            if (started)
                            {
                                adc1.BeforeStop();
                                adc1.AfterStop();
                            }
                            else
                                adc1.Start();
                            started = !started;
                        }
                    }
                });
                th1.Priority = ThreadPriority.Highest;
                th1.Start();

                ThreadPool.QueueUserWorkItem(o =>
                {
                    while (true)
                    {
                        //if (started)
                        {
                            //Thread.Sleep(1);
                        //    continue;
                        }

                        lock (adc1)
                        {
                           // if (started)
                            //    continue;

                            for (int i = 0; i < 4; i++)                            
                                adc1.SetupAmplification(gains1[random.Next(4)], i);
                            
                        }
                    }
                }, null);

                ThreadPool.QueueUserWorkItem(o =>
                {
                    while (true)
                    {
                        for (int i = 0; i < 4; i++)  
                            adc1.GetAbsVolt(i);
                    }
                }, null); 
            }

            Console.WriteLine("start Inc824? y,n");
            if (Console.ReadLine() == "y")
            {
                var gains2 = new[] 
                { 
                    IncModules.Ink824.GainValues.Gain_0,
                    IncModules.Ink824.GainValues.Gain_10,
                    IncModules.Ink824.GainValues.Gain_20,
                    IncModules.Ink824.GainValues.Gain_30,
                };

                var adc2 = new IncModules.Ink824.Ink824ModuleInt
                {
                    BoardNumber = 1,
                    ChannelsCount = 8,
                    DeviceType = IncModules.IncDevices.INK1210,
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

                var random2 = new Random((int)DateTime.Now.Ticks);
                var started2 = false;
                var th2 = new Thread(o =>
                {
                    while (true)
                    {
                        //Thread.Sleep(random2.Next(2000));

                        lock (adc2)
                        {
                            if (started2)
                            {
                                adc2.Stop();
                            }
                            else
                                adc2.Start();
                            started2 = !started2;
                        }
                    }
                });
                th2.Priority = ThreadPriority.Highest;
                th2.Start();

                ThreadPool.QueueUserWorkItem(o =>
                {
                    while (true)
                    {
                        if (started2)
                        {
                            //Thread.Sleep(1);
                            continue;
                        }

                        lock (adc2)
                        {
                            if (started2)
                                continue;

                            for (int i = 0; i < 8; i++)
                                adc2.SetupAmplification(gains2[random2.Next(4)], i);

                        }
                    }
                }, null);

                ThreadPool.QueueUserWorkItem(o =>
                {
                    while (true)
                    {
                        for (int i = 0; i < 8; i++)  
                            adc2.GetAbsVolt(i);
                    }
                }, null); 

                Console.WriteLine("started!");
            }

            Console.ReadLine();
        }
    }
}
