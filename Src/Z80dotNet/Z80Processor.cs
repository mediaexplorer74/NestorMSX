// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.Z80Processor
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

using Konamiman.Z80dotNet.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Konamiman.Z80dotNet
{
  public class Z80Processor : IZ80Processor, IZ80ProcessorAgent, IExecutionStopper
  {
    private const int MemorySpaceSize = 65536;
    private const int PortSpaceSize = 65536;
    private const Decimal MaxEffectiveClockSpeed = 100M;
    private const Decimal MinEffectiveClockSpeed = 0.001M;
    private const ushort NmiServiceRoutine = 102;
    private const byte NOP_opcode = 0;
    private const byte RST38h_opcode = 255;
    private const byte RETI_RETN_prefix = 237;
    private const byte RETI_opcode = 77;
    private const byte RETN_opcode = 69;
    private byte _InterruptMode;
    private IZ80Registers _Registers;
    private IMemory _Memory;
    private MemoryAccessMode[] memoryAccessModes = new MemoryAccessMode[65536];
    private IMemory _PortsSpace;
    private MemoryAccessMode[] portsAccessModes = new MemoryAccessMode[65536];
    private readonly object nmiInterruptPendingSync = new object();
    private bool _nmiInterruptPending;
    private Decimal effectiveClockFrequency;
    private Decimal _ClockFrequencyInMHz = 1M;
    private Decimal _ClockSpeedFactor = 1M;
    private byte[] memoryWaitStatesForM1 = new byte[65536];
    private byte[] memoryWaitStatesForNonM1 = new byte[65536];
    private byte[] portWaitStates = new byte[65536];
    private IZ80InstructionExecutor _InstructionExecutor;
    private IClockSynchronizer clockSynchronizer;
    protected InstructionExecutionContext executionContext;

    public Z80Processor()
    {
      this.ClockSynchronizer = (IClockSynchronizer) new Konamiman.Z80dotNet.ClockSynchronizer();
      this.ClockFrequencyInMHz = 4M;
      this.ClockSpeedFactor = 1M;
      this.AutoStopOnDiPlusHalt = true;
      this.AutoStopOnRetWithStackEmpty = false;
      this.StartOfStack = (short) -1;
      this.SetMemoryWaitStatesForM1((ushort) 0, 65536, (byte) 0);
      this.SetMemoryWaitStatesForNonM1((ushort) 0, 65536, (byte) 0);
      this.SetPortWaitStates((ushort) 0, 65536, (byte) 0);
      this.Memory = (IMemory) new PlainMemory(65536);
      this.PortsSpace = (IMemory) new PlainMemory(65536);
      this.SetMemoryAccessMode((ushort) 0, 65536, MemoryAccessMode.ReadAndWrite);
      this.SetPortsSpaceAccessMode((byte) 0, 65536, MemoryAccessMode.ReadAndWrite);
      this.Registers = (IZ80Registers) new Z80Registers();
      this.InterruptSources = (IList<IZ80InterruptSource>) new List<IZ80InterruptSource>();
      this.InstructionExecutor = (IZ80InstructionExecutor) new Z80InstructionExecutor();
      this.StopReason = StopReason.NeverRan;
      this.State = ProcessorState.Stopped;
    }

    public void Start(object userState = null)
    {
      if (userState != null)
        this.UserState = userState;
      this.Reset();
      this.TStatesElapsedSinceStart = 0UL;
      this.InstructionExecutionLoop();
    }

    public void Continue() => this.InstructionExecutionLoop();

    private int InstructionExecutionLoop(bool isSingleInstruction = false)
    {
      try
      {
        return this.InstructionExecutionLoopCore(isSingleInstruction);
      }
      catch
      {
        this.State = ProcessorState.Stopped;
        this.StopReason = StopReason.ExceptionThrown;
        throw;
      }
    }

    private int InstructionExecutionLoopCore(bool isSingleInstruction)
    {
      if (this.clockSynchronizer != null)
        this.clockSynchronizer.Start();
      this.executionContext = new InstructionExecutionContext();
      this.StopReason = StopReason.NotApplicable;
      this.State = ProcessorState.Running;
      int periodLengthInCycles = 0;
      while (!this.executionContext.MustStop)
      {
        this.executionContext.StartNewInstruction();
        this.FireBeforeInstructionFetchEvent();
        if (!this.executionContext.MustStop)
        {
          int tStates = this.ExecuteNextOpcode() + this.executionContext.AccummulatedMemoryWaitStates;
          this.TStatesElapsedSinceStart += (ulong) tStates;
          this.TStatesElapsedSinceReset += (ulong) tStates;
          this.ThrowIfNoFetchFinishedEventFired();
          if (!isSingleInstruction)
          {
            this.CheckAutoStopForHaltOnDi();
            this.CheckForAutoStopForRetWithStackEmpty();
            this.CheckForLdSpInstruction();
          }
          this.FireAfterInstructionExecutionEvent(tStates);
          if (!this.IsHalted)
            this.IsHalted = this.executionContext.IsHaltInstruction;
          int num = this.AcceptPendingInterrupt();
          periodLengthInCycles = tStates + num;
          this.TStatesElapsedSinceStart += (ulong) num;
          this.TStatesElapsedSinceReset += (ulong) num;
          if (isSingleInstruction)
            this.executionContext.StopReason = StopReason.ExecuteNextInstructionInvoked;
          else if (this.clockSynchronizer != null)
            this.clockSynchronizer.TryWait(periodLengthInCycles);
        }
        else
          break;
      }
      if (this.clockSynchronizer != null)
        this.clockSynchronizer.Stop();
      this.StopReason = this.executionContext.StopReason;
      this.State = this.StopReason == StopReason.PauseInvoked ? ProcessorState.Paused : ProcessorState.Stopped;
      this.executionContext = (InstructionExecutionContext) null;
      return periodLengthInCycles;
    }

    private int ExecuteNextOpcode()
    {
      if (!this.IsHalted)
        return this.InstructionExecutor.Execute(this.FetchNextOpcode());
      this.executionContext.OpcodeBytes.Add((byte) 0);
      return this.InstructionExecutor.Execute((byte) 0);
    }

    private int AcceptPendingInterrupt()
    {
      if (this.executionContext.IsEiOrDiInstruction)
        return 0;
      if (this.NmiInterruptPending)
      {
        this.IsHalted = false;
        this.Registers.IFF1 = (Bit) 0;
        this.ExecuteCall((ushort) 102);
        this.TriggerInterruptEvent(InterruptType.NonMaskable);
        return 11;
      }
      if (!this.InterruptsEnabled)
        return 0;
      IZ80InterruptSource z80InterruptSource = this.InterruptSources.FirstOrDefault<IZ80InterruptSource>((Func<IZ80InterruptSource, bool>) (s => s.IntLineIsActive));
      if (z80InterruptSource == null)
        return 0;
      this.Registers.IFF1 = (Bit) 0;
      this.Registers.IFF2 = (Bit) 0;
      this.IsHalted = false;
      switch (this.InterruptMode)
      {
        case 0:
          byte valueOrDefault = z80InterruptSource.ValueOnDataBus.GetValueOrDefault(byte.MaxValue);
          this.TriggerInterruptEvent(InterruptType.Maskable);
          this.InstructionExecutor.Execute(valueOrDefault);
          return 13;
        case 1:
          this.InstructionExecutor.Execute(byte.MaxValue);
          this.TriggerInterruptEvent(InterruptType.Maskable);
          return 13;
        case 2:
          ushort address = NumberUtils.CreateUshort(z80InterruptSource.ValueOnDataBus.GetValueOrDefault(byte.MaxValue), this.Registers.I);
          this.ExecuteCall(NumberUtils.CreateUshort(this.ReadFromMemoryInternal(address), this.ReadFromMemoryInternal((ushort) ((uint) address + 1U))));
          this.TriggerInterruptEvent(InterruptType.Maskable);
          return 19;
        default:
          return 0;
      }
    }

    public void ExecuteCall(ushort address)
    {
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.WriteToMemoryInternal(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.WriteToMemoryInternal(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = address;
    }

    private void TriggerInterruptEvent(InterruptType interruptType)
    {
      if (interruptType != InterruptType.Maskable)
      {
        if (interruptType != InterruptType.NonMaskable)
          throw new InvalidOperationException(string.Format("Unknown interrupt type: {0}", (object) interruptType));
        EventHandler interruptServicingStart = this.NonMaskableInterruptServicingStart;
        if (interruptServicingStart == null)
          return;
        interruptServicingStart((object) this, EventArgs.Empty);
      }
      else
      {
        EventHandler interruptServicingStart = this.MaskableInterruptServicingStart;
        if (interruptServicingStart == null)
          return;
        interruptServicingStart((object) this, EventArgs.Empty);
      }
    }

    public void ExecuteRet()
    {
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ReadFromMemoryInternal(sp), this.ReadFromMemoryInternal((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
    }

    private void ThrowIfNoFetchFinishedEventFired()
    {
      if (!this.executionContext.FetchComplete)
        throw new InstructionFetchFinishedEventNotFiredException((ushort) ((uint) this.Registers.PC - (uint) this.executionContext.OpcodeBytes.Count), this.executionContext.OpcodeBytes.ToArray());
    }

    private void CheckAutoStopForHaltOnDi()
    {
      if (!this.AutoStopOnDiPlusHalt || !this.executionContext.IsHaltInstruction || this.InterruptsEnabled)
        return;
      this.executionContext.StopReason = StopReason.DiPlusHalt;
    }

    private void CheckForAutoStopForRetWithStackEmpty()
    {
      if (!this.AutoStopOnRetWithStackEmpty || !this.executionContext.IsRetInstruction || !this.StackIsEmpty())
        return;
      this.executionContext.StopReason = StopReason.RetWithStackEmpty;
    }

    private void CheckForLdSpInstruction()
    {
      if (!this.executionContext.IsLdSpInstruction)
        return;
      this.StartOfStack = this.Registers.SP;
    }

    private bool StackIsEmpty() => (int) this.executionContext.SpAfterInstructionFetch == (int) this.StartOfStack;

    private bool InterruptsEnabled => this.Registers.IFF1 == 1;

    private void FireAfterInstructionExecutionEvent(int tStates)
    {
      byte[] array = this.executionContext.OpcodeBytes.ToArray();
      EventHandler<AfterInstructionExecutionEventArgs> instructionExecution1 = this.AfterInstructionExecution;
      if (instructionExecution1 != null)
        instructionExecution1((object) this, new AfterInstructionExecutionEventArgs(array, (IExecutionStopper) this, this.executionContext.LocalUserStateFromPreviousEvent, tStates));
      if (array[0] != (byte) 237)
        return;
      array[1] &= (byte) 207;
      if (array[1] == (byte) 77)
      {
        EventHandler instructionExecution2 = this.AfterRetiInstructionExecution;
        if (instructionExecution2 == null)
          return;
        instructionExecution2((object) this, EventArgs.Empty);
      }
      else
      {
        if (array[1] != (byte) 69)
          return;
        EventHandler instructionExecution3 = this.AfterRetnInstructionExecution;
        if (instructionExecution3 == null)
          return;
        instructionExecution3((object) this, EventArgs.Empty);
      }
    }

    private void InstructionExecutor_InstructionFetchFinished(
      object sender,
      InstructionFetchFinishedEventArgs e)
    {
      if (this.executionContext.FetchComplete)
        return;
      this.executionContext.FetchComplete = true;
      this.executionContext.IsRetInstruction = e.IsRetInstruction;
      this.executionContext.IsLdSpInstruction = e.IsLdSpInstruction;
      this.executionContext.IsHaltInstruction = e.IsHaltInstruction;
      this.executionContext.IsEiOrDiInstruction = e.IsEiOrDiInstruction;
      this.executionContext.SpAfterInstructionFetch = this.Registers.SP;
      this.executionContext.LocalUserStateFromPreviousEvent = this.FireBeforeInstructionExecutionEvent().LocalUserState;
    }

    private void FireBeforeInstructionFetchEvent()
    {
      BeforeInstructionFetchEventArgs e = new BeforeInstructionFetchEventArgs((IExecutionStopper) this);
      if (this.BeforeInstructionFetch != null)
      {
        this.executionContext.ExecutingBeforeInstructionEvent = true;
        try
        {
          this.BeforeInstructionFetch((object) this, e);
        }
        finally
        {
          this.executionContext.ExecutingBeforeInstructionEvent = false;
        }
      }
      this.executionContext.LocalUserStateFromPreviousEvent = e.LocalUserState;
    }

    private BeforeInstructionExecutionEventArgs FireBeforeInstructionExecutionEvent()
    {
      byte[] array = this.executionContext.OpcodeBytes.ToArray();
      BeforeInstructionExecutionEventArgs e = new BeforeInstructionExecutionEventArgs(array, this.executionContext.LocalUserStateFromPreviousEvent);
      EventHandler<BeforeInstructionExecutionEventArgs> instructionExecution1 = this.BeforeInstructionExecution;
      if (instructionExecution1 != null)
        instructionExecution1((object) this, e);
      if (array[0] == (byte) 237)
      {
        array[1] &= (byte) 207;
        if (array[1] == (byte) 77)
        {
          EventHandler instructionExecution2 = this.BeforeRetiInstructionExecution;
          if (instructionExecution2 != null)
            instructionExecution2((object) this, EventArgs.Empty);
        }
        else if (array[1] == (byte) 69)
        {
          EventHandler instructionExecution3 = this.BeforeRetnInstructionExecution;
          if (instructionExecution3 != null)
            instructionExecution3((object) this, EventArgs.Empty);
        }
      }
      return e;
    }

    public void Reset()
    {
      this.Registers.IFF1 = (Bit) 0;
      this.Registers.IFF2 = (Bit) 0;
      this.Registers.PC = (ushort) 0;
      this.Registers.AF = (short) -1;
      this.Registers.SP = (short) -1;
      this.InterruptMode = (byte) 0;
      this.NmiInterruptPending = false;
      this.IsHalted = false;
      this.TStatesElapsedSinceReset = 0UL;
      this.StartOfStack = this.Registers.SP;
    }

    public int ExecuteNextInstruction() => this.InstructionExecutionLoop(true);

    public ulong TStatesElapsedSinceStart { get; private set; }

    public ulong TStatesElapsedSinceReset { get; private set; }

    public StopReason StopReason { get; private set; }

    public ProcessorState State { get; private set; }

    public object UserState { get; set; }

    public bool IsHalted { get; protected set; }

    public byte InterruptMode
    {
      get => this._InterruptMode;
      set => this._InterruptMode = value <= (byte) 2 ? value : throw new ArgumentException("Interrupt mode can be set to 0, 1 or 2 only");
    }

    public short StartOfStack { get; protected set; }

    public IZ80Registers Registers
    {
      get => this._Registers;
      set => this._Registers = value != null ? value : throw new ArgumentNullException(nameof (Registers));
    }

    public IMemory Memory
    {
      get => this._Memory;
      set => this._Memory = value != null ? value : throw new ArgumentNullException(nameof (Memory));
    }

    public void SetMemoryAccessMode(ushort startAddress, int length, MemoryAccessMode mode) => this.SetArrayContents<MemoryAccessMode>(this.memoryAccessModes, startAddress, length, mode);

    private void SetArrayContents<T>(T[] array, ushort startIndex, int length, T value)
    {
      if (length < 0)
        throw new ArgumentException("length can't be negative");
      if ((int) startIndex + length > array.Length)
        throw new ArgumentException("start + length go beyond " + (array.Length - 1).ToString());
      Array.Copy((Array) Enumerable.Repeat<T>(value, length).ToArray<T>(), 0, (Array) array, (int) startIndex, length);
    }

    public MemoryAccessMode GetMemoryAccessMode(ushort address) => this.memoryAccessModes[(int) address];

    public IMemory PortsSpace
    {
      get => this._PortsSpace;
      set => this._PortsSpace = value != null ? value : throw new ArgumentNullException(nameof (PortsSpace));
    }

    public void SetPortsSpaceAccessMode(byte startPort, int length, MemoryAccessMode mode) => this.SetArrayContents<MemoryAccessMode>(this.portsAccessModes, (ushort) startPort, length, mode);

    public MemoryAccessMode GetPortAccessMode(ushort portNumber) => this.portsAccessModes[(int) portNumber];

    private IList<IZ80InterruptSource> InterruptSources { get; set; }

    public void RegisterInterruptSource(IZ80InterruptSource source)
    {
      if (this.InterruptSources.Contains(source))
        return;
      this.InterruptSources.Add(source);
      source.NmiInterruptPulse += (EventHandler) ((sender, args) => this.NmiInterruptPending = true);
    }

    private bool NmiInterruptPending
    {
      get
      {
        lock (this.nmiInterruptPendingSync)
        {
          int num = this._nmiInterruptPending ? 1 : 0;
          this._nmiInterruptPending = false;
          return num != 0;
        }
      }
      set
      {
        lock (this.nmiInterruptPendingSync)
          this._nmiInterruptPending = value;
      }
    }

    public IEnumerable<IZ80InterruptSource> GetRegisteredInterruptSources() => (IEnumerable<IZ80InterruptSource>) this.InterruptSources.ToArray<IZ80InterruptSource>();

    public void UnregisterAllInterruptSources()
    {
      foreach (IZ80InterruptSource interruptSource in (IEnumerable<IZ80InterruptSource>) this.InterruptSources)
        interruptSource.NmiInterruptPulse -= (EventHandler) ((sender, args) => this.NmiInterruptPending = true);
      this.InterruptSources.Clear();
    }

    public Decimal ClockFrequencyInMHz
    {
      get => this._ClockFrequencyInMHz;
      set
      {
        this.SetEffectiveClockFrequency(value, this.ClockSpeedFactor);
        this._ClockFrequencyInMHz = value;
      }
    }

    private void SetEffectiveClockFrequency(Decimal clockFrequency, Decimal clockSpeedFactor)
    {
      Decimal num = clockFrequency * clockSpeedFactor;
      this.effectiveClockFrequency = !(num > 100M) && !(num < 0.001M) ? num : throw new ArgumentException(string.Format("Clock frequency multiplied by clock speed factor must be a number between {0} and {1}", (object) 0.001M, (object) 100M));
      if (this.clockSynchronizer == null)
        return;
      this.clockSynchronizer.EffectiveClockFrequencyInMHz = num;
    }

    public Decimal ClockSpeedFactor
    {
      get => this._ClockSpeedFactor;
      set
      {
        this.SetEffectiveClockFrequency(this.ClockFrequencyInMHz, value);
        this._ClockSpeedFactor = value;
      }
    }

    public bool AutoStopOnDiPlusHalt { get; set; }

    public bool AutoStopOnRetWithStackEmpty { get; set; }

    public void SetMemoryWaitStatesForM1(ushort startAddress, int length, byte waitStates) => this.SetArrayContents<byte>(this.memoryWaitStatesForM1, startAddress, length, waitStates);

    public byte GetMemoryWaitStatesForM1(ushort address) => this.memoryWaitStatesForM1[(int) address];

    public void SetMemoryWaitStatesForNonM1(ushort startAddress, int length, byte waitStates) => this.SetArrayContents<byte>(this.memoryWaitStatesForNonM1, startAddress, length, waitStates);

    public byte GetMemoryWaitStatesForNonM1(ushort address) => this.memoryWaitStatesForNonM1[(int) address];

    public void SetPortWaitStates(ushort startPort, int length, byte waitStates) => this.SetArrayContents<byte>(this.portWaitStates, startPort, length, waitStates);

    public byte GetPortWaitStates(ushort portNumber) => this.portWaitStates[(int) portNumber];

    public IZ80InstructionExecutor InstructionExecutor
    {
      get => this._InstructionExecutor;
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (InstructionExecutor));
        if (this._InstructionExecutor != null)
          this._InstructionExecutor.InstructionFetchFinished -= new EventHandler<InstructionFetchFinishedEventArgs>(this.InstructionExecutor_InstructionFetchFinished);
        this._InstructionExecutor = value;
        this._InstructionExecutor.ProcessorAgent = (IZ80ProcessorAgent) this;
        this._InstructionExecutor.InstructionFetchFinished += new EventHandler<InstructionFetchFinishedEventArgs>(this.InstructionExecutor_InstructionFetchFinished);
      }
    }

    public IClockSynchronizer ClockSynchronizer
    {
      get => this.clockSynchronizer;
      set
      {
        this.clockSynchronizer = value;
        if (value == null)
          return;
        this.clockSynchronizer.EffectiveClockFrequencyInMHz = this.effectiveClockFrequency;
      }
    }

    public event EventHandler<MemoryAccessEventArgs> MemoryAccess;

    public event EventHandler<BeforeInstructionFetchEventArgs> BeforeInstructionFetch;

    public event EventHandler<BeforeInstructionExecutionEventArgs> BeforeInstructionExecution;

    public event EventHandler<AfterInstructionExecutionEventArgs> AfterInstructionExecution;

    public event EventHandler MaskableInterruptServicingStart;

    public event EventHandler NonMaskableInterruptServicingStart;

    public event EventHandler BeforeRetiInstructionExecution;

    public event EventHandler AfterRetiInstructionExecution;

    public event EventHandler BeforeRetnInstructionExecution;

    public event EventHandler AfterRetnInstructionExecution;

    public byte FetchNextOpcode()
    {
      this.FailIfNoExecutionContext();
      if (this.executionContext.FetchComplete)
        throw new InvalidOperationException("FetchNextOpcode can be invoked only before the InstructionFetchFinished event has been raised.");
      byte num;
      if (!this.executionContext.PeekedOpcode.HasValue)
      {
        ushort pc = this.Registers.PC;
        num = this.ReadFromMemoryOrPort(pc, this.Memory, this.GetMemoryAccessMode(pc), MemoryAccessEventType.BeforeMemoryRead, MemoryAccessEventType.AfterMemoryRead, this.GetMemoryWaitStatesForM1(pc));
      }
      else
      {
        this.executionContext.AccummulatedMemoryWaitStates += (int) this.GetMemoryWaitStatesForM1(this.executionContext.AddressOfPeekedOpcode);
        byte? nullable1 = this.executionContext.PeekedOpcode;
        num = nullable1.Value;
        InstructionExecutionContext executionContext = this.executionContext;
        nullable1 = new byte?();
        byte? nullable2 = nullable1;
        executionContext.PeekedOpcode = nullable2;
      }
      this.executionContext.OpcodeBytes.Add(num);
      ++this.Registers.PC;
      return num;
    }

    public byte PeekNextOpcode()
    {
      this.FailIfNoExecutionContext();
      if (this.executionContext.FetchComplete)
        throw new InvalidOperationException("PeekNextOpcode can be invoked only before the InstructionFetchFinished event has been raised.");
      if (this.executionContext.PeekedOpcode.HasValue)
        return this.executionContext.PeekedOpcode.Value;
      ushort pc = this.Registers.PC;
      byte num = this.ReadFromMemoryOrPort(pc, this.Memory, this.GetMemoryAccessMode(pc), MemoryAccessEventType.BeforeMemoryRead, MemoryAccessEventType.AfterMemoryRead, (byte) 0);
      this.executionContext.PeekedOpcode = new byte?(num);
      this.executionContext.AddressOfPeekedOpcode = this.Registers.PC;
      return num;
    }

    private void FailIfNoExecutionContext()
    {
      if (this.executionContext == null)
        throw new InvalidOperationException("This method can be invoked only when an instruction is being executed.");
    }

    public byte ReadFromMemory(ushort address)
    {
      this.FailIfNoExecutionContext();
      this.FailIfNoInstructionFetchComplete();
      return this.ReadFromMemoryInternal(address);
    }

    private byte ReadFromMemoryInternal(ushort address) => this.ReadFromMemoryOrPort(address, this.Memory, this.GetMemoryAccessMode(address), MemoryAccessEventType.BeforeMemoryRead, MemoryAccessEventType.AfterMemoryRead, this.GetMemoryWaitStatesForNonM1(address));

    protected virtual void FailIfNoInstructionFetchComplete()
    {
      if (this.executionContext != null && !this.executionContext.FetchComplete)
        throw new InvalidOperationException("IZ80ProcessorAgent members other than FetchNextOpcode can be invoked only after the InstructionFetchFinished event has been raised.");
    }

    private byte ReadFromMemoryOrPort(
      ushort address,
      IMemory memory,
      MemoryAccessMode accessMode,
      MemoryAccessEventType beforeEventType,
      MemoryAccessEventType afterEventType,
      byte waitStates)
    {
      MemoryAccessEventArgs memoryAccessEventArgs = this.FireMemoryAccessEvent(beforeEventType, address, byte.MaxValue);
      byte num = memoryAccessEventArgs.CancelMemoryAccess || accessMode != MemoryAccessMode.ReadAndWrite && accessMode != MemoryAccessMode.ReadOnly ? memoryAccessEventArgs.Value : memory[(int) address];
      if (this.executionContext != null)
        this.executionContext.AccummulatedMemoryWaitStates += (int) waitStates;
      return this.FireMemoryAccessEvent(afterEventType, address, num, memoryAccessEventArgs.LocalUserState, memoryAccessEventArgs.CancelMemoryAccess).Value;
    }

    private MemoryAccessEventArgs FireMemoryAccessEvent(
      MemoryAccessEventType eventType,
      ushort address,
      byte value,
      object localUserState = null,
      bool cancelMemoryAccess = false)
    {
      MemoryAccessEventArgs e = new MemoryAccessEventArgs(eventType, address, value, localUserState, cancelMemoryAccess);
      EventHandler<MemoryAccessEventArgs> memoryAccess = this.MemoryAccess;
      if (memoryAccess != null)
        memoryAccess((object) this, e);
      return e;
    }

    public void WriteToMemory(ushort address, byte value)
    {
      this.FailIfNoExecutionContext();
      this.FailIfNoInstructionFetchComplete();
      this.WriteToMemoryInternal(address, value);
    }

    private void WriteToMemoryInternal(ushort address, byte value) => this.WritetoMemoryOrPort(address, value, this.Memory, this.GetMemoryAccessMode(address), MemoryAccessEventType.BeforeMemoryWrite, MemoryAccessEventType.AfterMemoryWrite, this.GetMemoryWaitStatesForNonM1(address));

    private void WritetoMemoryOrPort(
      ushort address,
      byte value,
      IMemory memory,
      MemoryAccessMode accessMode,
      MemoryAccessEventType beforeEventType,
      MemoryAccessEventType afterEventType,
      byte waitStates)
    {
      MemoryAccessEventArgs memoryAccessEventArgs = this.FireMemoryAccessEvent(beforeEventType, address, value);
      if (!memoryAccessEventArgs.CancelMemoryAccess && (accessMode == MemoryAccessMode.ReadAndWrite || accessMode == MemoryAccessMode.WriteOnly))
        memory[(int) address] = memoryAccessEventArgs.Value;
      if (this.executionContext != null)
        this.executionContext.AccummulatedMemoryWaitStates += (int) waitStates;
      this.FireMemoryAccessEvent(afterEventType, address, memoryAccessEventArgs.Value, memoryAccessEventArgs.LocalUserState, memoryAccessEventArgs.CancelMemoryAccess);
    }

    public byte ReadFromPort(ushort portNumber)
    {
      this.FailIfNoExecutionContext();
      this.FailIfNoInstructionFetchComplete();
      return this.ReadFromMemoryOrPort(portNumber, this.PortsSpace, this.GetPortAccessMode(portNumber), MemoryAccessEventType.BeforePortRead, MemoryAccessEventType.AfterPortRead, this.GetPortWaitStates(portNumber));
    }

    public void WriteToPort(ushort portNumber, byte value)
    {
      this.FailIfNoExecutionContext();
      this.FailIfNoInstructionFetchComplete();
      this.WritetoMemoryOrPort(portNumber, value, this.PortsSpace, this.GetPortAccessMode(portNumber), MemoryAccessEventType.BeforePortWrite, MemoryAccessEventType.AfterPortWrite, this.GetPortWaitStates(portNumber));
    }

    public void SetInterruptMode(byte interruptMode)
    {
      this.FailIfNoExecutionContext();
      this.FailIfNoInstructionFetchComplete();
      this.InterruptMode = interruptMode;
    }

    public void Stop(bool isPause = false)
    {
      this.FailIfNoExecutionContext();
      if (!this.executionContext.ExecutingBeforeInstructionEvent)
        this.FailIfNoInstructionFetchComplete();
      this.executionContext.StopReason = isPause ? StopReason.PauseInvoked : StopReason.StopInvoked;
    }

    IZ80Registers IZ80ProcessorAgent.Registers => this._Registers;
  }
}
