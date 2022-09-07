using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ssh_mon.SSH
{
    public class connections : make_list
    {
        static public IDictionary<int, string> names = new Dictionary<int, string>();

        public List<Task> connections123 = new List<Task>();
        public static int xD = 100;
        public Task run(CancellationTokenSource cancel, bool Already_enrypted)
        {
            build_serverlist(Already_enrypted);


            CancellationTokenSource cts = cancel;

            int ite = 0;
            foreach (var serv in server_DICT)
            {

                var SERVER = serv.Value;


                names.Add(ite, serv.Value.name);

                //     Console.WriteLine(SERVER.keystr);
                Stream Key = new MemoryStream(Encoding.ASCII.GetBytes(SERVER.keystr));

                // PrivateKeyFile pvk = new PrivateKeyFile("priv.txt");
                PrivateKeyFile pvk = new PrivateKeyFile(Key);


                ConnectionInfo conn = new ConnectionInfo(SERVER.ip, Convert.ToInt32(SERVER.port.Trim()), SERVER.user, new AuthenticationMethod[]
                {
                new PrivateKeyAuthenticationMethod(SERVER.user,pvk)
                });

                connections123.Add(Task.Factory.StartNew(() => connection_and_tests(conn, cts.Token, SERVER.name, ite), cts.Token));

                Thread.Sleep(100);
                ite++;

            }

            //connections123.Add(Task.Run(() => Stop(cts)));




            //   Task.WhenAll(connections123);
            //  Task.WaitAll(connections123.ToArray());
            Program.CompletedCreatingConnectionList = true;
            return Task.CompletedTask;
        }



        private Task connection_and_tests(ConnectionInfo connection, CancellationToken cts, string _serverID, int int_key)
        {
            string _serverID_ = _serverID;
            using (var sshClient = new SshClient(connection))
            {
                try
                {
                    Tests.tests tests = new Tests.tests(sshClient, int_key);
                    sshClient.Connect();
                    
                    //   var cmd = sshClient.CreateCommand("df -h");
                    // var cmd = sshClient.CreateCommand("top -n1");
                    //   string result = cmd.Execute();
                    //  Console.WriteLine(result);
                    while (!cts.IsCancellationRequested)
                    {
                        Task.Delay(2000).Wait();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }

                
                    
                

            }
            return Task.CompletedTask;
        }






        private static Task Stop(CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo e = Console.ReadKey(true);
                    if (e.Key == ConsoleKey.Escape)
                    {

                        cts.Cancel();
                    }
                }
            }
            return Task.CompletedTask;
        }








































    }
}

