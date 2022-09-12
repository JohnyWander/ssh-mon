//EXAMPLE MODULE FOR SSH-MON 
using System;
using System.Collections.Generic;

namespace Module // MUST BE THE SAME
{
  

    public class MODULE_MAIN_CLASS // Name must be the same
    {
        private bool Loaded_Correctly=false;

        private IDictionary<int, string> CommandsToExecute = new Dictionary<int, string> //commands to run
        {
            {1,@"df - h"},
            {2,@"ps -ef --forest | grep ""ssh"" "}
        };

        private IDictionary<int, string> OutputsResultShouldContain = new Dictionary<int, string>
        {
            {1,"/dev/sda1"},


        };

        public MODULE_MAIN_CLASS()
        {
            Loaded_Correctly = true; // if constructor run was ok -- Must be!



        }



        public bool GET_Loaded_correctly()
        {
            return Loaded_Correctly;
        }




    }
}
