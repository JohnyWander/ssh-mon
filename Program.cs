using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ssh_mon
{
    class Program
    {
      
        private static string LANG;
        static public CancellationTokenSource cancel_all = new CancellationTokenSource();//for gentle connection termination
        private static bool Already_Encrypted = true;
        public static bool CompletedCreatingConnectionList
        {
           
            set
            {
                StartGUI();
            }
        }
        private static Action StartGUI;



       public static Tools.Interfaces.IInputPassword passwordinput = new Tools.Tools();
        static Tools.Interfaces.IDisableQuickEdit Disable_QE = new Tools.Interop();
        static Action DisableQuickEdit = Disable_QE.DisableQuickEdit;
        static Action EnableQuickEdit = Disable_QE.EnableQuickEdit;
        static void Main(string[] args)
        {
            DisableQuickEdit();
            init(); // chekcing for folders/ files presence etc.
            Readconf.run(); ; // Reading configuration file
            LANG = Readconf.LANG;
            init_lang_strings(LANG); // Setting strings to provided language
            Modules.LoadAssemblies LoadAssembly = new Modules.LoadAssemblies(); // Loading custom modules 
            menu(); // main menu

        A: // i know c:
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
                  
                    var con = new SSH.connections();//
                    int servers = Directory.GetFiles("servers").Count();
                    StartGUI= GUI.Default_GUI.run;
                    con.run(cancel_all, Already_Encrypted);// building server list, connecting, on connection completion launching GUI by invoking action by SET in CompletedCreatingConnectionList boolj

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
                        string password = passwordinput.input_password();
                        Console.WriteLine("\n" + GUI.Language_strings.language_strings["input_password_confirm"]);
                        string password_confirm = passwordinput.input_password();
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

                                List<Task> files_to_encrypt = new List<Task>();
                                foreach (string f in files)
                                {
                                    files_to_encrypt.Add(Task.Run(() => File.WriteAllText(f, encrypt.Encrypt(File.ReadAllText(f,Encoding.UTF8), password), Encoding.UTF8))); // encrypting files simultaneously
                                }
                                Task.WaitAll(files_to_encrypt.ToArray()); // Waiting For encryption processes to end
                                Already_Encrypted = true;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\n" + GUI.Language_strings.language_strings["encryption_success"]);// if encryption worked
                                Console.ForegroundColor = ConsoleColor.Gray;
                                goto A;
                            }
                            catch (Exception)
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
                            string password = passwordinput.input_password();


                            string[] encryptedStrings = new string[files.Length];

                            bool fail = false;
                            List<Task> files_to_encrypt = new List<Task>();
                            foreach (string f in files)
                            {
                                files_to_encrypt.Add(Task.Run(() =>
                                {
                                    string decrypted = encrypt.Decrypt(File.ReadAllText(f, Encoding.UTF8), password); //decryption simultaneously

                                    if (decrypted.Length != 0)
                                    {
                                        File.WriteAllText(f, decrypted,Encoding.UTF8);
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
                        catch (Exception)
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
            try { Directory.GetFiles("modules"); }
            catch { Directory.CreateDirectory("modules"); }

            try { Directory.GetFiles("lang"); }
            catch (DirectoryNotFoundException) { Directory.CreateDirectory("lang"); File.WriteAllText("lang\\ENG.lang", Resources.Config_strings.default_lang); }


            try { File.OpenRead("config.cfg"); }
            catch (FileNotFoundException) { File.WriteAllText("config.cfg", Resources.Config_strings.default_conf); }



            try { Directory.GetFiles("servers"); }
            catch (DirectoryNotFoundException)
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

        

    }
}