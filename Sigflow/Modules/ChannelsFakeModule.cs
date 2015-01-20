using System;
using System.Collections.Generic;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules
{
    /// <remark>
    /// INSPECTED 30/04/2013
    /// </remark>
    public class ChannelsFakeModule<T> : IExecuteModule
        where T : struct 
    {
        public ChannelsFakeModule()
        {
            In=new List<ISignalReader<T>>();
            Out = new List<ISignalWriter<T>>();
        }

        public IList<ISignalReader<T>> In { get; private set; }

        public IList<ISignalWriter<T>> Out { get; private set; }
        
        public bool? Execute()
        {
            var result=false;

            for (var i = 0; i < Math.Min(In.Count, Out.Count); i++)
            {
                var data = In[i].Take();
                if (data == null)
                    continue;

                Out[i].Write(data);

                In[i].Put(data);

                result = true;
            }

            return result;
        }
    }
}
