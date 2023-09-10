


public abstract class RudpPacketHandler
{
    public abstract void Handle(ReadOnlySpan<byte> packet);
}