using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigflow.Schema
{
    public static class XmlSchemaFactoryLogger
    {
        public static string BuildLog()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Сообщения:");
            Messages.ForEach(b => sb.AppendLine(b));

            sb.AppendLine();
            sb.AppendLine("Стек:");
            BuildTree.ToList().ForEach(b => sb.AppendLine(b));
            
            return sb.ToString();
        }

        private static readonly List<string> Messages = new List<string>();

        public static void AddWarning(string message)
        {
            Messages.Add(message);
        }

        

        private static readonly Stack<string> BuildTree = new Stack<string>();

        public static void AddToTree(string work)
        {
            BuildTree.Push(work);
        }

        public static void RemoveFromTree()
        {
            BuildTree.Pop();
        }

        public static void Clear()
        {
            Messages.Clear();
            BuildTree.Clear();
        }
    }
}
