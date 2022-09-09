using System;
using System.Security.Cryptography;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Net;

namespace ssh_mon
{
    class Program
    {

        private static string LANG;
        static public CancellationTokenSource cancel_all = new CancellationTokenSource();
        public static bool Already_Encrypted { private get; set; } = true;


        private static Action StartGui; // assigned in Main()                                    // STARTING GUI ON CONNECTION LIST CREATING COMPLETION
        public static bool CompletedCreatingConnectionList { set { StartGui(); } }

        public static Tools.Interfaces.IInputPassword inputPassword = new Tools.Tools();
        public static Tools.Interfaces.IConsoleWrite ConsoleWrite = new Tools.Tools();

        public static string[] files { get; set; }
        static void Main(string[] args)
        {

            Initialization.Initialization.init(); // chekcing for folders/ files presence etc.
            Readconf.run();
            LANG = Readconf.LANG;
            init_lang_strings(LANG); // Setting strings to provided language
                                     // Modules.LoadAssemblies.run();


            files = Directory.GetFiles("servers");
            StartGui = GUI.Default_GUI.run; // Assigning gui start void Action delegate to be invoked later
            menu();// Writing menu
            UserInput(); // Hanling rest of program with user input


        }

        public static void restart()
        {
            Console.WriteLine(GUI.Language_strings.language_strings["press_any"]);
            Console.ReadKey();
        }



        private static void menu()
        {
            try
            {
                IDictionary<string, string> lang_strings = GUI.Language_strings.language_strings;
                Console.WriteLine(lang_strings["start_test"]);
                Console.WriteLine(lang_strings["encrypt_server_dir"]);
                Console.WriteLine(lang_strings["decrypt_server_dir"]);
            }
            catch
            {
                init_lang_strings("ENG");
                menu();
            }

        }

        private static void UserInput()
        {

            ConsoleKeyInfo key = Console.ReadKey();
            Console.Write("\b \b");



            char key_c = key.KeyChar;
            byte xd = Convert.ToByte(key_c);
            byte[] xdw = new byte[] { xd };
            int switch_i = Convert.ToInt32(Encoding.UTF8.GetString(xdw));
            string[] files = Directory.GetFiles("servers");

            foreach (string f in files)
            {
                if (File.ReadAllLines(f).Contains("[server]"))
                {
                    Already_Encrypted = false;
                }


            }




            if (switch_i == 1)
            {
                Handle_Purpose(); // Actual work of program starts here !
            }

            if (switch_i == 2)
            {


                foreach (string f in files)
                {
                    if (File.ReadAllLines(f).Contains("[server]"))
                    {
                        Already_Encrypted = false;
                    }


                }

                if (Already_Encrypted == true)
                {
                    ConsoleWrite.color_consoleWriteLine(ConsoleColor.Yellow, GUI.Language_strings.language_strings["already_encrypted"]);
                    UserInput();
                }
                else
                {
                    
                    Console.WriteLine(GUI.Language_strings.language_strings["input_password"]);
                    string password = inputPassword.input_password();
                    Console.WriteLine("\n" + GUI.Language_strings.language_strings["input_password_confirm"]);
                    string password_confirm = inputPassword.input_password();

                    if (password != password_confirm)
                    {
                        ConsoleWrite.color_consoleWriteLine(ConsoleColor.Red, GUI.Language_strings.language_strings["input_password_no_match"]);
                        UserInput();
                    }
                    else if (password == password_confirm)
                    {

                        AES.Enrypt_server_dir.encrypt(password); /// encrypting servers dir
                        UserInput(); // listening for next input
                        return;

                    }
                }
            }
            if (switch_i == 3)
            {
                foreach (string f in files)
                {
                    if (File.ReadAllLines(f).Contains("[server]"))
                    {
                        Already_Encrypted = false;
                    }
                }
                if (Already_Encrypted == false)
                {
                    ConsoleWrite.color_consoleWriteLine(ConsoleColor.Yellow, GUI.Language_strings.language_strings["not_encrypted"]);

                }
                else
                {
                    string password = Program.inputPassword.input_password();
                    AES.Enrypt_server_dir.encrypt(password);
                    UserInput();
                }
            }


        }
    
        private static void Handle_Purpose()
        {

            Interop._DisableQuickEdit_.DisableQuickEdit(); // clicking on console window was messing up gui



            var con = new SSH.connections();// building server list starting tests 

            //   Task cancel = Task.Run(() => Stop(cancel_all));
            int servers = Directory.GetFiles("servers").Count();
            con.run(cancel_all, Already_Encrypted);
            // Task gui = Task.Run(() => GUI.Default_GUI.run());

            //Task.WaitAll(con.connections_list.ToArray());
            //   gui.Wait();
            Interop._DisableQuickEdit_.EnableQuickEdit();
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