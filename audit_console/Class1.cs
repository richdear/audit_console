using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Xml;
using System.Xml.Linq;

namespace audit_console
{
    class Class1
    {

        public static XmlDocument doc = new XmlDocument();
        public static XmlNode root = doc.CreateElement("parameters");

        //----------------------------------------Write data--------------------------------------------------------------
        public static void Con(string s, string p = "no")
        {
            if (p != "no")
            {
                XmlNode nameNode = doc.CreateElement(ReplaceWhitespace(p));
                nameNode.AppendChild(doc.CreateTextNode(s));
                root.AppendChild(nameNode);
            }
            else
            {
                Console.WriteLine(s);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------
        //------------------------------------Remove whitespace---------------------------------------------
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement = "")
        { return sWhitespace.Replace(input, replacement); }

        //--------------------------------GEt users and groups-----------------------------------------------












        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<















        //--------------------------------------Print dots---------------------------------
        public static void Dots()
        {
            Con("----------------------------------------------------------------------------------");
        }



        //-----------------------------------Write to xml file--------------------------------------------
        public static void WriteXML()
        {
            new XDocument(
                new XElement("root",
                    new XElement("someNode", "someValue")
                )
            )
            .Save("foo.xml");
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode productsNode = doc.CreateElement("products");
            doc.AppendChild(productsNode);

            XmlNode productNode = doc.CreateElement("product");
            XmlAttribute productAttribute = doc.CreateAttribute("id");
            productAttribute.Value = "01";
            productNode.Attributes.Append(productAttribute);
            productsNode.AppendChild(productNode);

            XmlNode nameNode = doc.CreateElement("Name");
            nameNode.AppendChild(doc.CreateTextNode("Java"));
            productNode.AppendChild(nameNode);
            XmlNode priceNode = doc.CreateElement("Price");
            priceNode.AppendChild(doc.CreateTextNode("Free"));
            productNode.AppendChild(priceNode);

            // Create and add another product node.
            productNode = doc.CreateElement("product");
            productAttribute = doc.CreateAttribute("id");
            productAttribute.Value = "02";
            productNode.Attributes.Append(productAttribute);
            productsNode.AppendChild(productNode);
            nameNode = doc.CreateElement("Name");
            nameNode.AppendChild(doc.CreateTextNode("C#"));
            productNode.AppendChild(nameNode);
            priceNode = doc.CreateElement("Price");
            priceNode.AppendChild(doc.CreateTextNode("Free"));
            productNode.AppendChild(priceNode);

            doc.Save("example.xml");
        }




        //---------------------------------------Set Registry-----------------------------------
        public static void SetUAC()
        {
            RegistryKey rkey1;
            rkey1 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
            //RegistryKey key = Registry.CurrentUser.OpenSubKey("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);

            // adding/editing a value 
            rkey1.SetValue("EnableLUA", "1"); //sets 'someData' in 'someValue' 

            rkey1.Close();
            RegistryKey rkey;
            rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
            //RegistryKey key = Registry.CurrentUser.OpenSubKey("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);

            // adding/editing a value 
            rkey.SetValue("ConsentPromptBehaviorAdmin", "2"); //sets 'someData' in 'someValue' 

            rkey.Close();
        }

        //------------------------------------Registry test------------------------------------
        public static void RegistryTest()
        {
            // Create subkey 'CodiCode' and a value pair “testKey”,”123123”
            Microsoft.Win32.RegistryKey rkey;
            rkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Test\\CodiCode");
            rkey.SetValue("TestKey", "123123");
            rkey.Close();



            rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\ABBYY\\Lingvo\\16\\General");
            if (rkey == null)
            {
                // the Key does not exist
                Con("Key is null here.");
            }
            else
            {

                string myTestKey = (string)rkey.GetValue("ShowBaloon");
                Con("Key is not null " + myTestKey);
            }


            rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\7-Zip");
            if (rkey == null)
            {
                // the Key does not exist
                Con("Key is null here.");
            }
            else
            {

                string myTestKey = (string)rkey.GetValue("Path");
                Con("Key is not null " + myTestKey);
            }
        }







    }




}
