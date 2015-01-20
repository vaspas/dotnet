using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using IppModules;
using Sigflow.Schema;
using TdGraphsParts.Renderers.Graph;
using ViewModules;

namespace WindowsFormsGenerator.Schemes
{
    class TalkStreamClient : ISchema
    {
        public Action OnRedraw { get; set; }

        private SchemaContainer _container;

        public List<ISignalSource<float>> Build()
        {
            var f = new XmlSchemaFactory { Document = new XmlDocument() };

            using (var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream(GetType().Namespace + ".TalkStreamClient.xml"))
                f.Document.Load(stream);

            _container = f.Build();

            var client = _container.Get<TalkModules.StreamClientModule>("signal");
            client.OnException = e => MessageBox.Show(e.Message);
            client.OnReconnect = v => { if (v) MessageBox.Show("Подключен!"); };

            var fpsController = _container.Get<FpsSignalReadControllerModule>("fpscontroller");
            fpsController.OnRedraw = () => OnRedraw();
            fpsController.SignalReaderController =
                _container.Get<SignalReaderController<float>>("oscillographreadcontroller");

            return new List<ISignalSource<float>>
                       {
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
