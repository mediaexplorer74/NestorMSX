// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.IZ80Registers
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

namespace Konamiman.Z80dotNet
{
  public interface IZ80Registers : IMainZ80Registers
  {
    IMainZ80Registers Alternate { get; set; }

    short IX { get; set; }

    short IY { get; set; }

    ushort PC { get; set; }

    short SP { get; set; }

    short IR { get; set; }

    Bit IFF1 { get; set; }

    Bit IFF2 { get; set; }

    byte IXH { get; set; }

    byte IXL { get; set; }

    byte IYH { get; set; }

    byte IYL { get; set; }

    byte I { get; set; }

    byte R { get; set; }
  }
}
