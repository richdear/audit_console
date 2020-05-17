using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace audit_console
{
    class MSOffice
    {
        //-------------------------------------get office version------------------------------------
        public static void GetOfficeVersion()
        {
            string[] versions = new string[] { "7.0", "8.0", "9.0", "10.0", "11.0", "12.0", "14.0 (sic!)", "15.0", "16.0", "16.0 (sic!)" };
            string[] versionsText = new string[] { "Office 97 ", "Office 98", "Office 2000", "Office XP", "Office 2003", "Office 2007", "Office 2010", "Office 2013", "Office 2016", "Office 2019" };
            Console.WriteLine("Getting Office Version............................");
            string data = "<msoffice>" + Program.new_line;
            for (int i = 0; i < versions.Length; i++)
            {
                if (checkIfExists(versions[i]))
                {
                    data += "<product version='" + versionsText[i] + "' />" + Program.new_line;
                    XMLWriter.WriteXML("MSOfficeVersion", versionsText[i]);
                    Console.WriteLine(versionsText[i]);
                }
            }
            data += "</msoffice>" + Program.new_line;
            XMLWriter.Write(data);
        }


        public static bool checkIfExists(string version)
        {
            Microsoft.Win32.RegistryKey rkey;
            rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Office\\" + version + "\\Word\\InstallRoot");
            if (rkey == null)
            {
                // the Key does not exist
                //Con("Key is null here.");
                return false;
            }
            else
            {

                string myTestKey = (string)rkey.GetValue("Path");
                // Con("Key is not null " + myTestKey);
                return true;
            }

        }


        //---------------------------------------------------------------------------------------------

    }
}
