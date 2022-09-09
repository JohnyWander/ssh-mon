using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Net;
using ssh_mon.Tools.Interfaces;
using static System.Net.WebRequestMethods;

namespace ssh_mon.AES
{
    internal static class Enrypt_server_dir
    {
       static AES.Interfaces.IEnryptDecrypt encryptor = new AES.Cryptography();
        public static void encrypt(string password)
        {

            try
            {
                string[] encryptedStrings = new string[Program.files.Length];
                int i = 0;
                List<Task> files_to_encrypt = new List<Task>();
                foreach (string f in Program.files)
                {
                    files_to_encrypt.Add(Task.Run(() => System.IO.File.WriteAllText(f, encryptor.Encrypt(System.IO.File.ReadAllText(f), password))));
                }

                Task.WaitAll(files_to_encrypt.ToArray());

                Program.Already_Encrypted = true;
                
                Program.ConsoleWrite.color_consoleWriteLine(ConsoleColor.Green,
                    "\n" + GUI.Language_strings.language_strings["encryption_success"]);
              
            }
            catch
            {
                Program.ConsoleWrite.color_consoleWriteLine(ConsoleColor.Red,
                "\n"+GUI.Language_strings.language_strings["encryption_fail"]);
                
            }
        }

        public static void decrypt(string password)
        {
            try
            {
               

                string[] encryptedStrings = new string[Program.files.Length];
                int i = 0;
                List<Task> files_to_encrypt = new List<Task>();
                foreach (string f in Program.files)
                {
                    files_to_encrypt.Add(Task.Run(() => System.IO.File.WriteAllText(f, encryptor.Decrypt(System.IO.File.ReadAllText(f), password)))) ; // Starting Decryption in parallel
                }

                Task.WaitAll(files_to_encrypt.ToArray()); // waiting for decryption to end
                Program.Already_Encrypted = false;

                Program.ConsoleWrite.color_consoleWriteLine(ConsoleColor.Green,"\n" + GUI.Language_strings.language_strings["decryption_success"]);
             
            }
            catch (Exception e)
            {
                Program.ConsoleWrite.color_consoleWriteLine(ConsoleColor.Red,
                GUI.Language_strings.language_strings["decryption_fail"]);                
            }
        }
    }


    
}
