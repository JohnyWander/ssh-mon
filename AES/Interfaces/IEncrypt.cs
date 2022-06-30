namespace ssh_mon.AES.Interfaces
{
    interface IEnryptDecrypt
    {
        public string Encrypt(string value, string password);
        public string Decrypt(string value, string password);
    }
}
