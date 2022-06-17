using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ssh_mon.SSH
{

    
    abstract public class make_list
    {
        public List<Server> serverlist = new List<Server>();

        public IDictionary<string,Server>server_DICT = new Dictionary<string,Server>();
        //serwisy
   
     public void build_serverlist()
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
                
                foreach (string server in server_files)
                {
                    string[] lines = File.ReadAllLines(server);
                    current_filename = server.Split(@"\")[1];
                    name = lines[1].Split('=')[1].Trim();
                    login = lines[2].Split('=')[1].Trim();
                    ip = lines[3].Split('=')[1].Trim();
                    port = lines[4].Split('=')[1].Trim();
                    enabled = Convert.ToBoolean(lines[5].Split("=")[1]);

                    for (int i = 6; i < lines.Length; i++)
                    {
                        key += lines[i].Trim() + "\n";

                    }

                    if (port != "22")
                    {
                        ip += ":" + port;
                    }
                

                    server_DICT.Add(name, new Server(name,ip, login, key));

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
        public string name { get; }
        public string ip { get; }
        public string user { get; }
        public string keystr { get; }

        public Server(string namec,string ipc,string userc, string keystrc)
        {
            name = namec;
            ip = ipc;
            user = userc;
            keystr = keystrc;

        }


    }

  
}

/*

var keyStr = @"-----BEGIN RSA PRIVATE KEY-----
MIIEogIBAAKCAQEAikdGpJRMjrbsAq1thMtr8kCrru8ySXayzkr/E05zT+npBKbF
64SG3DS2I6QzmONllkroitqINPKjWf0xvFpV2e6YQES7tYqdym6y9MW/cw5FiUE0
T1WZGhmuXnxWfRRLGBCWtlSB2t6r5+H14zdfLUy0Cbe0nHQ/H0+horbu5SRKgrpn
FDC3QhUMBJnU5afOB1DaPY736TvtyPmVCXheqnc7sxjyZJCBZaEOA1jwCYGMiKUl
sN5wB/7+lQpC21K3Dbd9OJXpAnbC2/n6HWocKSHU2tCIbsBLk5OXs8TxA34WtyyS
TOLLk8gXSbiHi+PH2vigVrksGKU5MlrSspBhfwIBJQKCAQEAhoqJ7D1Dkca8Hkfg
Li21Iw59h5yfoWySTC1LNWEWP+qB2wMhb4fdKUgLKZjYQbqoB9ouXZZbAxWYBIem
A17QEk/uFQSa9dL6ZBiuHpbjyeRfVRzY8z6HIFBWoR8ICv789NHevz13bSuuLblC
RNvt5uLYr4JO3WovlBYguf4Y3vFulvAM8dfWjBoCtkLeuAP3h50HAizsYDdfFkD8
NHzjGiwNZ1Ztxcp1ns7KSWSqbomc8NnST0HcEMNGOEGMU3YOsX35FHwGZeep15x1
UxvANnDyvdc4hyoTTzFt536ahwVWfrrAU+GLgPHTTzW5qvnqzRI2OoitsMIaRQ6G
ngXqbQKBgQDaFrqzl1JC1kiRmL+l91Pm+NWy1FGHPh5SnBH1S35YqspCXEs600WS
NVs/ie4ZIw7FdRG/nHRRo/kMVFuWY8T8mNFmTgDb7r6gob+EHwKikSSD7Du2nM8z
9en+SWpurnEGbqX29Fs89ybEPMabK4gWquJzQw59y9fRiN3L+FYw5wKBgQCiUN5Q
2aQtP1xpaWrKBoi234uK3+SaK8hAjuvx3V05CFUIAe3GvuWEhfFFrfxELbqs4yVN
7Z9Ri68Ght0VIcYrWuu6eRWUuTSzqtmoE6dl0dBXcpQQnFsub9M9yqacZrla8PFV
rPwxAsU53xcmcKHxpG4gAixtkAXpG2b8Xuz/qQKBgHvHthLnLrBQG1mOC+iTS0u9
qbiGV8lMxRohOp/6ak4AEe5QDwW2Lme9ayQQA61m7LVCcdt0eV7E2XWzVpOgaOJk
koYsRa1CUISu1Htri9+6KXtOuhuJbq7J19WDnUW89AqR09FaQZ8kfclS7UNQDvgb
zKJIp1wnlirRI/BOrXWzAoGBAIObg9LZ+rznRAJxJidDkXGuVXeFGjfQeNpmC2op
GydS3SkWUhbES2SIRxXnA+RAwN80tnaJUL6om8AaUmQpOOTh75BGgDNlvAdMQcaM
eePajT/8CVmTiDN9Q341m9jB/hJifnzRcoiadmZGLnIxz2n68YG8krmzC7YWN9OL
PLOdAoGAE9MAJA2SCV+fJzDqbuiiCDQNZ6l0VNnrgf0ukzpZvZFN+KSh/GTlKZk6
ZP50I+V8LjFT1+lcBsuaVIGjjXp7vERPpX0e0putG3CXBzBvn1ShQTgRSH8kXkMZ
FqmlTbm2bzFYcIsg9ghATekOq4RZyIPVLnRRNsoK5fTXV54KXRY=
-----END RSA PRIVATE KEY-----
";

*/