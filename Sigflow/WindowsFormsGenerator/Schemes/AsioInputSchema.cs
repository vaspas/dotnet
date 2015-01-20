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
    class AsioInputSchema : ISchema
    {
        public Action OnRedraw { get; set; }

        private SchemaContainer _container;

        public List<ISignalSource<float>> Build()
        {
            var f = new XmlSchemaFactory { Document = new XmlDocument() };

            using (var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream(GetType().Namespace + ".AsioInputSchema.xml"))
                f.Document.Load(stream);

            _container = f.Build();

            var client = _container.Get<SoundBlasterModules.Asio.AsioInputModule>("signal");
            client.OnException = e => MessageBox.Show(e.Message);

            var fpsController = _container.Get<FpsSignalReadControllerModule>("fpscontroller");
            fpsController.OnRedraw = () => OnRedraw();
            fpsController.SignalReaderController =
                _container.Get<IList<SignalReaderController<float>>>("oscillographreadcontroller")[0];

            var res = new List<ISignalSource<float>>();
            res.Add(_container.Get<IList<SignalSourceAdapterModule<float>>>("oscillograph")[1]);
            //res.AddRange(_container.Get<IList<SignalSourceAdapterModule<float>>>("oscillograph"));

            return res;
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
