using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IppModules;
using Modules;
using Sigflow.Dataflow;
using Sigflow.Performance;
using SoundBlasterModules.Asio;
using TalkDotNET.Models;
using TalkDotNET.Serialization;
using TalkModules;

namespace TalkStreamDeviceSound
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            Console.WriteLine("talker host");
            var talkerhost = Console.ReadLine();

            Console.WriteLine("talker port");
            var talkerport = int.Parse(Console.ReadLine());


            Console.WriteLine("device");
            var device = int.Parse(Console.ReadLine());

            Console.WriteLine("channel");
            var channel = int.Parse(Console.ReadLine());

            Console.WriteLine("frequency");
            var fq = int.Parse(Console.ReadLine());


            var talker = TalkDotNET.TalkClassConfigurator.Create(talkerhost, talkerport, false)
                .ThreadSafe()
                .LogExceptions(Console.WriteLine)
                .Result;

            talker.Connect(new TestReader());
            talker.Connect();

            var request = PacketFactory.Create<RequestSignalInfoDevicePacket>();
            request.RequestId = (int) DateTime.Now.Ticks;
            request.DeviceId = device;
            request.Channel = channel;
            request.Frequency = fq;
            request.NearestType = 0;

            talker.Write(request);
            
            Console.ReadLine();
        }
    }

    class TestReader: TalkDotNET.Interfaces.ITalkReader<SignalInfoPacket>
    {
        public void OnRead(SignalInfoPacket packet)
        {
            

            if(_packet!=null)
                return;

            _packet = packet;

            new Thread(Start){IsBackground = true, ApartmentState =  ApartmentState.STA}.Start();
        }

        private SignalInfoPacket _packet;
        private void Start()
        {
            var conv = new PacketToStringConverter
                           {
                               Writer=new StringBuilder()
                           };
            conv.Write(_packet);
            
            Console.WriteLine("Ok!");
            Console.WriteLine(conv.Writer.ToString());

            var blocksize = 1024;

            var performer = new Performer();

            var buffer1 = new ThreadSafeQueue<byte> { MaxCapacity = 100 };
            //var buffer2 = new Block<float>();
            //var buffer3 = new Buffer<int>();
            var buffer4 = new ThreadSafeQueue<float> { MaxCapacity = 20,ClearOnOverflow = true};

            var buffer1Decorator = new DataWriterBeat<byte> { Internal = buffer1 };
            performer.Beat = buffer1Decorator;

            int c = 0;
            buffer1Decorator.Impulse += () =>
            {
                Console.Clear();
                Console.Write(c++);
                Console.Write(" ");
                Console.Write(buffer1.Count * 100f / buffer1.MaxCapacity);
                Console.Write("%");
                Console.Write(" ");
                Console.Write(buffer4.Count * 100f / buffer4.MaxCapacity);
                Console.Write("%");
            };

            /*int c = 0;
            buffer1Decorator.Impulse += () =>
            {
                Console.Clear();
                Console.Write(c++);
                Console.Write(" ");
                Console.Write(buffer1.Count * 100f / buffer1.MaxCapacity);
                Console.Write("%");
                Console.Write(" ");
                Console.Write(buffer4.Count * 100f / buffer4.MaxCapacity);
                Console.Write("%");
            };*/

            var client = new StreamClientModule
            {
                AutoConnectPeriod = 1000,
                PingPeriod = 1000,
                Host = _packet.Host,
                Port = _packet.Port,
                OnException = e => Console.WriteLine(e.ToString()),
                OnReconnect = v => { if (v) Console.WriteLine("reconnected"); },
                ReadBlockSize = blocksize * sizeof(float),
                Out = buffer1Decorator
            };
            performer.AddModule(client);


            var converter1 = new FromByteArrayModule<float>
            {
                In = buffer1,
                Out = buffer4
            };
            performer.AddModule(converter1);


            /*var converter2 = new ConvertFloatToIntModule
            {
                In = buffer2,
                Out = buffer4,
                ScaleFactor = -30
            };
            performer.AddModule(converter2);

            var asio = new AsioOutputModule
            {
                BufferSize = blocksize,
                DriverNumber = 0,
                OnException = e => Console.WriteLine(e.ToString()),
                SampleRate = _packet.Frequency
            };
            asio.In.Add(buffer4);
            performer.AddModule(asio);*/

            /*var blockSizeBuilder = new SetBlockSizeModule<float>
                                       {
                                           In = buffer3,
                                           BlockSize = blocksize,
                                           Out = buffer4
                                       };
            performer.AddModule(blockSizeBuilder);*/
            
            var wav = new SoundBlasterModules.WaveApi.Output.WaveOutputModule
                          {
                              BufferSize = blocksize,
                              DriverNumber = 0,
                              OnException = e => Console.WriteLine(e.ToString()),
                              SampleRate = _packet.Frequency,
                              ChannelsCount = 1,
                              In = buffer4
                          };
            performer.AddModule(wav);

            performer.Start();
        }
    }
}
