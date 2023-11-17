// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.IMainZ80Registers
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

namespace Konamiman.Z80dotNet
{
  public interface IMainZ80Registers
  {
    short AF { get; set; }

    short BC { get; set; }

    short DE { get; set; }

    short HL { get; set; }

    byte A { get; set; }

    byte F { get; set; }

    byte B { get; set; }

    byte C { get; set; }

    byte D { get; set; }

    byte E { get; set; }

    byte H { get; set; }

    byte L { get; set; }

    Bit CF { get; set; }

    Bit NF { get; set; }

    Bit PF { get; set; }

    Bit Flag3 { get; set; }

    Bit HF { get; set; }

    Bit Flag5 { get; set; }

    Bit ZF { get; set; }

    Bit SF { get; set; }
  }
}
