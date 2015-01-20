
using System;
using Sigflow.Dataflow;
using Sigflow.Module;
using TalkDotNET.Interfaces;

namespace TalkModules
{
    /// <remarks>
    /// INSPECTED 02/05/2013
    /// </remarks>
    public class StreamClientModule : IMasterModule
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public int ReadBlockSize { get; set; }

        public int AutoConnectPeriod { get; set; }

        public int PingPeriod { get; set; }

        public Action<Exception> OnException { get; set; }

        public Action<bool> OnReconnect { get; set; }

        public ISignalWriter<byte> Out { get; set; }

        public bool AsyncConnection { get; set; }
        
        private ITalkStreamClient _client;
        public bool Start()
        {
            var conf = TalkDotNET.TalkStreamClientConfigurator.Create(Host, Port, ReadBlockSize)
                .ThreadSafe()
                .LogExceptions(e =>
                                   {
                                       var a = OnException;
                                       if (a != null)
                                           a(e);
                                   })
                .AutoConnect(AutoConnectPeriod, v =>
                                                    {
                                                        var a = OnReconnect;
                                                        if (a != null)
                                                            a(v);
                                                    })
                .Pinger(PingPeriod);

            if (AsyncConnection)
                conf = conf.AsyncConnection(v =>
                                                {
                                                    var a = OnReconnect;
                                                    if (a != null)
                                                        a(v);
                                                });

            _client = conf.Result;

            _client.OnReceived = data => Out.Write(data);

            _client.Connect();

            return true;
        }

        public void BeforeStop()
        {
            _client.Disconnect();
            _client = null;
        }

        public void AfterStop()
        {
        }
    }
}
