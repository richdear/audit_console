using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace audit_console
{
    class SecurityPolicy
    {
        //------------------------------------Remove whitespace---------------------------------------------
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement = "")
        { return sWhitespace.Replace(input, replacement); }

        //------------------------------------Local security policy-----------------------------------------
        public static void PasswordPolicy()
        {
            int counter = 0;
            string data = "<security_policy>" + Program.new_line;

            string line;
            var secpol_friendly_name = new Dictionary<string, string> {
                { "PasswordHistorySize","Enforce Password History"},
                 {"MaximumPasswordAge","Maximum Password Age"},
                { "MinimumPasswordAge","Minimum Password Age"},
                { "MinimumPasswordLength","Minimum Password Length"},
                {"PasswordComplexity", "Password must meet complexity requirements"},
                {"ClearTextPassword", "Store passwords using reversible encryption"},

                //-----------------------Account Lockout----------------------------
                {"ResetLockoutCount","Reset account lockout after" },
                {"LockoutBadCount","Account lockout threshold" },
                {"LockoutDuration","Account Lockout Duration" },
                //-------------------------------------------------------------------
                //-----------------------Audit Policy-----------------------------
                {"AuditAccountLogon","Audit account logon events" },
                {"AuditAccountManage","Audit account management" },
                {"AuditDSAccess","Audit directory service access" },
                {"AuditLogonEvents","Audit logon events" },
                {"AuditObjectAccess","Audit Object Access" },
                {"AuditPolicyChange","Audit policy change" },
                {"AuditPrivilegeUse", "Audit privilage use" },
                {"AuditProcessTracking","Audit process tracking"},
                {"AuditSystemEvents","Audit system events" },
                //-------------------------------------------------------------------
                //-----------------------Security options-----------------------
                {"EnableGuestAccount","Accounts: Guest account status" },
                {"NewAdministratorName","Accounts: Rename administrator account" },
                {"DisableCAD","Interactive logon: Do not require CTRL+ALT+DEL" },
                {"DontDisplayUserName","Interactive logon: Do not display username at sign-in" },
                {"ClearPageFileAtShutdown","Shutdown: Clear virtual memory pagefile" }


            };
            // string[] coded_name = { "MinimumPasswordAge","MaximumPasswordAge", "MinimumPasswordLength", "PasswordComplexity","PasswordHistorySize", "ClearTextPassword" };
            // string[] friendly_name = { "Minimum Password Age", "Maximum Password Age" , "Minimum Password Length", "Password must meet complexity requirements","Enforce Password History", "Store passwords using reversible encryption" };

            // Con("Enforce Password History:");

            System.IO.StreamReader file =
                new System.IO.StreamReader("pol.txt");
            Dictionary<string, string> secpol = new Dictionary<string, string>();


            while ((line = file.ReadLine()) != null)
            {

                // Con(friendly_name[counter]);
                try
                {
                    String[] tokens = line.Split('=');
                    String key = ReplaceWhitespace(tokens[0]);
                    string value = ReplaceWhitespace(tokens[1]);
                    int index = key.LastIndexOf('\\');
                    if (index > 0)
                    {
                        key = key.Substring(index + 1);
                    }
                    // Con(key + "------------");
                    // Con(value + "------------");
                    secpol.Add(key, value);

                    counter++;
                }
                catch (Exception e)
                {
                    //Con("Exception");
                }

            }

            //Con("Line number: {0}", counter);

            file.Close();
            foreach (string key in secpol_friendly_name.Keys)
            {
                if (secpol.ContainsKey(key))
                {
                    Console.WriteLine(secpol[key], key);
                    Console.WriteLine(secpol_friendly_name[key] + "--" + secpol[key]);
                    data += "<policy name='" + secpol_friendly_name[key] + "'  value='" + secpol[key] + "' />"+Program.new_line;
                    XMLWriter.WriteXML("PolicyName", secpol_friendly_name[key]);
                    XMLWriter.WriteXML("PolicyValue", secpol[key]);
                }
                else
                {
                    Console.WriteLine("0", key);
                    Console.WriteLine(secpol_friendly_name[key] + "--" + "Not Set");
                    data += "<policy name='" + secpol_friendly_name[key] + "'  value='" + "Not set" + "' />"+Program.new_line;
                    XMLWriter.WriteXML("PolicyName", secpol_friendly_name[key]);
                    XMLWriter.WriteXML("PolicyValue", "Not Set");
                }
            }
            data += "</security_policy>" + Program.new_line;
            XMLWriter.Write(data);
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    }
}
