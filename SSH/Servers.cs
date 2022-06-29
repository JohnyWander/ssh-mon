using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;
namespace ssh_mon.SSH
{

    
    abstract public class make_list
    {
        public List<Server> serverlist = new List<Server>();

        public IDictionary<string,Server>server_DICT = new Dictionary<string,Server>();
        public List<Renci.SshNet.AuthenticationMethod> auths = new List <Renci.SshNet.AuthenticationMethod>();
        //serwisy
   
     public void build_serverlist(bool Already_encrypted)
        {
            string name = "";
            string login = "";
            string ip = "";
            string port = "";
            string key = "";
            bool enabled = false;
            string current_filename = "";
            try
            {
                string[] server_files = Directory.GetFiles("servers");


                if (Already_encrypted == true)
                {
                    AES.Interfaces.IEnryptDecrypt decrypt = new AES.Cryptography();
                   Console.WriteLine( GUI.Language_strings.language_strings["input_password"]+"\n");
                    string password = Program.input_password();
                   string key_Aes = decrypt.Decrypt(File.ReadAllText("servers//KEYFILE"), password);

                    foreach (string server in server_files)
                    {
                        if (!server.Contains("KEYFILE"))
                        {
                            string file = decrypt.Decrypt(File.ReadAllText(server), password);
                            string[] lines = file.Split("\n");
                            string name_Aes = lines[1].Split('=')[1].Trim();
                            string login_Aes = lines[2].Split('=')[1].Trim();
                            string ip_Aes = lines[3].Split('=')[1].Trim();
                            string port_Aes = lines[4].Split('=')[1].Trim();
                            bool enabled_Aes = Convert.ToBoolean(lines[5].Split("=")[1]);




                            if (enabled_Aes == true)
                            {

                                server_DICT.Add(name_Aes, new Server(name_Aes, ip_Aes,port_Aes ,login_Aes, key_Aes));

                            }
                        }
                    }

                }
                else
                {
                    key = File.ReadAllText("servers//KEYFILE");

                    foreach (string server in server_files)
                    {
                        if (!server.Contains("KEYFILE"))
                        {
                            string[] lines = File.ReadAllLines(server);
                            current_filename = server.Split(@"\")[1];
                            name = lines[1].Split('=')[1].Trim();
                            login = lines[2].Split('=')[1].Trim();
                            ip = lines[3].Split('=')[1].Trim();
                            port = lines[4].Split('=')[1].Trim();
                            enabled = Convert.ToBoolean(lines[5].Split("=")[1]);




                            if (enabled == true)
                            {

                                server_DICT.Add(name, new Server(name, ip,port, login, key));

                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(GUI.Language_strings.language_strings["server_config_error"]+" "+current_filename);
            }
            




           // serverlist.Add(new Server(ip_S, user_S, keyStr_SERWISY));
           // server_DICT.Add(0, new Server(ip_S, user_S, keyStr_SERWISY));
        }




    }



  public class Server
    {
        public string name { get; private set; }
        public string ip { get; private set; }
        public string user { get; private set; }
        public string keystr { get; private set; }

        public string port { get; private set; }
        public Server(string namec,string ipc,string portc,string userc, string keystrc)
        {
            name = namec;
            ip = ipc;
            user = userc;
            keystr = keystrc;
            port = portc;
        }


    }

  
}

/*


*/