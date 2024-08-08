using Pitaya.Buffers;

namespace Pitaya.VideoCodec.Frames;

public struct MpegTsPacket : IPitayaPlayloadPacket
{
    public MpegTsPacket()
    {
    }

    public byte[] Origin { get; set; } = [];
}
