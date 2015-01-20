using System.Collections;
using System.Xml;
using Sigflow.Performance;

namespace Sigflow.Schema
{
    class XmlBeatConnectionBuilder
    {
        public XmlElement Node { get; set; }

        public SchemaContainer Container { get; set; }

        public Performer Performer { get; set; }

        public object Beat { get; set; }

        public void Build()
        {
            var beatId = Node.GetAttribute(Words.Beat);

            if (string.IsNullOrEmpty(beatId))
                return;

            XmlSchemaFactoryLogger.AddToTree("Установка beat");

            var beatCollection = Container.Get<IBeatCollection>(Performer, beatId)
                                 ?? Container.Get<IBeatCollection>(beatId);

            if (beatCollection == null)
            {
                XmlSchemaFactoryLogger.AddWarning(string.Format(
                    "Не существует beat \"{0}\" указанного для объекта \"{1}\"",
                    beatId, Node.Name));
            }

            if(Beat is ICollection)
                foreach (var b in (Beat as ICollection))
                    beatCollection.Add((IBeat)b);
            else
                beatCollection.Add((IBeat)Beat);

            XmlSchemaFactoryLogger.RemoveFromTree();
        }
    }
}
