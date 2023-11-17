// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.IZ80Processor
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

using System;
using System.Collections.Generic;

namespace Konamiman.Z80dotNet
{
  public interface IZ80Processor
  {
    void Start(object userState = null);

    void Continue();

    void Reset();

    int ExecuteNextInstruction();

    ulong TStatesElapsedSinceStart { get; }

    ulong TStatesElapsedSinceReset { get; }

    StopReason StopReason { get; }

    ProcessorState State { get; }

    object UserState { get; set; }

    bool IsHalted { get; }

    byte InterruptMode { get; set; }

    short StartOfStack { get; }

    IZ80Registers Registers { get; set; }

    IMemory Memory { get; set; }

    void SetMemoryAccessMode(ushort startAddress, int length, MemoryAccessMode mode);

    MemoryAccessMode GetMemoryAccessMode(ushort address);

    IMemory PortsSpace { get; set; }

    void SetPortsSpaceAccessMode(byte startPort, int length, MemoryAccessMode mode);

    MemoryAccessMode GetPortAccessMode(ushort portNumber);

    void RegisterInterruptSource(IZ80InterruptSource source);

    IEnumerable<IZ80InterruptSource> GetRegisteredInterruptSources();

    void UnregisterAllInterruptSources();

    Decimal ClockFrequencyInMHz { get; set; }

    Decimal ClockSpeedFactor { get; set; }

    bool AutoStopOnDiPlusHalt { get; set; }

    bool AutoStopOnRetWithStackEmpty { get; set; }

    void SetMemoryWaitStatesForM1(ushort startAddress, int length, byte waitStates);

    byte GetMemoryWaitStatesForM1(ushort address);

    void SetMemoryWaitStatesForNonM1(ushort startAddress, int length, byte waitStates);

    byte GetMemoryWaitStatesForNonM1(ushort address);

    void SetPortWaitStates(ushort startPort, int length, byte waitStates);

    byte GetPortWaitStates(ushort portNumber);

    IZ80InstructionExecutor InstructionExecutor { get; set; }

    IClockSynchronizer ClockSynchronizer { get; set; }

    event EventHandler<MemoryAccessEventArgs> MemoryAccess;

    event EventHandler<BeforeInstructionFetchEventArgs> BeforeInstructionFetch;

    event EventHandler<BeforeInstructionExecutionEventArgs> BeforeInstructionExecution;

    event EventHandler<AfterInstructionExecutionEventArgs> AfterInstructionExecution;

    event EventHandler MaskableInterruptServicingStart;

    event EventHandler NonMaskableInterruptServicingStart;

    event EventHandler BeforeRetiInstructionExecution;

    event EventHandler AfterRetiInstructionExecution;

    event EventHandler BeforeRetnInstructionExecution;

    event EventHandler AfterRetnInstructionExecution;

    void ExecuteCall(ushort address);

    void ExecuteRet();
  }
}
