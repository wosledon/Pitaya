using System.Buffers;

namespace Pitaya.Buffers.Core;

public class PitayaArrayPool
{
    private static readonly ArrayPool<byte> _arrayPool;

    static PitayaArrayPool()
    {
        _arrayPool = ArrayPool<byte>.Create();
    }

    public static byte[] Rent(int minimumLength = 4096)
    {
        return _arrayPool.Rent(minimumLength);
    }

    public static void Return(byte[] array, bool clearArray = false)
    {
        _arrayPool.Return(array, clearArray);
    }
}
