using System.Security.Cryptography;
namespace ssh_mon.Tools.Interfaces
{
    interface IKeyGen
    {
        RSA keypair_gen();
    }
}
