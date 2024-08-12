using Pitaya.Buffers;
using System.Text.Json;

namespace Pitaya.Core;

/// <summary>
/// 解释器
/// </summary>
public interface IPitayaAnalyzable
{
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="writer"></param>
    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer);
}
