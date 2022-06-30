namespace ssh_mon.Resources
{
    public static class Config_strings
    {
        public static string default_conf { get; } = @"[Language] - ENG Supported for now
ENG
[Timers]
cpu_ram_check timer(ms)=2000
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
encrypt_server_dir=2. Encrypt Servers directory
decrypt_server_dir=3. Decrypt Servers directory
input_password=Please enter the password
input_password_confirm=Please CONFIRM the password
input_password_no_match= Passwords do not match! Cancelling...
encryption_success=Encryption was successful
encryption_fail=Encryption fail
decryption_success=Decryption was successful
decryption_fail=Decryption failed! Wrong Password?
press_any=Press any key to restart
[Second_menu]
select_server=Please enter server ID
[Warns]
already_encrypted=Servers directory seems to be already encrypted! Cancelling...
not_encrypted=Servers directory seems to not be encrypted! Cancelling...
[Exceptions]
server_config_error=Something is not ok with the server configuration file ! :
";

    }
}
