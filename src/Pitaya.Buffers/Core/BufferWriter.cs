using Pitaya.Buffers.Core;
using System.Buffers.Binary;
using System.Text;

namespace Pitaya.Buffers;

public ref struct BufferWriter
{
    private BufferWriterAux _aux;

    /// <summary>
    /// 初始化 <see cref="BufferWriter"/> 结构的新实例。
    /// </summary>
    /// <param name="buffer">要写入的字节缓冲区。</param>
    public BufferWriter(Span<byte> buffer)
    {
        _aux = new BufferWriterAux(buffer);
    }

    /// <summary>
    /// 刷新已编码的字节数组。
    /// </summary>
    /// <returns>已编码的字节数组。</returns>
    public byte[] FlushBytes()
    {
        return _aux.Written.Slice(_aux.BeforeCodingWrittenPosition).ToArray();
    }

    /// <summary>
    /// 刷新已编码的字节序列。
    /// </summary>
    /// <returns>已编码的字节序列。</returns>
    public ReadOnlySpan<byte> FlushSpan()
    {
        return _aux.Written.Slice(_aux.BeforeCodingWrittenPosition);
    }

    /// <summary>
    /// 刷新实际的字节数组。
    /// </summary>
    /// <returns>实际的字节数组。</returns>
    public byte[] FlushRealBytes()
    {
        return _aux.Written.ToArray();
    }

    /// <summary>
    /// 将一个字节设置为零，并返回其位置。
    /// </summary>
    /// <param name="position">设置为零的字节的位置。</param>
    public void Nil(out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        span[0] = 0;
        _aux.Advance(1);
    }

    /// <summary>
    /// 将指定数量的字节设置为零，并返回其位置。
    /// </summary>
    /// <param name="count">要设置为零的字节数量。</param>
    /// <param name="position">设置为零的字节的位置。</param>
    /// <param name="defaultValue">要设置的默认值，默认为 0x00。</param>
    public void Skip(in int count, out int position, in byte defaultValue = 0x00)
    {
        position = _aux.WrittenPosition;

        var span = _aux.Free;

        for (int i = 0; i < count; i++)
        {
            span[i] = defaultValue;
        }

        _aux.Advance(count);
    }

    public void WriteByte(in byte value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        span[0] = value;
        _aux.Advance(1);
    }

    public void WriteUInt16(in ushort value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        BinaryPrimitives.WriteUInt16BigEndian(span, value);
        _aux.Advance(2);
    }

    public void WriteUInt32(in uint value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        BinaryPrimitives.WriteUInt32BigEndian(span, value);
        _aux.Advance(4);
    }

    public void WriteUInt64(in ulong value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        BinaryPrimitives.WriteUInt64BigEndian(span, value);
        _aux.Advance(8);
    }

    public void WriteInt16(in short value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        BinaryPrimitives.WriteInt16BigEndian(span, value);
        _aux.Advance(2);
    }


    public void WriteInt32(in int value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        BinaryPrimitives.WriteInt32BigEndian(span, value);
        _aux.Advance(4);
    }

    public void WriteInt64(in long value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        BinaryPrimitives.WriteInt64BigEndian(span, value);
        _aux.Advance(8);
    }

    public void WriteChar(in char value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        span[0] = (byte)value;
        _aux.Advance(1);
    }

    public void WriteArray(in ReadOnlySpan<byte> value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        value.CopyTo(span);
        _aux.Advance(value.Length);
    }

    public void WriteAsciiString(in string value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        Encoding.ASCII.GetBytes(value, span);
        _aux.Advance(value.Length);
    }

    public void WriteUtf8String(in string value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        Encoding.UTF8.GetBytes(value, span);
        _aux.Advance(value.Length);
    }

    public void WriteUniCodeString(in string value, out int position)
    {
        position = _aux.WrittenPosition;
        var span = _aux.Free;
        Encoding.Unicode.GetBytes(value, span);
        _aux.Advance(value.Length);
    }

    public void WriteByteReturn(in byte value, in int position)
    {
        _aux.Written.Slice(position, 1)[0] = value;
    }

    public void WriteUInt16Return(in ushort value, in int position)
    {
        BinaryPrimitives.WriteUInt16BigEndian(_aux.Written.Slice(position, 2), value);
    }

    public void WriteUInt32Return(in uint value, in int position)
    {
        BinaryPrimitives.WriteUInt32BigEndian(_aux.Written.Slice(position, 4), value);
    }

    public void WriteUInt64Return(in ulong value, in int position)
    {
        BinaryPrimitives.WriteUInt64BigEndian(_aux.Written.Slice(position, 8), value);
    }

    public void WriteInt16Return(in short value, in int position)
    {
        BinaryPrimitives.WriteInt16BigEndian(_aux.Written.Slice(position, 2), value);
    }

    public void WriteInt32Return(in int value, in int position)
    {
        BinaryPrimitives.WriteInt32BigEndian(_aux.Written.Slice(position, 4), value);
    }

    public void WriteInt64Return(in long value, in int position)
    {
        BinaryPrimitives.WriteInt64BigEndian(_aux.Written.Slice(position, 8), value);
    }

    public void WriteCharReturn(in char value, in int position)
    {
        _aux.Written.Slice(position, 1)[0] = (byte)value;
    }

    public void WriteArrayReturn(in ReadOnlySpan<byte> value, in int position)
    {
        value.CopyTo(_aux.Written.Slice(position));
    }

    public void WriteAsciiStringReturn(in string value, in int position)
    {
        Encoding.ASCII.GetBytes(value, _aux.Written.Slice(position));
    }

    public void WriteUtf8StringReturn(in string value, in int position)
    {
        Encoding.UTF8.GetBytes(value, _aux.Written.Slice(position));
    }

    public void WriteUniCodeStringReturn(in string value, in int position)
    {
        Encoding.Unicode.GetBytes(value, _aux.Written.Slice(position));
    }

    public void WriteArray(in ReadOnlySpan<byte> value)
    {
        var span = _aux.Free;
        value.CopyTo(span);
        _aux.Advance(value.Length);
    }

    public void WriteXor(in int start, in int end)
    {
        if(start > end)
        {
            throw new ArgumentOutOfRangeException($"start > end: {start} > {end}");
        }

        var xorSpan = _aux.Written.Slice(start, end);
        byte result = xorSpan[0];
        for (int i = start + 1; i < end; i++)
        {
            result = (byte)(result ^ xorSpan[i]);
        }
        var span = _aux.Free;
        span[0] = result;
        _aux.Advance(1);
    }

    public void WriteXor(in int start)
    {
        if (_aux.WrittenPosition < start)
        {
            throw new ArgumentOutOfRangeException($"_aux.WrittenPosition < start: {_aux.WrittenPosition} < {start}");
        }

        var xorSpan = _aux.Written.Slice(start);

        byte result = xorSpan[0];
        for (int i = 1; i < xorSpan.Length; i++)
        {
            result = (byte)(result ^ xorSpan[i]);
        }
        var span = _aux.Free;
        span[0] = result;
        _aux.Advance(1);
    }

    public void WriteXor()
    {
        if (_aux.WrittenPosition < 1)
        {
            throw new ArgumentOutOfRangeException($"_aux.WrittenPosition < 1: {_aux.WrittenPosition} < 1");
        }

        var xorSpan = _aux.Written.Slice(1);
        byte result = xorSpan[0];
        for (int i = 1; i < xorSpan.Length; i++)
        {
            result = (byte)(result ^ xorSpan[i]);
        }
        var span = _aux.Free;
        span[0] = result;
        _aux.Advance(1);
    }

    public int GetCurrentPosition()
    {
        return _aux.WrittenPosition;
    }

    internal void WriteEncode()
    {
        var tmpSpan = _aux.Written;
        _aux.BeforeCodingWrittenPosition = _aux.WrittenPosition;
        var spanFree = _aux.Free;
        int tempOffset = 0;
        spanFree[tempOffset++] = tmpSpan[0];
        for (int i = 1; i < tmpSpan.Length - 1; i++)
        {
            spanFree[tempOffset++] = tmpSpan[i];
        }
        spanFree[tempOffset++] = tmpSpan[tmpSpan.Length - 1];
        _aux.Advance(tempOffset);
    }
}
