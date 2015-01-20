
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
    public class Raw13WriterModule : IExecuteModule
    {
        public ITalkWriter<IPacket> Writer { get; set; }

        public ISignalReader<float> In { get; set; }

        public int BaseNumber { get; set; }

        public bool? Execute()
        {
            var data = In.Take();
            if(data==null)
                return false;

            var packet = PacketFactory.Create<Raw13Packet>();

            packet.BaseNumber = BaseNumber;
            packet.SpectrData = data;
            
            Writer.Write(packet);

            In.Put(data);

            return true;
        }
    }
}
