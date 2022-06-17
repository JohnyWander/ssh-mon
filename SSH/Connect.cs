using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Renci.SshNet;
namespace ssh_mon.SSH
{
    public class connections : make_list
    {
      static public IDictionary<int, string> names = new Dictionary<int, string>();

       public List<Task> connections123 = new List<Task>();
        public static int xD = 100;
        public Task run(CancellationTokenSource cancel)
        {
            build_serverlist();


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


                ConnectionInfo conn = new ConnectionInfo(SERVER.ip, SERVER.user, new AuthenticationMethod[]
                {
                  
                new PrivateKeyAuthenticationMethod(SERVER.user,pvk)
                });

                conn.KeyExchangeAlgorithms.Remove("curve25519-sha256"); // Algo is security thread - client private key generated with System.Random
                conn.KeyExchangeAlgorithms.Remove("curve25519-sha256@libssg.org"); // which is  as insecure


                connections123.Add(Task.Factory.StartNew(() => connection_and_tests(conn, cts.Token,SERVER.name,ite),cts.Token));

                //Console.WriteLine(ite);
                Thread.Sleep(100); // ... :| it makes "ite" increment propertly

                ite++;
            }

          //connections123.Add(Task.Run(() => Stop(cts)));
           
            



            //   Task.WhenAll(connections123);
          //  Task.WaitAll(connections123.ToArray());

            return Task.CompletedTask;
        }



        private Task connection_and_tests(ConnectionInfo connection, CancellationToken cts, string _serverID,int int_key)
        {
            string _serverID_ = _serverID;
            using (var sshClient = new SshClient(connection))
            {
                try
                {
                  

                    Tests.tests tests = new Tests.tests(sshClient,int_key);
                    //sshClient.Connect();


                    while (!cts.IsCancellationRequested)
                    {
                        Task.Delay(1000).Wait(); // :| i know ;)
                    }

                    

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
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

