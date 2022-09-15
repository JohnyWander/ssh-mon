namespace ssh_mon.Resources
{
    public static class Config_strings
    {
        public static string default_conf { get; } = @"[Language] - ENG Supported for now
ENG
[Timers]
cpu_ram_check timer(ms)=2000
[debug]
show_module_debug=false
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
module_load_correct=Correctly loaded module
module_load_failt=Failed to load specified module
[Second_menu]
select_server_menu=1. Select server
deselect_server_menu=2. Deselect server
restart_gui=3. Restart GUI
execute_fix_command_menu=f - execute fix command for selected server
select_server=Please enter server ID
deselect_server=Please enter server ID to deselect
selected_server=SELECTED
[Warns]
already_encrypted=Servers directory seems to be already encrypted! Cancelling...
not_encrypted=Servers directory seems to not be encrypted! Cancelling...
execute_fix_command=Try to Execute fix command?
[Exceptions]
server_config_error=Something is not ok with the server configuration file ! :
";

    }
}
