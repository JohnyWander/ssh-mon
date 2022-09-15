using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
namespace ssh_mon.Modules
{
    public class LoadAssemblies
    {
        public List<Module> Module_List { private set; get; } = new List<Module>();

        public LoadAssemblies()
        {
            string[] paths_to_modules = Directory.GetFiles("modules");

            foreach (string path in paths_to_modules)
            {
                if (path.Contains(".dll"))
                {
                   
                    Module_List.Add(new Module(path));

                }



            }



        }


    }

    


    public class Module
    {
        public delegate bool BoolDelegateToDerive(); // as CreateDelegate needs type to be derived from delegate
        public Delegate LoadedCorrectly{ private set; get; }
        /// <summary>
        /// Checks if module was loaded correctly
        /// </summary>
        /// <returns>Bool</returns>
        /// ///////////////////////////////////////////////////
        
       

        public delegate IDictionary<int, string> IDictionaryToDerive();
        public Delegate GET_commands_to_execute { private set; get;}
        /// <summary>
        /// Gets commands to execute from module dll
        /// </summary>
        /// <returns>IDictionary with commands</returns>


        private delegate bool BoolWith2Args(object key, object value);
        public Delegate PUSH_commands_result { private set; get; } // sends output to the module for checks

        ///////////////////////////////
       
        public Delegate GET_fix_commands { private set; get; }




        public Module(string path)
        {
            Assembly module =  Assembly.LoadFrom(path); // Loads assembly module
            Type _MODULE_MAIN_CLASS_ = module.GetType("Module.MODULE_MAIN_CLASS"); // Gets main class from module
            object activaror = Activator.CreateInstance(_MODULE_MAIN_CLASS_); // Creates instance of the class

            // GETTING LOADED CORRETLY BOOL
           MethodInfo loaded_correctly= _MODULE_MAIN_CLASS_.GetMethod("GET_Loaded_correctly");
           LoadedCorrectly= Delegate.CreateDelegate(typeof(BoolDelegateToDerive), activaror, loaded_correctly);
            /////////////////
            ///
            object result = LoadedCorrectly.DynamicInvoke();
            if ((bool)result) { Program.ConsoleWrite.color_consoleWriteLine(ConsoleColor.Green,GUI.Language_strings.language_strings["module_load_correct"]); }
            else {Program.ConsoleWrite.color_consoleWriteLine(ConsoleColor.Red,GUI.Language_strings.language_strings["module_load_fail"]); };
            
            
               
            ////// IF module was loaded correctly -- creating all other delegates in order to make module operational
            if ((bool)result)
            {
                //// GET_commmands_to_execute -- gets commands to execute by tests class
                MethodInfo get_commands = _MODULE_MAIN_CLASS_.GetMethod("GET_commands_to_execute");
                GET_commands_to_execute = Delegate.CreateDelegate(typeof(IDictionaryToDerive), activaror, get_commands);

                //// Push commands_results -- sends output of commands to the module library
                MethodInfo push_commands_results = _MODULE_MAIN_CLASS_.GetMethod("PUSH_command_results");
                PUSH_commands_result = Delegate.CreateDelegate(typeof(BoolWith2Args), activaror, push_commands_results,true);
                bool ok= (bool)PUSH_commands_result.DynamicInvoke(new object[] { 1,"test" });

                /// Get_commands_results --- DEBUG not used by program
                /// 
                //  MethodInfo get_results = _MODULE_MAIN_CLASS_.GetMethod("GET_command_results");
                // object gr = get_results.Invoke(activaror, null);
                //  IDictionary<int, string> debug_after_push = (IDictionary<int, string>)gr;


                /// Get_fix_commands
                /// 

                MethodInfo get_fix_commands = _MODULE_MAIN_CLASS_.GetMethod("GET_fix_commands");
                GET_fix_commands = Delegate.CreateDelegate(typeof(IDictionaryToDerive), activaror, get_fix_commands);

            }












        }





    }


}