namespace Pitaya.Buffers.Extensions;

/// <summary>
/// </summary>
public static class HexUtil
{
    private static readonly char[] HexdumpTable = new char[256 * 4];

    static HexUtil()
    {
        char[] digits = "0123456789ABCDEF".ToCharArray();
        for (int i = 0; i < 256; i++)
        {
            HexdumpTable[i << 1] = digits[(int)((uint)i >> 4 & 0x0F)];
            HexdumpTable[(i << 1) + 1] = digits[i & 0x0F];
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer">    </param>
    /// <param name="fromIndex"> </param>
    /// <param name="length">    </param>
    /// <returns> </returns>
    public static string DoHexDump(ReadOnlySpan<byte> buffer, int fromIndex, int length)
    {
        if (length == 0)
        {
            return "";
        }
        int endIndex = fromIndex + length;
        var buf = new char[length << 1];
        int srcIdx = fromIndex;
        int dstIdx = 0;
        for (; srcIdx < endIndex; srcIdx++, dstIdx += 2)
        {
            Array.Copy(HexdumpTable, buffer[srcIdx] << 1, buf, dstIdx, 2);
        }
        return new string(buf);
    }

    /// <summary>
    /// </summary>
    /// <param name="array">     </param>
    /// <param name="fromIndex"> </param>
    /// <param name="length">    </param>
    /// <returns> </returns>
    public static string DoHexDump(byte[] array, int fromIndex, int length)
    {
        if (length == 0)
        {
            return "";
        }
        int endIndex = fromIndex + length;
        var buf = new char[length << 1];
        int srcIdx = fromIndex;
        int dstIdx = 0;
        for (; srcIdx < endIndex; srcIdx++, dstIdx += 2)
        {
            Array.Copy(HexdumpTable, (array[srcIdx] & 0xFF) << 1, buf, dstIdx, 2);
        }
        return new string(buf);
    }
}
