﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Mount_check
{
    internal static class initialize
    {
        private static string default_conf = @"[test apllies to]- servernames in servers folder
test1,test2,test3
[\test applies to]
[test commands and outputs]
$$$ command=""df -h"" || output_should_contain=""//192.168.0.2//test_mount""
$$$ command=""df -h"" || output_shoudl_contain=""/dev/sda1""
[\test commands and outputs]
[Enabled]
false
";

        public static void run()
        {
            try
            {
                File.ReadAllText("module.conf");
            }
            catch
            {
                File.WriteAllText("module.conf", default_conf);
            }


        }



    }
}
