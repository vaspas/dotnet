using System.Linq;
using System.Xml;
using Sigflow.Performance;

namespace Sigflow.Schema
{
    class XmlBeatFactory
    {
        public XmlElement BeatNode { get; set; }

        public PerformerContainer Container { get; set; }

        public void Create()
        {
            XmlSchemaFactoryLogger.AddToTree("Создание главного beat");

            Container.Performer.Beat = new BeatsOr();

            var id = BeatNode.GetAttribute(Words.Id);
            if (!string.IsNullOrEmpty(id))
                Container.Add(id, Container.Performer.Beat);

            new XmlFillProperties
            {
                Node = BeatNode,
                Object = Container.Performer.Beat
            }.Fill();

            BeatNode.ChildNodes.OfType<XmlElement>().ToList()
                .ForEach(n => Build(Container.Performer.Beat as IBeatCollection, n));

            //Build(Container.Performer.Beat as IBeatCollection, BeatNode);

            XmlSchemaFactoryLogger.RemoveFromTree();
        }

        private void Build(IBeatCollection parentBeat, XmlElement node)
        {
            XmlSchemaFactoryLogger.AddToTree("Создание дочернего beat");

            IBeatCollection newBeat;
            if (node.Name == Words.And)
                newBeat = new BeatsAnd();
            else if (node.Name == Words.Or)
                newBeat = new BeatsOr();
            else
                return;

            parentBeat.Add(newBeat);

            new XmlFillProperties
            {
                Node = node,
                Object = newBeat
            }.Fill();

            var id = node.GetAttribute(Words.Id);
            if (!string.IsNullOrEmpty(id))
                Container.Add(id, newBeat);

            node.ChildNodes.OfType<XmlElement>().ToList()
                .ForEach(n => Build(newBeat, n));

            XmlSchemaFactoryLogger.RemoveFromTree();
        }
    }
}
