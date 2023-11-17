// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.InstructionFetchFinishedEventNotFiredException
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

using System;

namespace Konamiman.Z80dotNet
{
  public class InstructionFetchFinishedEventNotFiredException : Exception
  {
    public ushort InstructionAddress { get; set; }

    public byte[] FetchedBytes { get; set; }

    public InstructionFetchFinishedEventNotFiredException(
      ushort instructionAddress,
      byte[] fetchedBytes,
      string message = null,
      Exception innerException = null)
      : base(message ?? "IZ80InstructionExecutor.Execute returned without having fired the InstructionFetchFinished event.", innerException)
    {
      this.InstructionAddress = instructionAddress;
      this.FetchedBytes = fetchedBytes;
    }
  }
}
