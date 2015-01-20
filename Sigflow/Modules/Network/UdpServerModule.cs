using System;
using System.Net.Sockets;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules.Network
{
    /// <summary>
    /// Рассылка udp сообщений на указанный хост, широковещательная или групповая.
    /// Размер буфера для отправки соответствует размеру входного массива данных.
    /// Для передачи нескольких каналов данных необходимо передавать в модуль мультиплектированные данные.
    /// </summary>
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class UdpServerModule : IExecuteModule, IMasterModule
    {
        public ISignalReader<byte> In { get; set; }

        public int Port { get; set; }

        public string Host { get; set; }

        public short Ttl { get; set; }

        public Action<SocketException> OnException { get; set; }


        private UdpClient _client;

        private void GenerateOnException(SocketException ex)
        {
            var onException = OnException;
            if (onException != null)
                onException(ex);
        }

        public bool? Execute()
        {
            if(In.Available==0 || _client==null)
                return false;
            
            var data=In.Take();
            
            try
            {
                _client.Send(data, data.Length);
            }
            catch (SocketException ex)
            {
                GenerateOnException(ex);
            }

            In.Put(data);
            
            return true;
        }

        public bool Start()
        {
            try
            {
                _client = new UdpClient(Host, Port);
                _client.Ttl = Ttl;
            }
            catch (SocketException ex)
            {
                GenerateOnException(ex);

                return false;
            }

            return true;
        }

        public void AfterStop()
        {

            if (_client == null)
                return;

            try
            {
                _client.Close();
            }
            catch (SocketException ex)
            {
                GenerateOnException(ex);
            }
        }

        public void BeforeStop()
        {
        }
    }
}
