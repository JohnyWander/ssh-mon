using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
namespace bpp_admin.GUI
{
    public static class Default_GUI
    {

        static private IDictionary<int, string> servers = SSH.connections.names;
        static private int Server_amount = servers.Count;

        static private int[] pos_x1 = new int[Server_amount];// Status/ err codes positions
        static private int[] pos_x2 = new int[Server_amount];//
        static private int[] pos_y1 = new int[Server_amount];//
        static private int[] pos_y2 = new int[Server_amount];//

        static private bool[] selection_indicator = new bool[Server_amount];// if serverl is selected bool
        static private int[] pos_x1_selection = new int[Server_amount];// selection position indicators
        static private int[] pos_x2_selection = new int[Server_amount];//
        static private int[] pos_y1_selection = new int[Server_amount];//
        static private int[] pos_y2_selection = new int[Server_amount];//


        static private bool[] is_error_present = new bool[Server_amount];
        static private bool[] is_set_ok = new bool[Server_amount];
        static private string[] error_string = new string[Server_amount];

        static private int x, y;

    //    private static char dot = '\u2022';

        public static void run()
        {
            Console.Clear();
            Tools.Interfaces.IStringTools str_tools = new Tools.Tools();

            Console.CursorVisible = false;
            Tools.Interfaces.IStringTools string_Tools = new Tools.Tools();
            foreach (KeyValuePair<int, string> server in SSH.connections.names)
            {
                str_tools.draw_hash();Console.Write(" "+ server.Key);(pos_x1_selection[server.Key], pos_y1_selection[server.Key]) = Console.GetCursorPosition();
                Console.Write("\n"+server.Value + ": ");
                Console.ForegroundColor = ConsoleColor.Green;
                (pos_x1[server.Key], pos_y1[server.Key]) = Console.GetCursorPosition();
                Console.Write("OK");
                (pos_x2[server.Key], pos_y2[server.Key]) = Console.GetCursorPosition();
                Console.ForegroundColor = ConsoleColor.Gray;
                is_set_ok[server.Key] = true;
            }
            Console.Write("\n\n");

            System.Timers.Timer Run_set_status = new System.Timers.Timer();
            Run_set_status.Elapsed += new ElapsedEventHandler((sender, e) => Set_status(sender,e));
            Run_set_status.Interval = 5000;
            Run_set_status.Enabled = true;

            Task.Run(() => Menu()).ConfigureAwait(true);

        }

        public static void Set_status(object source,ElapsedEventArgs e)
        {

            foreach (KeyValuePair<int, string> server in SSH.connections.names)
            {
                if (is_error_present[server.Key] == true)
                {
                    Set_Red(server.Key);
                    is_set_ok[server.Key] = false;
                }
                else if (is_error_present[server.Key] == false && is_set_ok[server.Key] == false)
                {
                    Set_Green(server.Key);
                    is_set_ok[server.Key] = true;
                }
                else
                {
              
                    continue;
                }



            }

        }
            private static void Set_Red(int number)
            {
                try
                {
                    Console.SetCursorPosition(pos_x1[number], pos_y1[number]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERR: " + error_string[number]);
                    (pos_x2[number], pos_y2[number]) = Console.GetCursorPosition();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            private static void Set_Green(int number)
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
                } catch (Exception e)
                {
                    Console.Write(e.Message);
                }



            }

            private static Task Menu()
            {
            int x, y;
            (x, y) = Console.GetCursorPosition();
            Console.WriteLine("1. Select server");
            Console.WriteLine("2. Deselect server");
            Console.WriteLine("3. Restart GUI");
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
                        byte xd = Convert.ToByte(key_c);
                        byte[] xdw = new byte[] { xd };
                        int switch_i = Convert.ToInt32(Encoding.UTF8.GetString(xdw));

                        switch (switch_i)
                        {
                            case 1:
                                option1_select_server();
                                break;
                            case 2:
                                option2_deselect_server();
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
             (int x,int y) = Console.GetCursorPosition();
            Console.Write(Language_strings.language_strings["select_server"]);
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
            select(switch_i,x,y);

        }

        private static void select(int id,int old_x, int old_y)
        {
            try
            {
                selection_indicator[id] = true;
                Console.SetCursorPosition(pos_x1_selection[id], pos_y1_selection[id]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" SELECTED");
                (pos_x2_selection[id], pos_y2_selection[id]) = Console.GetCursorPosition();
                Console.SetCursorPosition(old_x, old_y);
                Console.ForegroundColor = ConsoleColor.Gray;
            }catch(Exception e)
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

        private static void deselect(int id,int old_x,int old_y)
        {
            selection_indicator[id] = false;
            Console.SetCursorPosition(pos_x1_selection[id], pos_y1_selection[id]);
            for(int i=pos_x1_selection[id];i< pos_x2_selection[id]; i++) { Console.Write(" "); }
            Console.SetCursorPosition(old_x, old_y);
            Console.ForegroundColor = ConsoleColor.Gray;

        }
    }

}

