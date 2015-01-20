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
    class AsioOutputSchema : ISchema
    {
        public Action OnRedraw { get; set; }

        private SchemaContainer _container;

        public List<ISignalSource<float>> Build()
        {
            var f = new XmlSchemaFactory { Document = new XmlDocument() };

            using (var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream(GetType().Namespace + ".AsioOutputSchema.xml"))
                f.Document.Load(stream);

            _container = f.Build();

            var client = _container.Get<SoundBlasterModules.Asio.AsioOutputModule>("output");
            client.OnException = e => MessageBox.Show(e.Message);

            var res = new List<ISignalSource<float>>();

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
