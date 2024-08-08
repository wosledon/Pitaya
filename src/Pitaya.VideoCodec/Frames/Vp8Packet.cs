using Pitaya.Buffers;

namespace Pitaya.VideoCodec.Frames;

public struct Vp8Packet : IPitayaPlayloadPacket
{
    public Vp8Packet()
    {
    }

    public byte[] Origin { get; set; } = [];
}
