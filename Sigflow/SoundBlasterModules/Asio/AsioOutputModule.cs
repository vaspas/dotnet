using System;
using System.Collections.Generic;
using BlueWave.Interop.Asio;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace SoundBlasterModules.Asio
{
    /// <summary>
    /// Считывает данные в потоке драйвера прямо из входного канала.
    /// Поток выполняющий запуск запуск/остановку должен быть в STA режиме.
    /// </summary>
    public class AsioOutputModule:IMasterModule
    {
        public AsioOutputModule()
        {
            In=new List<ISignalReader<int>>( );
        }

        public int DriverNumber { get; set; }

        public int BufferSize { get; set; }

        public float SampleRate { get; set; }

        public Action<Exception> OnException { get; set; }

        public IList<ISignalReader<int>> In { get; private set; }

        private int[] _buffer=new int[0];
        private int[] _zeroBuffer = new int[0];


        public AsioDriver Driver { get; private set; }

        public bool Start()
        {
            try
            {
                Driver = AsioDriver.SelectDriver(AsioDriver.InstalledDrivers[DriverNumber]);

                Driver.SetSampleRate(SampleRate);

                Driver.CreateOutputBuffers(BufferSize);

                _buffer=new int[BufferSize];
                _zeroBuffer = new int[BufferSize];

                Driver.BufferUpdate += AsioDriverBufferUpdate;

                // and off we go
                Driver.Start();
            }
            catch (Exception ex)
            {
                if(OnException!=null)
                    OnException(ex);
                return false;
            }
            
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
            try
            {
                Driver.Stop();
                Driver.DisposeBuffers();
                Driver.Release();
            }
            catch (Exception ex)
            {
                if (OnException != null)
                    OnException(ex);
            }
        }

        /// <summary>
        /// Called when a buffer update is required
        /// </summary>
        private void AsioDriverBufferUpdate(object sender, EventArgs e)
        {
            for (var ch = 0; ch < In.Count;ch++ )
            {
                if(!In[ch].ReadTo(_buffer))
                {
                    Driver.OutputChannels[ch].Write(_zeroBuffer);
                    continue;
                }
                Driver.OutputChannels[ch].Write(_buffer);
            }
        }
    }
}
