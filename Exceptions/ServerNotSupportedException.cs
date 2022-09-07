using System;


namespace ssh_mon.Exceptions
{

    public class ServerNotSupportedException : Exception
    {
        public ServerNotSupportedException(string message) : base(message)
        {

        }



    }


    public class NotConnectedException : Exception
    {
        public NotConnectedException(string message) : base(message)
        {

        }
    }
}
