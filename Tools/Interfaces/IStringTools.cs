﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssh_mon.Tools.Interfaces
{
     interface IStringTools 
     {
     string left(string text, int count);
     string right(string text, int count);

     void draw_hash();
     }
}
