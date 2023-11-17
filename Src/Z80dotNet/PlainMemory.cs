// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.PlainMemory
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Konamiman.Z80dotNet
{
  public class PlainMemory : IMemory
  {
    private readonly byte[] memory;

    public PlainMemory(int size)
    {
      this.memory = size >= 1 ? new byte[size] : throw new InvalidOperationException("Memory size must be greater than zero");
      this.Size = size;
    }

    public int Size { get; private set; }

    public byte this[int address]
    {
      get => this.memory[address];
      set => this.memory[address] = value;
    }

    public void SetContents(int startAddress, byte[] contents, int startIndex = 0, int? length = null)
    {
      if (contents == null)
        throw new ArgumentNullException(nameof (contents));
      if (!length.HasValue)
        length = new int?(contents.Length);
      int num1 = startIndex;
      int? nullable1 = length;
      int? nullable2 = nullable1.HasValue ? new int?(num1 + nullable1.GetValueOrDefault()) : new int?();
      int length1 = contents.Length;
      if (nullable2.GetValueOrDefault() > length1 & nullable2.HasValue)
        throw new IndexOutOfRangeException("startIndex + length cannot be greater than contents.length");
      if (startIndex < 0)
        throw new IndexOutOfRangeException("startIndex cannot be negative");
      int num2 = startAddress;
      nullable1 = length;
      int? nullable3 = nullable1.HasValue ? new int?(num2 + nullable1.GetValueOrDefault()) : new int?();
      int size = this.Size;
      if (nullable3.GetValueOrDefault() > size & nullable3.HasValue)
        throw new IndexOutOfRangeException("startAddress + length cannot go beyond the memory size");
      Array.Copy((Array) contents, startIndex, (Array) this.memory, startAddress, length.Value);
    }

    public byte[] GetContents(int startAddress, int length)
    {
      if (startAddress >= this.memory.Length)
        throw new IndexOutOfRangeException("startAddress cannot go beyond memory size");
      if (startAddress + length > this.memory.Length)
        throw new IndexOutOfRangeException("startAddress + length cannot go beyond memory size");
      if (startAddress < 0)
        throw new IndexOutOfRangeException("startAddress cannot be negative");
      return ((IEnumerable<byte>) this.memory).Skip<byte>(startAddress).Take<byte>(length).ToArray<byte>();
    }
  }
}
