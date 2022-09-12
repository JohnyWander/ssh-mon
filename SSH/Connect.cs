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

        public List<Thread> connections_thread_list = new List<Thread>();
       
        public Task run(CancellationTokenSource cancel, bool Already_enrypted)
        {
            build_serverlist(Already_enrypted);


            foreach (var conn in serverlist)
            {
                names.Add(conn.ID, conn.name);
                Thread C = new Thread(() =>
                {
                    connection_and_tests(conn.SshClient, cancel.Token, conn.name, conn.ID);
                });
                C.Start();
                connections_thread_list.Add(C);


            }

            Program.CompletedCreatingConnectionList = true;
            return Task.CompletedTask;
        }



        private void connection_and_tests(SshClient client, CancellationToken cts, string _serverName, int int_keyID) { 
            string _serverName_ = _serverName;
           
            
                try
                {
                    Tests.tests tests = new Tests.tests(client, int_keyID,_serverName);
                
                //   var cmd = sshClient.CreateCommand("df -h");
                // var cmd = sshClient.CreateCommand("top -n1");
                //   string result = cmd.Execute();
                //  Console.WriteLine(result);
                while (!cts.IsCancellationRequested)
                    {
                        Task.Delay(2000).Wait();
                    }

                tests.Get_cpu_ram.Stop();
                //tests.Get_cpu_ram.Dispose();

                client.Disconnect();
                client.Dispose();
              
              
                




                }
                catch (Exception e)
                {
                   // Console.WriteLine(e.Message);
                  //  Console.ReadKey(true);
                    //Environment.Exit(0);
                }

                
                    
                

            
            
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

        private static void terminate_threads()
        {


        }







































    }
}

