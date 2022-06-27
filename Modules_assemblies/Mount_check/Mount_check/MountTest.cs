using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mount_check
{
    public class Module
    {
        private bool Test_failed = false;
        private string error_messege = "MountCheck Failed:";
        private string[] ssh_command_outputs;

        private int iteration_time = 10000;

        private List<string> names = new List<string>();
        private List<string[]> commands_and_output = new List<string[]>();

        
        public Module()
        {
            initialize.run();
            ReadConf.run();

            names = ReadConf.applies_to;
            commands_and_output = ReadConf.commands_and_outputs;
            





        }

        public void run_test()
        {
            error_messege = "MountCheck Failed:";
            IList<string> outputs = new List<string>();
            foreach(string[] array in commands_and_output)
            {
                outputs.Add(array[1]);
            }

            for (int i = 0; i <= outputs.Count; i++)
            {
                if (!ssh_command_outputs[i].Contains(outputs[i])){
                    error_messege+= "Command "+i+"# failed! ";
                }
            }


        }

        public bool get_Test_failed()
        {
            return Test_failed;
        }

        public string get_Error_messege()
        {
            return error_messege;
        }

        
        public List<string> get_names()
        {
            return names;
        }
            
        public List<string[]> get_commands_and_outputs()
        {
            return commands_and_output;
        }
        

        public void set_output(string[] output)
        {
            ssh_command_outputs = output;
        }

        public int get_iteration_time()
        {
            return iteration_time;
        }


    }
}
