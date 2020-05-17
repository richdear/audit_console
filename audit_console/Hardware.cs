using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;


namespace audit_console
{
    class Hardware
    {
        //------------------------------------Check if usb ports are enabled---------------------------
        public static void checkUSbPorts()
        {
            var value = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", null);
            string state = (Convert.ToInt32(value) == 3 ? "Enabled" : "Disabled");
            XMLWriter.WriteXML("USBPortsEnabled", state);
            Console.WriteLine("USB ports enabled: " +state);
            string data = "<usb_port state='" + state + "' />"+Program.new_line;
            XMLWriter.Write(data);
        }
        //--------------------------------------------------------------------------------------------------

        //------------------------------------Check if optical drive   enabled---------------------------
        public static void checkOpticalDrive()
        {
            var value = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\cdrom", "Start", null);
            string state = (Convert.ToInt32(value) == 1 ? "Enabled" : "Disabled");
            XMLWriter.WriteXML("OpticalDriveEnabled", state);
            Console.WriteLine("Optical drive enabled: " +state);

            string data = "<optical_disc state='" + state + "' />" + Program.new_line;
            XMLWriter.Write(data);
        }

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        //------------------------------------------------List usb devices------------------------------------------------
        public static void ListUSBDev()
        {
            try
            {

                string data = "<usb_devices>" + Program.new_line;
                Console.WriteLine("USB devices..............................................................");
                ArrayList ar = new ArrayList();
                string registry_key = @"SYSTEM\CurrentControlSet\Enum\USBSTOR";
                RegistryKey usb_tor = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USBSTOR");
                foreach (var disk_class_name in usb_tor.GetSubKeyNames())
                {
                    // Con("DISK CLASS : " + disk_class_name);
                    RegistryKey usb_class_key = usb_tor.OpenSubKey(disk_class_name);
                    if (usb_class_key != null)
                    {
                        //Con("USB class IS NOT NUll");
                        foreach (var usb_serial_number in usb_class_key.GetSubKeyNames())
                        {

                            // Con("SUB KEY: " + usb_serial_number);
                            RegistryKey usb_device = usb_class_key.OpenSubKey(usb_serial_number);
                            //string usbValue = Convert.ToString(usb_device.GetValue("DeviceDesc"));
                            string usbValue = Convert.ToString(usb_device.GetValue("FriendlyName"));
                            XMLWriter.WriteXML("USBDevice", usbValue);
                            Console.WriteLine(usbValue, "USBDevices");

                            data += "<device name='" + usbValue + "' />" + Program.new_line;

                            //if (usb_device != null)
                            //{
                            //    Con("usb_device IS NOT NUll");
                            //    foreach (var usb in usb_device.GetValueNames())
                            //    {
                            //        Con("\tValue:" + usb);

                            //        // Check for the publisher to ensure it's our product

                            //        // Do something with this valuable information
                            //    }
                            //}


                            //Con("\tValue:" + value);

                            //// Check for the publisher to ensure it's our product
                            //string keyValue = Convert.ToString(usb_class_key.GetValue("FriendlyName"));
                            //Con("Name: " + keyValue);
                            //// Do something with this valuable information
                        }
                    }
                }
                data += "</usb_devices>"+Program.new_line;
                XMLWriter.Write(data);
                Console.WriteLine("..............................................................");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        //------------------------------------------------List Printers------------------------------------------------
        public static void ListPrinters()
        {
            try
            {
                Console.WriteLine("Printers..............................................................");
                string data = "<printers>" + Program.new_line;
                ArrayList ar = new ArrayList();
                string registry_key = @"SYSTEM\CurrentControlSet\Enum\USBSTOR";
                RegistryKey usb_tor = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USBPRINT");
                foreach (var disk_class_name in usb_tor.GetSubKeyNames())
                {
                    // Con("DISK CLASS : " + disk_class_name);
                    RegistryKey usb_class_key = usb_tor.OpenSubKey(disk_class_name);
                    if (usb_class_key != null)
                    {
                        //Con("USB class IS NOT NUll");
                        foreach (var usb_serial_number in usb_class_key.GetSubKeyNames())
                        {

                            // Con("SUB KEY: " + usb_serial_number);
                            RegistryKey usb_device = usb_class_key.OpenSubKey(usb_serial_number);
                            string usbValue = Convert.ToString(usb_device.GetValue("DeviceDesc"));
                            //string usbValue = Convert.ToString(usb_device.GetValue("FriendlyName"));
                            Console.WriteLine("Name: " + usbValue);
                            Console.WriteLine(usbValue, "Printers");
                            XMLWriter.WriteXML("Printer", usbValue);

                            data += "<printer name='" + usbValue + "' />"+Program.new_line;
                            //if (usb_device != null)
                            //{
                            //    Con("usb_device IS NOT NUll");
                            //    foreach (var usb in usb_device.GetValueNames())
                            //    {
                            //        Con("\tValue:" + usb);

                            //        // Check for the publisher to ensure it's our product

                            //        // Do something with this valuable information
                            //    }
                            //}


                            //Con("\tValue:" + value);

                            //// Check for the publisher to ensure it's our product
                            //string keyValue = Convert.ToString(usb_class_key.GetValue("FriendlyName"));
                            //Con("Name: " + keyValue);
                            //// Do something with this valuable information
                        }
                    }
                }
                data += "</printers>" + Program.new_line;
                XMLWriter.Write(data);
                Console.WriteLine("..............................................................");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        //---------------------------------------Get usb devices-----------------------------

        public static void GetUSBDev()
        {
            var usbDevices = GetUSBDevices();

            foreach (var usbDevice in usbDevices)
            {
                Console.WriteLine("Device ID: " + usbDevice.DeviceID + "\n, PNP Device ID: " + usbDevice.PnpDeviceID + "\n, Description: " + usbDevice.Description + "\n");
            }

        }




        static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();
            // where DeviceID Like ""USB%""
            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity "))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }


        class USBDeviceInfo
        {
            public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
            {
                this.DeviceID = deviceID;
                this.PnpDeviceID = pnpDeviceID;
                this.Description = description;
            }
            public string DeviceID { get; private set; }
            public string PnpDeviceID { get; private set; }
            public string Description { get; private set; }
        }

    }
}
