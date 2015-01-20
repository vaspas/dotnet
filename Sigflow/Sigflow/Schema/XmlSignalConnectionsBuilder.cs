using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Sigflow.Dataflow;
using Sigflow.Module;
using Sigflow.Performance;

namespace Sigflow.Schema
{
    class XmlSignalConnectionsBuilder
    {
        public XmlElement ModuleNode { get; set; }

        public SchemaContainer Container { get; set; }

        public PerformerContainer PerformerContainer { get; set; }

        public object Module { get; set; }

        public object ParentModule { get; set; }

        public void Build()
        {
            ModuleNode.ChildNodes.OfType<XmlElement>().ToList()
                .FindAll(n => n.Name == Words.In)
                .ForEach(Build);
        }

        private void Build(XmlElement node)
        {
            var name = node.GetAttribute(Words.Name);
            var inProperyName = name.Split(new[] { '.' }).Last();
            var moduleName = name.Length > inProperyName.Length
                                 ? name.Remove(name.Length - inProperyName.Length - 1, inProperyName.Length + 1)
                                 : string.Empty;

            var sourceModule = GetSourceModule(node.GetAttribute(Words.Source));

            XmlSchemaFactoryLogger.AddToTree("Подключение сигналов от " + node.GetAttribute(Words.Source));

            int? collectionCount = null;

            if (Module is IModule || !string.IsNullOrEmpty(moduleName))
            {
                var m = Module is IModule ? Module : (Module as IList)[GetProperyIndex(moduleName)];

                var inValue = m.GetType().GetProperty(GetProperyNameWithoutIndex(inProperyName))
                    .GetValue(m, null);

                if(inValue is ICollection && !IsContainsPropertyIndex(name))
                {
                    if (sourceModule.Module is ICollection)
                        collectionCount = (sourceModule.Module as ICollection).Count;
                    else
                    {
                        var outValue = sourceModule.Module.GetType().GetProperty(
                            GetProperyNameWithoutIndex(sourceModule.OutProperyName))
                            .GetValue(sourceModule.Module, null);
                        if (outValue is ICollection)
                            collectionCount = (outValue as ICollection).Count;
                    }
                }
            }
            else
                collectionCount = (int?) (Module as ICollection).Count;

            XmlSchemaFactoryLogger.AddToTree("Получение канала");
            var channel = CreateOrGet<IChannel>(node, collectionCount);
            XmlSchemaFactoryLogger.RemoveFromTree();

            new XmlFillProperties
            {
                Node = node,
                Object = channel
            }.Fill();

            new XmlBeatConnectionBuilder
            {
                Node = node,
                Container = Container,
                Performer = PerformerContainer.Performer,
                Beat = channel
            }.Build();

            XmlSchemaFactoryLogger.AddToTree("Декорирование module.in");
            var reader = channel;
            node.ChildNodes.OfType<XmlElement>().ToList()
                        .FindAll(e => e.Name == Words.DecorateIn)
                        .ForEach(e => reader = Decorate(e, reader));
            XmlSchemaFactoryLogger.RemoveFromTree();

            XmlSchemaFactoryLogger.AddToTree("Установка module.in");
            if (Module is IModule)
                SetInProperty((IModule)Module, node.GetAttribute(Words.Name), reader);
            else
            {
                if(string.IsNullOrEmpty(moduleName))
                    for (var i = 0; i < (Module as ICollection).Count; i++)
                        SetInProperty((IModule) (Module as IList)[i], node.GetAttribute(Words.Name),
                                      (reader as IList)[i]);
                else if (reader is IList)
                    SetInProperty((IModule)(Module as IList)[GetProperyIndex(moduleName)], inProperyName,
                                      (reader as IList)[GetProperyIndex(moduleName)]);
                else
                    SetInProperty((IModule)(Module as IList)[GetProperyIndex(moduleName)], inProperyName,
                                      reader);
            }
            XmlSchemaFactoryLogger.RemoveFromTree();

            XmlSchemaFactoryLogger.AddToTree("Декорирование module.out");
            var writer = channel;
            node.ChildNodes.OfType<XmlElement>().ToList()
                        .FindAll(e => e.Name == Words.DecorateOut)
                        .ForEach(e => writer = Decorate(e, writer));
            XmlSchemaFactoryLogger.RemoveFromTree();

            XmlSchemaFactoryLogger.AddToTree("Установка module.out");
            SetOutProperty(sourceModule, writer);
            XmlSchemaFactoryLogger.RemoveFromTree();

            XmlSchemaFactoryLogger.RemoveFromTree();
        }

