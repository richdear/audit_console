using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace audit_console
{
    class Misc
    {

        public static string GetSystemLanguage()
        {

            CultureInfo ci = CultureInfo.InstalledUICulture;
            string lan = ci.Name;
            return lan;

        }
    }
}
