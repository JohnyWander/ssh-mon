using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bpp_admin.Resources
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
[OPENSSH_PRIV_KEY]
----- BEGIN RSA PRIVATE KEY-----
MIIEogIBAAKCAQEAikdGpJRMjrbsAq1thMtr8kCrru8ySXayzkr/E05zT+npBKbF
64SG3DS2I6QzmONllkroitqINPKjWf0xvFpV2e6YQES7tYqdym6y9MW/cw5FiUE0
T1WZGhmuXnxWfRRLGBCWtlSB2t6r5+H14zdfLUy0Cbe0nHQ/H0+horbu5SRKgrpn
-----END RSA PRIVATE KEY-----
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
