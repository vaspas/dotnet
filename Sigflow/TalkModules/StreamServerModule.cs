
using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace TalkModules
{
    /// <remarks>
    /// INSPECTED 02/05/2013
    /// </remarks>
    public class StreamServerModule:IMasterModule, IExecuteModule
    {
        public string IpAddress { get; set; }

        public int Port { get; set; }

        public Action<Exception> OnException { get; set; }

        public ISignalReader<byte> In { get; set; }

        public int GetClientsCount()
        {
            var s = _server;
            return s == null ? 0 : s.ClientsCount;
        }

        public bool? Execute()
        {
            var data=In.Take();

            if(data==null)
                return false;

            _server.Write(data);

            //не возвращаем, т.к. массив данных при записи может попасть в буффер
            //In.Put(data);

            return true;
        }

        private TalkDotNET.TalkStreamServer _server;
        public bool Start()
        {
            _server = new TalkDotNET.TalkStreamServer
            {
                Host = IpAddress,
                Port = Port,
                OnException = e =>
                                  {
                                      var a = OnException;
                                      if (a != null)
                                          a(e);
                                  }
            };

            _server.Start();

            return true;
        }

        public void BeforeStop()
        {}

        public void AfterStop()
        {
            if(_server==null)
                return;
            _server.Stop();
            _server = null;
        }
    }
}
