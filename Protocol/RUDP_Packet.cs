using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.InteropServices;

interface IReferencable<T> where T: IReferencable<T> {
    Reference<T> Acquire ();
    void Release();
}

struct Reference<T>: IDisposable where T: IReferencable<T>
{
    public readonly T Item;
    public Reference(T item) { Item = item; _released = false; }
    public void Dispose() { if (! _released ) { _released = true; Item.Release(); } }
    private bool _released;
}


[StructLayout(LayoutKind.Sequential)]
public struct RUDP_PacketHeader
{
    public static readonly int HEADER_SIZE = Marshal.SizeOf<RUDP_PacketHeader>();

    public enum Flag : ushort
    {
        SYN = 1 << 0,
        ACK = 1 << 1,
    }

    public uint packetNumber;
    public ushort bodySize;
    public Flag flag;

    public RUDP_PacketHeader()
    {
        bodySize = 0;
        flag = 0;
    }
}


// todo: 이 클래스를 어떻게 풀링할 방법이 있는 지 생각해야 한다.
public class RUDP_Packet : IReferencable<RUDP_Packet>
{
    // MTU가 약 1500임을 감안한다.
    // 이걸 사용해도 메모리가 pinned되어 단편화가 발생한다.
    // LOH에 배치될 수 있는 대형 단일 배열을 기반으로 한 arrayPool이 필요하다.
    private static readonly ArrayPool<byte> arrayPool = ArrayPool<byte>.Create(2048, 1024);

    public RUDP_PacketHeader Header { get; private set; }
    public ReadOnlySpan<byte> Body 
    {
        get
        {
            return body.AsSpan(RUDP_PacketHeader.HEADER_SIZE, Header.bodySize);
        }
    }

    private event Action onRelease;

    [AllowNull]
    private byte[] body;
    private int refCount;
    private bool isActive;


    public RUDP_Packet(Action onRelease)
    {
        this.onRelease = onRelease;
        this.isActive = false;
        this.refCount = 0;
    }

    public void Activate(RUDP_PacketHeader header, Span<byte> body)
    {
        // todo: exception 클래스 만들어!
        if (isActive)
            throw new Exception();

        this.Header = header;
        this.body = arrayPool.Rent(header.bodySize);

        body.CopyTo(this.body);
    }

    public void Deactivate()
    {
        // todo: exception 클래스 만들어!
        if (!isActive)
            throw new Exception();

        arrayPool.Return(body);
    }

    ~RUDP_Packet()
    {

    }


    Reference<RUDP_Packet> IReferencable<RUDP_Packet>.Acquire()
    {
        ++refCount;
        return new Reference<RUDP_Packet>(this);
    }

    public void Release()
    {
        if (--refCount <= 0)
            onRelease();
    }

    public int Serialize(byte[] buffer, int offset = 0)
    {
        return Serialize(this, buffer, offset);
    }

    public static int Serialize(RUDP_Packet packet, byte[] buffer, int offset = 0)
    {
        offset += StructToBytes<RUDP_PacketHeader>(packet.Header, buffer, offset);
        Array.Copy(packet.body, 0, buffer, offset, packet.Header.bodySize);

        return offset;
    }

    public static RUDP_Packet Deserialize(byte[] data)
    {
        RUDP_PacketHeader header = BytesToStruct<RUDP_PacketHeader>(data); // 패킷에서 헤더 추출
        Span<byte> body = new Span<byte>(data, RUDP_PacketHeader.HEADER_SIZE, header.bodySize); // 패킷에서 바디 추출

        // todo: 풀에서 꺼내!!!
        return new RUDP_Packet(() => {});
    }

    private static T BytesToStruct<T>(byte[] bytes) where T : struct
    {
        int structSize = Marshal.SizeOf<T>(); // 구조체 사이즈를 sizeof(T)로 체크할 수 없음
        var ptr = Marshal.AllocHGlobal(structSize); // 비관리 메모리 할당
        Marshal.Copy(bytes, 0, ptr, structSize); // 비관리 메모리에 값 복사
        T result = Marshal.PtrToStructure<T>(ptr); // 구조체로 변환
        Marshal.FreeHGlobal(ptr); // 비관리 메모리 해제

        return result;
    }

    private static int StructToBytes<T>(T data, byte[] buffer, int offset) where T : struct
    {
        int size = Marshal.SizeOf<T>();

        var ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr<T>(data, ptr, false);
        Marshal.Copy(ptr, buffer, offset, size);
        Marshal.FreeHGlobal(ptr);

        return size;
    }

}
