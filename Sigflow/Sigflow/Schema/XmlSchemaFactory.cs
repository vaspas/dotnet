using System.Linq;
using System.Xml;

namespace Sigflow.Schema
{
    public class XmlSchemaFactory
    {
        public XmlDocument Document { get; set; }

        private SchemaContainer _container;

        public SchemaContainer Build()
        {
            XmlSchemaFactoryLogger.Clear();

            _container=new SchemaContainerThreadSafe();

            var schemaNode = Document.ChildNodes.OfType<XmlElement>().First(e=>e.Name==Words.Schema);
            
            //создаем исполнителей и все объекты с id
            foreach(var node in schemaNode.ChildNodes.OfType<XmlElement>())
            {
                if (node.Name != Words.Performer)
                    continue;

                var c = new XmlPerformerObjectsFactory
                            {
                                PerformerNode = node
                            }.Create();

                _container.Add(c);

                new XmlModulesBuilder
                {
                    PerformerNode = node,
                    Container = _container,
                    PerformerContainer = c
                }.Build();
            }

            return _container;
        }
    }
}
