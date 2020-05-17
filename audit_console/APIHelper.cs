using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;


namespace audit_console
{
    class APIHelper
    {
        //---------------------get api's specific property from win32 API-------------------------------------
        public static string GetApiProperty(string queryObject, string property, string title, string filter = null)
        {
            ManagementObjectSearcher searcher;
            ArrayList hd = new ArrayList();
            string val = "";
            title = title.ToLower().Replace(' ','_');
            try
            {
                searcher = new ManagementObjectSearcher("SELECT " + property + " FROM " + queryObject);
                string data = "<"+title+" ";
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    XMLWriter.WriteXML(title, wmi_HD[property].ToString());
                    Console.Write(title + " : ");
                    data += " value='" + wmi_HD[property].ToString() + "' ";
                    Console.WriteLine(wmi_HD[property].ToString());
                    val = wmi_HD[property].ToString();
                }
                data += "/>"+Program.new_line;
                XMLWriter.Write(data);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }

            return val;
        }
        //----------------------------------------------------------------------------------------------------------
    }
}
