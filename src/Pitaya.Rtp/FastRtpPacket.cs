using Pitaya.Buffers;
using Pitaya.Buffers.Extensions;
using Pitaya.Core;
using System.Text.Json;

namespace Pitaya.Rtp;

/// <summary>
/// FastRtpPacket
/// </summary>
/// <remarks>
/// 快速RTP协议, 仅解包RTP中几个较为重要的字段，不包含其他内容
/// </remarks>
public struct FastRtpPacket : IPitayaPacket<FastRtpPacket>, IPitayaAnalyzable
{
    public FastRtpPacket()
    {
    }

    public byte PT { get; set; }
    public uint SSRC { get; set; }
    public ushort SequenceNumber { get; set; }
    public uint Timestamp { get; set; }
    public byte[] Origin { get; set; } = [];

    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        var origin = reader.OriginBuffer.ToArray();

        writer.WriteNumber(nameof(PT), origin[1]);
        writer.WriteNumber(nameof(SSRC), BitConverter.ToUInt32(origin, 8));
        writer.WriteNumber(nameof(SequenceNumber), BitConverter.ToUInt16(origin, 2));
        writer.WriteNumber(nameof(Timestamp), BitConverter.ToUInt32(origin, 4));
        writer.WriteString(nameof(Origin), origin.ToHexString());

    }

    public FastRtpPacket Decode(ref BufferReader reader)
    {
        var packet = new FastRtpPacket();
        
        packet.Origin = reader.OriginBuffer.ToArray();

        packet.PT = Origin[1];
        packet.SSRC = BitConverter.ToUInt32(Origin, 8);
        packet.SequenceNumber = BitConverter.ToUInt16(Origin, 2);
        packet.Timestamp = BitConverter.ToUInt32(Origin, 4);
        
        return packet;
    }

    public void Encode(ref BufferWriter writer)
    {
        Origin[1] = PT;
        BitConverter.TryWriteBytes(Origin.AsSpan(8), SSRC);
        BitConverter.TryWriteBytes(Origin.AsSpan(2), SequenceNumber);
        BitConverter.TryWriteBytes(Origin.AsSpan(4), Timestamp);

        writer.WriteArray(Origin);
    }
}