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
using System.Net;

namespace ssh_mon
{
    class Program
    {
        
        private static string LANG;
        static public CancellationTokenSource cancel_all = new CancellationTokenSource();
        private static bool Already_Encrypted = true;
        

        private static Action StartGui;                                      // STARTING GUI ON CONNECTION LIST CREATING COMPLETION
        public static bool CompletedCreatingConnectionList { set { StartGui(); } }

        public static Tools.Interfaces.IInputPassword inputPassword = new Tools.Tools();
        public static Tools.Interfaces.IConsoleWrite ConsoleWrite = new Tools.Tools();
        static void Main(string[] args)
        {

            Initialization.Initialization.init(); // chekcing for folders/ files presence etc.
            Readconf.run();
            LANG = Readconf.LANG;
            init_lang_strings(LANG); // Setting strings to provided language
           // Modules.LoadAssemblies.run();

        
            
        
            menu();
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




            if (switch_i == 1)
            {
                Interop._DisableQuickEdit_.DisableQuickEdit(); // clicking on console window was messing up gui
                
             

                var con = new SSH.connections();// building server list starting tests 
                StartGui=GUI.Default_GUI.run;
                //   Task cancel = Task.Run(() => Stop(cancel_all));
                int servers = Directory.GetFiles("servers").Count();
                con.run(cancel_all,Already_Encrypted);
                // Task gui = Task.Run(() => GUI.Default_GUI.run());
                
                Task.WaitAll(con.connections123.ToArray());
                //   gui.Wait();
                Interop._DisableQuickEdit_.EnableQuickEdit();

            }

            if (switch_i == 2)
            {
                
               
                foreach(string f in files)
                {
                    if (File.ReadAllLines(f).Contains("[server]"))
                    {
                        Already_Encrypted = false;
                    }    
                    

                }

                if(Already_Encrypted == true)
                {
                    ConsoleWrite.color_consoleWriteLine(ConsoleColor.Yellow,GUI.Language_strings.language_strings["already_encrypted"]);
                    goto A;
                }
                else
                {
                    AES.Interfaces.IEnryptDecrypt encrypt = new AES.Cryptography();
                    Console.WriteLine(GUI.Language_strings.language_strings["input_password"]);
                    string password = inputPassword.input_password();
                    Console.WriteLine("\n"+GUI.Language_strings.language_strings["input_password_confirm"]);
                    string password_confirm = inputPassword.input_password();

                    if (password != password_confirm)
                    {
                        Console.WriteLine(GUI.Language_strings.language_strings["input_password_no_match"]);
                        goto A;
                    }
                    else if( password == password_confirm) {

                        try
                        {
                            string[] encryptedStrings = new string[files.Length];
                            int i = 0;
                            List<Task> files_to_encrypt = new List<Task>();
                            foreach (string f in files)
                            {
                                files_to_encrypt.Add(Task.Run(() => File.WriteAllText(f, encrypt.Encrypt(File.ReadAllText(f), password))));
                            }

                            Task.WaitAll(files_to_encrypt.ToArray());

                            Already_Encrypted = true;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n"+GUI.Language_strings.language_strings["encryption_success"]);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            goto A;
                        }
                        catch(Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(GUI.Language_strings.language_strings["encryption_fail"]);
                            Console.ForegroundColor = ConsoleColor.Gray;             
                        }
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
                        string password = inputPassword.input_password();


                        string[] encryptedStrings = new string[files.Length];
                        int i = 0;
                        List<Task> files_to_encrypt = new List<Task>();
                        foreach (string f in files)
                        {
                            files_to_encrypt.Add(Task.Run(() => File.WriteAllText(f, encrypt.Decrypt(File.ReadAllText(f), password)))); // Starting Decryption in parallel
                        }

                        Task.WaitAll(files_to_encrypt.ToArray()); // waiting for decryption to end
                        Already_Encrypted = false;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n"+GUI.Language_strings.language_strings["decryption_success"]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    catch(Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(GUI.Language_strings.language_strings["decryption_fail"]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                goto A;
            }


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
       

        private static void init_lang_strings(string lang)
        {
            if (lang == "ENG")
            {
                GUI.Language_strings.run("ENG");
            }
        }

       

    }
}