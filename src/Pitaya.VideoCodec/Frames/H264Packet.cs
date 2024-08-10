using Pitaya.BasicType.BitTypes;
using Pitaya.Buffers;
using System.Text.Json;

namespace Pitaya.VideoCodec.Frames;

public struct H264Packet : IPitayaPayloadPacket, IPitayaPacket<H264Packet>, IPitayaAnalyzable
{
    public H264Packet()
    {
    }

    public byte[] Origin { get; set; } = [];

    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        throw new NotImplementedException();
    }

    public H264Packet Decode(ref BufferReader reader)
    {
        throw new NotImplementedException();
    }

    public void Encode(ref BufferWriter writer)
    {
        throw new NotImplementedException();
    }

    public struct NALUnitPacket : IPitayaPacket<NALUnitPacket>, IPitayaAnalyzable
    {
        public NALUnitPacket()
        {
        }

        public Bit ForbiddenZoreBit { get; set; } = Bit.Zero;
        public Bit2 NalRefIdc { get; set; } = Bit2.Zero;
        /// <summary>
        /// bit 5
        /// </summary>
        public byte NalUnitType { get; set; }

        public byte[] Origin { get; set; } = [];

        public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
        {
            throw new NotImplementedException();
        }

        public NALUnitPacket Decode(ref BufferReader reader)
        {
            throw new NotImplementedException();
        }

        public void Encode(ref BufferWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
