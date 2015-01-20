using System;
using System.Threading;
using IncModules.Ink824.Modifications;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IncModules.Ink824
{
    public class Ink824ModuleInt : IMasterModule
    {
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public Ink824ModuleInt()
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
        /// Режим запуска.
        /// </summary>
        public StartMode StartMode { get; set; }

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
        /// Режим внешней синхронизации.
        /// </summary>
        public bool ExternalSync { get; set; }

        /// <summary>
        /// Частота квантования.
        /// </summary>
        public float Frequency { get; set; }

        /// <summary>
        /// Время запуска АЦП, для старта по времени.
        /// </summary>
        public TimeSpan StartTime { get; set; }

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

            _adcHelper.PrepareBeforeRun();
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
            var res = _adcThread.Run(StartMode);
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
            if (_adcThread != null)
                lock (_adcThread.SyncObject)
                    _adcHelper.SetupAllAmplifications();
            else
                _adcHelper.SetupAllAmplifications();
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
            if (_adcThread != null)
                lock (_adcThread.SyncObject)
                    _adcHelper.SetupAmplification(channel);
            else
                _adcHelper.SetupAmplification(channel);
        }

        public void Stop()
        {
            if (_adcThread == null)
                return;

            _adcThread.Stop();
            _adcThread = null;
        }

        /// <summary>
        /// Выполянется при остановке модуля.
        /// </summary>
        void IMasterModule.BeforeStop()
        {
            Stop();
        }

        void IMasterModule.AfterStop()
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
            
            var value = Modifications.Get(gain, channel)*Wrapper.MioGetAbsVolt(BoardNumber);

            if (gain == Ink824.GainValues.Gain_10)
                value /= Math.Sqrt(10);
            if (gain == Ink824.GainValues.Gain_20)
                value /= 10;
            if (gain == Ink824.GainValues.Gain_30)
                value /= 10 * Math.Sqrt(10);

            //получаем значение
            return value;
        }

        /// <summary>
        /// Возвращает текущее время АЦП.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetDeviceTime()
        {
            if (_adcThread != null)
                lock(_adcThread.SyncObject)
                    return _adcHelper.GetDeviceTime();

            return _adcHelper.GetDeviceTime();
        }

    }
}
