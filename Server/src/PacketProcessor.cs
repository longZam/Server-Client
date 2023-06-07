using System.Diagnostics;
using System.Net;
using System.Threading.Tasks.Dataflow;
using log4net;


// 수신 받은 패킷에 대한 처리
public class RUDP_PacketProcessor
{
    private bool isRunning;
    private Task? processTask;

    private readonly BufferBlock<RUDP_Packet> msgBuffer;
    private readonly Dictionary<RUDP_PacketHeader.Flag, Action<RUDP_Packet>> packetHandlers;
    private readonly ClientSessionManager sessionManager;
    private readonly ILog log;
    private readonly CancellationTokenSource destroyTokenSource;
    


    public RUDP_PacketProcessor(ILog log, ClientSessionManager sessionManager)
    {
        this.isRunning = false;
        this.msgBuffer = new BufferBlock<RUDP_Packet>();
        this.packetHandlers = new Dictionary<RUDP_PacketHeader.Flag, Action<RUDP_Packet>>(sizeof(RUDP_PacketHeader.Flag));
        this.sessionManager = sessionManager;
        this.log = log;
        this.destroyTokenSource = new CancellationTokenSource();
    }

    public void Run()
    {
        if (isRunning)
            throw new AlreadyRunningException();
        
        isRunning = true;
        processTask = Process(destroyTokenSource.Token);
    }

    public void Destroy()
    {
        if (!isRunning)
            return;
        
        isRunning = false;
        destroyTokenSource.Cancel();
    }

    public void RegistPacketHandler(RUDP_PacketHeader.Flag flag, Action<RUDP_Packet> handler)
    {
        if (packetHandlers.ContainsKey(flag))
            throw new AlreadyRegistException();
        
        packetHandlers[flag] = handler;
    }

    public void InsertPacket(RUDP_Packet packet)
    {
        msgBuffer.Post(packet);
    }

    private async Task Process(CancellationToken token)
    {
        while (isRunning)
        {
            try
            {
                // 다른 스레드에서 넣은 패킷 꺼내기
                RUDP_Packet packet = await msgBuffer.ReceiveAsync(token);

                if (packetHandlers.ContainsKey(packet.Header.flag))
                {
                    packetHandlers[packet.Header.flag](packet);
                }
                else
                {
                    log.DebugFormat("Unhandled packet, Packet Flag {0}", packet.Header.flag);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
            catch (Exception e)
            {
                log.ErrorFormat("Unhandled exception: {0}", e);
            }
        }
    }
}

#region Exceptions

public class AlreadyRunningException : Exception
{

}

public class AlreadyRegistException : Exception
{

}

#endregion