       /* private object CreateOrGet<T>(XmlElement node, int? collectionCount)
        {
            var id = node.GetAttribute(Words.Id);
            var channeltype = Type.GetType(node.GetAttribute(Words.Type));

            if (collectionCount==null)
                return string.IsNullOrEmpty(id)
                           ? Activator.CreateInstance(channeltype)
                           : Container.Get<T>(Performer, id);

            if (!string.IsNullOrEmpty(id))
                return Container.Get<ICollection>(Performer, id);

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(channeltype));
            for (var i = 0; i < collectionCount; i++)
                list.Add(Activator.CreateInstance(channeltype));

            return list;
        }*/

        private object CreateOrGet<T>(XmlElement node, int? collectionCount)
        {
            var id = node.GetAttribute(Words.Id);
            var channeltype = Type.GetType(node.GetAttribute(Words.Type));

            if (collectionCount == null)
            {
                var obj=Activator.CreateInstance(channeltype);
                if(!string.IsNullOrEmpty(id))
                    PerformerContainer.Add(id, obj);
                return obj;
            }

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(channeltype));
            for (var i = 0; i < collectionCount; i++)
                list.Add(Activator.CreateInstance(channeltype));

            if(!string.IsNullOrEmpty(id))
                PerformerContainer.Add(id, list);

            return list;
        }

        private static void SetInProperty(IModule module, string name, object reader)
        {
            var propInfo = module.GetType().GetProperty(GetProperyNameWithoutIndex(name));

            var value = propInfo.GetValue(module, null);

            if (value is IList && !(reader is IList))
            {
                var index = GetProperyIndex(name);
                while ((value as IList).Count <= index)
                    (value as IList).Add(null);

                var pi = value.GetType().GetProperty("Item");

                pi.SetValue(value, reader, new object[] {index});
            }
            else if (value is IList && reader is IList)
            {
                while ((value as IList).Count < (reader as IList).Count)
                    (value as IList).Add(null);

                var pi = value.GetType().GetProperty("Item");

                for (var i = 0; i < (reader as IList).Count; i++)
                    pi.SetValue(value, (reader as IList)[i], new object[] {i});
            }
            else
                propInfo.SetValue(module, reader, null);
        }

        private object Decorate(XmlElement node, object obj)
        {
            var decorator = CreateOrGet<IDecorator>(node,
                                                    (obj is ICollection)
                                                        ? (int?)(obj as ICollection).Count
                                                        : null);

            new XmlFillProperties
            {
                Node = node,
                Object = decorator
            }.Fill();

            new XmlBeatConnectionBuilder
            {
                Node = node,
                Container = Container,
                Performer = PerformerContainer.Performer,
                Beat = decorator
            }.Build();

            if (obj is ICollection)
                for (var i = 0; i < (obj as ICollection).Count; i++)
                {
                    (decorator as IList)[i].GetType().GetProperty("Internal")
                        .SetValue((decorator as IList)[i], (obj as IList)[i], null);
                }
            else
                decorator.GetType().GetProperty("Internal")
                    .SetValue(decorator, obj, null);

            return decorator;
        }

        class SourceItem
        {
            public string OutProperyName;
            public string ModuleName;
            public object Module;
        }

