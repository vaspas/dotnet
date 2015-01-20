using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using IppModules;
using IppModules.Analiz.NarrowBandSpectrum;
using IppModules.Generator;
using Sigflow.Module;
using Sigflow.Performance;
using Sigflow.Dataflow;
using Sigflow.Schema;
using TdGraphsParts.Renderers.Graph;
using ViewModules;

namespace WindowsFormsGenerator.Schemes
{
    class Schema2 : ISchema
    {
        public Action OnRedraw { get; set; }

        private SchemaContainer _container;

        public List<ISignalSource<float>> Build()
        {
            var f = new XmlSchemaFactory {Document = new XmlDocument()};

            using (var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream(typeof(Schema1).Namespace + ".schema2.xml"))
                f.Document.Load(stream);

            _container = f.Build();

            var fpsController = _container.Get<FpsSignalReadControllerModule>("fpscontroller");
            fpsController.OnRedraw = () => OnRedraw();
            fpsController.SignalReaderController=
                //_container.Get<SignalReaderController<float>>("signalreadercontroller");
                _container.Get<SignalReaderController<float>>("oscillographreadcontroller");

            return new List<ISignalSource<float>>
                       {
                           _container.Get<SignalSourceAdapterModule<float>>("signalsourcelin"),
                           _container.Get<SignalSourceAdapterModule<float>>("signalsourceexp"),
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
