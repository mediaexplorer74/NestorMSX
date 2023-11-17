// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.ClockSynchronizer
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

using System;
using System.Diagnostics;
using System.Threading;

namespace Konamiman.Z80dotNet
{
  public class ClockSynchronizer : IClockSynchronizer
  {
    private const int MinMicrosecondsToWait = 10000;
    private Stopwatch stopWatch = new Stopwatch();
    private Decimal accummulatedMicroseconds;

    public Decimal EffectiveClockFrequencyInMHz { get; set; }

    public void Start()
    {
      this.stopWatch.Reset();
      this.stopWatch.Start();
    }

    public void Stop() => this.stopWatch.Stop();

    public void TryWait(int periodLengthInCycles)
    {
      this.accummulatedMicroseconds += (Decimal) periodLengthInCycles / this.EffectiveClockFrequencyInMHz;
      Decimal num = this.accummulatedMicroseconds - (Decimal) this.stopWatch.ElapsedMilliseconds;
      if (!(num >= 10000M))
        return;
      Thread.Sleep((int) (num / 1000M));
      this.accummulatedMicroseconds = 0M;
      this.stopWatch.Reset();
    }
  }
}
