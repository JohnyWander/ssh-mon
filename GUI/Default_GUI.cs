using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.Timers;
using System.Runtime.ConstrainedExecution;
using ssh_mon.SSH.Tests;
using System.Dynamic;

namespace ssh_mon.GUI
{
    public class queued_job {

        public Task job;
        public string[] args;


  

    }

    public static class Default_GUI
    {

        static private IDictionary<int, string> servers = SSH.connections.names;
        static private int Server_amount = servers.Count;

        static private int[] pos_x1 = new int[Server_amount];// Status/ err codes positions
        static private int[] pos_x2 = new int[Server_amount];//
        static private int[] pos_y1 = new int[Server_amount];//
        static private int[] pos_y2 = new int[Server_amount];//

        private static string[] cpu_percentage = new string[Server_amount]; // Processor % and positions of output 
        static private int[] cpu_x1 = new int[Server_amount];
        static private int[] cpu_x2 = new int[Server_amount];
        static private int[] cpu_y1 = new int[Server_amount];
        static private int[] cpu_y2 = new int[Server_amount];
        static private int[] cpu_color_index = new int[Server_amount];


        private static string[] ram_percentage = new string[Server_amount];
        static private string[] ram_total = new string[Server_amount];
        static private string[] ram_used = new string[Server_amount];
        static private string[] ram_free = new string[Server_amount];
        static private int[] ram_x1 = new int[Server_amount]; // Ram %
        static private int[] ram_x2 = new int[Server_amount];
        static private int[] ram_y1 = new int[Server_amount];
        static private int[] ram_y2 = new int[Server_amount];
        static private int[] ram_color_index = new int[Server_amount];
       // static public  List<bool> selection_indicator = new List<bool>(Server_amount);
        static public bool[] selection_indicator = new bool[Server_amount];// if serverl is selected bool
        static private int[] pos_x1_selection = new int[Server_amount];// selection position indicators
        static private int[] pos_x2_selection = new int[Server_amount];//
        static private int[] pos_y1_selection = new int[Server_amount];//
        static private int[] pos_y2_selection = new int[Server_amount];//

       public static bool[] Execute_fix_command = new bool[Server_amount];

       // public static List<bool> Execute_fix_command = new List<bool>();


        static public bool[] is_error_present = new bool[Server_amount];
        static private bool[] is_set_ok = new bool[Server_amount];
        static public string[] error_string = new string[Server_amount];



        static Channel<KeyValuePair<Action<object[]>,object[]>> queue = Channel.CreateUnbounded<KeyValuePair<Action<object[]>,object[]>>();
     
        



        public static bool _CONSOLE_block { set; private get; } = false;

        //    private static char dot = '\u2022';




        public static Action ExecuteFixCOMMAND;





        public static void run()
        {


            Console.Clear();
            Tools.Interfaces.IStringTools str_tools = new Tools.Tools();

            Console.CursorVisible = false;
            Tools.Interfaces.IStringTools string_Tools = new Tools.Tools();
            foreach (KeyValuePair<int, string> server in SSH.connections.names)
            {
                str_tools.draw_hash(); Console.Write(" " + server.Key); (pos_x1_selection[server.Key], pos_y1_selection[server.Key]) = Console.GetCursorPosition();
                Console.Write("\n" + server.Value + " - status: ");
                Console.ForegroundColor = ConsoleColor.Green;
                (pos_x1[server.Key], pos_y1[server.Key]) = Console.GetCursorPosition();//
                Console.Write("OK");///
                (pos_x2[server.Key], pos_y2[server.Key]) = Console.GetCursorPosition();///
                Console.ForegroundColor = ConsoleColor.Gray;//// OVERALL STATUS BLOCK

                Console.Write("\nCPU: "); (cpu_x1[server.Key], cpu_y1[server.Key]) = Console.GetCursorPosition();
                Console.Write("     "); (cpu_x2[server.Key], cpu_y2[server.Key]) = Console.GetCursorPosition(); Console.Write("\n"); // CPU status block

                Console.Write("RAM: "); (ram_x1[server.Key], ram_y1[server.Key]) = Console.GetCursorPosition();
                Console.Write("                                        "); (ram_x2[server.Key], ram_y2[server.Key]) = Console.GetCursorPosition(); Console.Write("\n");






                is_set_ok[server.Key] = true;
            }
            Console.Write("\n\n");



            Task.Run(() => Menu());

            Task.Run(async () =>
            {
                while (await queue.Reader.WaitToReadAsync())
                {
                    var job_n_args = await queue.Reader.ReadAsync();
                    Action<object[]> job = job_n_args.Key;
                    object[] args = job_n_args.Value;
                    job(args);
                }

            });


        }