        private SourceItem GetSourceModule(string source)
        {
            var item = new SourceItem();

            item.OutProperyName = source.Split(new[] { '.' }).Last();
            item.ModuleName = source.Length > item.OutProperyName.Length
                               ? source.Remove(source.Length - item.OutProperyName.Length - 1, item.OutProperyName.Length + 1)
                               : string.Empty;
            var moduleId = string.IsNullOrEmpty(item.ModuleName)
                               ? string.Empty
                               : GetProperyNameWithoutIndex(item.ModuleName);

            item.Module= string.IsNullOrEmpty(moduleId)
                             ? ParentModule
                             : (Container.Get<object>(PerformerContainer.Performer, moduleId) ?? Container.Get<object>(moduleId));

            return item;
        }

        private static void SetOutProperty(SourceItem source, object writer)
        {
            //подключение канала
            //...один канал к одному модулю
            if (writer is ISignalWriter && source.Module is IModule)
                SetOutProperty(source.Module as IModule, source.OutProperyName, writer as ISignalWriter);
            //...один канал к одному модулю из списка
            else if (writer is ISignalWriter && source.Module is IList)
                SetOutProperty((IModule)(source.Module as IList)[GetProperyIndex(source.ModuleName)], source.OutProperyName, writer as ISignalWriter);
            //...список каналов к списку модулей
            else if (writer is IList && source.Module is IList)
                for(var i=0;i<(writer as IList).Count;i++)
                    SetOutProperty((IModule)(source.Module as IList)[i], source.OutProperyName, (ISignalWriter)(writer as IList)[i]);
            //...список каналов к одному модулю
            else if (writer is IList && source.Module is IModule)
                for (var i = 0; i < (writer as IList).Count; i++)
                    SetOutProperty((IModule)source.Module, string.Format("{0}[{1}]", source.OutProperyName, i), (ISignalWriter)(writer as IList)[i]);
            else
                throw new InvalidOperationException();
        }

        private static void SetOutProperty(IModule module, string propertyName, ISignalWriter writer)
        {
            var outPropInfo = module.GetType().GetProperty(GetProperyNameWithoutIndex(propertyName));

            var srcWriter = outPropInfo.GetValue(module, null);

            if (srcWriter is IList)
            {
                var index = GetProperyIndex(propertyName);
                while ((srcWriter as IList).Count <= index)
                    (srcWriter as IList).Add(null);

                SetWriter((srcWriter as IList)[index],
                    srcWriter.GetType().GetProperty("Item"),
                    srcWriter, writer, new object[] { index });
            }
            else
                SetWriter(srcWriter, outPropInfo, module, writer, null);
        }

        private static void SetWriter(object srcWriter, PropertyInfo propertyInfo, object obj, ISignalWriter writer, object[] index)
        {
            if (srcWriter == null)
                propertyInfo.SetValue(obj, writer, index);
            else if (!(writer is ISignalWriterCollector))
            {
                //TODO: просмотреть иерархию наследования
                var dataType = writer.GetType().GetInterfaces().ToList()
                    .First(i => i.GetInterfaces().Any(ii => ii == typeof(ISignalWriter))).GetGenericArguments()[0];

                var generic = typeof(SignalWriterCollector<>).MakeGenericType(dataType);


                var collector = (ISignalWriterCollector)Activator.CreateInstance(generic);
                collector.Add((ISignalWriter)srcWriter);
                collector.Add(writer);
                propertyInfo.SetValue(obj, collector, index);
            }
            else
                (writer as ISignalWriterCollector).Add(writer);
        }


        private static string GetProperyNameWithoutIndex(string propertyName)
        {
            return propertyName.Split(new[] { '[', ']' })[0];
        }

        private static int GetProperyIndex(string propertyName)
        {
            return int.Parse(propertyName.Split(new[] { '[', ']' })[1]);
        }

        private static bool IsContainsPropertyIndex(string propertyName)
        {
            var splitted = propertyName.Split(new[] {'[', ']'});
            if (splitted.Length < 2)
                return false;

            int temp;
            return int.TryParse(splitted[1], out temp);
        }
    }
}
