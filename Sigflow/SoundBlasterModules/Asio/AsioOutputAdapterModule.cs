using System;
using System.Collections.Generic;
using BlueWave.Interop.Asio;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace SoundBlasterModules.Asio
{
    public class AsioOutputAdapterModule:IMasterModule
    {
        public AsioOutputAdapterModule()
        {
            In=new List<ISignalReader<int>>( );
        }

        public AsioDriver AsioDriver { get; set; }


        public IList<ISignalReader<int>> In { get; private set; }

        private int[] _buffer=new int[0];


        public bool Start()
        {
            _buffer = new int[AsioDriver.BufferSizeOutput];

            AsioDriver.BufferUpdate += AsioDriverBufferUpdate;

            return true;
        }

        public void AfterStart()
        {
        }

        public void BeforeStop()
        {
        }


        public void AfterStop()
        {
            AsioDriver.BufferUpdate -= AsioDriverBufferUpdate;
        }

        /// <summary>
        /// Called when a buffer update is required
        /// </summary>
        private void AsioDriverBufferUpdate(object sender, EventArgs e)
        {
            for (var ch = 0; ch < In.Count;ch++ )
            {
                if(!In[ch].ReadTo(_buffer))
                    continue;

                AsioDriver.OutputChannels[ch].Write(_buffer);
            }
        }
    }
}
