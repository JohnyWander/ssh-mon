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
        private static bool Already_Encrypted = true;
        static void Main(string[] args)
        {
            init(); // chekcing for folders/ files presence etc.
            Readconf.run(); ; // Reading configuration file
            LANG = Readconf.LANG;
            init_lang_strings(LANG); // Setting strings to provided language
            Modules.LoadAssemblies.run(); // Loading custom modules 
            menu(); // main menu

        A:
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


            switch (switch_i)
            {
                case 1:
                    DisableQuickEdit(); // clicking on console window was messing up gui
                    var con = new SSH.connections();//
                    int servers = Directory.GetFiles("servers").Count();
                    con.run(cancel_all, Already_Encrypted);// building server list, starting tests
                    GUI.Default_GUI.run();
                    Task.WaitAll(con.connections123.ToArray()); // Waiting for Connections to end                   
                    EnableQuickEdit();
                break;
                case 2:

                    foreach (string f in files)
                    {
                        if (File.ReadAllLines(f).Contains("[server]"))
                        {
                            Already_Encrypted = false;
                        }
                    }
                    if (Already_Encrypted == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(GUI.Language_strings.language_strings["already_encrypted"]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        goto A;
                    }
                    else
                    {
                        AES.Interfaces.IEnryptDecrypt encrypt = new AES.Cryptography();
                        Console.WriteLine(GUI.Language_strings.language_strings["input_password"]);
                        string password = input_password();
                        Console.WriteLine("\n" + GUI.Language_strings.language_strings["input_password_confirm"]);
                        string password_confirm = input_password();
                        if (password != password_confirm)
                        {
                            Console.WriteLine(GUI.Language_strings.language_strings["input_password_no_match"]);
                            goto A;
                        }
                        else if (password == password_confirm)
                        {
                            try
                            {
                                string[] encryptedStrings = new string[files.Length];
                                int i = 0;
                                List<Task> files_to_encrypt = new List<Task>();
                                foreach (string f in files)
                                {
                                    files_to_encrypt.Add(Task.Run(() => File.WriteAllText(f, encrypt.Encrypt(File.ReadAllText(f), password)))); // encrypting files simultaneously
                                }
                                Task.WaitAll(files_to_encrypt.ToArray()); // Waiting For encryption processes to end
                                Already_Encrypted = true;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\n" + GUI.Language_strings.language_strings["encryption_success"]);// if encryption worked
                                Console.ForegroundColor = ConsoleColor.Gray;
                                goto A;
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(GUI.Language_strings.language_strings["encryption_fail"]); // if encryption failed
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                    }
                break;
                case 3:
                    foreach (string f in files)
                    {
                        if (File.ReadAllLines(f).Contains("[server]"))
                        {
                            Already_Encrypted = false;
                        }
                    }
                    if (Already_Encrypted == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(GUI.Language_strings.language_strings["not_encrypted"]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        try
                        {
                            AES.Interfaces.IEnryptDecrypt encrypt = new AES.Cryptography();
                            Console.WriteLine(GUI.Language_strings.language_strings["input_password"]);
                            string password = input_password();


                            string[] encryptedStrings = new string[files.Length];
                            int i = 0;
                            bool fail = false;
                            List<Task> files_to_encrypt = new List<Task>();
                            foreach (string f in files)
                            {
                                files_to_encrypt.Add(Task.Run(() =>
                                {
                                    string decrypted = encrypt.Decrypt(File.ReadAllText(f), password); //decryption simultaneously

                                    if (decrypted.Length != 0)
                                    {
                                        File.WriteAllText(f, decrypted);
                                    }
                                    else
                                    {
                                        fail = true;
                                    }
                                })); // Starting Decryption in parallel
                            }

                            Task.WaitAll(files_to_encrypt.ToArray()); // waiting for decryption to end

                            if (fail == false)
                            {
                                Already_Encrypted = false;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\n" + GUI.Language_strings.language_strings["decryption_success"]);
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n" + GUI.Language_strings.language_strings["decryption_fail"]);
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(GUI.Language_strings.language_strings["decryption_fail"]);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                    goto A;
                    break;
            }
        }

        public static void restart()
        {
            Console.WriteLine(GUI.Language_strings.language_strings["press_any"]);
            Console.ReadKey();
        }

        private static void init()
        {
            try { Directory.GetFiles("modules");}
            catch{ Directory.CreateDirectory("modules");}

            try { Directory.GetFiles("lang"); }
            catch (DirectoryNotFoundException) { Directory.CreateDirectory("lang"); File.WriteAllText("lang\\ENG.lang", Resources.Config_strings.default_lang); }


            try { File.OpenRead("config.cfg"); }
            catch (FileNotFoundException) { File.WriteAllText("config.cfg", Resources.Config_strings.default_conf); }
            
               

            try { Directory.GetFiles("servers");}
            catch (DirectoryNotFoundException){ Directory.CreateDirectory("servers");
                File.WriteAllText("servers\\DEFAULT_SERVER_CONFIG_FILE.txt", Resources.Config_strings.default_server);
            }


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
       

        private static void init_lang_strings(string lang)
        {
            if (lang == "ENG")
            {
                GUI.Language_strings.run("ENG");
            }
        }

        public static string input_password()
        {
            string pass = "";
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return pass;
        }

    }
}