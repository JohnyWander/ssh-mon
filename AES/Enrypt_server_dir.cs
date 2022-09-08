using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
                    files_to_encrypt.Add(Task.Run(() => File.WriteAllText(f, encryptor.Encrypt(File.ReadAllText(f), password))));
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




    }
}
