using Pitaya.BasicType.Extensions;

namespace Pitaya.BasicType.BitTypes;

public struct Bit2 : IEquatable<Bit2>, IComparable<Bit2>
{
    private byte _value;

    public Bit2(byte value)
    {
        _value = value;
    }

    public Bit2(bool value)
    {
        _value = value ? (byte)1 : (byte)0;
    }

    public static implicit operator bool(Bit2 bit) => bit._value > 0;
    public static implicit operator byte(Bit2 bit) => bit._value;

    public static implicit operator Bit2(byte value) => new(value);

    public static Bit2 operator &(Bit2 left, Bit2 right) => new((byte)(left._value & right._value));

    public static Bit2 operator |(Bit2 left, Bit2 right) => new((byte)(left._value | right._value));

    public static Bit2 operator ^(Bit2 left, Bit2 right) => new((byte)(left._value ^ right._value));

    public static Bit2 operator ~(Bit2 bit) => new((byte)~bit._value);

    public static Bit2 operator !(Bit2 bit) => new((byte)(bit._value == 0 ? 1 : 0));

    public override string ToString() => _value.ToString();

    public override bool Equals(object? obj) => obj is Bit2 bit && _value == bit._value;

    public override int GetHashCode() => _value;

    public static bool operator ==(Bit2 left, Bit2 right) => left._value == right._value;

    public static bool operator !=(Bit2 left, Bit2 right) => left._value != right._value;

    public static Bit2 Zero => new(0);

    public static Bit2 One => new(1);

    public static Bit2 OneMinus => new(0b11);

    public static Bit2 Parse(string value) => new(byte.Parse(value));

    public static bool TryParse(string value, out Bit2 bit)
    {
        if (byte.TryParse(value, out var result))
        {
            bit = new Bit2(result);
            return true;
        }
        else
        {
            bit = default;
            return false;
        }
    }

    public static bool TryParseFromNumber(object value, out Bit2 bit)
    {
        if (value.IsNumber())
        {
            bit = new Bit2((byte)value);
            return true;
        }
        else
        {
            bit = default;
            return false;
        }
    }

    public bool Equals(Bit2 other)
    {
        return _value == other._value;
    }

    public int CompareTo(Bit2 other)
    {
        return _value.CompareTo(other._value);
    }

    public static bool operator <(Bit2 left, Bit2 right) => left._value < right._value;

    public static bool operator <=(Bit2 left, Bit2 right) => left._value <= right._value;

    public static bool operator >(Bit2 left, Bit2 right) => left._value > right._value;

    public static bool operator >=(Bit2 left, Bit2 right) => left._value >= right._value;

    public static Bit2 operator ++(Bit2 bit) => new((byte)(bit._value + 1));

    public static Bit2 operator --(Bit2 bit) => new((byte)(bit._value - 1));
}