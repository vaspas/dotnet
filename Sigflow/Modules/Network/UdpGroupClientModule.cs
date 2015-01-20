using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules.Network
{
    /// <summary>
    /// Модуль для приема udp сообщений групповой рассылки.
    /// Также принимает любые другие udp сообщения высылаемые на указанный порт.
    /// Для приема нескольких каналов данных необходимо подключить модуль демультиплектирования данных.
    /// </summary>
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class UdpGroupClientModule : IMasterModule
    {
        public UdpGroupClientModule()
        {
            ThreadPriority = ThreadPriority.Normal;
        }

        public ISignalWriter<byte> Out { get; set; }

        /// <summary>
        /// Номер порта.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Адрес группы.
        /// </summary>
        public string GroupAddress { get; set; }

        public int SubscribeTtl { get; set; }

        public Action<SocketException> OnException { get; set; }


        public ThreadPriority ThreadPriority { get; set; }

        private UdpClient _client;

        private void GenerateOnException(SocketException ex)
        {
            var onException = OnException;
            if (onException != null)
                onException(ex);
        }


        private Thread _thread;

        private void ThreadFunc()
        {
            var endPoint=new IPEndPoint(IPAddress.Any, 0);

            while(true)
            {
                try
                {
                    Out.Write(_client.Receive(ref endPoint));
                }
                catch (SocketException ex)
                {
                    //условие закрытия сокета
                    if (ex.ErrorCode == 10004)
                        return;

                    GenerateOnException(ex);

                    return;
                }
            }
        }

        public bool Start()
        {
            try
            {
                _client = new UdpClient(Port);

                _client.JoinMulticastGroup(IPAddress.Parse(GroupAddress), SubscribeTtl);
            }
            catch (SocketException ex)
            {
                GenerateOnException(ex);
                return false;
            }

            _thread = new Thread(ThreadFunc)
                          {
                              IsBackground = true,
                              Name = "UdpGroupClientModule",
                              Priority = ThreadPriority
                          };
            _thread.Start();

            return true;
        }

        public void AfterStop()
        {
            if (_client == null)
                return;

            try
            {
                _client.DropMulticastGroup(IPAddress.Parse(GroupAddress));
            }
            catch (SocketException ex)
            {
                GenerateOnException(ex);
            }

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
