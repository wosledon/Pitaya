using System.Text.Json;
using Pitaya.Buffers;
using Pitaya.Buffers.Extensions;
using Pitaya.Core;

namespace Pitaya.Flv;

[Obsolete("use tags.", true)]
public struct FlvTagPacket : IPitayaPacket<FlvTagPacket>, IPitayaAnalyzable
{
    public FlvTagPacket()
    {
    }

    public byte TagType { get; set; }

    /// <summary>
    /// 3字节, 表示数据大小
    /// </summary>
    public uint DataSize { get; set; }

    /// <summary>
    /// 3字节, 表示时间戳
    /// </summary>
    public uint Timestamp { get; set; }

    public byte TimestampExtended { get; set; }

    /// <summary>
    /// 3字节, 表示流ID, 通常为0
    /// </summary>
    public uint StreamId { get; set; } = 0;

    public byte[] TagData { get; set; } = [];

    public uint PreviousTagSize { get; private set; } = 0;


    public byte[] Origin { get; set; } = [];

    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        writer.WriteNumber(nameof(TagType), TagType);
        writer.WriteNumber(nameof(DataSize), DataSize);
        writer.WriteNumber(nameof(Timestamp), Timestamp);
        writer.WriteNumber(nameof(TimestampExtended), TimestampExtended);
        writer.WriteNumber(nameof(StreamId), StreamId);
        writer.WriteString(nameof(TagData), TagData.ToHexString());
        writer.WriteNumber(nameof(PreviousTagSize), PreviousTagSize);
    }

    public FlvTagPacket Decode(ref BufferReader reader)
    {
        Origin = reader.OriginBuffer.ToArray();

        TagType = reader.ReadByte();
        var dataSizeBytes = reader.ReadArray(3);
        DataSize = (uint)(dataSizeBytes[0] << 16 | dataSizeBytes[1] << 8 | dataSizeBytes[2]);
        var timestampBytes = reader.ReadArray(3);
        Timestamp = (uint)(timestampBytes[0] << 16 | timestampBytes[1] << 8 | timestampBytes[2]);
        TimestampExtended = reader.ReadByte();
        var streamIdBytes = reader.ReadArray(3);
        StreamId = (uint)(streamIdBytes[0] << 16 | streamIdBytes[1] << 8 | streamIdBytes[2]);
        TagData = reader.ReadArray((int)DataSize).ToArray();
        PreviousTagSize = reader.ReadUInt32();

        return this;
    }

    public void Encode(ref BufferWriter writer)
    {
        writer.WriteByte(TagType, out _);
        writer.WriteArray(new byte[] { (byte)(DataSize >> 16), (byte)(DataSize >> 8), (byte)DataSize });
        writer.WriteArray(new byte[] { (byte)(Timestamp >> 16), (byte)(Timestamp >> 8), (byte)Timestamp });
        writer.WriteByte(TimestampExtended, out _);
        writer.WriteArray(new byte[] { (byte)(StreamId >> 16), (byte)(StreamId >> 8), (byte)StreamId });
        writer.WriteArray(TagData, out int position);
        writer.WriteUInt32((uint)(position + 1), out _);
    }
}