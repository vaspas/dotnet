using Sigflow.Module;
using TalkDotNET.Interfaces;

namespace TalkModules
{
    public class TalkConnectorModule:IMasterModule
    {
        public ITalkConnector Connector { get; set; }

        public bool Start()
        {
            Connector.Connect();

            return true;
        }

        public void BeforeStop()
        {
        }

        public void AfterStop()
        {
            Connector.Disconnect();
        }
    }
}
