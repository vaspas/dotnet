using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sigflow.Performance;

namespace Sigflow.Schema
{
    class XmlPerformerObjectsFactory
    {
        public XmlElement PerformerNode { get; set; }

        private PerformerContainer _container;

        public PerformerContainer Create()
        {
            _container = new PerformerContainer 
            {
                Performer = new Performer(),
                Id = PerformerNode.GetAttribute(Words.Id)
            };

            new XmlFillProperties
                {
                    Node = PerformerNode,
                    Object = _container.Performer
                }.Fill();

            new XmlBeatFactory
                {
                    BeatNode = PerformerNode.ChildNodes.OfType<XmlElement>().First(e=>e.Name==Words.Beat),
                    Container = _container
                }.Create();

            CreateModules(PerformerNode);

            return _container;
        }

        /*private void CreateObjects(XmlElement node, int? parentModuleCollectionCount)
        {
            var id = node.GetAttribute(Words.Id);
            var type = node.GetAttribute(Words.Type);
            
            var collectionCountString = node.GetAttribute(Words.IsCollection);
            var collectionCount = !string.IsNullOrEmpty(collectionCountString)
                                      ? int.Parse(collectionCountString)
                                      : parentModuleCollectionCount;

            
            
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type))
            {
                var objecttype = Type.GetType(type);
                if (collectionCount!=null)
                {
                    var list = (IList)Activator.CreateInstance(typeof (List<>).MakeGenericType(objecttype));
                    for (var i = 0; i < collectionCount.Value; i++)
                        list.Add(Activator.CreateInstance(Type.GetType(type)));
                    _container.Add(id, list);
                }
                else
                    _container.Add(id, Activator.CreateInstance(Type.GetType(type)));
            }

            var moduleCollectionCount = parentModuleCollectionCount;
            if (node.Name == Words.Module)
                moduleCollectionCount = !string.IsNullOrEmpty(collectionCountString)
                                      ? (int?)int.Parse(collectionCountString)
                                      : null;

            node.ChildNodes.OfType<XmlElement>().ToList()
                .ForEach(e => CreateObjects(e, moduleCollectionCount));
        }*/

        private void CreateModules(XmlElement node)
        {
            if (node.Name == Words.Module)
            {

                var id = node.GetAttribute(Words.Id);
                var type = node.GetAttribute(Words.Type);

                var collectionCountString = node.GetAttribute(Words.IsCollection);
                var collectionCount = !string.IsNullOrEmpty(collectionCountString)
                                          ? (int?) int.Parse(collectionCountString)
                                          : null;

                XmlSchemaFactoryLogger.AddToTree(
                    string.Format("Создание модуля {0} {1}",
                                  string.IsNullOrEmpty(id) ? string.Empty : id,
                                  string.IsNullOrEmpty(type) ? string.Empty : type));

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type))
                {
                    var objecttype = Type.GetType(type);
                    if (collectionCount != null)
                    {
                        var list = (IList) Activator.CreateInstance(typeof (List<>).MakeGenericType(objecttype));
                        for (var i = 0; i < collectionCount.Value; i++)
                            list.Add(Activator.CreateInstance(Type.GetType(type)));
                        _container.Add(id, list);
                    }
                    else
                        _container.Add(id, Activator.CreateInstance(Type.GetType(type)));
                }

                XmlSchemaFactoryLogger.RemoveFromTree();
            }

            node.ChildNodes.OfType<XmlElement>().ToList()
                .ForEach(CreateModules);
        }
    }
}
