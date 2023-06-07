using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using log4net;


public class RUDP_SocketListener
{
    private readonly ILog logger;
    private readonly ushort port;
    private readonly ClientSessionManager sessionManager;
    private readonly CancellationTokenSource cancelTokenSource;
    private readonly List<RUDP_PacketProcessor> processors;
    // 수신자에 대한 정보가 필요하다. 그렇다고 패킷에 수신자 정보를 넣을 수는 없다.
    // 패킷을 한 번 래핑해서 요청한다는 의미의 클래스를 만들어보자.
    // 근데 그 클래스도 GC에 수거될 수 있어 주의가 필요한데...
    // 구조체 써도 괜찮을 지도...
    private readonly BufferBlock<RUDP_Packet> msgBuffer;
    private bool isRunning;


    public RUDP_SocketListener(ILog logger, ushort port, ClientSessionManager sessionManager)
    {
        this.logger = logger;
        this.port = port;
        this.sessionManager = sessionManager;
        this.cancelTokenSource = new CancellationTokenSource();
        this.processors = new List<RUDP_PacketProcessor>();
        this.msgBuffer = new BufferBlock<RUDP_Packet>();
        this.isRunning = false;
    }

    public void Run()
    {
        if (isRunning)
            throw new AlreadyRunningException();

        var endPoint = new IPEndPoint(IPAddress.Any, port);
        Socket socket = new Socket(endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(endPoint);
        isRunning = true;
        
        logger.Info("Started running PacketListener");

        Task.WaitAll(ProcessReceive(socket, cancelTokenSource.Token),
                     ProcessSend(socket, cancelTokenSource.Token));
    }

    public void Destroy()
    {
        if (!isRunning)
            return;
        
        isRunning = false;
        cancelTokenSource.Cancel();
        logger.Info("Stopping PacketListener");
    }

    public void RegistPacketProcessor(RUDP_PacketProcessor processor)
    {
        // todo: exception 클래스 만들자
        if (processors.Contains(processor))
            throw new Exception();
        
        processors.Add(processor);
    }

    public void Send()
    {

    }

    private async Task ProcessReceive(Socket socket, CancellationToken token)
    {
        IPEndPoint any = new IPEndPoint(IPAddress.Any, 0);
        byte[] buffer = new byte[1024];

        while (isRunning)
        {
            try
            {
                var result = await socket.ReceiveMessageFromAsync(buffer, any, token); // 패킷 수신
                RUDP_Packet packet = RUDP_Packet.Deserialize(buffer); // 패킷 역직렬화
                ClientSession session = sessionManager.GetSession(result.RemoteEndPoint); // 세션 얻어오기

                foreach (var processor in processors)
                {
                    processor.InsertPacket(packet);
                }
            }
            catch
            {

            }
        }
    }

    private async Task ProcessSend(Socket socket, CancellationToken token)
    {
        while (isRunning)
        {
            try
            {
                var msg = await msgBuffer.ReceiveAsync(token);
            }
            catch
            {

            }
        }
    }
}