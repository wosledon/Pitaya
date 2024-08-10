using Pitaya.Buffers;

namespace Pitaya.VideoCodec.Frames;

public struct Vp8Packet : IPitayaPayloadPacket
{
    public Vp8Packet()
    {
    }

    public byte[] Origin { get; set; } = [];
}
