using System.Buffers.Binary;
using System.Text;

namespace Pitaya.Buffers;

/// <summary>
/// 表示一个缓冲区读取器。
/// </summary>
public ref struct BufferReader
{
    /// <summary>
    /// 获取缓冲区的只读字节数组。
    /// </summary>
    public ReadOnlySpan<byte> Buffer { get; private set; }

    /// <summary>
    /// 获取原始缓冲区的只读字节数组。
    /// </summary>
    public ReadOnlySpan<byte> OriginBuffer { get; private set; }

    /// <summary>
    /// 获取或设置当前读取的偏移量。
    /// </summary>
    public int Offset { get; private set; }

    /// <summary>
    /// 初始化 <see cref="BufferReader"/> 结构的新实例。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    public BufferReader(ReadOnlySpan<byte> buffer)
    {
        Buffer = buffer;
        OriginBuffer = buffer;
        Offset = 0;
    }

    /// <summary>
    /// 从缓冲区中读取指定长度的字节数组。
    /// </summary>
    /// <param name="length">要读取的字节数。</param>
    /// <returns>从缓冲区中读取的字节数组。</returns>
    private ReadOnlySpan<byte> Read(int length)
    {
        Offset += length;
        return Buffer.Slice(Offset - length);
    }

    /// <summary>
    /// 从虚拟缓冲区中读取指定长度的字节数组。
    /// </summary>
    /// <param name="length">要读取的字节数。</param>
    /// <returns>从虚拟缓冲区中读取的字节数组。</returns>
    private ReadOnlySpan<byte> VirtualRead(int length)
    {
        return Buffer.Slice(Offset, length);
    }

    /// <summary>
    /// 从当前位置读取到缓冲区末尾的字节数组。
    /// </summary>
    /// <returns>从当前位置读取到缓冲区末尾的字节数组。</returns>
    private ReadOnlySpan<byte> ReadToEnd()
    {
        Offset = Buffer.Length;
        return Buffer.Slice(Offset);
    }

    /// <summary>
    /// 从当前位置读取到虚拟缓冲区末尾的字节数组。
    /// </summary>
    /// <returns>从当前位置读取到虚拟缓冲区末尾的字节数组。</returns>
    private ReadOnlySpan<byte> ReadToEndVirtual()
    {
        return Buffer.Slice(Offset);
    }

    /// <summary>
    /// 跳过指定长度的字节数。
    /// </summary>
    /// <param name="length">要跳过的字节数。</param>
    public void Skip(int length)
    {
        Offset += length;
    }

    /// <summary>
    /// 重置读取器的偏移量和缓冲区。
    /// </summary>
    public void Reset()
    {
        Offset = 0;
        Buffer = OriginBuffer;
    }

    /// <summary>
    /// 从缓冲区中读取一个字节。
    /// </summary>
    /// <returns>从缓冲区中读取的字节。</returns>
    public byte ReadByte()
    {
        return Read(1)[0];
    }

    /// <summary>
    /// 从缓冲区中读取一个 16 位无符号整数。
    /// </summary>
    /// <returns>从缓冲区中读取的 16 位无符号整数。</returns>
    public ushort ReadUInt16()
    {
        return BinaryPrimitives.ReadUInt16BigEndian(Read(2));
    }

    /// <summary>
    /// 从缓冲区中读取一个 32 位无符号整数。
    /// </summary>
    /// <returns>从缓冲区中读取的 32 位无符号整数。</returns>
    public uint ReadUInt32()
    {
        return BinaryPrimitives.ReadUInt32BigEndian(Read(4));
    }

    /// <summary>
    /// 从缓冲区中读取一个 64 位无符号整数。
    /// </summary>
    /// <returns>从缓冲区中读取的 64 位无符号整数。</returns>
    public ulong ReadUInt64()
    {
        return BinaryPrimitives.ReadUInt64BigEndian(Read(8));
    }

    /// <summary>
    /// 从缓冲区中读取一个 16 位有符号整数。
    /// </summary>
    /// <returns>从缓冲区中读取的 16 位有符号整数。</returns>
    public short ReadInt16()
    {
        return BinaryPrimitives.ReadInt16BigEndian(Read(2));
    }

    /// <summary>
    /// 从缓冲区中读取一个 32 位有符号整数。
    /// </summary>
    /// <returns>从缓冲区中读取的 32 位有符号整数。</returns>
    public int ReadInt32()
    {
        return BinaryPrimitives.ReadInt32BigEndian(Read(4));
    }

    /// <summary>
    /// 从缓冲区中读取一个 64 位有符号整数。
    /// </summary>
    /// <returns>从缓冲区中读取的 64 位有符号整数。</returns>
    public long ReadInt64()
    {
        return BinaryPrimitives.ReadInt64BigEndian(Read(8));
    }

    /// <summary>
    /// 从缓冲区中读取一个字符。
    /// </summary>
    /// <returns>从缓冲区中读取的字符。</returns>
    public char ReadChar()
    {
        return BitConverter.ToChar(Read(2));
    }

    /// <summary>
    /// 从虚拟缓冲区中读取一个字节。
    /// </summary>
    /// <returns>从虚拟缓冲区中读取的字节。</returns>
    public byte VirtualReadByte()
    {
        return VirtualRead(1)[0];
    }

    /// <summary>
    /// 从虚拟缓冲区中读取一个 16 位无符号整数。
    /// </summary>
    /// <returns>从虚拟缓冲区中读取的 16 位无符号整数。</returns>
    public ushort VirtualReadUInt16()
    {
        return BinaryPrimitives.ReadUInt16BigEndian(VirtualRead(2));
    }

    /// <summary>
    /// 从虚拟缓冲区中读取一个 32 位无符号整数。
    /// </summary>
    /// <returns>从虚拟缓冲区中读取的 32 位无符号整数。</returns>
    public uint VirtualReadUInt32()
    {
        return BinaryPrimitives.ReadUInt32BigEndian(VirtualRead(4));
    }

    /// <summary>
    /// 从虚拟缓冲区中读取一个 64 位无符号整数。
    /// </summary>
    /// <returns>从虚拟缓冲区中读取的 64 位无符号整数。</returns>
    public ulong VirtualReadUInt64()
    {
        return BinaryPrimitives.ReadUInt64BigEndian(VirtualRead(8));
    }

    /// <summary>
    /// 从虚拟缓冲区中读取一个 16 位有符号整数。
    /// </summary>
    /// <returns>从虚拟缓冲区中读取的 16 位有符号整数。</returns>
    public short VirtualReadInt16()
    {
        return BinaryPrimitives.ReadInt16BigEndian(VirtualRead(2));
    }

    /// <summary>
    /// 从虚拟缓冲区中读取一个 32 位有符号整数。
    /// </summary>
    /// <returns>从虚拟缓冲区中读取的 32 位有符号整数。</returns>
    public int VirtualReadInt32()
    {
        return BinaryPrimitives.ReadInt32BigEndian(VirtualRead(4));
    }

    /// <summary>
    /// 从虚拟缓冲区中读取一个 64 位有符号整数。
    /// </summary>
    /// <returns>从虚拟缓冲区中读取的 64 位有符号整数。</returns>
    public long VirtualReadInt64()
    {
        return BinaryPrimitives.ReadInt64BigEndian(VirtualRead(8));
    }

    /// <summary>
    /// 从虚拟缓冲区中读取一个字符。
    /// </summary>
    /// <returns>从虚拟缓冲区中读取的字符。</returns>
    public char VirtualReadChar()
    {
        return BitConverter.ToChar(VirtualRead(2));
    }

    /// <summary>
    /// 从缓冲区中读取指定长度的字节数组。
    /// </summary>
    /// <param name="length">要读取的字节数。</param>
    /// <returns>从缓冲区中读取的字节数组。</returns>
    public ReadOnlySpan<byte> ReadArray(int length)
    {
        return Read(length);
    }

    /// <summary>
    /// 从虚拟缓冲区中读取指定长度的字节数组。
    /// </summary>
    /// <param name="length">要读取的字节数。</param>
    /// <returns>从虚拟缓冲区中读取的字节数组。</returns>
    public ReadOnlySpan<byte> VirtualReadArray(int length)
    {
        return VirtualRead(length);
    }

    /// <summary>
    /// 从虚拟缓冲区中读取指定范围的字节数组。
    /// </summary>
    /// <param name="start">起始位置。</param>
    /// <param name="end">结束位置。</param>
    /// <returns>从虚拟缓冲区中读取的字节数组。</returns>
    public ReadOnlySpan<byte> VirtualReadArray(int start, int end)
    {
        return Buffer.Slice(start, end);
    }

    /// <summary>
    /// 从缓冲区中读取指定长度的 ASCII 字符串。
    /// </summary>
    /// <param name="length">要读取的字符长度。</param>
    /// <returns>从缓冲区中读取的 ASCII 字符串。</returns>
    public string ReadAsciiString(int length)
    {
        return Encoding.ASCII.GetString(Read(length));
    }

    /// <summary>
    /// 从缓冲区中读取指定长度的 UTF-8 字符串。
    /// </summary>
    /// <param name="length">要读取的字符长度。</param>
    /// <returns>从缓冲区中读取的 UTF-8 字符串。</returns>
    public string ReadUtf8String(int length)
    {
        return Encoding.UTF8.GetString(Read(length));
    }

    /// <summary>
    /// 从当前位置读取到缓冲区末尾的字节数组。
    /// </summary>
    /// <returns>从当前位置读取到缓冲区末尾的字节数组。</returns>
    public ReadOnlySpan<byte> ReadContent()
    {
        return Buffer.Slice(Offset);
    }

    /// <summary>
    /// 从当前位置读取指定长度的字节数组。
    /// </summary>
    /// <param name="length">要读取的字节数。</param>
    /// <returns>从当前位置读取的字节数组。</returns>
    public ReadOnlySpan<byte> ReadContent(int length)
    {
        return Buffer.Slice(Offset, length - Offset);
    }
}
