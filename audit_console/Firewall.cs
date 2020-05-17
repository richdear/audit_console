using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace audit_console
{
    class Firewall
    {


        //-------------------------------------------Get firewall status----------------------------------
        public static void checkFirewall()
        {




            // Create the firewall type.
            Type FWManagerType = Type.GetTypeFromProgID("HNetCfg.FwMgr");

            // Use the firewall type to create a firewall manager object.
            dynamic FWManager = Activator.CreateInstance(FWManagerType);

            // Check the status of the firewall.
            string state = (FWManager.LocalPolicy.CurrentProfile.FirewallEnabled == true ? "ON" : "OFF");
            string text = "Firewall is " + state;
            string data = "<firewall state='" + state + "' />" + Program.new_line;
            XMLWriter.Write(data);

            XMLWriter.WriteXML("FirewallState", state);
            Console.WriteLine(text);
        }

        //----------------------------------------------------------------------------------------------------
    }
}
