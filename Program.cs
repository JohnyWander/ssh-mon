using System;
using System.Security.Cryptography;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
namespace bpp_admin
{
    class Program
    {
        private static string LANG;
       static public CancellationTokenSource cancel_all = new CancellationTokenSource();
        static void Main(string[] args)
        {
            init();
            read_conf();
            init_lang_strings(LANG);
            menu();
         
            
                ConsoleKeyInfo key = Console.ReadKey();
            Console.Write("\b \b");

            

                char key_c = key.KeyChar;
                byte xd = Convert.ToByte(key_c);
                byte[] xdw = new byte[] { xd };
                int switch_i = Convert.ToInt32(Encoding.UTF8.GetString(xdw));


                if (switch_i == 1)
                {


                    var con = new SSH.connections();

                //   Task cancel = Task.Run(() => Stop(cancel_all));
                int servers = Directory.GetFiles("servers").Count();
                    con.run(cancel_all);

                while (true)
                {
                    if (servers == con.connections123.Count)
                    {
                        break;
                    }
                }


                    // Task gui = Task.Run(() => GUI.Default_GUI.run());
                    GUI.Default_GUI.run();
                    Task.WaitAll(con.connections123.ToArray());
                    //   gui.Wait();






                

            }
        }
        private static void init()
        {
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
            catch(DirectoryNotFoundException)
            {
                Directory.CreateDirectory("servers");
                File.WriteAllText("servers\\DEFAULT_SERVER_CONFIG_FILE.txt", Resources.Config_strings.default_server);
            }


        }

        private static void menu()
        {
            try
            {
                IDictionary<string, string> lang_strings = GUI.Language_strings.language_strings;
                Console.WriteLine(lang_strings["start_test"]);
            }
            catch
            {
                init_lang_strings("ENG");
                menu();
            }
            
        }
        private static void read_conf()
        {
            string[] config_lines = File.ReadAllLines("config.cfg");
            int ite = 0;
            foreach(string c in config_lines)
            {
                if (ite == 1) { LANG = c; }

                ite++;
            }



        }

        private static void init_lang_strings(string lang)
        {

            if (lang == "ENG")
            {
                GUI.Language_strings.run("ENG");
            }


        }

 

    }
}
