using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.Timers;
using System.Reflection;
namespace ssh_mon.SSH.Tests
{
    public class tests
    {


        public string os;
        public int id;
        public string cpu_usage { get; private set; }
        public string cpu_usage_top_process;

        public double ram_total { get; private set; }
        public double ram_used { get; private set; }
        public double ram_free { get; private set; }

        public string[] cpu_ram_returns;

        public System.Timers.Timer Get_cpu_ram { get; set; } = new System.Timers.Timer() ;
        public tests(SshClient client, int server_ID, string servername)
        {
            id = server_ID;
            // client.Connect();

            // Console.WriteLine(id);

            
            Get_cpu_ram.Elapsed += new ElapsedEventHandler((sender, e) => cpu_usage = get_cpu_ram_usage(sender, e, client));
            Get_cpu_ram.Elapsed += new ElapsedEventHandler((sender, e) => ssh_mon.GUI.Default_GUI.fetch_cpu_ram_result(id, cpu_usage, ram_total, ram_used, ram_free));
            Get_cpu_ram.Interval = Readconf.cpu_ram_timer;
            Get_cpu_ram.Enabled = true;


            ModuleTests MT = new ModuleTests(Program.Modules.Module_List,client,id);




        }
        private string get_cpu_ram_usage(object source, ElapsedEventArgs e, SshClient client)
        {
            string[] values_to_return = new string[4];
            //   var cmd = client.CreateCommand("top -b -n5 -d.3 | grep \"Mem\" | tail -n1 | awk '{print($2)}' | cut -d'%' -f1");
            var cmd = client.CreateCommand(@"top -b -n10 -d.2 | grep 'Cpu(s)\|Mem' | grep -v 'Swap' | tail -n2");
            string result = cmd.Execute();
           //Console.WriteLine(result);
            string[] lines = result.Split("\n");
            //Console.WriteLine(lines[2]);
            string proc_use = lines[0].Substring(8, 5);

            
            bool Mem_in_MiB = false;
           // bool comma_in_double = false;
            if(lines[1].ToCharArray().Count(c => c == ',') > 3) {
                StringBuilder if_comma = new StringBuilder(lines[1]);
           
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

            

            Task<double> get_First = Task.Run(() =>
            {
                if (mem_line[0].Contains("otal"))
                {

                }
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

            Task<double> get_second = Task.Run(() =>
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
                return Math.Round(output, 2);
            });

            Task<double> get_third = Task.Run(() =>
            {
                double output = 0;
                StringBuilder buffer = new StringBuilder();
                foreach (char s in mem_line[2])
                {
                    if (s != 'u' && s != 's' && s != 'e' && s != 'd' && s != 'k' && s != ',' && s != 'i' && s != 'B' && s != 'f' && s != 'r')
                    {
                        buffer.Append(s);
                    }

                }
                string inbuff = buffer.ToString().Trim('\t').Trim();
                output = Convert.ToDouble(inbuff.Replace('.', ','));
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
                return Math.Round(output, 2);
            });

            Task.WaitAll(get_First, get_second,get_third);

            if (mem_line[0].Contains("otal"))   // as versions of top not always have same order of free total/used
            {
                ram_total = get_First.Result;
            }else if (mem_line[0].Contains("ree")){
                ram_free = get_First.Result;
            }else if(mem_line[0].Contains("sed")){
                ram_used = get_First.Result;
            }
            else
            {
                throw new Exceptions.ServerNotSupportedException("Your server uses not supported version of top");
            }


            if (mem_line[1].Contains("otal"))   // as versions of top not always have same order of free total/used
            {
                ram_total = get_second.Result;
            }
            else if (mem_line[1].Contains("ree"))
            {
                ram_free = get_second.Result;
            }
            else if (mem_line[1].Contains("sed"))
            {
                ram_used = get_second.Result;
            }
            else
            {
                throw new Exceptions.ServerNotSupportedException("Your server uses not supported version of top");
            }


            if (mem_line[2].Contains("otal"))   // as versions of top not always have same order of free total/used
            {
                ram_total = get_third.Result;
            }
            else if (mem_line[2].Contains("ree"))
            {
                ram_free = get_third.Result;
            }
            else if (mem_line[2].Contains("sed"))
            {
                ram_used = get_third.Result;
            }
            else
            {
                throw new Exceptions.ServerNotSupportedException("Your server uses not supported version of top");
            }

          

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
