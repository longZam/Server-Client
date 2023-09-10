using System.Runtime.InteropServices;

namespace Core;


[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct RudpPacketHeader
{
    public enum PacketType : uint
    {

    }

    [FieldOffset(0)]
    public uint packetId;

    [FieldOffset(sizeof(uint))]
    public uint senderId;
    
    [FieldOffset(sizeof(uint) + sizeof(uint))]
    public PacketType type;
    
    [FieldOffset(sizeof(uint) + sizeof(uint) + sizeof(PacketType))]
    public uint bodyLength;
}