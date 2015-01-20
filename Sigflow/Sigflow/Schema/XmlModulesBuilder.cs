 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sigflow.Module;
using Sigflow.Performance;

namespace Sigflow.Schema
{
    class XmlModulesBuilder
    {
        public XmlElement PerformerNode { get; set; }

        public SchemaContainer Container { get; set; }

        public PerformerContainer PerformerContainer { get; set; }

        public void Build()
        {
            PerformerNode.ChildNodes.OfType<XmlElement>().ToList()
                .FindAll(n => n.Name == Words.Module)
                .ForEach(n => Build(null, n));
        }

        private void Build(object parent, XmlElement node)
        {
            var id = node.GetAttribute(Words.Id);
            var type = node.GetAttribute(Words.Type);
            var collectionCount = node.GetAttribute(Words.IsCollection);

            XmlSchemaFactoryLogger.AddToTree(
                string.Format("модуль {0} {1} {2}",
                              string.IsNullOrEmpty(collectionCount) ? string.Empty : collectionCount + "шт",
                              string.IsNullOrEmpty(id) ? string.Empty : id,
                              type));

            object module;
            if (string.IsNullOrEmpty(collectionCount))
                module = string.IsNullOrEmpty(id)
                             ? Activator.CreateInstance(Type.GetType(type))
                             : Container.Get<IModule>(PerformerContainer.Performer, id);
            else if (!string.IsNullOrEmpty(id))
                module = Container.Get<ICollection>(PerformerContainer.Performer, id);
            else
            {
                var moduletype = Type.GetType(type);
                module = Activator.CreateInstance(typeof(List<>).MakeGenericType(moduletype));
                for (var i = 0; i < int.Parse(collectionCount); i++)
                    (module as IList).Add(Activator.CreateInstance(Type.GetType(type)));
            }

            if(module is ICollection)
                foreach (var m in (module as ICollection))
                    PerformerContainer.Performer.AddModule(m as IModule);
            else
                PerformerContainer.Performer.AddModule(module as IModule);
                

            new XmlFillProperties
            {
                Node = node,
                Object = module
            }.Fill();

            new XmlSignalConnectionsBuilder
                {
                    ModuleNode = node,
                    Container = Container,
                    Module = module,
                    ParentModule = parent,
                    PerformerContainer = PerformerContainer
                }.Build();

            new XmlBeatConnectionBuilder
                {
                    Node = node,
                    Container = Container,
                    Performer = PerformerContainer.Performer,
                    Beat = module
                }.Build();

            XmlSchemaFactoryLogger.RemoveFromTree();

            node.ChildNodes.OfType<XmlElement>().ToList()
                .FindAll(n=>n.Name==Words.Module)
                .ForEach(n => Build(module,n));
        }
    }
}
