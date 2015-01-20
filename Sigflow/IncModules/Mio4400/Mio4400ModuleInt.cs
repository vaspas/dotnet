using System;
using System.Threading;
using IncModules.Mio4400.Modifications;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IncModules.Mio4400
{
    public class Mio4400ModuleInt : IMasterModule
    {
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public Mio4400ModuleInt()
        {
            IncHelper.Instance.IncApiInit();
            Modifications = new FakeModifications();
            ThreadPriority = ThreadPriority.Normal;
            OnMessage = s => { };
        }
        

        public bool InitDevice()
        {
            //создаем управляющего
            _adcHelper = new AdcHelper
                                { Module = this };

            if (!_adcHelper.InitializeDevice())
                _adcHelper = null;

            return _adcHelper != null;
        }


        

        public ISignalWriter<int> Out { get; set; }

        #region параметры

        /// <summary>
        /// Номер устройства.
        /// </summary>
        public int BoardNumber { get; set; }
        
        /// <summary>
        /// Кол-во каналов для чтения.
        /// </summary>
        public byte ChannelsCount { get; set; }

        /// <summary>
        /// Размер блока в отсчетах для одного канала.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// Режимы усиления по каналам.
        /// </summary>
        public GainValues[] GainValues { get; set; }
        
        /// <summary>
        /// Частота квантования.
        /// </summary>
        public float Frequency { get; set; }
        
        /// <summary>
        /// Возвращает тип устройства.
        /// </summary>
        public IncDevices DeviceType { get; set; }

        /// <summary>
        /// Приоритет потока.
        /// </summary>
        public ThreadPriority ThreadPriority { get; set; }

        public IModifications Modifications { get; set; }

        #endregion


        private AdcHelper _adcHelper;

        private AdcThread _adcThread;

        public bool Start()
        {
            //проверяем, инициализировано ли устройство
            if (_adcHelper == null)
            {
                OnMessage("Устройство не инициализировано");
                return false;
            }

            Wrapper.MioSetFilterFreq((float)(Frequency * (1 + Modifications.GetQuantumFreqCorrPpu() / 1000000)), BoardNumber);
            SetupAllAmplifications();

            _adcThread = new AdcThread
                             {
                                 OnDataReceived = a=> Out.Write(a),
                                 BlockSize = BlockSize,
                                 ChannelsCount = ChannelsCount,
                                 BoardNumber = BoardNumber,
                                 ThreadPriority = ThreadPriority
                             };

            //запускаем АЦП
            var res = _adcThread.Run();
            //проверяем запуск
            if (res != (int)RunSborErrors.Success)
            {
                //вызываем событие
                OnMessage(("Ошибка " + (RunSborErrors) res));
                return false;
            }

            return true;
        }

        public void SetupAllAmplifications()
        {
            //проверяем, инициализировано ли устройство
            if (_adcHelper == null)
            {
                OnMessage("Устройство не инициализировано");
                return;
            }

            //устанавливаем усиления
            //устанавливаем усиления
            for (var i = 0; i < ChannelsCount; i++)
                Wrapper.MioSetGain(IncHelper.GainAdresses[i], (byte)GainValues[i], BoardNumber);
        }

        public void SetupAmplification(GainValues gainValue, int channel)
        {
            //проверяем, инициализировано ли устройство
            if (_adcHelper == null)
            {
                OnMessage("Устройство не инициализировано");
                return;
            }

            GainValues[channel] = gainValue;

            //устанавливаем усиления
            Wrapper.MioSetGain(IncHelper.GainAdresses[channel], (byte)GainValues[channel], BoardNumber);
        }

        /// <summary>
        /// Выполянется при остановке модуля.
        /// </summary>
        public void BeforeStop()
        {
            if (_adcThread == null)
                return;

            _adcThread.Stop();
            _adcThread = null;
        }

        public void AfterStop()
        {
        }


        /// <summary>
        /// Событие сообщение.
        /// </summary>
        public Action<string> OnMessage;

        /// <summary>
        /// Возвращает вес младшего разряда АЦП.
        /// </summary>
        /// <returns></returns>
        public double GetAbsVolt(int channel)
        {
            var gain = GainValues[channel];

            var value = Modifications.Get(gain, channel) * Wrapper.MioGetAbsVolt(BoardNumber);

            if (gain == Mio4400.GainValues.Gain10)
                value /= Math.Sqrt(10);
            if (gain == Mio4400.GainValues.Gain20)
                value /= 10;
            if (gain == Mio4400.GainValues.Gain30)
                value /= 10 * Math.Sqrt(10);

            //получаем значение
            return value;
        }


    }
}
