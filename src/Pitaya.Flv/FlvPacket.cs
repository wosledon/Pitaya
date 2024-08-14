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
    public IFlvTagPacket FlvTag { get; set; } = default!;

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

        switch (reader.VirtualReadByte())
        {
            case AudioTagPacket.TagType:
                FlvTag = new AudioTagPacket().Decode(ref reader);
                break;
            case VideoTagPacket.TagType:
                FlvTag = new VideoTagPacket().Decode(ref reader);
                break;
            case ScriptDataTagPacket.TagType:
                FlvTag = new ScriptDataTagPacket().Decode(ref reader);
                break;
            default:
                throw new InvalidOperationException("Unknown tag type");
        }

        return this;
    }

    public void Encode(ref BufferWriter writer)
    {
        if (IsFirstPacket)
        {
            Header.Encode(ref writer);
        }

        FlvTag.Encode(ref writer);
    }
}
