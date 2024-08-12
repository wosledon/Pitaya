using Pitaya.Buffers;
using Pitaya.Buffers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pitaya.Core;

/// <summary>
/// 提供序列化、反序列化和分析功能的 PitayaConvert 类。
/// </summary>
public static class PitayaConvert
{
    /// <summary>
    /// 将指定的 IPitayaPacket 对象序列化为字节数组。
    /// </summary>
    /// <typeparam name="T">IPitayaPacket 对象的类型。</typeparam>
    /// <param name="packet">要序列化的 IPitayaPacket 对象。</param>
    /// <param name="minBufferSize">序列化时使用的最小缓冲区大小。</param>
    /// <returns>序列化后的字节数组。</returns>
    public static byte[] Serialize<T>(this IPitayaPacket<T> packet, int minBufferSize = 4096)
    {
        var buffer = PitayaArrayPool.Rent(minBufferSize);
        try
        {
            var writer = new BufferWriter(buffer);
            packet.Encode(ref writer);
            return writer.FlushBytes();
        }
        catch
        {
            throw;
        }
        finally
        {
            PitayaArrayPool.Return(buffer);
        }
    }

    /// <summary>
    /// 将字节数组反序列化为指定类型的 IPitayaPacket 对象。
    /// </summary>
    /// <typeparam name="T">IPitayaPacket 对象的类型。</typeparam>
    /// <param name="data">要反序列化的字节数组。</param>
    /// <returns>反序列化后的 IPitayaPacket 对象。</returns>
    public static T Deserialize<T>(this byte[] data)
        where T : IPitayaPacket<T>, new()
    {
        var packet = new T();
        var reader = new BufferReader(data);
        packet.Decode(ref reader);

        return packet;
    }

    /// <summary>
    /// 分析字节数组并生成对应的 JSON 字符串。
    /// </summary>
    /// <typeparam name="T">IPitayaAnalyzable 对象的类型。</typeparam>
    /// <param name="data">要分析的字节数组。</param>
    /// <returns>生成的 JSON 字符串。</returns>
    public static string Analyze<T>(this byte[] data)
        where T : IPitayaAnalyzable, new()
    {
        var packet = new T();
        var reader = new BufferReader(data);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Indented = true
        });
        writer.WriteStartObject();
        packet.Analyze(ref reader, writer);
        writer.WriteEndObject();
        writer.Flush();

        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