        public static void Set_Red(int number)
        {
            if (_CONSOLE_block == false)
            {
                _CONSOLE_block = true;
                int old_X = 0; int old_Y = 0;
                try
                {
                    (old_X, old_Y) = Console.GetCursorPosition();
                    Console.SetCursorPosition(pos_x1[number], pos_y1[number]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERR: " + error_string[number]);
                    (pos_x2[number], pos_y2[number]) = Console.GetCursorPosition();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    _CONSOLE_block = false;
                    Console.SetCursorPosition(old_X, old_Y);
                }

            }
            else
            {
                while (_CONSOLE_block == true)
                {

                }

                Set_Red(number);

            }
        }

        public static void Set_Green(int number)
        {
            if (_CONSOLE_block == false)
            {
                try
                {
                    Console.SetCursorPosition(pos_x1[number], pos_y1[number]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK");
                    for (int i = pos_x1[number]; i < pos_x2[number]; i++)
                    {
                        Console.Write(" ");
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

            }
            else
            {
                while (_CONSOLE_block == true)
                {

                }

                Set_Green(number);
            }

        }

        private static Task Menu()
        {
            int x, y;
            (x, y) = Console.GetCursorPosition();
            Console.WriteLine(ssh_mon.GUI.Language_strings.language_strings["select_server_menu"] + "    " + ssh_mon.GUI.Language_strings.language_strings["execute_fix_command_menu"]);
            Console.WriteLine(ssh_mon.GUI.Language_strings.language_strings["deselect_server_menu"] + "  ");
            Console.WriteLine(ssh_mon.GUI.Language_strings.language_strings["restart_gui"] + "       ");

            Console.WriteLine("ESC. quit");
            while (!Program.cancel_all.IsCancellationRequested)
            {
                try
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.Write("\b \b");
                    if (key.Key != ConsoleKey.Escape)
                    {

                        char key_c = key.KeyChar;
                        //  byte xd = Convert.ToByte(key_c);
                        //  byte[] xdw = new byte[] { xd };
                        //  int switch_i = Convert.ToInt32(Encoding.UTF8.GetString(xdw)); // removed for char option



                        switch (key_c)
                        {
                            case '1':
                                option1_select_server();
                                break;
                            case '2':
                                option2_deselect_server();
                                break;
                            case '3':
                                Console.Clear();
                                run();
                                //    return Task.CompletedTask;
                                break;
                            case 'f':
                                ExecuteFixCOMMAND();
                                break;
                            
                        }
                    }
                    else
                    {

                        Program.cancel_all.Cancel();
                        Console.WriteLine("Quitiing...");
                    }

                }
                catch (FormatException)
                {
                    continue;
                }



            }
            return Task.CompletedTask;
        }




        private static void option1_select_server()
        {
            (int x, int y) = Console.GetCursorPosition();

            int x1; int y2;
            Console.Write(Language_strings.language_strings["select_server"]);

            ///!!!!!!!!!!!!
            (x1,y2)  = Console.GetCursorPosition(); 
            ConsoleKeyInfo key = Console.ReadKey();
            // Console.Write("\b \b");

            //Console.Write(x + "" + x1);
            Console.SetCursorPosition(x, y);
            for (int i = x; i < x1; i++) { Console.Write("  "); }
            Console.SetCursorPosition(x, y);



            byte xd = Convert.ToByte(key.KeyChar);
            byte[] xdw = new byte[] { xd };
            int switch_i = Convert.ToInt32(Encoding.UTF8.GetString(xdw)); ;
            select(switch_i, x, y);

        }

        private static void select(int id_, int old_x_, int old_y_)
        {
            try
            {
                object[] args = new object[] { id_, old_x_, old_y_ };
                Action<object[]> job = (args) =>
                {
                    int id = (int)args[0];
                    int old_x = (int)args[1];
                    int old_y = (int)args[2];



                    selection_indicator[id] = true;
                    Console.SetCursorPosition(pos_x1_selection[id], pos_y1_selection[id]);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(Language_strings.language_strings["selected_server"]);
                    (pos_x2_selection[id], pos_y2_selection[id]) = Console.GetCursorPosition();
                    Console.SetCursorPosition(old_x, old_y);
                    Console.ForegroundColor = ConsoleColor.Gray;
                };

                Task.Run(async () =>
                {

                    var pair = new KeyValuePair<Action<object[]>, object[]>(job, args);
                    await queue.Writer.WaitToWriteAsync();
                    await queue.Writer.WriteAsync(pair);
                });

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        private static void option2_deselect_server()
        {
            (int x, int y) = Console.GetCursorPosition();
            Console.Write(Language_strings.language_strings["deselect_server"]);
            (int x1, int y2) = Console.GetCursorPosition();
            ConsoleKeyInfo key = Console.ReadKey();
            // Console.Write("\b \b");

            //Console.Write(x + "" + x1);
            Console.SetCursorPosition(x, y);
            for (int i = x; i < x1; i++) { Console.Write("  "); }
            Console.SetCursorPosition(x, y);



            byte xd = Convert.ToByte(key.KeyChar);
            byte[] xdw = new byte[] { xd };
            int switch_i = Convert.ToInt32(Encoding.UTF8.GetString(xdw)); ;
            deselect(switch_i, x, y);
        }

        private static void deselect(int id_, int old_x_, int old_y_)
        {
            object[] args = new object[] { id_, old_x_, old_y_ };
            Action<object[]> job = (args) =>
            {
                int id=(int)id_;
                int old_x=(int)old_x_;
                int old_y=(int)old_y_;

                selection_indicator[id] = false;
                Console.SetCursorPosition(pos_x1_selection[id], pos_y1_selection[id]);
                for (int i = pos_x1_selection[id]; i < pos_x2_selection[id]; i++) { Console.Write(" "); }
                Console.SetCursorPosition(old_x, old_y);
                Console.ForegroundColor = ConsoleColor.Gray;
            };

            Task.Run(async () =>
            {
                
                var pair = new KeyValuePair<Action<object[]>, object[]>(job, args);
                await queue.Writer.WaitToWriteAsync();
                await queue.Writer.WriteAsync(pair);
            });






        }


        public static async Task fetch_cpu_ram_result(int id, string cpu_usage, double total, double used, double free)
        {
            int console_color_index = 0;

            cpu_percentage[id] = cpu_usage.Trim('%').Trim().Replace('.', ',');
            // Console.WriteLine(cpu_percentage[id]);
            int cpu_int_percentage = Convert.ToInt32(Convert.ToDouble(cpu_percentage[id]));
            if (cpu_int_percentage <= 50)
            {
                console_color_index = 10; // green
            }
            else if (cpu_int_percentage > 50 && cpu_int_percentage <= 80)
            {
                console_color_index = 14;// yellow
            }

            if (cpu_int_percentage > 80)
            {
                console_color_index = 12; //red
            }
            // Console.WriteLine(id);


            int console_color_index_ram = 0;
            double ram_percentage_doub = (double)Math.Round((double)(used * 100) / total, 2);
            ram_percentage[id] = Convert.ToString(ram_percentage_doub) + "%";
            ram_total[id] = Convert.ToString(total) + "GB";
            ram_used[id] = Convert.ToString(used) + "GB";
            ram_free[id] = Convert.ToString(free) + "GB";

            // Console.WriteLine((int)ram_percentage_doub);
            if ((int)ram_percentage_doub <= 50)
            {
                console_color_index_ram = 10;
            }
            else if ((int)ram_percentage_doub > 50 && (int)cpu_int_percentage <= 80)
            {
                console_color_index_ram = 14;
            }

            if ((int)ram_percentage_doub > 80)
            {

                console_color_index_ram = 12;
            }




            //  Task.Run(() => insert_cpu_percentage(cpu_usage, id, console_color_index)).Wait();// Old
            //  Task.Run(() => insert_ram_percentage(ram_percentage[id], id, ram_total[id], ram_used[id], console_color_index_ram)).Wait();// Old
           Task cpu= Task.Run(async() =>
            {
                
                    await queue.Writer.WaitToWriteAsync(); //waiting for chance to write to channel
                    Action<object[]> insert_cpu = insert_cpu_percentage; // creatinc actionn delegate
                    object[] args = new object[] { cpu_usage, id, console_color_index }; // object array for action delegates arguments
                    var pair = new KeyValuePair<Action<object[]>, object[]>(insert_cpu, args); // creating keypair of Key - action delegate - value arguments for it
                    await queue.Writer.WriteAsync(pair); // waiting for writer to to it's job
                  
                
            });

            Task ram =Task.Run(async () =>
            {
                await queue.Writer.WaitToWriteAsync(); // same as above ^
                Action<object[]> insert_ram = insert_ram_percentage;//  |
                object[] args = new object[] { ram_percentage[id], id, ram_total[id], ram_used[id], console_color_index_ram };
                var pair = new KeyValuePair<Action<object[]>, object[]>(insert_ram, args);
                await queue.Writer.WriteAsync(pair);
            });

       
        }

        private static void insert_cpu_percentage(object[] args)
        {
            string percentage = (string)args[0];
            int server_id = (int)args[1];
            int console_color_index = (int)args[2];


            Console.ForegroundColor = (ConsoleColor)console_color_index;
            (int old_x, int old_y) = Console.GetCursorPosition();
            Console.SetCursorPosition(cpu_x1[server_id], cpu_y1[server_id]);
            Console.Write(percentage);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(old_x, old_y);
          
        }

        private static void insert_ram_percentage(object[] args)
        {
            string percentage=(string)args[0];
            int server_id =(int)args[1];
            string total=(string)args[2];
            string used=(string)args[3];
            int Console_color_index=(int)args[4];
            Console.ForegroundColor = (ConsoleColor)Console_color_index;
            (int old_x, int old_y) = Console.GetCursorPosition();
            Console.SetCursorPosition(ram_x1[server_id], ram_y1[server_id]);
            Console.Write(percentage + "   " + used + "/" + total);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(old_x, old_y);
        }

        public static void insert_error_from_module(object[] args)
        {
            Action<object[]> insert_module_error = (args) =>
            {
                string error = (string)args[0];
                int server_id = (int)args[1];
                (int old_x, int old_y) = Console.GetCursorPosition();
                Console.SetCursorPosition(pos_x1[server_id], pos_y1[server_id]);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(error);
                (pos_x2[server_id], pos_y2[server_id]) = Console.GetCursorPosition();
                Console.ResetColor();
                Console.SetCursorPosition(old_x, old_y);

            };

            Task.Run(async () =>
            {
               await queue.Writer.WaitToWriteAsync();
               var pair = new KeyValuePair<Action<object[]>, object[]>(insert_module_error, args);
               await queue.Writer.WriteAsync(pair);
            });
            
        }

        public static void module_clear_error(object[] args)
        {

            Action<object[]> clear = (args) =>
            {
                int serverid = (int)args[0];
                Console.SetCursorPosition(pos_x1[serverid], pos_y1[serverid]);
                (int old_x, int old_y) = Console.GetCursorPosition();
                for(int i = pos_x1[serverid]; i <= pos_x2[serverid]; i++)
                {
                    Console.Write(" ");
                }
                Console.SetCursorPosition(old_x, old_y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("OK");
                Console.ResetColor();
                Console.SetCursorPosition(old_x, old_y);

            };


            Task.Run(async () =>
            {
                await queue.Writer.WaitToWriteAsync();
                var pair = new KeyValuePair<Action<object[]>, object[]>(clear, args);
                await queue.Writer.WriteAsync(pair);
            });
        }

        
    }

  

    // public static void


}
