using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.Timers;
namespace bpp_admin.SSH.Tests
{
    public class tests
    {

        public string os;
        public int id;
        public string cpu_usage { get; set; } 
        public string cpu_usage_top_process;

        public string ram_top;

        public tests(SshClient client,int server_ID)
        {
            id = server_ID;
            client.Connect();
            
           // Console.WriteLine(id);

            System.Timers.Timer Get_cpu_ram= new System.Timers.Timer();
            Get_cpu_ram.Elapsed += new ElapsedEventHandler((sender, e) => cpu_usage=get_cpu_ram_usage(sender, e,client));
            Get_cpu_ram.Elapsed += new ElapsedEventHandler((sender, e) => bpp_admin.GUI.Default_GUI.fetch_cpu_ram_result(id,cpu_usage));
            Get_cpu_ram.Interval = 1000;
            Get_cpu_ram.Enabled = true;
            
           
        }

        private static string get_cpu_ram_usage(object source, ElapsedEventArgs e, SshClient client)
        {

            //   var cmd = client.CreateCommand("top -b -n5 -d.3 | grep \"Mem\" | tail -n1 | awk '{print($2)}' | cut -d'%' -f1");
            var cmd = client.CreateCommand(@"top -b -n5 -d.2 | grep 'Cpu(s)\|Mem' | tail -n2");
            string result = cmd.Execute();
            //Console.WriteLine(result);
            string[] lines = result.Split("\n");
            //Console.WriteLine(lines[2]);
            string proc_use = lines[0].Substring(8, 5);

            string[] mem_line = lines[1].Split(',');

            //C//onsole.WriteLine(mem_line[1]);

            Task<int> get_total = Task.Run(() =>
            {
                int output = 0;
                StringBuilder buffer = new StringBuilder();
                foreach (char s in mem_line[0])
                {
                    if (s != 'M' && s != 'e' &&s!='m' && s != ':' && s != 'k' && s != 't' && s != 'o' && s != 'a' && s != 'l' && s != ',')
                    {
                        buffer.Append(s);
                    }
                }

                output = Convert.ToInt32(buffer.ToString().Trim());
                output = output / 1048576; // KB to GB
                
                return output;//output;
            });

            Task<double> get_used = Task.Run(() =>
            {
                double output = 0;
                StringBuilder buffer = new StringBuilder();
                foreach (char s in mem_line[1])
                {
                    if (s != 'u' && s != 's' && s != 'e' && s != 'd' && s != 'k' && s != ',')
                    {
                        buffer.Append(s);
                    }

                }
                output = Convert.ToInt32(buffer.ToString().Trim());
               // Console.WriteLine(buffer.ToString());
                output = Math.Round(output / 1048576,2); // KB to GB
                Console.Write(output);
                return output;
            });

            Task.WaitAll(get_total, get_used);



            int index = 12;

           // Console.WriteLine(proc_use);

            if (proc_use.Contains("%"))
            {
                return proc_use;
            }
            else
            {
                return proc_use + "%";
            }

        }

    }
}
