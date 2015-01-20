using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Xml;
using IppModules;
using IppModules.Analiz.NarrowBandSpectrum;
using IppModules.Generator;
using Sigflow.Module;
using Sigflow.Performance;
using Sigflow.Dataflow;
using Sigflow.Schema;
using TalkModules;
using TdGraphsParts.Renderers.Graph;
using ViewModules;

namespace WpfTest
{
    class Schema1
    {
        public Action OnRedraw { get; set; }

        private SchemaContainer _container;

        public List<ISignalSource<float>> Build()
        {
            var f = new XmlSchemaFactory {Document = new XmlDocument()};

            using (var stream = Assembly.GetAssembly(typeof(Schema1)).GetManifestResourceStream(typeof(Schema1).Namespace + ".schema1.xml"))
                f.Document.Load(stream);

            _container = f.Build();
            

            var fpsController = _container.Get<FpsSignalReadControllerModule>("fpscontroller");
            fpsController.OnRedraw = () => OnRedraw();
            fpsController.SignalReaderController=
                //_container.Get<SignalReaderController<float>>("signalreadercontroller");
                _container.Get<SignalReaderController<float>>("oscillographreadcontroller");

            var talker = TalkDotNET.TalkClassConfigurator.Create("127.0.0.1", 5070, false)
                .LogExceptions(e => { } /*MessageBox.Show(e.Message)*/)
                .AutoConnect(1000, ()=> { })
                .Result;

            _container.Get<TalkConnectorModule>("talkconnector")
                .Connector = talker;
            _container.Get<Raw13WriterModule>("raw13writer")
                .Writer = talker;
            _container.Get<Anal13WriterModule>("anal13writer")
                .Writer = talker;

            _container.Get<SigProModules.SignalToNodeModuleFloat>("ToSigPro").OnMessage = s => MessageBox.Show(s);

            return new List<ISignalSource<float>>
                       {
                           _container.Get<SignalSourceAdapterModule<float>>("signalsource"),
                           _container.Get<SignalSourceAdapterModule<float>>("tasignalsource"),
                           _container.Get<SignalSourceAdapterModule<float>>("oscillograph")
                       };
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
