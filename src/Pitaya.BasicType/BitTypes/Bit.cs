using Pitaya.BasicType.Extensions;

namespace Pitaya.BasicType.BitTypes;

public struct Bit : IEquatable<Bit>, IComparable<Bit>
{
    private byte _value;

    public Bit(bool value)
    {
        _value = value ? (byte)1 : (byte)0;
    }

    public Bit(byte value)
    {
        _value = (byte)(value & 1);
    }

    public static implicit operator bool(Bit bit) => bit._value == 1;

    public static implicit operator Bit(bool value) => new(value);
    public static implicit operator Bit(byte value) => new(value);
    public static implicit operator byte(Bit v) => v._value;

    public static Bit operator &(Bit left, Bit right) => new(left._value == 1 && right._value == 1);

    public static Bit operator |(Bit left, Bit right) => new(left._value == 1 || right._value == 1);

    public static Bit operator ^(Bit left, Bit right) => new(left._value != right._value);

    public static Bit operator ~(Bit bit) => new(bit._value == 0);

    public static Bit operator !(Bit bit) => new(bit._value == 0);

    public static bool operator true(Bit bit) => bit._value == 1;

    public static bool operator false(Bit bit) => bit._value == 0;

    public override string ToString() => _value == 1 ? "1" : "0";

    public override bool Equals(object? obj) => obj is Bit bit && _value == bit._value;

    public override int GetHashCode() => _value;

    public static bool operator ==(Bit left, Bit right) => left._value == right._value;
    public static bool operator !=(Bit left, Bit right) => left._value != right._value;
    public static bool operator ==(Bit left, bool right) => left._value == (right ? (byte)1 : (byte)0);
    public static bool operator !=(Bit left, bool right) => left._value != (right ? (byte)1 : (byte)0);
    public static bool operator ==(bool left, Bit right) => (left ? (byte)1 : (byte)0) == right._value;
    public static bool operator !=(bool left, Bit right) => (left ? (byte)1 : (byte)0) != right._value;
  
    public static Bit Zero => new(false);

    public static Bit One => new(true);

    public static Bit Parse(string value) => new(value == "1");

    public static bool TryParse(string value, out Bit bit)
    {
        if (value == "1")
        {
            bit = new Bit(true);
            return true;
        }
        else if (value == "0")
        {
            bit = new Bit(false);
            return true;
        }
        else
        {
            bit = default;
            return false;
        }
    }

    public static bool TryParseFromNumber(object value, out Bit bit)
    {
        if (value.IsNumber())
        {
            bit = new Bit(!value.Equals(0));
            return true;
        }
        else
        {
            bit = default;
            return false;
        }
    }

    public bool Equals(Bit other)
    {
        return _value == other._value;
    }

    public int CompareTo(Bit other)
    {
        return _value.CompareTo(other._value);
    }

    public static bool operator <(Bit left, Bit right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Bit left, Bit right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Bit left, Bit right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Bit left, Bit right)
    {
        return left.CompareTo(right) >= 0;
    }
}
