using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace SoundBlasterModules.WaveApi.Input
{
    public class WaveInputModule : IMasterModule
    {
        public int DeviceNumber { get; set; }

        public int LineNumber { get; set; }

        public int BufferSize { get; set; }

        public int SampleRate { get; set; }

        public int ChannelsCount { get; set; }

        public Action<Exception> OnException { get; set; }

        /// <summary>
        /// Мультиплексированные данные.
        /// </summary>
        public ISignalWriter<short> Out { get; set; }
        

        private SoundBlaster _driver;

        public bool Start()
        {
            try
            {
                _driver = new SoundBlaster();

                _driver.DeviceNumber = DeviceNumber;
                _driver.BlockSize = BufferSize;
                _driver.Frequency = SampleRate;
                _driver.ChannelsCount = ChannelsCount;

                _driver.SetLine(_driver.LineNumber);

                _driver.NewDataReceived += DriverBufferUpdate;

                _driver.Start();
            }
            catch (Exception ex)
            {
                OnException(ex);
                return false;
            }

            return true;
        }


        public void BeforeStop()
        {
            try
            {
                _driver.NewDataReceived -= DriverBufferUpdate;
                _driver.Stop();
                _driver = null;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }


        public void AfterStop()
        {
            
        }

        /// <summary>
        /// Called when a buffer update is required
        /// </summary>
        private void DriverBufferUpdate()
        {
            Out.Write(_driver.DataArray);
        }


    }
}

