using System.Buffers;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Core;


public class RudpPacketReceiver : IDisposable
{
    private readonly Thread thread;
    private readonly Socket socket;
    private readonly Dictionary<RudpPacketHeader.PacketType, RudpPacketHandler> handlerMap;

    private bool isRunning;
    private bool disposedValue;


    public RudpPacketReceiver(Socket socket)
    {
        this.socket = socket;
        this.thread = new Thread(PacketReceive);
        this.handlerMap = new Dictionary<RudpPacketHeader.PacketType, RudpPacketHandler>();
    }

    public void Run()
    {
        Debug.Assert(!isRunning);

        isRunning = true;
        thread.Start();
    }

    public void Pause()
    {
        Debug.Assert(isRunning);

        isRunning = false;
        thread.Join();
    }

    public void RegistPacketHandler(RudpPacketHandler handler)
    {
        
    }

    private void PacketReceive()
    {
        byte[] buffer = new byte[65535];

        while (isRunning)
        {
            int packetLength = socket.Receive(buffer);
            int headerLength = Marshal.SizeOf<RudpPacketHeader>();
            int bodyLength = packetLength - headerLength;
            RudpPacketHeader header = GetHeaderFromPacket(buffer);

            // 헤더로 전달받은 길이와 실제 패킷 길이가 불일치
            // 오류일 수 있으나, 보통 패킷 변조일 확률이 높음
            if (header.bodyLength != bodyLength)
            {
                Console.WriteLine("유효하지 않은 패킷 감지");
                continue;
            }

            if (!handlerMap.ContainsKey(header.type))
            {
                Console.WriteLine($"등록되지 않은 핸들링 '{header.type}' 사용됨");
                continue;
            }

            handlerMap[header.type].Handle(buffer.AsSpan(headerLength, bodyLength));
        }
    }

    private RudpPacketHeader GetHeaderFromPacket(byte[] packet, int offset = 0)
    {
        int size = Marshal.SizeOf<RudpPacketHeader>();

        IntPtr ptr = Marshal.AllocHGlobal(size); // 비관리 메모리 할당
        Marshal.Copy(packet, offset, ptr, size); // 비관리 메모리에 패킷 복사
        RudpPacketHeader result = Marshal.PtrToStructure<RudpPacketHeader>(ptr); // 구조체로 변환
        Marshal.FreeHGlobal(ptr); // 비관리 메모리 해제
        
        return result;
    }

    #region Dispose

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                socket.Dispose();
            }

            disposedValue = true;
        }
    }


    // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
    // ~UdpPacketReceiver()
    // {
    //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}