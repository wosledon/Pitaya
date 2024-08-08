﻿namespace Pitaya.Buffers.Extensions;

public static partial class BinaryExtensions
{
    /// <summary>
    /// 16进制数组转16进制字符串
    /// </summary>
    /// <param name="source"> </param>
    /// <returns> </returns>
    public static string ToHexString(this byte[] source)
    {
        return HexUtil.DoHexDump(source, 0, source.Length).ToUpper();
    }

    /// <summary>
    /// 16进制字符串转16进制数组
    /// </summary>
    /// <param name="hexString"> </param>
    /// <returns> </returns>
    public static byte[] ToHexBytes(this string hexString)
    {
        hexString = hexString.Replace(" ", "");
        byte[] buf = new byte[hexString.Length / 2];
        ReadOnlySpan<char> readOnlySpan = hexString.AsSpan();
        for (int i = 0; i < hexString.Length; i++)
        {
            if (i % 2 == 0)
            {
                buf[i / 2] = Convert.ToByte(readOnlySpan.Slice(i, 2).ToString(), 16);
            }
        }
        return buf;
    }

    /// <summary>
    /// 从内存块中读取16进制字符串
    /// </summary>
    /// <param name="read">   </param>
    /// <param name="offset"> </param>
    /// <param name="len">    </param>
    /// <returns> </returns>
    public static string ReadHexStringLittle(ReadOnlySpan<byte> read, ref int offset, int len)
    {
        //ReadOnlySpan<byte> source = read.Slice(offset, len);
        string hex = HexUtil.DoHexDump(read, offset, len);
        offset += len;
        return hex;
    }

    /// <summary>
    /// 将16进制字符串写入对应数组中
    /// </summary>
    /// <param name="bytes">  </param>
    /// <param name="offset"> </param>
    /// <param name="data">   </param>
    /// <param name="len">    </param>
    /// <returns> </returns>
    public static int WriteHexStringLittle(byte[] bytes, int offset, string data, int len)
    {
        if (data == null) data = "";
        data = data.Replace(" ", "");
        int startIndex = 0;
        if (data.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            startIndex = 2;
        }
        int length = len;
        if (length == -1)
        {
            length = (data.Length - startIndex) / 2;
        }
        int noOfZero = length * 2 + startIndex - data.Length;
        if (noOfZero > 0)
        {
            data = data.Insert(startIndex, new string('0', noOfZero));
        }
        int byteIndex = 0;
        while (startIndex < data.Length && byteIndex < length)
        {
            bytes[offset + byteIndex] = Convert.ToByte(data.Substring(startIndex, 2), 16);
            startIndex += 2;
            byteIndex++;
        }
        return length;
    }

    /// <summary>
    /// </summary>
    /// <param name="value">  </param>
    /// <param name="format"> </param>
    /// <returns> </returns>
    public static string ReadNumber(this byte value, string format = "X2")
    {
        return value.ToString(format);
    }

    /// <summary>
    /// </summary>
    /// <param name="value">  </param>
    /// <param name="format"> </param>
    /// <returns> </returns>
    public static string ReadNumber(this int value, string format = "X8")
    {
        return value.ToString(format);
    }

    /// <summary>
    /// </summary>
    /// <param name="value">  </param>
    /// <param name="format"> </param>
    public static string ReadNumber(this uint value, string format = "X8")
    {
        return value.ToString(format);
    }

    /// <summary>
    /// </summary>
    /// <param name="value">  </param>
    /// <param name="format"> </param>
    public static string ReadNumber(this long value, string format = "X16")
    {
        return value.ToString(format);
    }

    /// <summary>
    /// </summary>
    /// <param name="value">  </param>
    /// <param name="format"> </param>
    public static string ReadNumber(this ulong value, string format = "X16")
    {
        return value.ToString(format);
    }

    /// <summary>
    /// </summary>
    /// <param name="value">  </param>
    /// <param name="format"> </param>
    public static string ReadNumber(this short value, string format = "X4")
    {
        return value.ToString(format);
    }

    /// <summary>
    /// </summary>
    /// <param name="value">  </param>
    /// <param name="format"> </param>
    public static string ReadNumber(this ushort value, string format = "X4")
    {
        return value.ToString(format);
    }

    /// <summary>
    /// </summary>
    /// <param name="value"> </param>
    /// <returns> </returns>
    public static ReadOnlySpan<char> ReadBinary(this ushort value)
    {
        return Convert.ToString(value, 2).PadLeft(16, '0').AsSpan();
    }

    /// <summary>
    /// </summary>
    /// <param name="value"> </param>
    /// <returns> </returns>
    public static ReadOnlySpan<char> ReadBinary(this short value)
    {
        return Convert.ToString(value, 2).PadLeft(16, '0').AsSpan();
    }

    /// <summary>
    /// </summary>
    /// <param name="value"> </param>
    /// <returns> </returns>
    public static ReadOnlySpan<char> ReadBinary(this uint value)
    {
        return Convert.ToString(value, 2).PadLeft(32, '0').AsSpan();
    }

    /// <summary>
    /// </summary>
    /// <param name="value"> </param>
    /// <returns> </returns>
    public static ReadOnlySpan<char> ReadBinary(this int value)
    {
        return Convert.ToString(value, 2).PadLeft(32, '0').AsSpan();
    }

    /// <summary>
    /// </summary>
    /// <param name="value"> </param>
    /// <returns> </returns>
    public static ReadOnlySpan<char> ReadBinary(this byte value)
    {
        return Convert.ToString(value, 2).PadLeft(8, '0').AsSpan();
    }
}
