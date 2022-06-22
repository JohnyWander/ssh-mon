using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace ssh_mon.Tools.Interfaces
{
    interface IKeyGen 
    {
        RSA keypair_gen();
    }
}
