using System.Net.Sockets;
using log4net;


public class MainServer
{
    private readonly ServerSettings settings;
    private readonly ILog logger;
    private readonly RUDP_SocketListener packetListener;
    private readonly RUDP_PacketProcessor packetProcessor;
    private readonly ClientSessionManager sessionManager;


    public MainServer(ServerSettings settings, ILog logger)
    {
        this.settings = settings;
        this.logger = logger;
        this.sessionManager = new ClientSessionManager();
        this.packetListener = new RUDP_SocketListener(logger, settings.Port, sessionManager);
        this.packetProcessor = new RUDP_PacketProcessor(logger, sessionManager);

        this.packetListener.RegistPacketProcessor(packetProcessor);
        this.packetProcessor.RegistPacketHandler(RUDP_PacketHeader.Flag.SYN, (packet) =>
        {
            logger.Info("Receive SYN Packet!");
        });
    }
    ~MainServer()
    {
        
    }

    public void Run()
    {
        try
        {
            logger.InfoFormat("RUN! PORT: {0}", settings.Port);

            packetListener.Run();
            packetProcessor.Run();

            while (true) {}
        }
        catch
        {
            
        }
    }
}