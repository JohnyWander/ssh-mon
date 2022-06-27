using System;
using System.Collections.Generic;
using System.IO;
namespace Mount_check
{
    internal static class ReadConf
    {
        private static string path_conf = "modules\\Mount_check.conf";
        public static List<string> applies_to = new List<string>();
       public static List<string[]> commands_and_outputs = new List<string[]>();

        public static void run()
        {
            string[] lines = File.ReadAllLines(path_conf);

            foreach(string applies in lines[1].Split(','))
            {
                applies_to.Add(applies);
            }

            int commands_pos=0;
            int commands_end_pos=0;

            int ite = 0;
            foreach(string l in lines)
            {
                if(l.Contains("[test commands and outputs]")){ commands_pos = ite; }
                if(l.Contains(@"[\test commands and outputs]")) { commands_end_pos = ite; }

                if (l.Contains("$$$"))
                {
                    string[] c_o = lines[ite].Split("||");
                    string c = c_o[0].Split('=')[1].Replace("\"", string.Empty).Trim();
                    string o = c_o[1].Split('=')[1].Replace("\"", string.Empty).Trim(); ;

                    commands_and_outputs.Add(new string[] { c, o });
                }

                ite++;
            }


         
              
                

              
            




        }



    }
}
