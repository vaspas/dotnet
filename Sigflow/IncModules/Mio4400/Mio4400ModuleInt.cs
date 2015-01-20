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
        /// ����������� ������.
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
        /// ������� �����������.
        /// </summary>
        public float Frequency { get; set; }
        
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

            //��������� ���
            var res = _adcThread.Run();
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
            //������������� ��������
            for (var i = 0; i < ChannelsCount; i++)
                Wrapper.MioSetGain(IncHelper.GainAdresses[i], (byte)GainValues[i], BoardNumber);
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
            Wrapper.MioSetGain(IncHelper.GainAdresses[channel], (byte)GainValues[channel], BoardNumber);
        }

        /// <summary>
        /// ����������� ��� ��������� ������.
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

            var value = Modifications.Get(gain, channel) * Wrapper.MioGetAbsVolt(BoardNumber);

            if (gain == Mio4400.GainValues.Gain10)
                value /= Math.Sqrt(10);
            if (gain == Mio4400.GainValues.Gain20)
                value /= 10;
            if (gain == Mio4400.GainValues.Gain30)
                value /= 10 * Math.Sqrt(10);

            //�������� ��������
            return value;
        }


    }
}
