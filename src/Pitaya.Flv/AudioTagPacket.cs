using System.Text.Json;
using Pitaya.Buffers;
using Pitaya.Buffers.Extensions;
using Pitaya.Core;

namespace Pitaya.Flv;

public struct AudioTagPacket : IFlvTagPacket
{
    public AudioTagPacket()
    {
    }

    public const byte TagType = 8;

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

    public byte SoundFormat { get; set; }

    public byte SoundRate { get; set; }

    public byte SoundSize { get; set; }

    public byte SoundType { get; set; }

    public byte[] AudioData { get; set; } = [];

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
        writer.WriteNumber(nameof(SoundFormat), SoundFormat);
        writer.WriteNumber(nameof(SoundRate), SoundRate);
        writer.WriteNumber(nameof(SoundSize), SoundSize);
        writer.WriteNumber(nameof(SoundType), SoundType);
        writer.WriteString(nameof(AudioData), AudioData.ToHexString());
        writer.WriteNumber(nameof(PreviousTagSize), PreviousTagSize);
    }

    public AudioTagPacket Decode(ref BufferReader reader)
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
        var soundFormatByte = reader.ReadByte();
        SoundFormat = (byte)(soundFormatByte >> 4);
        SoundRate = (byte)((soundFormatByte >> 2) & 0b11);
        SoundSize = (byte)((soundFormatByte >> 1) & 0b1);
        SoundType = (byte)(soundFormatByte & 0b1);
        AudioData = reader.ReadArray((int)DataSize).ToArray();
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
        writer.WriteByte((byte)((SoundFormat << 4) | (SoundRate << 2) | (SoundSize << 1) | SoundType), out _);
        writer.WriteArray(AudioData, out int position);
        writer.WriteUInt32((uint)(position + 1), out _);
    }

    IFlvTagPacket IPitayaPacket<IFlvTagPacket>.Decode(ref BufferReader reader)
    {
        return Decode(ref reader);
    }
}
