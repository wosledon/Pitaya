using Pitaya.Buffers;

namespace Pitaya.VideoCodec.Frames;

public struct H265Packet : IPitayaPlayloadPacket
{
    public H265Packet()
    {
    }

    public byte[] Origin { get; set; } = [];
}
