using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using IppModules;
using Sigflow.Dataflow;
using Sigflow.Schema;
using TdGraphsParts.Renderers.Graph;
using ViewModules;

namespace WindowsFormsGenerator.Schemes
{
    class Mio4400 : ISchema
    {
        public Action OnRedraw { get; set; }

        private SchemaContainer _container;

        public List<ISignalSource<float>> Build()
        {
            var f = new XmlSchemaFactory { Document = new XmlDocument() };

            using (var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream(GetType().Namespace + ".Mio4400.xml"))
                f.Document.Load(stream);

            _container = f.Build();

            var adc = _container.Get<IncModules.Mio4400.Mio4400ModuleInt>("adc");

            adc.OnMessage =
                s => MessageBox.Show(s);
            adc.InitDevice();

            var adcqueue = _container.Get<ThreadSafeQueue<int>>("adcqueue");
            //adcqueue.OnOverflow += 
            //    () => { throw new OverflowException();};
            /*var t = new System.Timers.Timer(10000);
            t.Elapsed += (e, a) => MessageBox.Show((adcqueue.Count/adcqueue.MaxCapacity).ToString());
            t.Start();*/

            _container.Get<ConvertIntToFloatModule>("converter").Norma =
                (float) adc.GetAbsVolt(0);

            var fpsController = _container.Get<FpsSignalReadControllerModule>("fpscontroller");
            fpsController.OnRedraw = () => OnRedraw();
            fpsController.SignalReaderController =
                //_container.Get<SignalReaderController<float>>("signalreadercontroller");
                _container.Get<IList<SignalReaderController<float>>>("oscillographreadcontroller")[0];

            var result = new List<ISignalSource<float>>();

            _container.Get<IList<SignalSourceAdapterModule<float>>>("oscillograph").ToList()
                .ForEach(result.Add);

            _container.Get<IList<SignalSourceAdapterModule<float>>>("signalsourcelintodb").ToList()
                .ForEach(result.Add);

            _container.Get<IList<SignalSourceAdapterModule<float>>>("taSignalsourcelintodb").ToList()
                .ForEach(result.Add);

            return result;
        }

        public void Start()
        {
            _container.Start();
        }

        public void Stop()
        {
            _container.Stop();
        }

    }
}
