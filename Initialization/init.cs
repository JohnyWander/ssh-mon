using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssh_mon.Initialization
{
    internal static class Initialization
    {
        public static void init()
        {
            try
            {
                Directory.GetFiles("modules");
            }
            catch
            {
                Directory.CreateDirectory("modules");
            }
            try
            {
                Directory.GetFiles("lang");

            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("lang");
                File.WriteAllText("lang\\ENG.lang", Resources.Config_strings.default_lang);
            }

            try
            {
                File.OpenRead("config.cfg");
            }
            catch (FileNotFoundException)
            {
                File.WriteAllText("config.cfg", Resources.Config_strings.default_conf);
            }

            try
            {
                Directory.GetFiles("servers");
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("servers");
                File.WriteAllText("servers\\DEFAULT_SERVER_CONFIG_FILE.txt", Resources.Config_strings.default_server);
            }


            
            







        }
    }
}
