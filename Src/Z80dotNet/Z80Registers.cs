// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.Z80Registers
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

using System;

namespace Konamiman.Z80dotNet
{
  public class Z80Registers : MainZ80Registers, IZ80Registers, IMainZ80Registers
  {
    private IMainZ80Registers _Alternate;

    public Z80Registers() => this.Alternate = (IMainZ80Registers) new MainZ80Registers();

    public IMainZ80Registers Alternate
    {
      get => this._Alternate;
      set => this._Alternate = value != null ? value : throw new ArgumentNullException(nameof (Alternate));
    }

    public short IX { get; set; }

    public short IY { get; set; }

    public ushort PC { get; set; }

    public short SP { get; set; }

    public short IR { get; set; }

    public Bit IFF1 { get; set; }

    public Bit IFF2 { get; set; }

    public byte IXH
    {
      get => this.IX.GetHighByte();
      set => this.IX = this.IX.SetHighByte(value);
    }

    public byte IXL
    {
      get => this.IX.GetLowByte();
      set => this.IX = this.IX.SetLowByte(value);
    }

    public byte IYH
    {
      get => this.IY.GetHighByte();
      set => this.IY = this.IY.SetHighByte(value);
    }

    public byte IYL
    {
      get => this.IY.GetLowByte();
      set => this.IY = this.IY.SetLowByte(value);
    }

    public byte I
    {
      get => this.IR.GetHighByte();
      set => this.IR = this.IR.SetHighByte(value);
    }

    public byte R
    {
      get => this.IR.GetLowByte();
      set => this.IR = this.IR.SetLowByte(value);
    }
  }
}
