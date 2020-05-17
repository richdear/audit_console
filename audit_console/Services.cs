using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace audit_console
{
    class Services
    {

        //-------------------------------------------Get service status------------------------------------
        public static void GetServiceStatus()
        {
            string data = "<services>" + Program.new_line;
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                
                data += "<service  display_name='" + service.DisplayName.Replace("'"," ") + "'  name='"+service.ServiceName+"'  status='" + service.Status + "' />" + Program.new_line;
                XMLWriter.WriteXML("ServiceName", service.ServiceName);
                XMLWriter.WriteXML("ServiceStatus", service.Status.ToString());
                Console.WriteLine(service.ServiceName + "==" + service.Status);
                Console.WriteLine(service.Status.ToString(), service.ServiceName.ToString());
            }
            data += "</services>" + Program.new_line;
            XMLWriter.Write(data);
        }
        //---------------------------------------------------------------------------------------------------
    }
}
