using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;


namespace audit_console
{
    class OperatingSystem
    {
        public static void GetOSName()
        {
            ManagementObjectSearcher searcher;
            ArrayList hd = new ArrayList();
            string val = "";
            string title = "OSName";
            title = title.ToLower().Replace(' ', '_');
            try
            {
                searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_OperatingSystem");
                string data = "<" + title + " ";
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                 
                    string[] temp = wmi_HD["Name"].ToString().Split(' ');
                    string name = temp[1] + " " + temp[2];
                    XMLWriter.WriteXML(title, name);
                    data += " value='" +name + "' ";
                    Console.Write(title + " : ");
                    Console.WriteLine(temp);
                }
                data += "/>" + Program.new_line;
                XMLWriter.Write(data);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }

        
         
        }
        //-------------------------------------Get date time-----------------------------------------
        public static void GetSystemDateTime()
        {
            String date = DateTime.Now.ToString("MM/dd/yyyy");
            String time = DateTime.Now.ToString("H:mm");
            XMLWriter.WriteXML("SystemDate", date);
            XMLWriter.WriteXML("SystemTime", time);
            string data = "<system date='" + date + "' " + " time='" + time + "' />" + Program.new_line;
            XMLWriter.Write(data);

            Console.WriteLine("System date " + date);
            Console.WriteLine("System time " + time);
                }
        //------------------------------------------------------------------------------------------------------
        //-------------------------------------Get Shared Folders-------------------------------------------
        public static void GetSharedFolders()
        {
            string data = "<shared_folders>" + Program.new_line;
            Console.WriteLine("Shared Folders:");
            using (ManagementClass exportedShares = new
                        ManagementClass("Win32_Share"))
            using (ManagementClass computer = new
            ManagementClass("Win32_computersystem"))
            {
                string localSystem = null;
                ManagementObjectCollection localComputer = computer.GetInstances();
                foreach (ManagementObject mo in localComputer)
                {
                    localSystem = mo["Name"].ToString();
                }
                ManagementObjectCollection shares = exportedShares.GetInstances();
                String share2 = "";
                foreach (ManagementObject share in shares)
                    // dump UNC path
                     share2= share["Name"].ToString();
                data += "<folder path='" + share2 + "' />" + Program.new_line;
                    Console.WriteLine(@"UNC path: \\{0}\{1}", localSystem,share2 );
                    XMLWriter.WriteXML("SharedFolderPath", share2);
                    
            }
            data += "</shared_folders>" + Program.new_line;
            XMLWriter.Write(data);
        }
        //------------------------------------Check windows activation status ----------------------------
        public static void CheckLicense()
        {
            Console.WriteLine("Windows Activation status:");
            bool c = IsWindowsActivated();
            String status = c == true ? "Windows Is Activated." : "Windows is NOT Activated!";
            string data = "<windows_activation status='" + status + "' />";
            XMLWriter.Write(data);
            Console.WriteLine(status);
            XMLWriter.WriteXML("WindowsActivationStatus", status);
        }
        //-------------------------------------Get windows activation status------------------------------
        public static bool IsWindowsActivated2()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM SoftwareLicensingProduct WHERE PartialProductKey <> null AND ApplicationId='55c92734-d682-4d71-983e-d6ec3f16059f' AND LicenseIsAddon=False");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("LicenseStatus: " + queryObj["LicenseStatus"] + " - ProductKeyID: " + queryObj["ProductKeyID"]);

