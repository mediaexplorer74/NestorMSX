// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.MemoryAccessEventArgs
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

namespace Konamiman.Z80dotNet
{
  public class MemoryAccessEventArgs : ProcessorEventArgs
  {
    public MemoryAccessEventArgs(
      MemoryAccessEventType eventType,
      ushort address,
      byte value,
      object localUserState = null,
      bool cancelMemoryAccess = false)
    {
      this.EventType = eventType;
      this.Address = address;
      this.Value = value;
      this.LocalUserState = localUserState;
      this.CancelMemoryAccess = cancelMemoryAccess;
    }

    public MemoryAccessEventType EventType { get; private set; }

    public ushort Address { get; private set; }

    public byte Value { get; set; }

    public bool CancelMemoryAccess { get; set; }
  }
}
