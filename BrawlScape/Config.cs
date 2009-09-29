using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace BrawlScape
{
    class Config
    {
        public static string LastDataPath
        {
            get
            {
                return Registry.GetValue("HKEY_CURRENT_USER\\Software\\SmashTools\\BrawlScape", "LastDataPath", "") as String;
            }
            set
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\SmashTools\\BrawlScape", "LastDataPath", value);
            }
        }
        public static string LastWorkingPath
        {
            get
            {
                return Registry.GetValue("HKEY_CURRENT_USER\\Software\\SmashTools\\BrawlScape", "LastWorkingPath", "") as String;
            }
            set
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\SmashTools\\BrawlScape", "LastWorkingPath", value);
            }
        }
    }
}