                    return Convert.ToBoolean(queryObj["LicenseStatus"]);
                }
            }
            catch (ManagementException me)
            {
                Console.WriteLine("WMI Error: " + me.Message);

            }
            return false;
        }
        //----------------------------------------------------------------------------------------------------------
        //-------------------------------------For xp there is another method---------------------------
        public static bool IsWindowsActivated()
        {
            ManagementScope scope = new ManagementScope(@"\\" + System.Environment.MachineName + @"\root\cimv2");
            scope.Connect();

            SelectQuery searchQuery = new SelectQuery("SELECT * FROM SoftwareLicensingProduct WHERE ApplicationID = '55c92734-d682-4d71-983e-d6ec3f16059f' and LicenseStatus = 1");
            ManagementObjectSearcher searcherObj = new ManagementObjectSearcher(scope, searchQuery);

            using (ManagementObjectCollection obj = searcherObj.Get())
            {
                return obj.Count > 0;
            }
        }
        //-------------------------------------------------------------------------------------------
        //--------------------------------------Get edition----------------------------------------
        public static string GetEdition(string str)
        {
            string a = str;
            bool checker = false;
            int val;
            int index = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (Char.IsDigit(a[i]))
                {
                    checker = true;
                }
                if (!Char.IsDigit(a[i]) && checker)
                {
                    index = i;
                    break;
                }

            }
            return a.Substring(index);
        }

        //------------------------------------------------------------------------------------------------------------------------------

        //---------------------------------------------get install date of windows---------------------------------------------------
        public static ArrayList GetInstallDate(string queryObject, string property, string title)
        {
            ManagementObjectSearcher searcher;
            ArrayList hd = new ArrayList();
            title = title.ToLower().Replace(' ', '_');
            string data = "<" + title + " ";
            try
            {
                searcher = new ManagementObjectSearcher("SELECT " + property + " FROM " + queryObject);

                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    string[] insDate = ConvertToDateTime(wmi_HD[property].ToString()).Split(' ');
                    data += " value='" + insDate[0] + "' ";
                    XMLWriter.WriteXML(title, insDate[0]);
                    Console.Write(title + " : ");
                    Console.WriteLine(ConvertToDateTime(wmi_HD[property].ToString()));
                }
                data += "/>" + Program.new_line;
                XMLWriter.Write(data);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }

            return hd;
        }



        //--------------------------Convert string to date time------------------------------------
        private static string ConvertToDateTime(string unconvertedTime)
        {
            string convertedTime = "";
            int year = int.Parse(unconvertedTime.Substring(0, 4));
            int month = int.Parse(unconvertedTime.Substring(4, 2));
            int date = int.Parse(unconvertedTime.Substring(6, 2));
            int hours = int.Parse(unconvertedTime.Substring(8, 2));
            int minutes = int.Parse(unconvertedTime.Substring(10, 2));
            int seconds = int.Parse(unconvertedTime.Substring(12, 2));
            string meridian = "AM";
            //if (hours > 12)
            //{
            //    hours -= 12;
            //    meridian = "PM";
            //}
            convertedTime = date.ToString() + "/" + month.ToString() + "/" + year.ToString() + " " +
            hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString();// + " " + meridian;
            return convertedTime;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------get screen saver----------------------------------------------------------
        public static ArrayList GetScreenSaver(string queryObject, string property, string title, string filter)
        {
            ManagementObjectSearcher searcher;
            ArrayList hd = new ArrayList();
            try
            {
                title = title.ToLower().Replace(' ', '_');
                searcher = new ManagementObjectSearcher("SELECT * FROM " + queryObject);
                bool checker = false;
                string data = "<" + title + " ";
                foreach (ManagementObject wmi_HD in searcher.Get())
                {

                    if (wmi_HD["Name"].ToString().Equals(filter))
                    {
                        data += " value='" + wmi_HD[property].ToString() + "' ";
                        XMLWriter.WriteXML(title, wmi_HD[property].ToString());
                        Console.Write(title + " : ");
                        Console.WriteLine(wmi_HD[property].ToString());
                        break;
                    }

                }
                data += "/>" + Program.new_line;
                XMLWriter.Write(data);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }

            return hd;
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        // ----------------------------get user name for screen saver------------------------------------------
        public static string GetUserNameScreenSaver()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            return userName;
        }





        public static ArrayList GetStuff(string queryObject)
        {
            ManagementObjectSearcher searcher;
            ArrayList hd = new ArrayList();
            try
            {
                searcher = new ManagementObjectSearcher("SELECT * FROM " + queryObject);
                foreach (ManagementObject wmi_HD in searcher.Get())
                {

                    PropertyDataCollection searcherProperties = wmi_HD.Properties;
                    foreach (PropertyData sp in searcherProperties)
                    {
                        hd.Add(sp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }

            return hd;
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        //-------------------------------------Get edition from API------------------------------
        public static ArrayList GetEditionFromAPI(string queryObject, string property, string title)
        {
            ManagementObjectSearcher searcher;
            ArrayList hd = new ArrayList();
            title = title.ToLower().Replace(' ', '_');
            try
            {
                searcher = new ManagementObjectSearcher("SELECT " + property + " FROM " + queryObject);
                string data = "<" + title + " ";
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    XMLWriter.WriteXML(title, GetEdition(wmi_HD[property].ToString()));
                    data += " value='" + GetEdition(wmi_HD[property].ToString()) + "' ";
                    Console.Write(title + " : ");   
                    Console.WriteLine(GetEdition(wmi_HD[property].ToString()));

                }

                data += "/>" + Program.new_line;
                XMLWriter.Write(data);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }

            return hd;
        }

        //-------------------------------------List installed programs---------------------------------
        public static void InstalledPrograms()
        {
            Console.WriteLine("Installed Programs..............................................................");
            ArrayList ar = new ArrayList();
            string data = "<installed_programs>" + Program.new_line;
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        ar.Add(subkey.GetValue("DisplayName"));
                        //Con(subkey.GetValue("DisplayName"));
                    }
                }
            }
            ar.Sort();
            for (int i = 0; i < ar.Count; i++)
            {
                if (ar[i] != null)
                {
                    data += "<product name='" + ar[i].ToString() + "' />"+Program.new_line;
                    XMLWriter.WriteXML("InstalledPrograms", ar[i].ToString());
                    Console.WriteLine(ar[i].ToString());
                }
                   
            }
            data += "</installed_programs>" + Program.new_line;
            XMLWriter.Write(data);
            Console.WriteLine("..............................................................");
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


    }
}
