using System.Net;
using System.Collections.Concurrent;


public class ClientSessionManager
{
    private readonly ConcurrentDictionary<EndPoint, ClientSession> sessions;


    public ClientSessionManager()
    {
        sessions = new ConcurrentDictionary<EndPoint, ClientSession>();
    }

    public ClientSession GetSession(EndPoint client)
    {
        if (!sessions.ContainsKey(client))
            sessions[client] = new ClientSession(client);
        
        return sessions[client];
    }
}