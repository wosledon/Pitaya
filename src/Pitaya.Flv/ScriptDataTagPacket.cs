using System.Text.Json;
using Pitaya.Buffers;
using Pitaya.Buffers.Extensions;
using Pitaya.Core;

namespace Pitaya.Flv;

public struct ScriptDataTagPacket : IFlvTagPacket
{
    public ScriptDataTagPacket()
    {
    }

    public const byte TagType = 18;

    /// <summary>
    /// 3字节, 表示数据大小
    /// </summary>
    public uint DataSize { get; set; }

    /// <summary>
    /// 3字节, 表示时间戳
    /// </summary>
    public uint Timestamp { get; set; }

    public byte TimestampExtended { get; set; }

    public uint StreamId { get; set; } = 0;


    public byte[] AMFData { get; set; } = [];

    /// <summary>
    /// End of the tag data
    /// </summary>
    public uint PreviousTagSize { get; private set; } = 0;

    public byte[] Origin { get; set; } = [];

    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        writer.WriteNumber(nameof(TagType), TagType);
        writer.WriteNumber(nameof(DataSize), DataSize);
        writer.WriteNumber(nameof(Timestamp), Timestamp);
        writer.WriteNumber(nameof(TimestampExtended), TimestampExtended);
        writer.WriteNumber(nameof(StreamId), StreamId);
        writer.WriteString(nameof(AMFData), AMFData.ToHexString());
        writer.WriteNumber(nameof(PreviousTagSize), PreviousTagSize);
    }

    public ScriptDataTagPacket Decode(ref BufferReader reader)
    {
        Origin = reader.OriginBuffer.ToArray();

        if (reader.ReadByte() != TagType)
        {
            throw new InvalidOperationException("Tag type mismatch");
        }

        var dataSizeBytes = reader.ReadArray(3);
        DataSize = (uint)(dataSizeBytes[0] << 16 | dataSizeBytes[1] << 8 | dataSizeBytes[2]);
        var timestampBytes = reader.ReadArray(3);
        Timestamp = (uint)(timestampBytes[0] << 16 | timestampBytes[1] << 8 | timestampBytes[2]);
        TimestampExtended = reader.ReadByte();
        var streamIdBytes = reader.ReadArray(3);
        StreamId = (uint)(streamIdBytes[0] << 16 | streamIdBytes[1] << 8 | streamIdBytes[2]);
        AMFData = reader.ReadArray((int)DataSize).ToArray();
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
        writer.WriteArray(AMFData);
        writer.WriteUInt32(PreviousTagSize, out _);
    }

    IFlvTagPacket IPitayaPacket<IFlvTagPacket>.Decode(ref BufferReader reader)
    {
        return Decode(ref reader);
    }
}
