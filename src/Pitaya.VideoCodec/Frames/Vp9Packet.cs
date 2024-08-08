using Pitaya.Buffers;

namespace Pitaya.VideoCodec.Frames;

public struct Vp9Packet : IPitayaPlayloadPacket
{
    public Vp9Packet()
    {
    }

    public byte[] Origin { get; set; } = [];
}
