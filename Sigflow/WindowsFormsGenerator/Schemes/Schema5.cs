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
    class Schema5 : ISchema
    {
        public Action OnRedraw { get; set; }

        private SchemaContainer _container;

        public List<ISignalSource<float>> Build()
        {
            var f = new XmlSchemaFactory { Document = new XmlDocument() };

            using (var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream(GetType().Namespace + ".schema5.xml"))
                f.Document.Load(stream);

            _container = f.Build();

            var adc = _container.Get<IncModules.Mio4400.Mio4400ModuleInt>("adc");

            adc.OnMessage =
                s => MessageBox.Show(s);
            adc.InitDevice();

            return new List<ISignalSource<float>>();
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
