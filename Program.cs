using System;
using System.Security.Cryptography;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
namespace ssh_mon
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(
            IntPtr hConsoleHandle,
            out int lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(
            IntPtr hConsoleHandle,
            int ioMode);

        public const int STD_INPUT_HANDLE = -10;

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        /// <summary>
        /// This flag enables the user to use the mouse to select and edit text. To enable
        /// this option, you must also set the ExtendedFlags flag.
        /// </summary>
        const int QuickEditMode = 64;

        // ExtendedFlags must be combined with
        // InsertMode and QuickEditMode when setting
        /// <summary>
        /// ExtendedFlags must be enabled in order to enable InsertMode or QuickEditMode.
        /// </summary>
        const int ExtendedFlags = 128;

        static void DisableQuickEdit()
        {
            IntPtr conHandle = GetStdHandle(STD_INPUT_HANDLE);
            int mode;

            if (!GetConsoleMode(conHandle, out mode))
            {
                // error getting the console mode. Exit.
                return;
            }

            mode = mode & ~(QuickEditMode | ExtendedFlags);

            if (!SetConsoleMode(conHandle, mode))
            {
                // error setting console mode.
            }
        }
static void EnableQuickEdit()
        {
       
            IntPtr conHandle = GetStdHandle(STD_INPUT_HANDLE);
            int mode;

            if (!GetConsoleMode(conHandle, out mode))
            {
                // error getting the console mode. Exit.
                return;
            }

            mode = mode | (QuickEditMode | ExtendedFlags);

            if (!SetConsoleMode(conHandle, mode))
            {
                // error setting console mode.
            }
        }
        private static string LANG;
       static public CancellationTokenSource cancel_all = new CancellationTokenSource();
        static void Main(string[] args)
        {
            
            
                DisableQuickEdit();
            
          
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






                EnableQuickEdit();

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
