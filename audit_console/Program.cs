using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;


namespace audit_console
{
    class Program
    {
        public static XmlDocument doc = new XmlDocument();
        public static XmlNode root = doc.CreateElement("parameters");
        public static string FILE_NAME = "audit_info.xml";
        public static string new_line = Environment.NewLine;
        public static void Main()
        {
            XMLWriter.PrepareXMLWriter();
            Process.Start("cmd", @"/c  secedit /export /areas SECURITYPOLICY /cfg pol.txt");
            Networking.CheckInternet();
            APIHelper.GetApiProperty("Win32_ComputerSystem", "Model", "PC model");
            APIHelper.GetApiProperty("Win32_BIOS", "SerialNumber", "Serial Number");
           APIHelper. GetApiProperty("Win32_OperatingSystem", "CSName", "PC name");
            Networking.GetIpAndMac();
            Console.WriteLine("Language is " + Misc.GetSystemLanguage());
            Users.GetUserData();
            OperatingSystem.GetScreenSaver("Win32_Desktop", "ScreenSaverActive", "Screen Saver", OperatingSystem.GetUserNameScreenSaver());
            //APIHelper.GetApiProperty("Win32_OperatingSystem", "Name", "Operating System Name");
            OperatingSystem.GetOSName();
            OperatingSystem.GetInstallDate("Win32_OperatingSystem", "InstallDate", "Windows Installed date");
           APIHelper.GetApiProperty("Win32_OperatingSystem", "ServicePackMajorVersion", "Service Pack Major");
           APIHelper.GetApiProperty("Win32_OperatingSystem", "ServicePackMinorVersion", "Service Pack Minor");
           APIHelper.GetApiProperty("Win32_OperatingSystem", "BuildNumber", "Build");
            OperatingSystem.GetEditionFromAPI("Win32_OperatingSystem", "Caption", "Edition");
           // OperatingSystem.CheckLicense();
            Antivirus.getAntivirusName();
           OperatingSystem.InstalledPrograms();
           MSOffice.GetOfficeVersion();
           Domain.checkDomain();
           Firewall.checkFirewall();
           Services.GetServiceStatus();
           Hardware.checkUSbPorts();
           Hardware.checkOpticalDrive();
           Hardware.ListUSBDev();
           Hardware.ListPrinters();
            //WriteXML();
           SecurityPolicy.PasswordPolicy();
            OperatingSystem.GetSharedFolders();
            OperatingSystem.GetSystemDateTime();
            Networking.GetOpenPort();
            XMLWriter.CloseXML();

            /* 
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            doc.AppendChild(root);
            Dots();
            GetApiProperty("Win32_ComputerSystem","Model","PC model");
            Dots();
            GetApiProperty("Win32_ComputerSystem", "Manufacturer", "Manufacturer");
            Dots();
            GetApiProperty("Win32_BIOS", "SerialNumber", "Serial Number");
            Dots();
            GetApiProperty("Win32_OperatingSystem", "CSName", "PC name");
            Dots();
            GetUserName();
            Dots();
            GetIpAndMac();
            Dots();
            Con("Is Password SET : " + (PasswordRequired()== true?"YES":"NO"));
            Dots();
            GetScreenSaver("Win32_Desktop", "ScreenSaverActive", "Screen Saver",GetUserNameScreenSaver());
            Dots();
            GetApiProperty("Win32_OperatingSystem", "Name", "Operating System Name");
            Dots();
            GetInstallDate("Win32_OperatingSystem", "InstallDate", "Windows Installed date");
            Dots();
            GetApiProperty("Win32_OperatingSystem", "ServicePackMajorVersion", "Service Pack Major");
            Dots();
            GetApiProperty("Win32_OperatingSystem", "ServicePackMinorVersion", "Service Pack Minor");
            Dots();
            GetApiProperty("Win32_OperatingSystem", "BuildNumber", "Build");
            Dots();
            GetEditionFromAPI("Win32_OperatingSystem", "Caption", "Edition");
            //Con("Windows Activation status:");
            //bool c=IsWindowsActivated();
            //Con(c==true?"Windows Is Activated.":"Windows is NOT Activated!");
            Dots();
            getAntivirusName();
            Dots();
            InstalledPrograms();
            Dots();
            GetOfficeVersion();
            Dots();
            checkDomain();
            Dots();
            checkFirewall();
            Dots();
            GetServiceStatus();
            Dots();
            checkUSbPorts();
            Dots();
            checkOpticalDrive();
            Dots();
            ListUSBDev();
            Dots();
            ListPrinters();
            Dots();
            Con("Local Security Policy");
            PasswordPolicy();
           
            WriteXML();
            // Process.Start("cmd",@"/c  secedit /export /areas SECURITYPOLICY /cfg pol.txt");            
            //Process.Start("cmd", "secedit /export /areas SECURITYPOLICY /cfg C:\\Users\\SuperUser\\Desktop\\pol.txt");
            //GetUSBDev();
            //SetUAC();
            doc.Save("example.xml");*/
          
            Console.ReadLine();
            
        }

        

    }
}
