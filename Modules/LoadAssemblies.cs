using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
namespace ssh_mon.Modules
{
    public  class LoadAssemblies
    {

       
        
        public LoadAssemblies()
        {

            Assembly module = Assembly.LoadFrom("modules\\Mount_Check.dll");

            Type MODULE_MAIN_CLASS = module.GetType("Module.MODULE_MAIN_CLASS");
           var method= MODULE_MAIN_CLASS.GetMethod("x");
            object instance = Activator.CreateInstance(MODULE_MAIN_CLASS);

            object result = method.Invoke(instance, null);



        }





    }

   

}