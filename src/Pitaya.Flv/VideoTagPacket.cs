using System.Text.Json;
using Pitaya.Buffers;
using Pitaya.Buffers.Extensions;
using Pitaya.Core;

namespace Pitaya.Flv;

public struct VideoTagPacket : IFlvTagPacket
{
    public VideoTagPacket()
    {
    }

    public const byte TagType = 9;

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

    public byte FrameType { get; set; }

    public byte CodecId { get; set; }

    public byte[] VideoData { get; set; } = [];

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
        writer.WriteNumber(nameof(FrameType), FrameType);
        writer.WriteNumber(nameof(CodecId), CodecId);
        writer.WriteString(nameof(VideoData), VideoData.ToHexString());
        writer.WriteNumber(nameof(PreviousTagSize), PreviousTagSize);
    }

    public VideoTagPacket Decode(ref BufferReader reader)
    {
        Origin = reader.OriginBuffer.ToArray();

        if (reader.ReadByte() != TagType)
        {
            throw new Exception("Invalid tag type");
        }

        var dataSizeBytes = reader.ReadArray(3);
        DataSize = (uint)(dataSizeBytes[0] << 16 | dataSizeBytes[1] << 8 | dataSizeBytes[2]);
        var timestampBytes = reader.ReadArray(3);
        Timestamp = (uint)(timestampBytes[0] << 16 | timestampBytes[1] << 8 | timestampBytes[2]);
        TimestampExtended = reader.ReadByte();
        StreamId = reader.ReadUInt32();
        FrameType = reader.ReadByte();
        CodecId = reader.ReadByte();
        VideoData = reader.ReadArray((int)DataSize).ToArray();
        PreviousTagSize = reader.ReadUInt32();

        return this;
    }

    public void Encode(ref BufferWriter writer)
    {
        writer.WriteByte(TagType, out _);
        writer.WriteArray(new byte[] { (byte)(DataSize >> 16), (byte)(DataSize >> 8), (byte)DataSize });
        writer.WriteArray(new byte[] { (byte)(Timestamp >> 16), (byte)(Timestamp >> 8), (byte)Timestamp });
        writer.WriteByte(TimestampExtended, out _);
        writer.WriteUInt32(StreamId, out _);
        writer.WriteByte(FrameType, out _);
        writer.WriteByte(CodecId, out _);
        writer.WriteArray(VideoData, out int position);
        writer.WriteUInt32((uint)(position + 1), out _);
    }

    IFlvTagPacket IPitayaPacket<IFlvTagPacket>.Decode(ref BufferReader reader)
    {
        return Decode(ref reader);
    }
}
