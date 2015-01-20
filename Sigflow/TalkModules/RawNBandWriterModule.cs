
using System.Collections.Generic;
using System.Linq;
using Sigflow.Dataflow;
using Sigflow.Module;
using TalkDotNET.Interfaces;
using TalkDotNET.Models;
using TalkDotNET.Serialization;

namespace TalkModules
{
    /// <remarks>
    /// INSPECTED 02/05/2013
    /// </remarks>
    public class RawNBandWriterModule : IExecuteModule
    {
        public RawNBandWriterModule()
        {
            In=new List<ISignalReader<float>>();
        }

        public ITalkWriter<IPacket> Writer { get; set; }

        public IList<ISignalReader<float>> In { get; private set; }

        public int DeviceId { get; set; }

        public float StepFrequency { get; set; }

        public float Period { get; set; }

        private int _counter;

        public bool? Execute()
        {
            if(In.Any(r=>r.Available==0))
                return false;

            var packet = PacketFactory.Create<RawNBandPacket>();
            packet.Data=new List<float[]>();

            foreach (var sr in In)
                packet.Data.Add(sr.Take());

            packet.Counter = _counter++;
            packet.DeviceId = DeviceId;
            packet.StepFrequency = StepFrequency;
            packet.Period = Period;

            Writer.Write(packet);

            for (var i = 0; i < In.Count;i++ )
                In[i].Put(packet.Data[i]);
            
            return true;
        }
    }
}
