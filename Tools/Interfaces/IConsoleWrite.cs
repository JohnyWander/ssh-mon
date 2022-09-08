using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssh_mon.Tools.Interfaces
{
   public interface IConsoleWrite
    {
         public void color_consoleWriteLine(ConsoleColor consoleColor, string text);
    }
}
