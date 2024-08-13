using System.Text.Json;
using Pitaya.Buffers;
using Pitaya.Core;

namespace Pitaya.Flv;

public struct FlvPacket : IPitayaPacket<FlvPacket>, IPitayaAnalyzable
{
    public FlvPacket()
    {
    }

    public byte[] Origin { get; set; } = [];

    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        throw new NotImplementedException();
    }

    public FlvPacket Decode(ref BufferReader reader)
    {
        throw new NotImplementedException();
    }

    public void Encode(ref BufferWriter writer)
    {
        throw new NotImplementedException();
    }
}
