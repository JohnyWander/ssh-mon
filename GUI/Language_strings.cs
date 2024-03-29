﻿using System.Collections.Generic;
using System.IO;
namespace ssh_mon.GUI
{
    public static class Language_strings
    {
        public static IDictionary<string, string> language_strings = new Dictionary<string, string>();
        //menu
        public static void run(string lang)
        {
            try
            {
                string[] lang_lines = File.ReadAllLines("lang\\" + lang + ".lang");

                foreach (string l in lang_lines)
                {
                    if (!l.Contains("["))
                    {
                        string[] split = l.Split('=');
                        language_strings[split[0]] = split[1];


                    }
                }
            }
            catch (FileNotFoundException)
            {
                File.WriteAllText("lang\\" + lang + ".lang", Resources.Config_strings.default_lang);
            }


        }


    }
}
