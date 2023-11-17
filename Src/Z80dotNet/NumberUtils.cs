// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.NumberUtils
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

using System;

namespace Konamiman.Z80dotNet
{
  public static class NumberUtils
  {
    public static byte GetHighByte(this short value) => (byte) ((uint) value >> 8);

    public static byte GetHighByte(this ushort value) => (byte) ((uint) value >> 8);

    public static ushort SetHighByte(this ushort value, byte highByte) => (ushort) ((int) value & (int) byte.MaxValue | (int) highByte << 8);

    public static short SetHighByte(this short value, byte highByte) => ((int) value & (int) byte.MaxValue | (int) highByte << 8).ToShort();

    public static byte GetLowByte(this short value) => (byte) ((uint) value & (uint) byte.MaxValue);

    public static byte GetLowByte(this ushort value) => (byte) ((uint) value & (uint) byte.MaxValue);

    public static ushort SetLowByte(this ushort value, byte lowByte) => (ushort) ((uint) value & 65280U | (uint) lowByte);

    public static short SetLowByte(this short value, byte lowByte) => (short) ((int) value & 65280 | (int) lowByte);

    public static short CreateShort(byte lowByte, byte highByte) => (short) ((int) highByte << 8 | (int) lowByte);

    public static ushort CreateUshort(byte lowByte, byte highByte) => (ushort) ((uint) highByte << 8 | (uint) lowByte);

    public static Bit GetBit(this byte value, int bitPosition)
    {
      if (bitPosition < 0 || bitPosition > 7)
        throw new InvalidOperationException("bit position must be between 0 and 7");
      return (Bit) ((int) value & 1 << bitPosition);
    }

    public static byte WithBit(this byte number, int bitPosition, Bit value)
    {
      if (bitPosition < 0 || bitPosition > 7)
        throw new InvalidOperationException("bit position must be between 0 and 7");
      return (bool) value ? (byte) ((uint) number | (uint) (1 << bitPosition)) : (byte) ((uint) number & (uint) ~(1 << bitPosition));
    }

    public static short ToShort(this int value) => (short) (ushort) value;

    public static short ToShort(this ushort value) => (short) value;

    public static ushort ToUShort(this int value) => (ushort) value;

    public static ushort ToUShort(this short value) => (ushort) value;

    public static sbyte ToSignedByte(this int value) => (sbyte) value;

    public static sbyte ToSignedByte(this byte value) => (sbyte) value;

    public static short Inc(this short value) => (short) ((int) value + 1);

    public static ushort Inc(this ushort value) => (ushort) ((uint) value + 1U);

    public static ushort Dec(this ushort value) => (ushort) ((uint) value - 1U);

    public static short Dec(this short value) => (short) ((int) value - 1);

    public static short Add(this short value, short amount) => (short) ((int) value + (int) amount);

    public static ushort Add(this ushort value, ushort amount) => (ushort) ((uint) value + (uint) amount);

    public static ushort Add(this ushort value, sbyte amount) => (ushort) ((uint) value + (uint) amount);

    public static short Sub(this short value, short amount) => (short) ((int) value - (int) amount);

    public static ushort Sub(this ushort value, ushort amount) => (ushort) ((uint) value - (uint) amount);

    public static byte Inc(this byte value) => (byte) ((uint) value + 1U);

    public static byte Dec(this byte value) => (byte) ((uint) value - 1U);

    public static short Add(this byte value, byte amount) => (short) (byte) ((uint) value + (uint) amount);

    public static short Add(this byte value, int amount) => (short) (byte) ((uint) value + (uint) (byte) amount);

    public static short Sub(this byte value, byte amount) => (short) ((int) value - (int) amount);

    public static short Sub(this byte value, int amount) => (short) (byte) ((uint) value - (uint) (byte) amount);

    public static byte Inc7Bits(this byte value) => ((int) value & 128) == 0 ? (byte) ((int) value + 1 & (int) sbyte.MaxValue) : (byte) ((int) value + 1 & (int) sbyte.MaxValue | 128);

    public static bool Between(this byte value, byte fromInclusive, byte toInclusive) => (int) value >= (int) fromInclusive && (int) value <= (int) toInclusive;

    public static ushort AddSignedByte(this ushort value, byte amount) => amount >= (byte) 128 ? value.Sub((ushort) (256U - (uint) amount)) : value.Add((ushort) amount);

    public static byte[] ToByteArray(this short value) => new byte[2]
    {
      value.GetLowByte(),
      value.GetHighByte()
    };

    public static byte[] ToByteArray(this ushort value) => new byte[2]
    {
      value.GetLowByte(),
      value.GetHighByte()
    };
  }
}
