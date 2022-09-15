//EXAMPLE MODULE FOR SSH-MON 
using System;
using System.Collections.Generic;

namespace Module // MUST BE THE SAME
{


    public class MODULE_MAIN_CLASS // Name must be the same
    {
        private bool Loaded_Correctly = false;

        private IDictionary<int, string> CommandsToExecute = new Dictionary<int, string> //commands to run
        {
            {1,@"df - h"},
            {2,@"ps -ef --forest | grep ""ssh"" "}
        };

        private IDictionary<int, string> CommandsResults = new Dictionary<int, string>
        {
            {1,"" },
            {2,""}
        };


        public MODULE_MAIN_CLASS()
        {
            Loaded_Correctly = true; // if constructor run was ok -- Must be!



        }



        public bool GET_Loaded_correctly()
        {
            return Loaded_Correctly;
        }

        public IDictionary<int, string> GET_commands_to_execute()
        {
            return CommandsToExecute;
        }


        public bool PUSH_command_results(object key, object value)
        {
            try
            {
                CommandsResults[(int)key] = (string)value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IDictionary<int,string> GET_command_results()
        {
            return CommandsResults;
        }







    }
}
