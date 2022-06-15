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
            Get_cpu_ram.Elapsed += new ElapsedEventHandler((sender, e) => Task.Run(()=>bpp_admin.GUI.Default_GUI.fetch_cpu_ram_result(id,cpu_usage).Wait()));
            Get_cpu_ram.Interval = 1000;
            Get_cpu_ram.Enabled = true;
            
           
        }

        private static string get_cpu_ram_usage(object source, ElapsedEventArgs e, SshClient client)
        {

            //   var cmd = client.CreateCommand("top -b -n5 -d.3 | grep \"Mem\" | tail -n1 | awk '{print($2)}' | cut -d'%' -f1");
            var cmd = client.CreateCommand(@"top -b -n10 -d.2 | grep 'Cpu(s)\|Mem' | grep -v 'Swap' | tail -n2");
            string result = cmd.Execute();
           //Console.WriteLine(result);
            string[] lines = result.Split("\n");
            //Console.WriteLine(lines[2]);
            string proc_use = lines[0].Substring(8, 5);

            
            bool Mem_in_MiB = false;
            bool comma_in_double = false;
            if(lines[1].ToCharArray().Count(c => c == ',') > 3) {
                StringBuilder if_comma = new StringBuilder(lines[1]);
                bool del = true;
                int[] comma_pos = new int[lines[1].ToCharArray().Count(c => c == ',')];
                int ite = 0;
                for(int i = 0; i < if_comma.Length; i++)
                {

                    if (if_comma[i] == ',')
                    {
                        comma_pos[ite] = i;
                        ite++;
                    }

                }
                if_comma.Remove(comma_pos[0], 1);if_comma.Insert(comma_pos[0], '.');
                if_comma.Remove(comma_pos[2], 1); if_comma.Insert(comma_pos[2], '.');
                if_comma.Remove(comma_pos[4], 1); if_comma.Insert(comma_pos[4], '.');




                //Console.WriteLine(if_comma.ToString());
                lines[1] = if_comma.ToString();
            }
            string[] mem_line = lines[1].Split(',');
            if (mem_line[0].Contains("MiB")) { Mem_in_MiB = true; }
       

            Task<double> get_total = Task.Run(() =>
            {
                double output = 0;
                StringBuilder buffer = new StringBuilder();
          
              
                foreach (char s in mem_line[0])
                {
                    if (s != 'M' && s != 'e' &&s!='m' && s != ':' && s != 'k' && s != 't' && s != 'o' && s != 'a' && s != 'l' && s != ','&& s!='i'&&s!='B'&&s!='f'&&s!='r')
                    {
                        buffer.Append(s);
                    }
                }
                string inbuff = buffer.ToString();
                inbuff = inbuff.Trim('\t').Trim();
              //  Console.WriteLine(inbuff);
                output = Convert.ToDouble(inbuff.Replace('.',','));
              //  output = Convert.ToInt32(776.2);
                if (Mem_in_MiB == true)
                {
                    output = output / 1024;//MiB to GiB
                }
                else
                {
                    output = output / 1048576; // KB to GB
                }
                return Math.Round(output,2);//output;
            });

            Task<double> get_used = Task.Run(() =>
            {
                double output = 0;
                StringBuilder buffer = new StringBuilder();
                foreach (char s in mem_line[1])
                {
                    if (s != 'u' && s != 's' && s != 'e' && s != 'd' && s != 'k' && s != ',' && s != 'i' && s != 'B' && s != 'f' && s != 'r') 
                    {
                        buffer.Append(s);
                    }

                }
                string inbuff = buffer.ToString().Trim('\t').Trim();
                output = Convert.ToDouble(inbuff.Replace('.',','));
              //  output = Convert.ToInt32(744.3);
                // Console.WriteLine(buffer.ToString());
                if (Mem_in_MiB == true)
                {
                    output = output / 1024;//MiB to GiB
                }
                else
                {
                    output = output / 1048576; // KB to GB
                }
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
