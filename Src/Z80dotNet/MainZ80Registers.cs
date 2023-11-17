// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.MainZ80Registers
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

namespace Konamiman.Z80dotNet
{
  public class MainZ80Registers : IMainZ80Registers
  {
    public short AF { get; set; }

    public short BC { get; set; }

    public short DE { get; set; }

    public short HL { get; set; }

    public byte A
    {
      get => this.AF.GetHighByte();
      set => this.AF = this.AF.SetHighByte(value);
    }

    public byte F
    {
      get => this.AF.GetLowByte();
      set => this.AF = this.AF.SetLowByte(value);
    }

    public byte B
    {
      get => this.BC.GetHighByte();
      set => this.BC = this.BC.SetHighByte(value);
    }

    public byte C
    {
      get => this.BC.GetLowByte();
      set => this.BC = this.BC.SetLowByte(value);
    }

    public byte D
    {
      get => this.DE.GetHighByte();
      set => this.DE = this.DE.SetHighByte(value);
    }

    public byte E
    {
      get => this.DE.GetLowByte();
      set => this.DE = this.DE.SetLowByte(value);
    }

    public byte H
    {
      get => this.HL.GetHighByte();
      set => this.HL = this.HL.SetHighByte(value);
    }

    public byte L
    {
      get => this.HL.GetLowByte();
      set => this.HL = this.HL.SetLowByte(value);
    }

    public Bit CF
    {
      get => this.F.GetBit(0);
      set => this.F = this.F.WithBit(0, value);
    }

    public Bit NF
    {
      get => this.F.GetBit(1);
      set => this.F = this.F.WithBit(1, value);
    }

    public Bit PF
    {
      get => this.F.GetBit(2);
      set => this.F = this.F.WithBit(2, value);
    }

    public Bit Flag3
    {
      get => this.F.GetBit(3);
      set => this.F = this.F.WithBit(3, value);
    }

    public Bit HF
    {
      get => this.F.GetBit(4);
      set => this.F = this.F.WithBit(4, value);
    }

    public Bit Flag5
    {
      get => this.F.GetBit(5);
      set => this.F = this.F.WithBit(5, value);
    }

    public Bit ZF
    {
      get => this.F.GetBit(6);
      set => this.F = this.F.WithBit(6, value);
    }

    public Bit SF
    {
      get => this.F.GetBit(7);
      set => this.F = this.F.WithBit(7, value);
    }
  }
}
