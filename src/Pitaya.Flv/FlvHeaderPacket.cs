using System.Text.Json;
using Pitaya.Buffers;
using Pitaya.Core;

namespace Pitaya.Flv;

public struct FlvHeaderPacket : IPitayaPacket<FlvHeaderPacket>, IPitayaAnalyzable
{
    public FlvHeaderPacket()
    {
    }

    public static byte[] Signature = [(byte)'F', (byte)'L', (byte)'V'];

    public byte Version { get; set; } = 1;

    /// <summary>
    /// 表示是否包含音频和视频数据
    /// </summary>
    public byte Flags { get; set; } = 0;

    public uint HeaderLength { get; set; } = 9;

    public uint PreviousTagSize { get; set; } = 0;

    public byte[] Origin { get; set; } = [];

    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        throw new NotImplementedException();
    }

    public FlvHeaderPacket Decode(ref BufferReader reader)
    {
        if (reader.ReadArray(3).SequenceEqual(Signature))
        {
            throw new Exception("Invalid FLV header signature");
        }

        Origin = reader.OriginBuffer.ToArray();

        Version = reader.ReadByte();
        Flags = reader.ReadByte();
        HeaderLength = reader.ReadUInt32();
        PreviousTagSize = reader.ReadUInt32();

        return this;
    }

    public void Encode(ref BufferWriter writer)
    {
        writer.WriteArray(Signature);
        writer.WriteByte(Version, out _);
        writer.WriteByte(Flags, out _);
        writer.WriteUInt32(HeaderLength, out _);
        writer.WriteUInt32(PreviousTagSize, out _);
    }
}