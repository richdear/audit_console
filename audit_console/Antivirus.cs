using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;


namespace audit_console
{
    class Antivirus
    {
        //------------------------------------Get Antivirus Name---------------------------------------------
        public static void getAntivirusName()
        {
            ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            ManagementObjectCollection data = wmiData.Get();
            String data1 = "<antivirus>"+Program.new_line;
            foreach (ManagementObject virusChecker in data)
            {
                XMLWriter.WriteXML("AntivirusName", virusChecker["displayName"].ToString());
                XMLWriter.WriteXML("PathToSigendExe", virusChecker["pathToSignedProductExe"].ToString());

                data1 += "<product name='" + virusChecker["displayName"].ToString() + "' ";
                data1 += " path='" + virusChecker["pathToSignedProductExe"] + " '";
                data1 += " state='" + virusChecker["productState"] + " ' />"+Program.new_line;

                Console.WriteLine("Antivirus Name : " + virusChecker["displayName"]);
                Console.WriteLine("Instance guid : " + virusChecker["instanceGuid"]);
                Console.WriteLine("Path to antivirus exe : " + virusChecker["pathToSignedProductExe"]);
                string state = Convert.ToInt64(virusChecker["productState"]).ToString("X");
                XMLWriter.WriteXML("ProductState", state);
                Console.WriteLine("Antivirus state : " + state);
            }
            data1 += "</antivirus>" + Program.new_line;
            XMLWriter.Write(data1);
        }
        //----------------------------------------------------------------------------------------------------------------
        //-------------------------------------Get antivirus product-------------------------------------
        public static bool AntivirusInstalled()
        {

            string wmipathstr = @"\\" + Environment.MachineName + @"\root\SecurityCenter";
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipathstr, "SELECT * FROM AntivirusProduct");
                ManagementObjectCollection instances = searcher.Get();
                return instances.Count > 0;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }
    }
}
