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

        public delegate IDictionary<int, string> IDictionraryToDerive();
       // public Delegate 


        public Module(string path)
        {
            Assembly module =  Assembly.LoadFrom(path); // Loads assembly module
            Type _MODULE_MAIN_CLASS_ = module.GetType("Module.MODULE_MAIN_CLASS"); // Gets main class from module
            object activaror = Activator.CreateInstance(_MODULE_MAIN_CLASS_); // Creates instance of the class

            // GETTING LOADED CORRETLY BOOL
           MethodInfo loaded_correctly= _MODULE_MAIN_CLASS_.GetMethod("GET_Loaded_correctly");
           LoadedCorrectly= Delegate.CreateDelegate(typeof(BoolDelegateToDerive), activaror, loaded_correctly);
         /////////////////
         

        }





    }


}