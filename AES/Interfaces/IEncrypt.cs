using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssh_mon.AES.Interfaces
{
    interface IEnryptDecrypt
    {
        public string Encrypt(string value,string password);
        public string Decrypt(string value, string password);
    }
}
