using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace audit_console
{
    class XMLWriter
    {
        public static XmlDocument doc = new XmlDocument();
        public static XmlNode root = doc.CreateElement("parameters");
      public static  XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
      public static  XmlNode productsNode = doc.CreateElement("Parameters");
        public static void PrepareXMLWriter()
        {
            if (File.Exists("audit_info.xml"))
            {
                File.Delete("audit_info.xml");
            }
            string data = "<pc_parameters>";
            File.AppendAllText(Program.FILE_NAME, data);
            doc.AppendChild(docNode);
           
            doc.AppendChild(productsNode);
        }
            public static void WriteXML(String parent,String child)
        {
            XmlNode nameNode = doc.CreateElement(SecurityPolicy.ReplaceWhitespace(parent));
            nameNode.AppendChild(doc.CreateTextNode(child));
            productsNode.AppendChild(nameNode);
        }

       
        public static void CloseXML()
        {
            string data = "</pc_parameters>";
            File.AppendAllText(Program.FILE_NAME, data);
            doc.Save("example.xml");
        }


        public static void Write(string data)
        {
            File.AppendAllText(Program.FILE_NAME, data);
        }
    }
}
