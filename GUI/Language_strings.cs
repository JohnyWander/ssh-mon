using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace bpp_admin.GUI
{
   public static class Language_strings
    {
        public static IDictionary<string, string> language_strings = new Dictionary<string, string>();
        //menu
        public static void run(string lang)
        {
            string[]lang_lines = File.ReadAllLines("lang\\"+lang+".lang");

            foreach(string l in lang_lines)
            {
                if (!l.Contains("["))
                {
                    string[] split = l.Split('=');
                    language_strings[split[0]] = split[1];
                    

                }
            }



        }


    }
}
