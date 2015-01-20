using System;
using System.Collections.Generic;
using System.Threading;
using BlueWave.Interop.Asio;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace SoundBlasterModules.Asio
{

    /// <summary>
    /// Записывает данные из потоке драйвера в выходной канала.
    /// Поток выполняющий запуск/остановку должен быть в Sta режиме.
    /// </summary>
    public class AsioInputModule:IMasterModule
    {
        public AsioInputModule()
        {
            Out=new List<ISignalWriter<int>>( );
        }

        public int DriverNumber { get; set; }

        public int BufferSize { get; set; }

        public float SampleRate { get; set; }

        public Action<Exception> OnException { get; set; }

        public IList<ISignalWriter<int>> Out { get; private set; }

        private int[] _buffer=new int[0];


        public AsioDriver Driver { get; private set; }

        public bool Start()
        {
            try
            {
                Driver = AsioDriver.SelectDriver(AsioDriver.InstalledDrivers[DriverNumber]);

                Driver.SetSampleRate(SampleRate);
                
                Driver.CreateInputBuffers(BufferSize);

                _buffer=new int[BufferSize];

                Driver.BufferUpdate += AsioDriverBufferUpdate;

                // and off we go
                Driver.Start();
            }
            catch (Exception ex)
            {
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
                OnException(ex);
            }
        }

        /// <summary>
        /// Called when a buffer update is required
        /// </summary>
        private void AsioDriverBufferUpdate(object sender, EventArgs e)
        {
            for (var ch = 0; ch < Out.Count;ch++ )
            {
                Driver.InputChannels[ch].Read(_buffer);
                Out[ch].Write(_buffer);
            }
        }
    }
}
