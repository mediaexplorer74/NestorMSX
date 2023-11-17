// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.Bit
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

namespace Konamiman.Z80dotNet
{
  public struct Bit
  {
    public Bit(int value)
      : this()
    {
      this.Value = value == 0 ? 0 : 1;
    }

    public int Value { get; private set; }

    public override string ToString() => this.Value.ToString();

    public static implicit operator Bit(int value) => new Bit(value);

    public static implicit operator int(Bit value) => value.Value;

    public static implicit operator bool(Bit value) => value.Value == 1;

    public static implicit operator Bit(bool value) => new Bit(value ? 1 : 0);

    public static Bit operator |(Bit value1, Bit value2) => (Bit) (value1.Value | value2.Value);

    public static Bit operator &(Bit value1, Bit value2) => (Bit) (value1.Value & value2.Value);

    public static Bit operator ^(Bit value1, Bit value2) => (Bit) (value1.Value ^ value2.Value);

    public static Bit operator ~(Bit value) => new Bit(value.Value ^ 1);

    public static Bit operator !(Bit value) => ~value;

    public static bool operator true(Bit value) => value.Value == 1;

    public static bool operator false(Bit value) => value.Value == 0;

    public static bool operator ==(Bit bitValue, int intValue)
    {
      if (bitValue.Value == 0 && intValue == 0)
        return true;
      return bitValue.Value == 1 && intValue != 0;
    }

    public static bool operator !=(Bit bitValue, int intValue) => !(bitValue == intValue);

    public override bool Equals(object obj) => obj is int num ? this == num : base.Equals(obj);

    public override int GetHashCode()
    {
        return -1937169414 + Value.GetHashCode();
    }
  }
}
