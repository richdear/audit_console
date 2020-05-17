using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;


namespace audit_console
{
    class Domain
    {

        //------------------------------------check domain-----------------------------------------------
        public static void checkDomain()
        {
            string strMachineName = System.Environment.MachineName;
            bool bLocal = WindowsIdentity.GetCurrent().Name.ToUpper().Contains(strMachineName.ToUpper());
            string data = "<domain  state='" + bLocal.ToString() + "' />"+Program.new_line;
            XMLWriter.Write(data);
            XMLWriter.WriteXML("CurrentUserDomainStatus", bLocal.ToString());
            Console.WriteLine(string.Format("Is Logged on Locally: {0}", bLocal.ToString()));
        }
        //---------------------------------------------------------------------------------------------------
    }
}
