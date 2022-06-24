﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
namespace ssh_mon.Modules
{
    public static class LoadAssemblies
    {

       public static IDictionary<int,Modules> ModuleAssembly= new Dictionary<int,Modules>();
      public  static void run()
        {

            string[] module_assemblies = Directory.GetFiles("modules");

            Console.WriteLine();
            int ite = 0;
            foreach(string assembly in module_assemblies)
            {
                ModuleAssembly.Add(ite, new Modules(assembly));
                ite++;
            }






        }


    }


    public class Modules
    {
        private Type module_main_class;
        private object activator;
        public Modules(string assembly)
        {
            //  Console.WriteLine(assembly);
            string module_namespace = assembly.Split("\\")[1].Replace(".dll", string.Empty);
            Assembly module = Assembly.LoadFrom(assembly);
            module_main_class = module.GetType(module_namespace + ".Module");



            //MethodInfos

           
            
           
           


            activator = Activator.CreateInstance(module_main_class);
           
          

           

        }


        public bool __get_test_failed()
        {
            var get_test_failed = module_main_class.GetMethod("get_Test_failed");
            object result = get_test_failed.Invoke(activator, null);
            return (bool)result;
        }
        public string __get_Error_messege()
        {
            var get_Error_messege = module_main_class.GetMethod("get_Error_messege");
            object result = get_Error_messege.Invoke(activator, null);
            return (string)result;
        }
        public List<string> __get_names()
        {
            var get_names = module_main_class.GetMethod("get_names");
            object result = get_names.Invoke(activator, null);
            return (List<string>)result;
        }
        public List<string[]> __get_commands_and_outputs()
        {
            var get_commands_and_outputs = module_main_class.GetMethod("get_commands_and_outputs");
            object result = get_commands_and_outputs.Invoke(activator, null);
            return (List<string[]>)result;
        }
        public void set_output(string[] commands_outputs)
        {
            var set_output = module_main_class.GetMethod("set_output", new Type[] { typeof(string[]) });
            object result = set_output.Invoke(activator, commands_outputs);
        }


    }









}