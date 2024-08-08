namespace Pitaya.Buffers;

/// <summary>
/// PitayaPacket
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPitayaPacket<T>
{
    /// <summary>
    /// 编码
    /// </summary>
    /// <param name="writer"></param>
    public void Encode(ref BufferWriter writer);
    
    /// <summary>
    /// 解码
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public T Decode(ref BufferReader reader);
}
