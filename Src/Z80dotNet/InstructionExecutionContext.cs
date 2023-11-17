// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.InstructionExecutionContext
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

using System.Collections.Generic;

namespace Konamiman.Z80dotNet
{
  public class InstructionExecutionContext
  {
    public InstructionExecutionContext()
    {
      this.StopReason = StopReason.NotApplicable;
      this.OpcodeBytes = new List<byte>();
    }

    public StopReason StopReason { get; set; }

    public bool MustStop => this.StopReason != 0;

    public void StartNewInstruction()
    {
      this.OpcodeBytes.Clear();
      this.FetchComplete = false;
      this.LocalUserStateFromPreviousEvent = (object) null;
      this.AccummulatedMemoryWaitStates = 0;
      this.PeekedOpcode = new byte?();
      this.IsEiOrDiInstruction = false;
    }

    public bool ExecutingBeforeInstructionEvent { get; set; }

    public bool FetchComplete { get; set; }

    public List<byte> OpcodeBytes { get; set; }

    public bool IsRetInstruction { get; set; }

    public bool IsLdSpInstruction { get; set; }

    public bool IsHaltInstruction { get; set; }

    public bool IsEiOrDiInstruction { get; set; }

    public short SpAfterInstructionFetch { get; set; }

    public object LocalUserStateFromPreviousEvent { get; set; }

    public int AccummulatedMemoryWaitStates { get; set; }

    public byte? PeekedOpcode { get; set; }

    public ushort AddressOfPeekedOpcode { get; set; }
  }
}
