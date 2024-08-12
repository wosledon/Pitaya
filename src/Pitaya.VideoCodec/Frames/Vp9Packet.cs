using Pitaya.Buffers;
using Pitaya.Core;

namespace Pitaya.VideoCodec.Frames;

public struct Vp9Packet : IPitayaPayloadPacket
{
    public Vp9Packet()
    {
    }

    public byte[] Origin { get; set; } = [];
}
