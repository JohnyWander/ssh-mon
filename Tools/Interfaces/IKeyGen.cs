using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace bpp_admin.Tools.Interfaces
{
    interface IKeyGen
    {
        RSA keypair_gen();
    }
}
