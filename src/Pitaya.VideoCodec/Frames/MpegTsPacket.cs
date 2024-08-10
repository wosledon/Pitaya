using Pitaya.Buffers;

namespace Pitaya.VideoCodec.Frames;

public struct MpegTsPacket : IPitayaPayloadPacket
{
    public MpegTsPacket()
    {
    }

    public byte[] Origin { get; set; } = [];
}
