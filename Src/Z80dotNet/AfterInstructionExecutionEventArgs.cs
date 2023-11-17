// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.AfterInstructionExecutionEventArgs
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

namespace Konamiman.Z80dotNet
{
  public class AfterInstructionExecutionEventArgs : ProcessorEventArgs
  {
    public AfterInstructionExecutionEventArgs(
      byte[] opcode,
      IExecutionStopper stopper,
      object localUserState,
      int tStates)
    {
      this.Opcode = opcode;
      this.ExecutionStopper = stopper;
      this.LocalUserState = localUserState;
      this.TotalTStates = tStates;
    }

    public byte[] Opcode { get; set; }

    public IExecutionStopper ExecutionStopper { get; private set; }

    public int TotalTStates { get; private set; }
  }
}
