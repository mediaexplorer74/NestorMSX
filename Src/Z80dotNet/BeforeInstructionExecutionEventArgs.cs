// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.BeforeInstructionExecutionEventArgs
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

namespace Konamiman.Z80dotNet
{
  public class BeforeInstructionExecutionEventArgs : ProcessorEventArgs
  {
    public BeforeInstructionExecutionEventArgs(byte[] opcode, object localUserState)
    {
      this.Opcode = opcode;
      this.LocalUserState = localUserState;
    }

    public byte[] Opcode { get; set; }
  }
}
