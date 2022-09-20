using Microsoft.VisualBasic;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssh_mon.SSH.Tests
{
    internal class ModuleTests
    {
        List<Task> jobs = new List<Task>();

        SshClient client;
        List<Modules.Module> Modules;
        int server_id;
        IDictionary<int, string> FixCommands;
        bool[] enabled_fix_command;

        public ModuleTests(List<Modules.Module> Module_List,SshClient client_instance,int server_ID)
        {
            this.Modules = Module_List;
            this.client = client_instance;
            this.server_id = server_ID; 
            Task asynchronicity_handler = Task.Run(handle_async);
            asynchronicity_handler.GetAwaiter().GetResult();

        }

        private async Task handle_async()
        {
            
            foreach(var Module in Modules)
            {
                jobs.Add(Task.Run(async()=>job(Module)));
            }

            await Task.WhenAll(jobs);

        }



        private async Task job(Modules.Module module)
        {
            
            await Task.Delay(3000);
            object delay=module.GET_Delay.DynamicInvoke();
            object CTE = module.GET_commands_to_execute.DynamicInvoke();
            IDictionary<int, string> CommandsToExecute = (IDictionary<int, string>)CTE;

            while (!Program.cancel_all.IsCancellationRequested) 
            {

                foreach (var command in CommandsToExecute)
                {
                    var cmd = client.CreateCommand(command.Value);
                    string cmd_out = cmd.Execute(); // Executing test command
                    module.PUSH_commands_result.DynamicInvoke(command.Key,cmd_out); // sending results to the module 
              

                 }

                object Test_passed = module.RUNTest.DynamicInvoke();

                if ((bool)Test_passed)
                {
                    GUI.Default_GUI.module_clear_error(new object[] { server_id });
                }
                else
                {
                    object errstring = module.GET_err_messege.DynamicInvoke();
                    object module_name = module.NAME;
                    errstring += " Source: " + module_name;
                    GUI.Default_GUI.insert_error_from_module(new object[] {errstring,server_id});

                    FixCommands = (IDictionary<int,string>)module.GET_fix_commands.DynamicInvoke();

                    Execute_fix_commands_listener(FixCommands, server_id);

                }



                await Task.Delay((int)delay);

            }
        }
        

        private void Execute_fix_commands_listener(IDictionary<int,string> fixCommands,int server_ID)
        {
            

                GUI.Default_GUI.ExecuteFixCOMMAND = () =>
                {

                    enabled_fix_command = GUI.Default_GUI.selection_indicator;

                    int pred_id = 0;
                    Parallel.ForEach(enabled_fix_command, action =>
                    {
                        if (action == true)
                        {
                            if (fixCommands.Count > 0)
                            {
                                foreach (KeyValuePair<int, string> pair in fixCommands)
                                {
                                    var command = this.client.CreateCommand(pair.Value);
                                    var result = command.Execute();
                                   // Console.WriteLine(result);
                                }

                            }
                        }
                        pred_id++;
                    });

                   // Console.WriteLine("FIXCOMMAND");

                };


                    
               
                
            



        }



        
    }
}
