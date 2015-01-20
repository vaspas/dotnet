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
        /// ����������� ������.
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
            //������� ������������
            _adcHelper = new AdcHelper
                                { Module = this };

            if (!_adcHelper.InitializeDevice())
                _adcHelper = null;

            return _adcHelper != null;
        }


        

        public ISignalWriter<int> Out { get; set; }

        #region ���������

        /// <summary>
        /// ����� ����������.
        /// </summary>
        public int BoardNumber { get; set; }

        /// <summary>
        /// ����� �������.
        /// </summary>
        public StartMode StartMode { get; set; }

        /// <summary>
        /// ���-�� ������� ��� ������.
        /// </summary>
        public byte ChannelsCount { get; set; }

        /// <summary>
        /// ������ ����� � �������� ��� ������ ������.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// ������ �������� �� �������.
        /// </summary>
        public GainValues[] GainValues { get; set; }

        /// <summary>
        /// ����� ������� �������������.
        /// </summary>
        public bool ExternalSync { get; set; }

        /// <summary>
        /// ������� �����������.
        /// </summary>
        public float Frequency { get; set; }

        /// <summary>
        /// ����� ������� ���, ��� ������ �� �������.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// ���������� ��� ����������.
        /// </summary>
        public IncDevices DeviceType { get; set; }

        /// <summary>
        /// ��������� ������.
        /// </summary>
        public ThreadPriority ThreadPriority { get; set; }

        public IModifications Modifications { get; set; }

        #endregion

        

        private AdcHelper _adcHelper;

        private AdcThread _adcThread;

        public bool Start()
        {
            //���������, ���������������� �� ����������
            if (_adcHelper == null)
            {
                OnMessage("���������� �� ����������������");
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

            //��������� ���
            var res = _adcThread.Run(StartMode);
            //��������� ������
            if (res != (int)RunSborErrors.Success)
            {
                //�������� �������
                OnMessage(("������ " + (RunSborErrors) res));
                return false;
            }

            return true;
        }

        public void SetupAllAmplifications()
        {
            //���������, ���������������� �� ����������
            if (_adcHelper == null)
            {
                OnMessage("���������� �� ����������������");
                return;
            }

            //������������� ��������
            if (_adcThread != null)
                lock (_adcThread.SyncObject)
                    _adcHelper.SetupAllAmplifications();
            else
                _adcHelper.SetupAllAmplifications();
        }

        public void SetupAmplification(GainValues gainValue, int channel)
        {
            //���������, ���������������� �� ����������
            if (_adcHelper == null)
            {
                OnMessage("���������� �� ����������������");
                return;
            }

            GainValues[channel] = gainValue;

            //������������� ��������
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
        /// ����������� ��� ��������� ������.
        /// </summary>
        void IMasterModule.BeforeStop()
        {
            Stop();
        }

        void IMasterModule.AfterStop()
        {
        }

        /// <summary>
        /// ������� ���������.
        /// </summary>
        public Action<string> OnMessage;


        /// <summary>
        /// ���������� ��� �������� ������� ���.
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

            //�������� ��������
            return value;
        }

        /// <summary>
        /// ���������� ������� ����� ���.
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
