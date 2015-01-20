using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules
{
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class ChannelsCommutatorModule<T> : IExecuteModule
        where T : struct 
    {
        public ChannelsCommutatorModule()
        {
            In=new List<ISignalReader<T>>();
            Out = new List<ISignalWriter<T>>();
        }

        public IList<ISignalReader<T>> In { get; private set; }

        public IList<ISignalWriter<T>> Out { get; private set; }

        /// <summary>
        /// Список номеров каналов на выходе начиная с 0.
        /// </summary>
        public int[] Parameters { get; set; }

        /// <summary>
        /// Названия каналов на выходе.
        /// </summary>
        public string[] OutChannelNames { get; set; }

        /// <summary>
        /// Потокобезопасный метод установки каналов на выходе.
        /// Номера каналов начинаются с 0.
        /// </summary>
        public void ThreadSafeChange(int outChannel, int inChannel)
        {
            Interlocked.Exchange(ref Parameters[outChannel], inChannel);
        }

        public void ThreadSafeChange(string outChannel, int inChannel)
        {
            Interlocked.Exchange(ref Parameters[Array.IndexOf(OutChannelNames, outChannel)], inChannel);
        }

        public bool? Execute()
        {
            var result = false;
            
            for (var i = 0; i < In.Count;i++ )
            {
                if (!Parameters.Contains(i))
                    continue;

                var data = In[i].Take();
                if (data == null)
                    continue;

                for (var j = 0; j < Math.Min(Parameters.Length, Out.Count); j++)
                {
                    if (Parameters[j] != i)
                        continue;

                    Out[j].Write(data);
                }

                In[i].Put(data);

                result = true;
            }

            return result;
        }
    }
}
