using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Sigflow.Schema
{
    class XmlFillProperties
    {
        public XmlElement Node { get; set; }

        public object Object { get; set; }

        public void Fill()
        {
            foreach(var n in Node.ChildNodes.OfType<XmlElement>().ToList().FindAll(e=>e.Name==Words.Property))
            {
                var name = n.GetAttribute(Words.Name);

                XmlSchemaFactoryLogger.AddToTree("Инициализация свойства " + name);

                if(Object is ICollection)
                    foreach (var o in (Object as ICollection))
                        SetProperty(o, name, n.InnerXml);
                else
                    SetProperty(Object, name, n.InnerXml);

                XmlSchemaFactoryLogger.RemoveFromTree();
            }

            foreach (var n in Node.ChildNodes.OfType<XmlElement>().ToList().FindAll(e => e.Name == Words.Properties))
            {
                XmlSchemaFactoryLogger.AddToTree("Инициализация свойств");
                using (var stream = new System.IO.StringReader(n.OuterXml))
                {
                    var r = XmlReader.Create(stream);
                    r.ReadStartElement(Words.Properties);
                    
                    while (r.NodeType != XmlNodeType.EndElement)
                    {
                        if (r.NodeType == XmlNodeType.Element)
                            SetProperty(Object, r);
                        else
                            r.Read();
                        
                    }

                    //mod = r.ReadElementContentAs(typeof(int), null);
                    //mod2 = r.ReadElementContentAs(typeof(double), null);
                }
                XmlSchemaFactoryLogger.RemoveFromTree();

                /*if (Object is ICollection)
                    foreach (var o in (Object as ICollection))
                        SetProperty(o, name, n.InnerXml);
                else
                    SetProperty(Object, name, n.InnerXml);*/
            }
        }

        private static void SetProperty(object obj, string name, string xmlText)
        {
            var propInfo = obj.GetType().GetProperty(name);

            if (propInfo == null)
            {
                XmlSchemaFactoryLogger.AddWarning(string.Format(
                    "Не существует свойства \"{0}\" для типа {1}",
                    name, obj.GetType().Name));
            }

            var s = new XmlSerializer(propInfo.PropertyType);
            using (var stream = new System.IO.StringReader(xmlText))
                propInfo.SetValue(obj, s.Deserialize(XmlReader.Create(stream)), null);
            
        }

        private static void SetProperty(object obj, XmlReader reader)
        {
            var realObj = obj is IList ? (obj as IList)[0] : obj;

            var propertyInfo = realObj.GetType().GetProperty(reader.Name);

            if (propertyInfo == null)
            {
                XmlSchemaFactoryLogger.AddWarning(string.Format(
                    "Не существует свойства \"{0}\" для типа {1}",
                    reader.Name, obj.GetType().Name));
            }

            var value = reader.ReadElementContentAs(propertyInfo.PropertyType, null);
            
            if (obj is IList)
                for (var i = 0; i < (obj as IList).Count;i++ )
                    propertyInfo.SetValue((obj as IList)[i], value, null);
            else
                propertyInfo.SetValue(obj, value, null);

        }

    }
}
