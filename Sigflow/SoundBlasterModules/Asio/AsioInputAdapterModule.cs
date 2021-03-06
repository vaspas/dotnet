﻿using System;
using System.Collections.Generic;
using BlueWave.Interop.Asio;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace SoundBlasterModules.Asio
{
    public class AsioInputAdapterModule:IMasterModule
    {
        public AsioInputAdapterModule()
        {
            Out=new List<ISignalWriter<int>>( );
        }

        public AsioDriver AsioDriver { get; set; }


        public IList<ISignalWriter<int>> Out { get; private set; }

        private int[] _buffer=new int[0];


        public bool Start()
        {
            _buffer = new int[AsioDriver.BufferSizeInput];

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
            for (var ch = 0; ch < Out.Count;ch++ )
            {
                AsioDriver.InputChannels[ch].Read(_buffer);
                Out[ch].Write(_buffer);
            }
        }
    }
}
