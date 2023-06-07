using System.Net;


public class ClientSession
{
    public EndPoint ClientIP { get; private set; }


    public ClientSession(EndPoint clientIP)
    {
        this.ClientIP = clientIP;
    }
}