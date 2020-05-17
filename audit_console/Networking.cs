using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;


namespace audit_console
{
    class Networking
    {
        //----------------------Check if user is connected to Internet-----------------
        public static void CheckInternet()
        {
            string data = "<internet state='" + PingNetwork() + "' />";
            XMLWriter.Write(data);
        }
            
            
            //-------------------------Check connection for given IP address----------------------------
        public static string PingNetwork()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    System.Console.WriteLine("INTERNET CONNECTION IS ON");
                return "connected";
            }
            catch
            {
                System.Console.WriteLine("INTERNET CONNECTION IS OFF");
                return "Not connected";
            }

        }





        //----------------------Get open ports-------------------------------------------
        public static string GetOpenPort()
        {
            int PortStartIndex = 1000;
            int PortEndIndex = 2000;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).Distinct().ToList<int>();
            string data = "<open_ports>" + Program.new_line; 
            foreach(object port in usedPorts)
            {
                data += "<port value='" + port.ToString() + "' />" + Program.new_line; ;
                XMLWriter.WriteXML("OpenPort", port.ToString());
                Console.WriteLine(port);
            }
            data += "</open_ports>" + Program.new_line; ;
            XMLWriter.Write(data);
            int unusedPort = 0;
            /*
            for (int port = PortStartIndex; port < PortEndIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            */
            return unusedPort.ToString();
        }




        //------------------------Get Ip and mac addresses-------------------------------------------------
        public static void GetIpAndMac()
        {
            Console.WriteLine("");
            Console.WriteLine("...........................................Network Interface information............................................");
            string data = "<networking>"+Program.new_line;
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                XMLWriter.WriteXML("MACAddress", netInterface.GetPhysicalAddress().ToString());
                Console.WriteLine("");
                data += "   <network_interface name= '" + netInterface.Name + "' ";
                data += " description= '" + netInterface.Description + "'>" + Program.new_line; ;
                data += "       <mac_address  value='" + netInterface.GetPhysicalAddress().ToString() + "' />" + Program.new_line; ;
                
                Console.WriteLine("Network Interface Name: " + netInterface.Name);
                Console.WriteLine("Description: " + netInterface.Description);
                Console.WriteLine("MAC address: " + netInterface.GetPhysicalAddress().ToString());
                Console.WriteLine("IP Addresses: ");
                IPInterfaceProperties ipProps = netInterface.GetIPProperties();
                String IPAddresses = "";
                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                {
                    data += "       <ip_address value='"+ addr.Address.ToString()+"' />" + Program.new_line; ;
                    IPAddresses += "" + addr.Address.ToString() + "\n";
                  //  XMLWriter.WriteXML("IPAddress", addr.Address.ToString());
                    Console.WriteLine(" " + addr.Address.ToString());
                }
                data += "   </network_interface>" + Program.new_line; ;
               
                XMLWriter.WriteXML("network", data);
            }
            data += "</networking>" + Program.new_line; ;
            XMLWriter.Write(data);
            Console.WriteLine(".........................................................................");
        }
        //---------------------------------------------------------------------------------------------------------------------------

    }
}
