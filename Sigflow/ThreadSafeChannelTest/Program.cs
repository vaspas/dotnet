using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sigflow.Dataflow;

namespace ThreadSafeChannelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            _channel = new ThreadSafeQueue<int>() { MaxCapacity = 100};
            //_channel = new Buffer<int>();

            var writeThread = new Thread(WriteFunc)
                                  {
                                      IsBackground = true
                                  };

            var readThread = new Thread(TakeFunc)
                                 {
                                     IsBackground = true
                                 };

            writeThread.Start();
            readThread.Start();

            for (var i = 0; i < _circles;i++ )
            {
                Thread.Sleep(100);
                Console.SetCursorPosition(0,0);
                //Console.Write(i);
                Console.Clear();
                Console.Write((_channel as ThreadSafeQueue<int>).Count);
                if((_channel as ThreadSafeQueue<int>).IsOverflow)
                    Console.Write(" overflow!!");
            }

            writeThread.Abort();
            readThread.Abort();

            Console.ReadLine();
        }

        private static IChannel<int> _channel;

        private static int _circles = 1000000;

        private static void WriteFunc()
        {
            var buffer = new int[1024];
            while(true)
            {
                Thread.Sleep(2);
                _channel.Write(buffer);
            }
        }

        private static void TakeFunc()
        {
            while (true)
            {
                Thread.Sleep(3);
                var r = _channel.Take();
                _channel.Put(r);
            }
        }

        private static void ReadFunc()
        {
            var buffer = new int[1024];
            while (true)
            {
                Thread.Sleep(1);
                _channel.ReadTo(buffer);
            }
        }
    }
}
