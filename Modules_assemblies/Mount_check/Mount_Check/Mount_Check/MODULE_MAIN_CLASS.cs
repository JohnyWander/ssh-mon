//EXAMPLE MODULE FOR SSH-MON 
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module // MUST BE THE SAME
{


    public class MODULE_MAIN_CLASS // Name must be the same
    {
        private bool Loaded_Correctly = false;
        private bool TEST_can_be_performed = false;
        private static int delay = 20000; // intervals between test executions (ms)

        //     private bool[] TestLock = new bool[3];// 3 important dicts function have to run
        List<bool> TestLock_list = new List<bool>
        {
            false,
            false,
            false
        };

        private string ERR_messege = "";



        
        private IDictionary<int, string> CommandsToExecute = new Dictionary<int, string> //commands to run
        {
            {1,@"df -h"},
            {2,@"ps -ef --forest | grep ""ssh"" "}
        };

        private IDictionary<int, string> CommandsResults = new Dictionary<int, string>
        {
            {1,"" },
            {2,""}
        };

        private IDictionary<int, string> FixCommands = new Dictionary<int, string>
        {
            {1,"pwd"} // could be left empty
        };

        private IDictionary<int, string> ERR_codes = new Dictionary<int, string>
        {
            {1,"No /dev/sda disk is present"},
            {2,"Root user is not connected" }
        };


        public MODULE_MAIN_CLASS()
        {
            Loaded_Correctly = true; // if constructor run was ok -- Must be!



        }



        public bool GET_Loaded_correctly()
        {
            TestLock_list[0] = true;
            return Loaded_Correctly;
        }

        public IDictionary<int, string> GET_commands_to_execute()
        {
            TestLock_list[1] = true;
            return CommandsToExecute;
        }


        public bool PUSH_command_results(object key, object value)
        {
            try
            {
                TestLock_list[2] = true;
                CommandsResults[(int)key] = (string)value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IDictionary<int,string> GET_command_results() // NOT USED DEBUGGING PURPOSE
        {
            return CommandsResults;
        }

        public IDictionary<int,string> GET_fix_commands()
        {
            return FixCommands;
        }

        public string GET_err_messege()
        {
            return ERR_messege;
        }

        public int GET_delay()
        {
            return delay;
        }

        public bool TEST()
        {
            Predicate<bool> test = ToPred;

            if (TestLock_list.TrueForAll(test)){

                if (CommandsResults[1].Contains("/dev/sda") && CommandsResults[2].Contains("root@pts"))
                {
                    return true;
                }
                else
                {
                    if (!CommandsResults[1].Contains("/dev/sda"))
                    {
                        ERR_messege = ERR_codes[1];
                    }
                    else if(!CommandsResults[2].Contains("root@pts"))
                    {
                        ERR_messege = ERR_codes[2];
                    }
                    else
                    {
                        ERR_messege = ERR_codes[1] + " and " + ERR_codes[2];
                    }

                    return false;
                }
                



            }
            else
            {
                throw new Exception("Test can't be performed cause key functions haven't been ran yet ");
                return false;
            }







        }


        private bool ToPred(bool x) // for Predicate delegate -- checking if all core functions succeeded
        {
            if (x == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



    }
}
