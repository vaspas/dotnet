using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace SoundBlasterModules.WaveApi.Output
{
    public class WaveOutputModule : IMasterModule
    {
        public int DriverNumber { get; set; }

        public int BufferSize { get; set; }

        public float SampleRate { get; set; }

        public int ChannelsCount { get; set; }

        public Action<Exception> OnException { get; set; }

        /// <summary>
        /// Мультиплексированные данные.
        /// </summary>
        public ISignalReader<float> In { get; set; }

        private float[] _buffer = new float[0];
        private float[] _zeroBuffer = new float[0];


        private SoundBlaster _driver;

        public bool Start()
        {
            try
            {
                _driver = new SoundBlaster();

                _driver.FloatType = true;
                _driver.BlockSize = BufferSize;
                _driver.Frequency = SampleRate;
                _driver.ChannelsCount = ChannelsCount;

                _buffer = new float[BufferSize];
                _zeroBuffer = new float[BufferSize];

                _driver.DataRequered += DriverBufferUpdate;

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
        }


        public void AfterStop()
        {
            try
            {
                _driver.DataRequered -= DriverBufferUpdate;
                _driver.Stop();
                _driver = null;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        /// <summary>
        /// Called when a buffer update is required
        /// </summary>
        private void DriverBufferUpdate()
        {
            if(!In.ReadTo(_buffer))
            {
                _driver.SetData(_zeroBuffer);
                return;
            }
            _driver.SetData(_buffer);
        }


    }
}

