using Pitaya.Buffers;

namespace Pitaya.VideoCodec.Frames;

public struct H264Packet : IPitayaPlayloadPacket
{
    public H264Packet()
    {
    }

    public byte[] Origin { get; set; } = [];
}
