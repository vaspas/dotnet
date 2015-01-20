using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using IncModules;
using IncModules.Mio4400;
using Sigflow.Dataflow;
using Sigflow.Performance;
using Sigflow.Schema;

namespace ConsoleGenerator
{
    unsafe class Program
    {
        static void Main(string[] args)
        {
            var arr = new GainValues[2];

            var s1 = new XmlSerializer(typeof(GainValues[]));
            var sb = new StringBuilder();
            using (var stream = new System.IO.StringWriter(sb))
                s1.Serialize(stream, arr);
            Console.Write(sb.ToString());
            
            /*var list = new List<ISignalWriter<int>>();
            list.Add(null);
            var propertyInfo = list.GetType().GetProperty("Item");

            propertyInfo.SetValue(list, new Buffer<int>(), new object[] { 0 });*/

           /* var par = new IppModules.ToDbConverterModuleFloat();

            var s = new XmlSerializer(typeof(IppModules.ToDbConverterModuleFloat));
            var sb = new StringBuilder();
            using (var stream = new System.IO.StringWriter(sb))
                s.Serialize(stream, par);

            Console.Write(sb.ToString());*/

            object mod, mod2, mod3;
            var s = new XmlSerializer(typeof(int));
            var text = string.Format("<{0}>20</{0}>", typeof(int).FullName);
            using (var stream = new System.IO.StringReader("<properties><rew>20</rew><r>30</r></properties>"))
            {
                var r = XmlReader.Create(stream);
                r.ReadStartElement("properties");
                mod= r.ReadElementContentAs(typeof(int),null);
                mod2 = r.ReadElementContentAs(typeof(double), null);
                if(r.NodeType!=XmlNodeType.EndElement)
                    mod3 = r.ReadElementContentAs(typeof(double), null);
            }
            var a1 = new int[] {1, 2};
            var a2 = new int[] { 3, 4 };
            fixed (int* pSrc = a1, pDst = a2)
                for (var i = 0; i < 2; i++)
                    *(pDst + i) = *(pSrc + i );

            

            Console.ReadLine();

            var f=new XmlSchemaFactory
                {
                    Document = new XmlDocument()
                };

            using (var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream("ConsoleGenerator.Schemes.schema1.xml"))
                f.Document.Load(stream);

            f.Build().Start();

            /*var engine = new Performer();
            var schema = new Schemes.Schema1 { Engine = engine };
            schema.Build();

            engine.Start();*/

            Console.ReadLine();
        }

        /*static void Main(string[] args)
        {
            var engine = new Performer();
            var schema = new Schemes.Schema1 {Engine = engine};
            schema.Build();

            engine.Start();

            Console.ReadLine();
        }*/
    }
}
