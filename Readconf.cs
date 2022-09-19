using System;
using System.IO;
namespace ssh_mon
{
    public static class Readconf
    {
        public static string LANG { private set; get; }
        public static int cpu_ram_timer { private set; get; }
        public static bool show_module_name { private set; get; }
        public static void run()
        {
            string[] lines = File.ReadAllLines("config.cfg");
            LANG = lines[1];
            cpu_ram_timer = Convert.ToInt32(lines[3].Split("=")[1]);

            show_module_name = bool.Parse(lines[7].Split("=")[1]);

        }



    }
}
