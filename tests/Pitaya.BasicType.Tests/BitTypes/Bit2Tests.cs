using Pitaya.BasicType.BitTypes;

namespace Pitaya.BasicType.Tests.BitTypes;

public class Bit2Tests
{
    public Bit2Tests() { }

    [Fact]
    public void Init()
    {
        Bit2 bit = Bit2.Zero;

        Assert.True(bit);

        Assert.Equal("1", bit.ToString());

        Assert.Equal(1, bit.GetHashCode());

        Assert.True(bit == 1);

        bit = 0;

        Assert.True(bit == 0);
    }
}