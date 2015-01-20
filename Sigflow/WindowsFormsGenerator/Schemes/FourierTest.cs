using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
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

namespace WindowsFormsGenerator.Schemes
{
    class FourierTest : ISchema
    {
        public Action OnRedraw { get; set; }

        private SchemaContainer _container;

        public List<ISignalSource<float>> Build()
        {
            var f = new XmlSchemaFactory {Document = new XmlDocument()};

            using (var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream(typeof(Schema1).Namespace + ".FourierTest.xml"))
                f.Document.Load(stream);

            _container = f.Build();
            

            var fpsController = _container.Get<FpsSignalReadControllerModule>("fpscontroller");
            fpsController.OnRedraw = () => OnRedraw();
            fpsController.SignalReaderController=
                //_container.Get<SignalReaderController<float>>("signalreadercontroller");
                _container.Get<SignalReaderController<float>>("oscillographreadcontroller");
            
            return new List<ISignalSource<float>>
                       {
                           //_container.Get<SignalSourceAdapterModule<float>>("signalsourceRe"),
                           //_container.Get<SignalSourceAdapterModule<float>>("signalsourceIm"),
                           _container.Get<SignalSourceAdapterModule<float>>("signalsourceMod"),
                           //_container.Get<SignalSourceAdapterModule<float>>("signalsourceModSignal"),
                           _container.Get<SignalSourceAdapterModule<float>>("signalsourceArg"),
                          // _container.Get<SignalSourceAdapterModule<float>>("signalsourceArgSignal"),
                           //_container.Get<SignalSourceAdapterModule<float>>("oscillograph"),

                           _container.Get<SignalSourceAdapterModule<float>>("signalsourceComplexFft"),
                           //_container.Get<SignalSourceAdapterModule<float>>("signalsourceRe2"),
                           //_container.Get<SignalSourceAdapterModule<float>>("signalsourceIm2"),
                           //_container.Get<SignalSourceAdapterModule<float>>("signalsourceMod2"),
                           //_container.Get<SignalSourceAdapterModule<float>>("signalsourceArg2")
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
