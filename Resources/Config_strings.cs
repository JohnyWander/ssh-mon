using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssh_mon.Resources
{
    public static class Config_strings
    {
        public static string default_conf { get; } = @"[Language] - EN,PL Supported
EN
";


        public static string default_server { get; } = @"[server]
name=server1
user=Johnny
ip=192.168.0.128
port=22
enabled=true
";







        public static string default_lang { get; } = @"[Menu]
start_test=1. Start Monitoring
[Second_menu]
select_server=Please enter server ID
[Exceptions]
server_config_error=Something is not ok with the server configuration file ! :
";

    }
}
