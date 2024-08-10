using Pitaya.Buffers;

namespace Pitaya.VideoCodec.Frames;

public struct H265Packet : IPitayaPayloadPacket
{
    public H265Packet()
    {
    }

    public byte[] Origin { get; set; } = [];
}
