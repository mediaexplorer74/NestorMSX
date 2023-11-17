// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.IZ80ProcessorAgent
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

namespace Konamiman.Z80dotNet
{
  public interface IZ80ProcessorAgent : IExecutionStopper
  {
    byte FetchNextOpcode();

    byte PeekNextOpcode();

    byte ReadFromMemory(ushort address);

    void WriteToMemory(ushort address, byte value);

    byte ReadFromPort(ushort portNumber);

    void WriteToPort(ushort portNumber, byte value);

    IZ80Registers Registers { get; }

    void SetInterruptMode(byte interruptMode);
  }
}
