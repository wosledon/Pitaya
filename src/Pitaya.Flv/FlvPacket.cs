using System.Text.Json;
using Pitaya.Buffers;
using Pitaya.Core;

namespace Pitaya.Flv;

public struct FlvPacket : IPitayaPacket<FlvPacket>, IPitayaAnalyzable
{
    public FlvPacket()
    {
    }

    public bool IsFirstPacket { get; set; }

    public FlvHeaderPacket Header { get; set; }
    public FlvTagPacket FlvTag { get; set; }

    public byte[] Origin { get; set; } = [];

    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        writer.WriteBoolean("IsFirstPacket", IsFirstPacket);
        if (IsFirstPacket)
        {
            writer.WriteStartObject(nameof(Header));
            Header.Analyze(ref reader, writer);
            writer.WriteEndObject();
        }
        writer.WriteStartObject(nameof(FlvTag));
        FlvTag.Analyze(ref reader, writer);
        writer.WriteEndObject();

    }

    public FlvPacket Decode(ref BufferReader reader)
    {
        Origin = reader.OriginBuffer.ToArray();
        IsFirstPacket = reader.VirtualReadArray(3).SequenceEqual(FlvHeaderPacket.Signature);

        if (IsFirstPacket)
        {
            Header = new FlvHeaderPacket().Decode(ref reader);
        }
        FlvTag = new FlvTagPacket().Decode(ref reader);

        return this;
    }

    public void Encode(ref BufferWriter writer)
    {
        if (IsFirstPacket)
        {
            Header.Encode(ref writer);
        }
        else
        {
            FlvTag.Encode(ref writer);
        }
    }
}

public struct ScriptDataTagPacket : IPitayaPacket<ScriptDataTagPacket>, IPitayaAnalyzable
{
    public ScriptDataTagPacket()
    {
    }

    public byte TagType { get; private set; } = 18;

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
        throw new NotImplementedException();
    }

    public ScriptDataTagPacket Decode(ref BufferReader reader)
    {
        Origin = reader.OriginBuffer.ToArray();
        return this;
    }

    public void Encode(ref BufferWriter writer)
    {
        throw new NotImplementedException();
    }
}

public struct VideoTagPacket : IPitayaPacket<VideoTagPacket>, IPitayaAnalyzable
{
    public VideoTagPacket()
    {
    }

    public byte TagType { get; private set; } = 9;

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
        throw new NotImplementedException();
    }

    public VideoTagPacket Decode(ref BufferReader reader)
    {
        Origin = reader.OriginBuffer.ToArray();
        return this;
    }

    public void Encode(ref BufferWriter writer)
    {
        throw new NotImplementedException();
    }
}

public struct AudioTagPacket : IPitayaPacket<AudioTagPacket>, IPitayaAnalyzable
{
    public AudioTagPacket()
    {
    }

    public byte TagType { get; private set; } = 8;

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
        throw new NotImplementedException();
    }

    public AudioTagPacket Decode(ref BufferReader reader)
    {
        Origin = reader.OriginBuffer.ToArray();
        return this;
    }

    public void Encode(ref BufferWriter writer)
    {
        throw new NotImplementedException();
    }
}
