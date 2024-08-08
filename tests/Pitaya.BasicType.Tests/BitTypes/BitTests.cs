using Pitaya.BasicType.BitTypes;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pitaya.BasicType.Tests.BitTypes;

public class BitTests
{
    public BitTests()
    {
    }

    [Fact]
    public void Init()
    {
        Bit bit = Bit.One;

        Assert.True(bit);

        Assert.Equal("1", bit.ToString());

        Assert.Equal(1, bit.GetHashCode());

        Assert.True(bit == new Bit(true));

        Assert.True(bit == 1);

        bit = 0;

        Assert.True(bit == 0);
    }
}
