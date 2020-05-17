using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace audit_console
{
    class Users
    {
        public static string data = "<users>"+Program.new_line;
       public static void GetUserData()
        {

            if (Misc.GetSystemLanguage() == "ru-RU")
            {
                GetUsersByGroup("Пользователи");
                GetUsersByGroup("Администраторы");
                GetUsersByGroup("Гости");

            }
            else
            {
                GetUsersByGroup("Users");
                GetUsersByGroup("Administrators");
                GetUsersByGroup("Guests");
            }
           

            data += "</users>" + Program.new_line;
            XMLWriter.Write(data);
        }



        //---------------------------------------------Get user by group-----------------------------------------------------------------------------
        public static void GetUsersByGroup(string group)
        {
          
            Console.WriteLine(group);
            using (System.DirectoryServices.DirectoryEntry d = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer"))
            {
                using (DirectoryEntry g = d.Children.Find(group, "group"))
                {
                    object members = g.Invoke("Members", null);
                    foreach (object member in (IEnumerable)members)
                    {
                        data += "<user ";
                        DirectoryEntry x = new DirectoryEntry(member);
                        data += " name='" + x.Name + "' ";
                        data += " group='" + group + "' ";
                        data += " password_set='" + PasswordRequired(x.Name).ToString() + "' />";
                        data+= Program.new_line;
                        XMLWriter.WriteXML("Group", group);
                        XMLWriter.WriteXML("UserName", x.Name);
                        XMLWriter.WriteXML("PasswordRequired", PasswordRequired(x.Name).ToString());
                        Console.Write(group+" : ");
                        Console.Write(x.Name+" : ");
                        Console.Write(PasswordRequired(x.Name));
                        Console.WriteLine();
                    }

                }

                Console.WriteLine();
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------

       //----------------------------------------------Get current user name and its domain------------------------------------
        public static void  GetCurrentUserName()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            string domainName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').First();

            data = "<current_user name='" + userName + "' domain='" + domainName + "' />"+Program.new_line;
            XMLWriter.Write(data);

            XMLWriter.WriteXML("CurrentUser", userName);
            XMLWriter.WriteXML("CurrentUserDomain", domainName);
            Console.WriteLine("Current User Name : " + userName);
            Console.WriteLine("Current Domain Name : " + domainName);
        }
        //-----------------------------------------------------------------------------------------------------------------------------

        //------------------------------------------Check if user has set password-------------------------------------------------
        public static bool PasswordRequired(string username)
        {

            IntPtr phToken;

            // http://www.pinvoke.net/default.aspx/advapi32/LogonUser.html
            bool loggedIn = LogonUser(username,
                null,
                "",
                (int)LogonType.LOGON32_LOGON_INTERACTIVE,
                (int)LogonProvider.LOGON32_PROVIDER_DEFAULT,
                out phToken);

            int error = Marshal.GetLastWin32Error();

            if (phToken != IntPtr.Zero)
                // http://www.pinvoke.net/default.aspx/kernel32/CloseHandle.html
                CloseHandle(phToken);

            // 1327 = empty password
            if (loggedIn || error == 1327)
                return false;
            else
                return true;

        }


        [DllImport("advapi32.dll", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LogonUser(
        [MarshalAs(UnmanagedType.LPStr)] string pszUserName,
        [MarshalAs(UnmanagedType.LPStr)] string pszDomain,
        [MarshalAs(UnmanagedType.LPStr)] string pszPassword,
        int dwLogonType,
        int dwLogonProvider,
        out IntPtr phToken);



        public enum LogonType
        {
            /// <summary>
            /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
            /// by a terminal server, remote shell, or similar process.
            /// This logon type has the additional expense of caching logon information for disconnected operations;
            /// therefore, it is inappropriate for some client/server applications,
            /// such as a mail server.
            /// </summary>
            LOGON32_LOGON_INTERACTIVE = 2,

            /// <summary>
            /// This logon type is intended for high performance servers to authenticate plaintext passwords.

            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_NETWORK = 3,

            /// <summary>
            /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without
            /// their direct intervention. This type is also for higher performance servers that process many plaintext
            /// authentication attempts at a time, such as mail or Web servers.
            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_BATCH = 4,

            /// <summary>
            /// Indicates a service-type logon. The account provided must have the service privilege enabled.
            /// </summary>
            LOGON32_LOGON_SERVICE = 5,

            /// <summary>
            /// This logon type is for GINA DLLs that log on users who will be interactively using the computer.
            /// This logon type can generate a unique audit record that shows when the workstation was unlocked.
            /// </summary>
            LOGON32_LOGON_UNLOCK = 7,

            /// <summary>
            /// This logon type preserves the name and password in the authentication package, which allows the server to make
            /// connections to other network servers while impersonating the client. A server can accept plaintext credentials
            /// from a client, call LogonUser, verify that the user can access the system across the network, and still
            /// communicate with other servers.
            /// NOTE: Windows NT:  This value is not supported.
            /// </summary>
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

            /// <summary>
            /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
            /// The new logon session has the same local identifier but uses different credentials for other network connections.
            /// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
            /// NOTE: Windows NT:  This value is not supported.
            /// </summary>
            LOGON32_LOGON_NEW_CREDENTIALS = 9,
        }

        public enum LogonProvider
        {
            /// <summary>
            /// Use the standard logon provider for the system.
            /// The default security provider is negotiate, unless you pass NULL for the domain name and the user name
            /// is not in UPN format. In this case, the default provider is NTLM.
            /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
            /// </summary>
            LOGON32_PROVIDER_DEFAULT = 0,
            LOGON32_PROVIDER_WINNT35 = 1,
            LOGON32_PROVIDER_WINNT40 = 2,
            LOGON32_PROVIDER_WINNT50 = 3
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        //---------------------------------------------------------------------------------------------------------------------
    }

}

