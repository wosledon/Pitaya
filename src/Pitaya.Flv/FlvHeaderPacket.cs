using System.Text.Json;
using Pitaya.Buffers;

namespace Pitaya.Flv;

public struct FlvHeaderPacket : IPitayaPacket<FlvHeaderPacket>, IPitayaAnalyzable
{
    public FlvHeaderPacket()
    {
    }

    public byte[] Origin { get; set; } = [];

    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        throw new NotImplementedException();
    }

    public FlvHeaderPacket Decode(ref BufferReader reader)
    {
        throw new NotImplementedException();
    }

    public void Encode(ref BufferWriter writer)
    {
        throw new NotImplementedException();
    }
}