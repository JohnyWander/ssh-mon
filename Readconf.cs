using System;
using System.IO;
namespace ssh_mon
{
    public static class Readconf
    {
        public static string LANG;
        public static int cpu_ram_timer;
        public static void run()
        {
            string[] lines = File.ReadAllLines("config.cfg");
            LANG = lines[1];
            cpu_ram_timer = Convert.ToInt32(lines[3].Split("=")[1]);



        }



    }
}
