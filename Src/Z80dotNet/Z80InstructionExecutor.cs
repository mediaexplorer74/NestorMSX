// Decompiled with JetBrains decompiler
// Type: Konamiman.Z80dotNet.Z80InstructionExecutor
// Assembly: Z80dotNet, Version=1.0.7.0, Culture=neutral, PublicKeyToken=null
// MVID: A179E7C4-15E1-42B8-A0BF-D766442C49E2
// Assembly location: C:\Users\Admin\Desktop\RE\YTSpectrum\Z80dotNet.dll

using System;
using System.Collections.Generic;

namespace Konamiman.Z80dotNet
{
  public class Z80InstructionExecutor : IZ80InstructionExecutor
  {
    private Func<byte>[] CB_InstructionExecutors;
    private Func<byte, byte>[] FDCB_InstructionExecutors;
    private Func<byte, byte>[] DDCB_InstructionExecutors;
    private IDictionary<byte, Func<byte>> DD_InstructionExecutors;
    private IDictionary<byte, Func<byte>> FD_InstructionExecutors;
    private Func<byte>[] ED_InstructionExecutors;
    private Func<byte>[] ED_Block_InstructionExecutors;
    private Func<byte>[] SingleByte_InstructionExecutors;
    private Bit[] Parity;
    private const int HF_NF_reset = 237;
    private const int CF_set = 1;
    private IZ80Registers Registers;

    private int Execute_DD_Instruction()
    {
      this.Inc_R();
      byte key = this.ProcessorAgent.PeekNextOpcode();
      if (key == (byte) 203)
      {
        this.Inc_R();
        int num1 = (int) this.ProcessorAgent.FetchNextOpcode();
        byte num2 = this.ProcessorAgent.FetchNextOpcode();
        return (int) this.DDCB_InstructionExecutors[(int) this.ProcessorAgent.FetchNextOpcode()](num2);
      }
      if (!this.DD_InstructionExecutors.ContainsKey(key))
        return (int) this.NOP();
      this.Inc_R();
      int num = (int) this.ProcessorAgent.FetchNextOpcode();
      return (int) this.DD_InstructionExecutors[key]();
    }

    private int Execute_FD_Instruction()
    {
      this.Inc_R();
      byte key = this.ProcessorAgent.PeekNextOpcode();
      if (key == (byte) 203)
      {
        this.Inc_R();
        int num1 = (int) this.ProcessorAgent.FetchNextOpcode();
        byte num2 = this.ProcessorAgent.FetchNextOpcode();
        return (int) this.FDCB_InstructionExecutors[(int) this.ProcessorAgent.FetchNextOpcode()](num2);
      }
      if (!this.FD_InstructionExecutors.ContainsKey(key))
        return (int) this.NOP();
      this.Inc_R();
      int num = (int) this.ProcessorAgent.FetchNextOpcode();
      return (int) this.FD_InstructionExecutors[key]();
    }

    private void Initialize_CB_InstructionsTable() => this.CB_InstructionExecutors = new Func<byte>[256]
    {
      new Func<byte>(this.RLC_B),
      new Func<byte>(this.RLC_C),
      new Func<byte>(this.RLC_D),
      new Func<byte>(this.RLC_E),
      new Func<byte>(this.RLC_H),
      new Func<byte>(this.RLC_L),
      new Func<byte>(this.RLC_aHL),
      new Func<byte>(this.RLC_A),
      new Func<byte>(this.RRC_B),
      new Func<byte>(this.RRC_C),
      new Func<byte>(this.RRC_D),
      new Func<byte>(this.RRC_E),
      new Func<byte>(this.RRC_H),
      new Func<byte>(this.RRC_L),
      new Func<byte>(this.RRC_aHL),
      new Func<byte>(this.RRC_A),
      new Func<byte>(this.RL_B),
      new Func<byte>(this.RL_C),
      new Func<byte>(this.RL_D),
      new Func<byte>(this.RL_E),
      new Func<byte>(this.RL_H),
      new Func<byte>(this.RL_L),
      new Func<byte>(this.RL_aHL),
      new Func<byte>(this.RL_A),
      new Func<byte>(this.RR_B),
      new Func<byte>(this.RR_C),
      new Func<byte>(this.RR_D),
      new Func<byte>(this.RR_E),
      new Func<byte>(this.RR_H),
      new Func<byte>(this.RR_L),
      new Func<byte>(this.RR_aHL),
      new Func<byte>(this.RR_A),
      new Func<byte>(this.SLA_B),
      new Func<byte>(this.SLA_C),
      new Func<byte>(this.SLA_D),
      new Func<byte>(this.SLA_E),
      new Func<byte>(this.SLA_H),
      new Func<byte>(this.SLA_L),
      new Func<byte>(this.SLA_aHL),
      new Func<byte>(this.SLA_A),
      new Func<byte>(this.SRA_B),
      new Func<byte>(this.SRA_C),
      new Func<byte>(this.SRA_D),
      new Func<byte>(this.SRA_E),
      new Func<byte>(this.SRA_H),
      new Func<byte>(this.SRA_L),
      new Func<byte>(this.SRA_aHL),
      new Func<byte>(this.SRA_A),
      new Func<byte>(this.SLL_B),
      new Func<byte>(this.SLL_C),
      new Func<byte>(this.SLL_D),
      new Func<byte>(this.SLL_E),
      new Func<byte>(this.SLL_H),
      new Func<byte>(this.SLL_L),
      new Func<byte>(this.SLL_aHL),
      new Func<byte>(this.SLL_A),
      new Func<byte>(this.SRL_B),
      new Func<byte>(this.SRL_C),
      new Func<byte>(this.SRL_D),
      new Func<byte>(this.SRL_E),
      new Func<byte>(this.SRL_H),
      new Func<byte>(this.SRL_L),
      new Func<byte>(this.SRL_aHL),
      new Func<byte>(this.SRL_A),
      new Func<byte>(this.BIT_0_B),
      new Func<byte>(this.BIT_0_C),
      new Func<byte>(this.BIT_0_D),
      new Func<byte>(this.BIT_0_E),
      new Func<byte>(this.BIT_0_H),
      new Func<byte>(this.BIT_0_L),
      new Func<byte>(this.BIT_0_aHL),
      new Func<byte>(this.BIT_0_A),
      new Func<byte>(this.BIT_1_B),
      new Func<byte>(this.BIT_1_C),
      new Func<byte>(this.BIT_1_D),
      new Func<byte>(this.BIT_1_E),
      new Func<byte>(this.BIT_1_H),
      new Func<byte>(this.BIT_1_L),
      new Func<byte>(this.BIT_1_aHL),
      new Func<byte>(this.BIT_1_A),
      new Func<byte>(this.BIT_2_B),
      new Func<byte>(this.BIT_2_C),
      new Func<byte>(this.BIT_2_D),
      new Func<byte>(this.BIT_2_E),
      new Func<byte>(this.BIT_2_H),
      new Func<byte>(this.BIT_2_L),
      new Func<byte>(this.BIT_2_aHL),
      new Func<byte>(this.BIT_2_A),
      new Func<byte>(this.BIT_3_B),
      new Func<byte>(this.BIT_3_C),
      new Func<byte>(this.BIT_3_D),
      new Func<byte>(this.BIT_3_E),
      new Func<byte>(this.BIT_3_H),
      new Func<byte>(this.BIT_3_L),
      new Func<byte>(this.BIT_3_aHL),
      new Func<byte>(this.BIT_3_A),
      new Func<byte>(this.BIT_4_B),
      new Func<byte>(this.BIT_4_C),
      new Func<byte>(this.BIT_4_D),
      new Func<byte>(this.BIT_4_E),
      new Func<byte>(this.BIT_4_H),
      new Func<byte>(this.BIT_4_L),
      new Func<byte>(this.BIT_4_aHL),
      new Func<byte>(this.BIT_4_A),
      new Func<byte>(this.BIT_5_B),
      new Func<byte>(this.BIT_5_C),
      new Func<byte>(this.BIT_5_D),
      new Func<byte>(this.BIT_5_E),
      new Func<byte>(this.BIT_5_H),
      new Func<byte>(this.BIT_5_L),
      new Func<byte>(this.BIT_5_aHL),
      new Func<byte>(this.BIT_5_A),
      new Func<byte>(this.BIT_6_B),
      new Func<byte>(this.BIT_6_C),
      new Func<byte>(this.BIT_6_D),
      new Func<byte>(this.BIT_6_E),
      new Func<byte>(this.BIT_6_H),
      new Func<byte>(this.BIT_6_L),
      new Func<byte>(this.BIT_6_aHL),
      new Func<byte>(this.BIT_6_A),
      new Func<byte>(this.BIT_7_B),
      new Func<byte>(this.BIT_7_C),
      new Func<byte>(this.BIT_7_D),
      new Func<byte>(this.BIT_7_E),
      new Func<byte>(this.BIT_7_H),
      new Func<byte>(this.BIT_7_L),
      new Func<byte>(this.BIT_7_aHL),
      new Func<byte>(this.BIT_7_A),
      new Func<byte>(this.RES_0_B),
      new Func<byte>(this.RES_0_C),
      new Func<byte>(this.RES_0_D),
      new Func<byte>(this.RES_0_E),
      new Func<byte>(this.RES_0_H),
      new Func<byte>(this.RES_0_L),
      new Func<byte>(this.RES_0_aHL),
      new Func<byte>(this.RES_0_A),
      new Func<byte>(this.RES_1_B),
      new Func<byte>(this.RES_1_C),
      new Func<byte>(this.RES_1_D),
      new Func<byte>(this.RES_1_E),
      new Func<byte>(this.RES_1_H),
      new Func<byte>(this.RES_1_L),
      new Func<byte>(this.RES_1_aHL),
      new Func<byte>(this.RES_1_A),
      new Func<byte>(this.RES_2_B),
      new Func<byte>(this.RES_2_C),
      new Func<byte>(this.RES_2_D),
      new Func<byte>(this.RES_2_E),
      new Func<byte>(this.RES_2_H),
      new Func<byte>(this.RES_2_L),
      new Func<byte>(this.RES_2_aHL),
      new Func<byte>(this.RES_2_A),
      new Func<byte>(this.RES_3_B),
      new Func<byte>(this.RES_3_C),
      new Func<byte>(this.RES_3_D),
      new Func<byte>(this.RES_3_E),
      new Func<byte>(this.RES_3_H),
      new Func<byte>(this.RES_3_L),
      new Func<byte>(this.RES_3_aHL),
      new Func<byte>(this.RES_3_A),
      new Func<byte>(this.RES_4_B),
      new Func<byte>(this.RES_4_C),
      new Func<byte>(this.RES_4_D),
      new Func<byte>(this.RES_4_E),
      new Func<byte>(this.RES_4_H),
      new Func<byte>(this.RES_4_L),
      new Func<byte>(this.RES_4_aHL),
      new Func<byte>(this.RES_4_A),
      new Func<byte>(this.RES_5_B),
      new Func<byte>(this.RES_5_C),
      new Func<byte>(this.RES_5_D),
      new Func<byte>(this.RES_5_E),
      new Func<byte>(this.RES_5_H),
      new Func<byte>(this.RES_5_L),
      new Func<byte>(this.RES_5_aHL),
      new Func<byte>(this.RES_5_A),
      new Func<byte>(this.RES_6_B),
      new Func<byte>(this.RES_6_C),
      new Func<byte>(this.RES_6_D),
      new Func<byte>(this.RES_6_E),
      new Func<byte>(this.RES_6_H),
      new Func<byte>(this.RES_6_L),
      new Func<byte>(this.RES_6_aHL),
      new Func<byte>(this.RES_6_A),
      new Func<byte>(this.RES_7_B),
      new Func<byte>(this.RES_7_C),
      new Func<byte>(this.RES_7_D),
      new Func<byte>(this.RES_7_E),
      new Func<byte>(this.RES_7_H),
      new Func<byte>(this.RES_7_L),
      new Func<byte>(this.RES_7_aHL),
      new Func<byte>(this.RES_7_A),
      new Func<byte>(this.SET_0_B),
      new Func<byte>(this.SET_0_C),
      new Func<byte>(this.SET_0_D),
      new Func<byte>(this.SET_0_E),
      new Func<byte>(this.SET_0_H),
      new Func<byte>(this.SET_0_L),
      new Func<byte>(this.SET_0_aHL),
      new Func<byte>(this.SET_0_A),
      new Func<byte>(this.SET_1_B),
      new Func<byte>(this.SET_1_C),
      new Func<byte>(this.SET_1_D),
      new Func<byte>(this.SET_1_E),
      new Func<byte>(this.SET_1_H),
      new Func<byte>(this.SET_1_L),
      new Func<byte>(this.SET_1_aHL),
      new Func<byte>(this.SET_1_A),
      new Func<byte>(this.SET_2_B),
      new Func<byte>(this.SET_2_C),
      new Func<byte>(this.SET_2_D),
      new Func<byte>(this.SET_2_E),
      new Func<byte>(this.SET_2_H),
      new Func<byte>(this.SET_2_L),
      new Func<byte>(this.SET_2_aHL),
      new Func<byte>(this.SET_2_A),
      new Func<byte>(this.SET_3_B),
      new Func<byte>(this.SET_3_C),
      new Func<byte>(this.SET_3_D),
      new Func<byte>(this.SET_3_E),
      new Func<byte>(this.SET_3_H),
      new Func<byte>(this.SET_3_L),
      new Func<byte>(this.SET_3_aHL),
      new Func<byte>(this.SET_3_A),
      new Func<byte>(this.SET_4_B),
      new Func<byte>(this.SET_4_C),
      new Func<byte>(this.SET_4_D),
      new Func<byte>(this.SET_4_E),
      new Func<byte>(this.SET_4_H),
      new Func<byte>(this.SET_4_L),
      new Func<byte>(this.SET_4_aHL),
      new Func<byte>(this.SET_4_A),
      new Func<byte>(this.SET_5_B),
      new Func<byte>(this.SET_5_C),
      new Func<byte>(this.SET_5_D),
      new Func<byte>(this.SET_5_E),
      new Func<byte>(this.SET_5_H),
      new Func<byte>(this.SET_5_L),
      new Func<byte>(this.SET_5_aHL),
      new Func<byte>(this.SET_5_A),
      new Func<byte>(this.SET_6_B),
      new Func<byte>(this.SET_6_C),
      new Func<byte>(this.SET_6_D),
      new Func<byte>(this.SET_6_E),
      new Func<byte>(this.SET_6_H),
      new Func<byte>(this.SET_6_L),
      new Func<byte>(this.SET_6_aHL),
      new Func<byte>(this.SET_6_A),
      new Func<byte>(this.SET_7_B),
      new Func<byte>(this.SET_7_C),
      new Func<byte>(this.SET_7_D),
      new Func<byte>(this.SET_7_E),
      new Func<byte>(this.SET_7_H),
      new Func<byte>(this.SET_7_L),
      new Func<byte>(this.SET_7_aHL),
      new Func<byte>(this.SET_7_A)
    };

    private void Initialize_FDCB_InstructionsTable() => this.FDCB_InstructionExecutors = new Func<byte, byte>[256]
    {
      new Func<byte, byte>(this.RLC_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RLC_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RLC_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RLC_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RLC_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RLC_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RLC_aIY_plus_n),
      new Func<byte, byte>(this.RLC_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RRC_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RRC_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RRC_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RRC_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RRC_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RRC_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RRC_aIY_plus_n),
      new Func<byte, byte>(this.RRC_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RL_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RL_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RL_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RL_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RL_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RL_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RL_aIY_plus_n),
      new Func<byte, byte>(this.RL_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RR_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RR_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RR_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RR_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RR_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RR_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RR_aIY_plus_n),
      new Func<byte, byte>(this.RR_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SLA_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SLA_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SLA_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SLA_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SLA_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SLA_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SLA_aIY_plus_n),
      new Func<byte, byte>(this.SLA_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SRA_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SRA_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SRA_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SRA_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SRA_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SRA_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SRA_aIY_plus_n),
      new Func<byte, byte>(this.SRA_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SLL_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SLL_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SLL_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SLL_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SLL_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SLL_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SLL_aIY_plus_n),
      new Func<byte, byte>(this.SLL_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SRL_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SRL_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SRL_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SRL_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SRL_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SRL_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SRL_aIY_plus_n),
      new Func<byte, byte>(this.SRL_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.BIT_0_aIY_plus_n),
      new Func<byte, byte>(this.BIT_0_aIY_plus_n),
      new Func<byte, byte>(this.BIT_0_aIY_plus_n),
      new Func<byte, byte>(this.BIT_0_aIY_plus_n),
      new Func<byte, byte>(this.BIT_0_aIY_plus_n),
      new Func<byte, byte>(this.BIT_0_aIY_plus_n),
      new Func<byte, byte>(this.BIT_0_aIY_plus_n),
      new Func<byte, byte>(this.BIT_0_aIY_plus_n),
      new Func<byte, byte>(this.BIT_1_aIY_plus_n),
      new Func<byte, byte>(this.BIT_1_aIY_plus_n),
      new Func<byte, byte>(this.BIT_1_aIY_plus_n),
      new Func<byte, byte>(this.BIT_1_aIY_plus_n),
      new Func<byte, byte>(this.BIT_1_aIY_plus_n),
      new Func<byte, byte>(this.BIT_1_aIY_plus_n),
      new Func<byte, byte>(this.BIT_1_aIY_plus_n),
      new Func<byte, byte>(this.BIT_1_aIY_plus_n),
      new Func<byte, byte>(this.BIT_2_aIY_plus_n),
      new Func<byte, byte>(this.BIT_2_aIY_plus_n),
      new Func<byte, byte>(this.BIT_2_aIY_plus_n),
      new Func<byte, byte>(this.BIT_2_aIY_plus_n),
      new Func<byte, byte>(this.BIT_2_aIY_plus_n),
      new Func<byte, byte>(this.BIT_2_aIY_plus_n),
      new Func<byte, byte>(this.BIT_2_aIY_plus_n),
      new Func<byte, byte>(this.BIT_2_aIY_plus_n),
      new Func<byte, byte>(this.BIT_3_aIY_plus_n),
      new Func<byte, byte>(this.BIT_3_aIY_plus_n),
      new Func<byte, byte>(this.BIT_3_aIY_plus_n),
      new Func<byte, byte>(this.BIT_3_aIY_plus_n),
      new Func<byte, byte>(this.BIT_3_aIY_plus_n),
      new Func<byte, byte>(this.BIT_3_aIY_plus_n),
      new Func<byte, byte>(this.BIT_3_aIY_plus_n),
      new Func<byte, byte>(this.BIT_3_aIY_plus_n),
      new Func<byte, byte>(this.BIT_4_aIY_plus_n),
      new Func<byte, byte>(this.BIT_4_aIY_plus_n),
      new Func<byte, byte>(this.BIT_4_aIY_plus_n),
      new Func<byte, byte>(this.BIT_4_aIY_plus_n),
      new Func<byte, byte>(this.BIT_4_aIY_plus_n),
      new Func<byte, byte>(this.BIT_4_aIY_plus_n),
      new Func<byte, byte>(this.BIT_4_aIY_plus_n),
      new Func<byte, byte>(this.BIT_4_aIY_plus_n),
      new Func<byte, byte>(this.BIT_5_aIY_plus_n),
      new Func<byte, byte>(this.BIT_5_aIY_plus_n),
      new Func<byte, byte>(this.BIT_5_aIY_plus_n),
      new Func<byte, byte>(this.BIT_5_aIY_plus_n),
      new Func<byte, byte>(this.BIT_5_aIY_plus_n),
      new Func<byte, byte>(this.BIT_5_aIY_plus_n),
      new Func<byte, byte>(this.BIT_5_aIY_plus_n),
      new Func<byte, byte>(this.BIT_5_aIY_plus_n),
      new Func<byte, byte>(this.BIT_6_aIY_plus_n),
      new Func<byte, byte>(this.BIT_6_aIY_plus_n),
      new Func<byte, byte>(this.BIT_6_aIY_plus_n),
      new Func<byte, byte>(this.BIT_6_aIY_plus_n),
      new Func<byte, byte>(this.BIT_6_aIY_plus_n),
      new Func<byte, byte>(this.BIT_6_aIY_plus_n),
      new Func<byte, byte>(this.BIT_6_aIY_plus_n),
      new Func<byte, byte>(this.BIT_6_aIY_plus_n),
      new Func<byte, byte>(this.BIT_7_aIY_plus_n),
      new Func<byte, byte>(this.BIT_7_aIY_plus_n),
      new Func<byte, byte>(this.BIT_7_aIY_plus_n),
      new Func<byte, byte>(this.BIT_7_aIY_plus_n),
      new Func<byte, byte>(this.BIT_7_aIY_plus_n),
      new Func<byte, byte>(this.BIT_7_aIY_plus_n),
      new Func<byte, byte>(this.BIT_7_aIY_plus_n),
      new Func<byte, byte>(this.BIT_7_aIY_plus_n),
      new Func<byte, byte>(this.RES_0_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_0_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_0_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_0_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_0_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_0_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_0_aIY_plus_n),
      new Func<byte, byte>(this.RES_0_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_1_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_1_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_1_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_1_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_1_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_1_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_1_aIY_plus_n),
      new Func<byte, byte>(this.RES_1_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_2_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_2_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_2_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_2_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_2_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_2_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_2_aIY_plus_n),
      new Func<byte, byte>(this.RES_2_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_3_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_3_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_3_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_3_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_3_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_3_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_3_aIY_plus_n),
      new Func<byte, byte>(this.RES_3_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_4_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_4_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_4_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_4_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_4_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_4_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_4_aIY_plus_n),
      new Func<byte, byte>(this.RES_4_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_5_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_5_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_5_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_5_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_5_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_5_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_5_aIY_plus_n),
      new Func<byte, byte>(this.RES_5_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_6_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_6_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_6_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_6_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_6_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_6_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_6_aIY_plus_n),
      new Func<byte, byte>(this.RES_6_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_7_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_7_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_7_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_7_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_7_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_7_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_7_aIY_plus_n),
      new Func<byte, byte>(this.RES_7_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_0_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_0_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_0_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_0_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_0_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_0_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_0_aIY_plus_n),
      new Func<byte, byte>(this.SET_0_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_1_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_1_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_1_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_1_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_1_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_1_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_1_aIY_plus_n),
      new Func<byte, byte>(this.SET_1_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_2_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_2_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_2_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_2_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_2_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_2_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_2_aIY_plus_n),
      new Func<byte, byte>(this.SET_2_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_3_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_3_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_3_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_3_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_3_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_3_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_3_aIY_plus_n),
      new Func<byte, byte>(this.SET_3_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_4_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_4_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_4_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_4_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_4_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_4_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_4_aIY_plus_n),
      new Func<byte, byte>(this.SET_4_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_5_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_5_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_5_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_5_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_5_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_5_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_5_aIY_plus_n),
      new Func<byte, byte>(this.SET_5_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_6_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_6_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_6_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_6_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_6_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_6_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_6_aIY_plus_n),
      new Func<byte, byte>(this.SET_6_aIY_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_7_aIY_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_7_aIY_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_7_aIY_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_7_aIY_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_7_aIY_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_7_aIY_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_7_aIY_plus_n),
      new Func<byte, byte>(this.SET_7_aIY_plus_n_and_load_A)
    };

    private void Initialize_DDCB_InstructionsTable() => this.DDCB_InstructionExecutors = new Func<byte, byte>[256]
    {
      new Func<byte, byte>(this.RLC_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RLC_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RLC_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RLC_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RLC_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RLC_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RLC_aIX_plus_n),
      new Func<byte, byte>(this.RLC_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RRC_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RRC_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RRC_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RRC_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RRC_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RRC_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RRC_aIX_plus_n),
      new Func<byte, byte>(this.RRC_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RL_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RL_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RL_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RL_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RL_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RL_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RL_aIX_plus_n),
      new Func<byte, byte>(this.RL_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RR_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RR_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RR_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RR_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RR_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RR_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RR_aIX_plus_n),
      new Func<byte, byte>(this.RR_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SLA_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SLA_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SLA_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SLA_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SLA_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SLA_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SLA_aIX_plus_n),
      new Func<byte, byte>(this.SLA_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SRA_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SRA_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SRA_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SRA_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SRA_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SRA_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SRA_aIX_plus_n),
      new Func<byte, byte>(this.SRA_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SLL_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SLL_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SLL_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SLL_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SLL_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SLL_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SLL_aIX_plus_n),
      new Func<byte, byte>(this.SLL_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SRL_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SRL_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SRL_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SRL_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SRL_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SRL_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SRL_aIX_plus_n),
      new Func<byte, byte>(this.SRL_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.BIT_0_aIX_plus_n),
      new Func<byte, byte>(this.BIT_0_aIX_plus_n),
      new Func<byte, byte>(this.BIT_0_aIX_plus_n),
      new Func<byte, byte>(this.BIT_0_aIX_plus_n),
      new Func<byte, byte>(this.BIT_0_aIX_plus_n),
      new Func<byte, byte>(this.BIT_0_aIX_plus_n),
      new Func<byte, byte>(this.BIT_0_aIX_plus_n),
      new Func<byte, byte>(this.BIT_0_aIX_plus_n),
      new Func<byte, byte>(this.BIT_1_aIX_plus_n),
      new Func<byte, byte>(this.BIT_1_aIX_plus_n),
      new Func<byte, byte>(this.BIT_1_aIX_plus_n),
      new Func<byte, byte>(this.BIT_1_aIX_plus_n),
      new Func<byte, byte>(this.BIT_1_aIX_plus_n),
      new Func<byte, byte>(this.BIT_1_aIX_plus_n),
      new Func<byte, byte>(this.BIT_1_aIX_plus_n),
      new Func<byte, byte>(this.BIT_1_aIX_plus_n),
      new Func<byte, byte>(this.BIT_2_aIX_plus_n),
      new Func<byte, byte>(this.BIT_2_aIX_plus_n),
      new Func<byte, byte>(this.BIT_2_aIX_plus_n),
      new Func<byte, byte>(this.BIT_2_aIX_plus_n),
      new Func<byte, byte>(this.BIT_2_aIX_plus_n),
      new Func<byte, byte>(this.BIT_2_aIX_plus_n),
      new Func<byte, byte>(this.BIT_2_aIX_plus_n),
      new Func<byte, byte>(this.BIT_2_aIX_plus_n),
      new Func<byte, byte>(this.BIT_3_aIX_plus_n),
      new Func<byte, byte>(this.BIT_3_aIX_plus_n),
      new Func<byte, byte>(this.BIT_3_aIX_plus_n),
      new Func<byte, byte>(this.BIT_3_aIX_plus_n),
      new Func<byte, byte>(this.BIT_3_aIX_plus_n),
      new Func<byte, byte>(this.BIT_3_aIX_plus_n),
      new Func<byte, byte>(this.BIT_3_aIX_plus_n),
      new Func<byte, byte>(this.BIT_3_aIX_plus_n),
      new Func<byte, byte>(this.BIT_4_aIX_plus_n),
      new Func<byte, byte>(this.BIT_4_aIX_plus_n),
      new Func<byte, byte>(this.BIT_4_aIX_plus_n),
      new Func<byte, byte>(this.BIT_4_aIX_plus_n),
      new Func<byte, byte>(this.BIT_4_aIX_plus_n),
      new Func<byte, byte>(this.BIT_4_aIX_plus_n),
      new Func<byte, byte>(this.BIT_4_aIX_plus_n),
      new Func<byte, byte>(this.BIT_4_aIX_plus_n),
      new Func<byte, byte>(this.BIT_5_aIX_plus_n),
      new Func<byte, byte>(this.BIT_5_aIX_plus_n),
      new Func<byte, byte>(this.BIT_5_aIX_plus_n),
      new Func<byte, byte>(this.BIT_5_aIX_plus_n),
      new Func<byte, byte>(this.BIT_5_aIX_plus_n),
      new Func<byte, byte>(this.BIT_5_aIX_plus_n),
      new Func<byte, byte>(this.BIT_5_aIX_plus_n),
      new Func<byte, byte>(this.BIT_5_aIX_plus_n),
      new Func<byte, byte>(this.BIT_6_aIX_plus_n),
      new Func<byte, byte>(this.BIT_6_aIX_plus_n),
      new Func<byte, byte>(this.BIT_6_aIX_plus_n),
      new Func<byte, byte>(this.BIT_6_aIX_plus_n),
      new Func<byte, byte>(this.BIT_6_aIX_plus_n),
      new Func<byte, byte>(this.BIT_6_aIX_plus_n),
      new Func<byte, byte>(this.BIT_6_aIX_plus_n),
      new Func<byte, byte>(this.BIT_6_aIX_plus_n),
      new Func<byte, byte>(this.BIT_7_aIX_plus_n),
      new Func<byte, byte>(this.BIT_7_aIX_plus_n),
      new Func<byte, byte>(this.BIT_7_aIX_plus_n),
      new Func<byte, byte>(this.BIT_7_aIX_plus_n),
      new Func<byte, byte>(this.BIT_7_aIX_plus_n),
      new Func<byte, byte>(this.BIT_7_aIX_plus_n),
      new Func<byte, byte>(this.BIT_7_aIX_plus_n),
      new Func<byte, byte>(this.BIT_7_aIX_plus_n),
      new Func<byte, byte>(this.RES_0_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_0_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_0_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_0_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_0_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_0_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_0_aIX_plus_n),
      new Func<byte, byte>(this.RES_0_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_1_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_1_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_1_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_1_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_1_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_1_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_1_aIX_plus_n),
      new Func<byte, byte>(this.RES_1_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_2_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_2_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_2_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_2_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_2_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_2_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_2_aIX_plus_n),
      new Func<byte, byte>(this.RES_2_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_3_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_3_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_3_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_3_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_3_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_3_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_3_aIX_plus_n),
      new Func<byte, byte>(this.RES_3_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_4_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_4_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_4_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_4_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_4_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_4_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_4_aIX_plus_n),
      new Func<byte, byte>(this.RES_4_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_5_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_5_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_5_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_5_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_5_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_5_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_5_aIX_plus_n),
      new Func<byte, byte>(this.RES_5_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_6_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_6_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_6_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_6_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_6_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_6_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_6_aIX_plus_n),
      new Func<byte, byte>(this.RES_6_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.RES_7_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.RES_7_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.RES_7_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.RES_7_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.RES_7_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.RES_7_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.RES_7_aIX_plus_n),
      new Func<byte, byte>(this.RES_7_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_0_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_0_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_0_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_0_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_0_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_0_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_0_aIX_plus_n),
      new Func<byte, byte>(this.SET_0_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_1_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_1_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_1_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_1_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_1_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_1_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_1_aIX_plus_n),
      new Func<byte, byte>(this.SET_1_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_2_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_2_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_2_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_2_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_2_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_2_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_2_aIX_plus_n),
      new Func<byte, byte>(this.SET_2_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_3_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_3_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_3_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_3_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_3_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_3_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_3_aIX_plus_n),
      new Func<byte, byte>(this.SET_3_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_4_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_4_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_4_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_4_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_4_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_4_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_4_aIX_plus_n),
      new Func<byte, byte>(this.SET_4_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_5_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_5_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_5_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_5_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_5_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_5_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_5_aIX_plus_n),
      new Func<byte, byte>(this.SET_5_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_6_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_6_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_6_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_6_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_6_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_6_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_6_aIX_plus_n),
      new Func<byte, byte>(this.SET_6_aIX_plus_n_and_load_A),
      new Func<byte, byte>(this.SET_7_aIX_plus_n_and_load_B),
      new Func<byte, byte>(this.SET_7_aIX_plus_n_and_load_C),
      new Func<byte, byte>(this.SET_7_aIX_plus_n_and_load_D),
      new Func<byte, byte>(this.SET_7_aIX_plus_n_and_load_E),
      new Func<byte, byte>(this.SET_7_aIX_plus_n_and_load_H),
      new Func<byte, byte>(this.SET_7_aIX_plus_n_and_load_L),
      new Func<byte, byte>(this.SET_7_aIX_plus_n),
      new Func<byte, byte>(this.SET_7_aIX_plus_n_and_load_A)
    };

    private void Initialize_DD_InstructionsTable() => this.DD_InstructionExecutors = (IDictionary<byte, Func<byte>>) new Dictionary<byte, Func<byte>>()
    {
      {
        (byte) 9,
        new Func<byte>(this.ADD_IX_BC)
      },
      {
        (byte) 25,
        new Func<byte>(this.ADD_IX_DE)
      },
      {
        (byte) 33,
        new Func<byte>(this.LD_IX_nn)
      },
      {
        (byte) 34,
        new Func<byte>(this.LD_aa_IX)
      },
      {
        (byte) 35,
        new Func<byte>(this.INC_IX)
      },
      {
        (byte) 36,
        new Func<byte>(this.INC_IXH)
      },
      {
        (byte) 37,
        new Func<byte>(this.DEC_IXH)
      },
      {
        (byte) 38,
        new Func<byte>(this.LD_IXH_n)
      },
      {
        (byte) 41,
        new Func<byte>(this.ADD_IX_IX)
      },
      {
        (byte) 42,
        new Func<byte>(this.LD_IX_aa)
      },
      {
        (byte) 43,
        new Func<byte>(this.DEC_IX)
      },
      {
        (byte) 44,
        new Func<byte>(this.INC_IXL)
      },
      {
        (byte) 45,
        new Func<byte>(this.DEC_IXL)
      },
      {
        (byte) 46,
        new Func<byte>(this.LD_IXL_n)
      },
      {
        (byte) 52,
        new Func<byte>(this.INC_aIX_plus_n)
      },
      {
        (byte) 53,
        new Func<byte>(this.DEC_aIX_plus_n)
      },
      {
        (byte) 54,
        new Func<byte>(this.LD_aIX_plus_n_N)
      },
      {
        (byte) 57,
        new Func<byte>(this.ADD_IX_SP)
      },
      {
        (byte) 68,
        new Func<byte>(this.LD_B_IXH)
      },
      {
        (byte) 69,
        new Func<byte>(this.LD_B_IXL)
      },
      {
        (byte) 70,
        new Func<byte>(this.LD_B_aIX_plus_n)
      },
      {
        (byte) 76,
        new Func<byte>(this.LD_C_IXH)
      },
      {
        (byte) 77,
        new Func<byte>(this.LD_C_IXL)
      },
      {
        (byte) 78,
        new Func<byte>(this.LD_C_aIX_plus_n)
      },
      {
        (byte) 84,
        new Func<byte>(this.LD_D_IXH)
      },
      {
        (byte) 85,
        new Func<byte>(this.LD_D_IXL)
      },
      {
        (byte) 86,
        new Func<byte>(this.LD_D_aIX_plus_n)
      },
      {
        (byte) 92,
        new Func<byte>(this.LD_E_IXH)
      },
      {
        (byte) 93,
        new Func<byte>(this.LD_E_IXL)
      },
      {
        (byte) 94,
        new Func<byte>(this.LD_E_aIX_plus_n)
      },
      {
        (byte) 96,
        new Func<byte>(this.LD_IXH_B)
      },
      {
        (byte) 97,
        new Func<byte>(this.LD_IXH_C)
      },
      {
        (byte) 98,
        new Func<byte>(this.LD_IXH_D)
      },
      {
        (byte) 99,
        new Func<byte>(this.LD_IXH_E)
      },
      {
        (byte) 100,
        new Func<byte>(this.LD_IXH_IXH)
      },
      {
        (byte) 101,
        new Func<byte>(this.LD_IXH_IXL)
      },
      {
        (byte) 102,
        new Func<byte>(this.LD_H_aIX_plus_n)
      },
      {
        (byte) 103,
        new Func<byte>(this.LD_IXH_A)
      },
      {
        (byte) 104,
        new Func<byte>(this.LD_IXL_B)
      },
      {
        (byte) 105,
        new Func<byte>(this.LD_IXL_C)
      },
      {
        (byte) 106,
        new Func<byte>(this.LD_IXL_D)
      },
      {
        (byte) 107,
        new Func<byte>(this.LD_IXL_E)
      },
      {
        (byte) 108,
        new Func<byte>(this.LD_IXL_H)
      },
      {
        (byte) 109,
        new Func<byte>(this.LD_IXL_IXL)
      },
      {
        (byte) 110,
        new Func<byte>(this.LD_L_aIX_plus_n)
      },
      {
        (byte) 111,
        new Func<byte>(this.LD_IXL_A)
      },
      {
        (byte) 112,
        new Func<byte>(this.LD_aIX_plus_n_B)
      },
      {
        (byte) 113,
        new Func<byte>(this.LD_aIX_plus_n_C)
      },
      {
        (byte) 114,
        new Func<byte>(this.LD_aIX_plus_n_D)
      },
      {
        (byte) 115,
        new Func<byte>(this.LD_aIX_plus_n_E)
      },
      {
        (byte) 116,
        new Func<byte>(this.LD_aIX_plus_n_H)
      },
      {
        (byte) 117,
        new Func<byte>(this.LD_aIX_plus_n_L)
      },
      {
        (byte) 119,
        new Func<byte>(this.LD_aIX_plus_n_A)
      },
      {
        (byte) 124,
        new Func<byte>(this.LD_A_IXH)
      },
      {
        (byte) 125,
        new Func<byte>(this.LD_A_IXL)
      },
      {
        (byte) 126,
        new Func<byte>(this.LD_A_aIX_plus_n)
      },
      {
        (byte) 132,
        new Func<byte>(this.ADD_A_IXH)
      },
      {
        (byte) 133,
        new Func<byte>(this.ADD_A_IXL)
      },
      {
        (byte) 134,
        new Func<byte>(this.ADD_A_aIX_plus_n)
      },
      {
        (byte) 140,
        new Func<byte>(this.ADC_A_IXH)
      },
      {
        (byte) 141,
        new Func<byte>(this.ADC_A_IXL)
      },
      {
        (byte) 142,
        new Func<byte>(this.ADC_A_aIX_plus_n)
      },
      {
        (byte) 148,
        new Func<byte>(this.SUB_IXH)
      },
      {
        (byte) 149,
        new Func<byte>(this.SUB_IXL)
      },
      {
        (byte) 150,
        new Func<byte>(this.SUB_aIX_plus_n)
      },
      {
        (byte) 156,
        new Func<byte>(this.SBC_A_IXH)
      },
      {
        (byte) 157,
        new Func<byte>(this.SBC_A_IXL)
      },
      {
        (byte) 158,
        new Func<byte>(this.SBC_A_aIX_plus_n)
      },
      {
        (byte) 164,
        new Func<byte>(this.AND_IXH)
      },
      {
        (byte) 165,
        new Func<byte>(this.AND_IXL)
      },
      {
        (byte) 166,
        new Func<byte>(this.AND_aIX_plus_n)
      },
      {
        (byte) 172,
        new Func<byte>(this.XOR_IXH)
      },
      {
        (byte) 173,
        new Func<byte>(this.XOR_IXL)
      },
      {
        (byte) 174,
        new Func<byte>(this.XOR_aIX_plus_n)
      },
      {
        (byte) 180,
        new Func<byte>(this.OR_IXH)
      },
      {
        (byte) 181,
        new Func<byte>(this.OR_IXL)
      },
      {
        (byte) 182,
        new Func<byte>(this.OR_aIX_plus_n)
      },
      {
        (byte) 188,
        new Func<byte>(this.CP_IXH)
      },
      {
        (byte) 189,
        new Func<byte>(this.CP_IXL)
      },
      {
        (byte) 190,
        new Func<byte>(this.CP_aIX_plus_n)
      },
      {
        (byte) 225,
        new Func<byte>(this.POP_IX)
      },
      {
        (byte) 227,
        new Func<byte>(this.EX_aSP_IX)
      },
      {
        (byte) 229,
        new Func<byte>(this.PUSH_IX)
      },
      {
        (byte) 233,
        new Func<byte>(this.JP_aIX)
      },
      {
        (byte) 249,
        new Func<byte>(this.LD_SP_IX)
      }
    };

    private void Initialize_FD_InstructionsTable() => this.FD_InstructionExecutors = (IDictionary<byte, Func<byte>>) new Dictionary<byte, Func<byte>>()
    {
      {
        (byte) 9,
        new Func<byte>(this.ADD_IY_BC)
      },
      {
        (byte) 25,
        new Func<byte>(this.ADD_IY_DE)
      },
      {
        (byte) 33,
        new Func<byte>(this.LD_IY_nn)
      },
      {
        (byte) 34,
        new Func<byte>(this.LD_aa_IY)
      },
      {
        (byte) 35,
        new Func<byte>(this.INC_IY)
      },
      {
        (byte) 36,
        new Func<byte>(this.INC_IYH)
      },
      {
        (byte) 37,
        new Func<byte>(this.DEC_IYH)
      },
      {
        (byte) 38,
        new Func<byte>(this.LD_IYH_n)
      },
      {
        (byte) 41,
        new Func<byte>(this.ADD_IY_IY)
      },
      {
        (byte) 42,
        new Func<byte>(this.LD_IY_aa)
      },
      {
        (byte) 43,
        new Func<byte>(this.DEC_IY)
      },
      {
        (byte) 44,
        new Func<byte>(this.INC_IYL)
      },
      {
        (byte) 45,
        new Func<byte>(this.DEC_IYL)
      },
      {
        (byte) 46,
        new Func<byte>(this.LD_IYL_n)
      },
      {
        (byte) 52,
        new Func<byte>(this.INC_aIY_plus_n)
      },
      {
        (byte) 53,
        new Func<byte>(this.DEC_aIY_plus_n)
      },
      {
        (byte) 54,
        new Func<byte>(this.LD_aIY_plus_n_N)
      },
      {
        (byte) 57,
        new Func<byte>(this.ADD_IY_SP)
      },
      {
        (byte) 68,
        new Func<byte>(this.LD_B_IYH)
      },
      {
        (byte) 69,
        new Func<byte>(this.LD_B_IYL)
      },
      {
        (byte) 70,
        new Func<byte>(this.LD_B_aIY_plus_n)
      },
      {
        (byte) 76,
        new Func<byte>(this.LD_C_IYH)
      },
      {
        (byte) 77,
        new Func<byte>(this.LD_C_IYL)
      },
      {
        (byte) 78,
        new Func<byte>(this.LD_C_aIY_plus_n)
      },
      {
        (byte) 84,
        new Func<byte>(this.LD_D_IYH)
      },
      {
        (byte) 85,
        new Func<byte>(this.LD_D_IYL)
      },
      {
        (byte) 86,
        new Func<byte>(this.LD_D_aIY_plus_n)
      },
      {
        (byte) 92,
        new Func<byte>(this.LD_E_IYH)
      },
      {
        (byte) 93,
        new Func<byte>(this.LD_E_IYL)
      },
      {
        (byte) 94,
        new Func<byte>(this.LD_E_aIY_plus_n)
      },
      {
        (byte) 96,
        new Func<byte>(this.LD_IYH_B)
      },
      {
        (byte) 97,
        new Func<byte>(this.LD_IYH_C)
      },
      {
        (byte) 98,
        new Func<byte>(this.LD_IYH_D)
      },
      {
        (byte) 99,
        new Func<byte>(this.LD_IYH_E)
      },
      {
        (byte) 100,
        new Func<byte>(this.LD_IYH_IYH)
      },
      {
        (byte) 101,
        new Func<byte>(this.LD_IYH_IYL)
      },
      {
        (byte) 102,
        new Func<byte>(this.LD_H_aIY_plus_n)
      },
      {
        (byte) 103,
        new Func<byte>(this.LD_IYH_A)
      },
      {
        (byte) 104,
        new Func<byte>(this.LD_IYL_B)
      },
      {
        (byte) 105,
        new Func<byte>(this.LD_IYL_C)
      },
      {
        (byte) 106,
        new Func<byte>(this.LD_IYL_D)
      },
      {
        (byte) 107,
        new Func<byte>(this.LD_IYL_E)
      },
      {
        (byte) 108,
        new Func<byte>(this.LD_IYL_H)
      },
      {
        (byte) 109,
        new Func<byte>(this.LD_IYL_IYL)
      },
      {
        (byte) 110,
        new Func<byte>(this.LD_L_aIY_plus_n)
      },
      {
        (byte) 111,
        new Func<byte>(this.LD_IYL_A)
      },
      {
        (byte) 112,
        new Func<byte>(this.LD_aIY_plus_n_B)
      },
      {
        (byte) 113,
        new Func<byte>(this.LD_aIY_plus_n_C)
      },
      {
        (byte) 114,
        new Func<byte>(this.LD_aIY_plus_n_D)
      },
      {
        (byte) 115,
        new Func<byte>(this.LD_aIY_plus_n_E)
      },
      {
        (byte) 116,
        new Func<byte>(this.LD_aIY_plus_n_H)
      },
      {
        (byte) 117,
        new Func<byte>(this.LD_aIY_plus_n_L)
      },
      {
        (byte) 119,
        new Func<byte>(this.LD_aIY_plus_n_A)
      },
      {
        (byte) 124,
        new Func<byte>(this.LD_A_IYH)
      },
      {
        (byte) 125,
        new Func<byte>(this.LD_A_IYL)
      },
      {
        (byte) 126,
        new Func<byte>(this.LD_A_aIY_plus_n)
      },
      {
        (byte) 132,
        new Func<byte>(this.ADD_A_IYH)
      },
      {
        (byte) 133,
        new Func<byte>(this.ADD_A_IYL)
      },
      {
        (byte) 134,
        new Func<byte>(this.ADD_A_aIY_plus_n)
      },
      {
        (byte) 140,
        new Func<byte>(this.ADC_A_IYH)
      },
      {
        (byte) 141,
        new Func<byte>(this.ADC_A_IYL)
      },
      {
        (byte) 142,
        new Func<byte>(this.ADC_A_aIY_plus_n)
      },
      {
        (byte) 148,
        new Func<byte>(this.SUB_IYH)
      },
      {
        (byte) 149,
        new Func<byte>(this.SUB_IYL)
      },
      {
        (byte) 150,
        new Func<byte>(this.SUB_aIY_plus_n)
      },
      {
        (byte) 156,
        new Func<byte>(this.SBC_A_IYH)
      },
      {
        (byte) 157,
        new Func<byte>(this.SBC_A_IYL)
      },
      {
        (byte) 158,
        new Func<byte>(this.SBC_A_aIY_plus_n)
      },
      {
        (byte) 164,
        new Func<byte>(this.AND_IYH)
      },
      {
        (byte) 165,
        new Func<byte>(this.AND_IYL)
      },
      {
        (byte) 166,
        new Func<byte>(this.AND_aIY_plus_n)
      },
      {
        (byte) 172,
        new Func<byte>(this.XOR_IYH)
      },
      {
        (byte) 173,
        new Func<byte>(this.XOR_IYL)
      },
      {
        (byte) 174,
        new Func<byte>(this.XOR_aIY_plus_n)
      },
      {
        (byte) 180,
        new Func<byte>(this.OR_IYH)
      },
      {
        (byte) 181,
        new Func<byte>(this.OR_IYL)
      },
      {
        (byte) 182,
        new Func<byte>(this.OR_aIY_plus_n)
      },
      {
        (byte) 188,
        new Func<byte>(this.CP_IYH)
      },
      {
        (byte) 189,
        new Func<byte>(this.CP_IYL)
      },
      {
        (byte) 190,
        new Func<byte>(this.CP_aIY_plus_n)
      },
      {
        (byte) 225,
        new Func<byte>(this.POP_IY)
      },
      {
        (byte) 227,
        new Func<byte>(this.EX_aSP_IY)
      },
      {
        (byte) 229,
        new Func<byte>(this.PUSH_IY)
      },
      {
        (byte) 233,
        new Func<byte>(this.JP_aIY)
      },
      {
        (byte) 249,
        new Func<byte>(this.LD_SP_IY)
      }
    };

    private void Initialize_ED_InstructionsTable()
    {
      this.ED_InstructionExecutors = new Func<byte>[64]
      {
        new Func<byte>(this.IN_B_C),
        new Func<byte>(this.OUT_C_B),
        new Func<byte>(this.SBC_HL_BC),
        new Func<byte>(this.LD_aa_BC),
        new Func<byte>(this.NEG),
        new Func<byte>(this.RETN),
        new Func<byte>(this.IM_0),
        new Func<byte>(this.LD_I_A),
        new Func<byte>(this.IN_C_C),
        new Func<byte>(this.OUT_C_C),
        new Func<byte>(this.ADC_HL_BC),
        new Func<byte>(this.LD_BC_aa),
        new Func<byte>(this.NEG),
        new Func<byte>(this.RETI),
        new Func<byte>(this.IM_0),
        new Func<byte>(this.LD_R_A),
        new Func<byte>(this.IN_D_C),
        new Func<byte>(this.OUT_C_D),
        new Func<byte>(this.SBC_HL_DE),
        new Func<byte>(this.LD_aa_DE),
        new Func<byte>(this.NEG),
        new Func<byte>(this.RETN),
        new Func<byte>(this.IM_1),
        new Func<byte>(this.LD_A_I),
        new Func<byte>(this.IN_E_C),
        new Func<byte>(this.OUT_C_E),
        new Func<byte>(this.ADC_HL_DE),
        new Func<byte>(this.LD_DE_aa),
        new Func<byte>(this.NEG),
        new Func<byte>(this.RETI),
        new Func<byte>(this.IM_2),
        new Func<byte>(this.LD_A_R),
        new Func<byte>(this.IN_H_C),
        new Func<byte>(this.OUT_C_H),
        new Func<byte>(this.SBC_HL_HL),
        new Func<byte>(this.LD_aa_HL),
        new Func<byte>(this.NEG),
        new Func<byte>(this.RETN),
        new Func<byte>(this.IM_0),
        new Func<byte>(this.RRD),
        new Func<byte>(this.IN_L_C),
        new Func<byte>(this.OUT_C_L),
        new Func<byte>(this.ADC_HL_HL),
        new Func<byte>(this.LD_HL_aa),
        new Func<byte>(this.NEG),
        new Func<byte>(this.RETI),
        new Func<byte>(this.IM_0),
        new Func<byte>(this.RLD),
        new Func<byte>(this.IN_F_C),
        new Func<byte>(this.OUT_C_0),
        new Func<byte>(this.SBC_HL_SP),
        new Func<byte>(this.LD_aa_SP),
        new Func<byte>(this.NEG),
        new Func<byte>(this.RETN),
        new Func<byte>(this.IM_1),
        new Func<byte>(this.NOP2),
        new Func<byte>(this.IN_A_C),
        new Func<byte>(this.OUT_C_A),
        new Func<byte>(this.ADC_HL_SP),
        new Func<byte>(this.LD_SP_aa),
        new Func<byte>(this.NEG),
        new Func<byte>(this.RETI),
        new Func<byte>(this.IM_2),
        new Func<byte>(this.NOP2)
      };
      this.ED_Block_InstructionExecutors = new Func<byte>[28]
      {
        new Func<byte>(this.LDI),
        new Func<byte>(this.CPI),
        new Func<byte>(this.INI),
        new Func<byte>(this.OUTI),
        null,
        null,
        null,
        null,
        new Func<byte>(this.LDD),
        new Func<byte>(this.CPD),
        new Func<byte>(this.IND),
        new Func<byte>(this.OUTD),
        null,
        null,
        null,
        null,
        new Func<byte>(this.LDIR),
        new Func<byte>(this.CPIR),
        new Func<byte>(this.INIR),
        new Func<byte>(this.OTIR),
        null,
        null,
        null,
        null,
        new Func<byte>(this.LDDR),
        new Func<byte>(this.CPDR),
        new Func<byte>(this.INDR),
        new Func<byte>(this.OTDR)
      };
    }

    private void Initialize_SingleByte_InstructionsTable() => this.SingleByte_InstructionExecutors = new Func<byte>[256]
    {
      new Func<byte>(this.NOP),
      new Func<byte>(this.LD_BC_nn),
      new Func<byte>(this.LD_aBC_A),
      new Func<byte>(this.INC_BC),
      new Func<byte>(this.INC_B),
      new Func<byte>(this.DEC_B),
      new Func<byte>(this.LD_B_n),
      new Func<byte>(this.RLCA),
      new Func<byte>(this.EX_AF_AF),
      new Func<byte>(this.ADD_HL_BC),
      new Func<byte>(this.LD_A_aBC),
      new Func<byte>(this.DEC_BC),
      new Func<byte>(this.INC_C),
      new Func<byte>(this.DEC_C),
      new Func<byte>(this.LD_C_n),
      new Func<byte>(this.RRCA),
      new Func<byte>(this.DJNZ_d),
      new Func<byte>(this.LD_DE_nn),
      new Func<byte>(this.LD_aDE_A),
      new Func<byte>(this.INC_DE),
      new Func<byte>(this.INC_D),
      new Func<byte>(this.DEC_D),
      new Func<byte>(this.LD_D_n),
      new Func<byte>(this.RLA),
      new Func<byte>(this.JR_d),
      new Func<byte>(this.ADD_HL_DE),
      new Func<byte>(this.LD_A_aDE),
      new Func<byte>(this.DEC_DE),
      new Func<byte>(this.INC_E),
      new Func<byte>(this.DEC_E),
      new Func<byte>(this.LD_E_n),
      new Func<byte>(this.RRA),
      new Func<byte>(this.JR_NZ_d),
      new Func<byte>(this.LD_HL_nn),
      new Func<byte>(this.LD_aa_HL),
      new Func<byte>(this.INC_HL),
      new Func<byte>(this.INC_H),
      new Func<byte>(this.DEC_H),
      new Func<byte>(this.LD_H_n),
      new Func<byte>(this.DAA),
      new Func<byte>(this.JR_Z_d),
      new Func<byte>(this.ADD_HL_HL),
      new Func<byte>(this.LD_HL_aa),
      new Func<byte>(this.DEC_HL),
      new Func<byte>(this.INC_L),
      new Func<byte>(this.DEC_L),
      new Func<byte>(this.LD_L_n),
      new Func<byte>(this.CPL),
      new Func<byte>(this.JR_NC_d),
      new Func<byte>(this.LD_SP_nn),
      new Func<byte>(this.LD_aa_A),
      new Func<byte>(this.INC_SP),
      new Func<byte>(this.INC_aHL),
      new Func<byte>(this.DEC_aHL),
      new Func<byte>(this.LD_aHL_N),
      new Func<byte>(this.SCF),
      new Func<byte>(this.JR_C_d),
      new Func<byte>(this.ADD_HL_SP),
      new Func<byte>(this.LD_A_aa),
      new Func<byte>(this.DEC_SP),
      new Func<byte>(this.INC_A),
      new Func<byte>(this.DEC_A),
      new Func<byte>(this.LD_A_n),
      new Func<byte>(this.CCF),
      new Func<byte>(this.LD_B_B),
      new Func<byte>(this.LD_B_C),
      new Func<byte>(this.LD_B_D),
      new Func<byte>(this.LD_B_E),
      new Func<byte>(this.LD_B_H),
      new Func<byte>(this.LD_B_L),
      new Func<byte>(this.LD_B_aHL),
      new Func<byte>(this.LD_B_A),
      new Func<byte>(this.LD_C_B),
      new Func<byte>(this.LD_C_C),
      new Func<byte>(this.LD_C_D),
      new Func<byte>(this.LD_C_E),
      new Func<byte>(this.LD_C_H),
      new Func<byte>(this.LD_C_L),
      new Func<byte>(this.LD_C_aHL),
      new Func<byte>(this.LD_C_A),
      new Func<byte>(this.LD_D_B),
      new Func<byte>(this.LD_D_C),
      new Func<byte>(this.LD_D_D),
      new Func<byte>(this.LD_D_E),
      new Func<byte>(this.LD_D_H),
      new Func<byte>(this.LD_D_L),
      new Func<byte>(this.LD_D_aHL),
      new Func<byte>(this.LD_D_A),
      new Func<byte>(this.LD_E_B),
      new Func<byte>(this.LD_E_C),
      new Func<byte>(this.LD_E_D),
      new Func<byte>(this.LD_E_E),
      new Func<byte>(this.LD_E_H),
      new Func<byte>(this.LD_E_L),
      new Func<byte>(this.LD_E_aHL),
      new Func<byte>(this.LD_E_A),
      new Func<byte>(this.LD_H_B),
      new Func<byte>(this.LD_H_C),
      new Func<byte>(this.LD_H_D),
      new Func<byte>(this.LD_H_E),
      new Func<byte>(this.LD_H_H),
      new Func<byte>(this.LD_H_L),
      new Func<byte>(this.LD_H_aHL),
      new Func<byte>(this.LD_H_A),
      new Func<byte>(this.LD_L_B),
      new Func<byte>(this.LD_L_C),
      new Func<byte>(this.LD_L_D),
      new Func<byte>(this.LD_L_E),
      new Func<byte>(this.LD_L_H),
      new Func<byte>(this.LD_L_L),
      new Func<byte>(this.LD_L_aHL),
      new Func<byte>(this.LD_L_A),
      new Func<byte>(this.LD_aHL_B),
      new Func<byte>(this.LD_aHL_C),
      new Func<byte>(this.LD_aHL_D),
      new Func<byte>(this.LD_aHL_E),
      new Func<byte>(this.LD_aHL_H),
      new Func<byte>(this.LD_aHL_L),
      new Func<byte>(this.HALT),
      new Func<byte>(this.LD_aHL_A),
      new Func<byte>(this.LD_A_B),
      new Func<byte>(this.LD_A_C),
      new Func<byte>(this.LD_A_D),
      new Func<byte>(this.LD_A_E),
      new Func<byte>(this.LD_A_H),
      new Func<byte>(this.LD_A_L),
      new Func<byte>(this.LD_A_aHL),
      new Func<byte>(this.LD_A_A),
      new Func<byte>(this.ADD_A_B),
      new Func<byte>(this.ADD_A_C),
      new Func<byte>(this.ADD_A_D),
      new Func<byte>(this.ADD_A_E),
      new Func<byte>(this.ADD_A_H),
      new Func<byte>(this.ADD_A_L),
      new Func<byte>(this.ADD_A_aHL),
      new Func<byte>(this.ADD_A_A),
      new Func<byte>(this.ADC_A_B),
      new Func<byte>(this.ADC_A_C),
      new Func<byte>(this.ADC_A_D),
      new Func<byte>(this.ADC_A_E),
      new Func<byte>(this.ADC_A_H),
      new Func<byte>(this.ADC_A_L),
      new Func<byte>(this.ADC_A_aHL),
      new Func<byte>(this.ADC_A_A),
      new Func<byte>(this.SUB_B),
      new Func<byte>(this.SUB_C),
      new Func<byte>(this.SUB_D),
      new Func<byte>(this.SUB_E),
      new Func<byte>(this.SUB_H),
      new Func<byte>(this.SUB_L),
      new Func<byte>(this.SUB_aHL),
      new Func<byte>(this.SUB_A),
      new Func<byte>(this.SBC_A_B),
      new Func<byte>(this.SBC_A_C),
      new Func<byte>(this.SBC_A_D),
      new Func<byte>(this.SBC_A_E),
      new Func<byte>(this.SBC_A_H),
      new Func<byte>(this.SBC_A_L),
      new Func<byte>(this.SBC_A_aHL),
      new Func<byte>(this.SBC_A_A),
      new Func<byte>(this.AND_B),
      new Func<byte>(this.AND_C),
      new Func<byte>(this.AND_D),
      new Func<byte>(this.AND_E),
      new Func<byte>(this.AND_H),
      new Func<byte>(this.AND_L),
      new Func<byte>(this.AND_aHL),
      new Func<byte>(this.AND_A),
      new Func<byte>(this.XOR_B),
      new Func<byte>(this.XOR_C),
      new Func<byte>(this.XOR_D),
      new Func<byte>(this.XOR_E),
      new Func<byte>(this.XOR_H),
      new Func<byte>(this.XOR_L),
      new Func<byte>(this.XOR_aHL),
      new Func<byte>(this.XOR_A),
      new Func<byte>(this.OR_B),
      new Func<byte>(this.OR_C),
      new Func<byte>(this.OR_D),
      new Func<byte>(this.OR_E),
      new Func<byte>(this.OR_H),
      new Func<byte>(this.OR_L),
      new Func<byte>(this.OR_aHL),
      new Func<byte>(this.OR_A),
      new Func<byte>(this.CP_B),
      new Func<byte>(this.CP_C),
      new Func<byte>(this.CP_D),
      new Func<byte>(this.CP_E),
      new Func<byte>(this.CP_H),
      new Func<byte>(this.CP_L),
      new Func<byte>(this.CP_aHL),
      new Func<byte>(this.CP_A),
      new Func<byte>(this.RET_NZ),
      new Func<byte>(this.POP_BC),
      new Func<byte>(this.JP_NZ_nn),
      new Func<byte>(this.JP_nn),
      new Func<byte>(this.CALL_NZ_nn),
      new Func<byte>(this.PUSH_BC),
      new Func<byte>(this.ADD_A_n),
      new Func<byte>(this.RST_00),
      new Func<byte>(this.RET_Z),
      new Func<byte>(this.RET),
      new Func<byte>(this.JP_Z_nn),
      null,
      new Func<byte>(this.CALL_Z_nn),
      new Func<byte>(this.CALL_nn),
      new Func<byte>(this.ADC_A_n),
      new Func<byte>(this.RST_08),
      new Func<byte>(this.RET_NC),
      new Func<byte>(this.POP_DE),
      new Func<byte>(this.JP_NC_nn),
      new Func<byte>(this.OUT_n_A),
      new Func<byte>(this.CALL_NC_nn),
      new Func<byte>(this.PUSH_DE),
      new Func<byte>(this.SUB_n),
      new Func<byte>(this.RST_10),
      new Func<byte>(this.RET_C),
      new Func<byte>(this.EXX),
      new Func<byte>(this.JP_C_nn),
      new Func<byte>(this.IN_A_n),
      new Func<byte>(this.CALL_C_nn),
      null,
      new Func<byte>(this.SBC_A_n),
      new Func<byte>(this.RST_18),
      new Func<byte>(this.RET_PO),
      new Func<byte>(this.POP_HL),
      new Func<byte>(this.JP_PO_nn),
      new Func<byte>(this.EX_aSP_HL),
      new Func<byte>(this.CALL_PO_nn),
      new Func<byte>(this.PUSH_HL),
      new Func<byte>(this.AND_n),
      new Func<byte>(this.RST_20),
      new Func<byte>(this.RET_PE),
      new Func<byte>(this.JP_aHL),
      new Func<byte>(this.JP_PE_nn),
      new Func<byte>(this.EX_DE_HL),
      new Func<byte>(this.CALL_PE_nn),
      null,
      new Func<byte>(this.XOR_n),
      new Func<byte>(this.RST_28),
      new Func<byte>(this.RET_P),
      new Func<byte>(this.POP_AF),
      new Func<byte>(this.JP_P_nn),
      new Func<byte>(this.DI),
      new Func<byte>(this.CALL_P_nn),
      new Func<byte>(this.PUSH_AF),
      new Func<byte>(this.OR_n),
      new Func<byte>(this.RST_30),
      new Func<byte>(this.RET_M),
      new Func<byte>(this.LD_SP_HL),
      new Func<byte>(this.JP_M_nn),
      new Func<byte>(this.EI),
      new Func<byte>(this.CALL_M_nn),
      null,
      new Func<byte>(this.CP_n),
      new Func<byte>(this.RST_38)
    };

    private void GenerateParityTable()
    {
      this.Parity = new Bit[256];
      for (int index1 = 0; index1 <= (int) byte.MaxValue; ++index1)
      {
        int num1 = 0;
        int num2 = index1;
        for (int index2 = 0; index2 <= 7; ++index2)
        {
          num1 += num2 & 1;
          num2 >>= 1;
        }
        this.Parity[index1] = (Bit) (num1 & 1 ^ 1);
      }
    }

    private byte AND_A()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.A);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte XOR_A()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.A);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte OR_A()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.A);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte AND_B()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.B);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte XOR_B()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.B);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte OR_B()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.B);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte AND_C()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.C);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte XOR_C()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.C);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte OR_C()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.C);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte AND_D()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.D);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte XOR_D()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.D);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte OR_D()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.D);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte AND_E()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.E);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte XOR_E()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.E);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte OR_E()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.E);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte AND_H()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.H);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte XOR_H()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.H);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte OR_H()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.H);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte AND_L()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.L);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte XOR_L()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.L);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte OR_L()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.L);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 4;
    }

    private byte AND_aHL()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL));
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 7;
    }

    private byte XOR_aHL()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL));
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 7;
    }

    private byte OR_aHL()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL));
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 7;
    }

    private byte AND_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) num);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 7;
    }

    private byte XOR_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) num);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 7;
    }

    private byte OR_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) num);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 7;
    }

    private byte AND_IXH()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.IXH);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte XOR_IXH()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.IXH);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte OR_IXH()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.IXH);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte AND_IXL()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.IXL);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte XOR_IXL()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.IXL);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte OR_IXL()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.IXL);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte AND_IYH()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.IYH);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte XOR_IYH()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.IYH);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte OR_IYH()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.IYH);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte AND_IYL()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.Registers.IYL);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte XOR_IYL()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.Registers.IYL);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte OR_IYL()
    {
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.Registers.IYL);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 8;
    }

    private byte AND_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num)));
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 19;
    }

    private byte XOR_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num)));
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 19;
    }

    private byte OR_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num)));
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 19;
    }

    private byte AND_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A & (uint) this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num)));
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 1;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 19;
    }

    private byte XOR_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A ^ (uint) this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num)));
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 19;
    }

    private byte OR_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte index = (byte) ((uint) this.Registers.A | (uint) this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num)));
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.Registers.CF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 19;
    }

    private byte ADC_A_A()
    {
      this.FetchFinished();
      byte a1 = this.Registers.A;
      byte a2 = this.Registers.A;
      int num1 = (int) a1 + ((int) a2 + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a1 ^ (int) num2 ^ (int) a2) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a1 ^ (int) a2 ^ 128) & ((int) a2 ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SBC_A_A()
    {
      this.FetchFinished();
      byte a1 = this.Registers.A;
      byte a2 = this.Registers.A;
      int num1 = (int) a1 - ((int) a2 + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a1 ^ (int) num2 ^ (int) a2) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a1 ^ (int) a2) & ((int) a1 ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte ADD_A_A()
    {
      this.FetchFinished();
      byte a1 = this.Registers.A;
      byte a2 = this.Registers.A;
      int num1 = (int) a1 + (int) a2;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a1 ^ (int) num2 ^ (int) a2) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a1 ^ (int) a2 ^ 128) & ((int) a2 ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SUB_A()
    {
      this.FetchFinished();
      byte a1 = this.Registers.A;
      byte a2 = this.Registers.A;
      int num1 = (int) a1 - (int) a2;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a1 ^ (int) num2 ^ (int) a2) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a1 ^ (int) a2) & ((int) a1 ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte CP_A()
    {
      this.FetchFinished();
      byte a1 = this.Registers.A;
      byte a2 = this.Registers.A;
      int num1 = (int) a1 - (int) a2;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a1 ^ (int) num2 ^ (int) a2) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a1 ^ (int) a2) & ((int) a1 ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(a2);
      return 4;
    }

    private byte CPI()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      byte num2 = (byte) ((int) a - (int) num1 & (int) byte.MaxValue);
      short bc = this.Registers.BC;
      ++this.Registers.HL;
      this.Registers.BC = (short) ((int) bc - 1);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) num1) & 16);
      this.Registers.PF = (Bit) (this.Registers.BC != (short) 0);
      this.Registers.NF = (Bit) 1;
      byte num3 = (byte) ((uint) num2 - (uint) (int) this.Registers.HF);
      this.Registers.Flag3 = num3.GetBit(3);
      this.Registers.Flag5 = num3.GetBit(1);
      return 16;
    }

    private byte CPD()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      byte num2 = (byte) ((int) a - (int) num1 & (int) byte.MaxValue);
      short bc = this.Registers.BC;
      --this.Registers.HL;
      this.Registers.BC = (short) ((int) bc - 1);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) num1) & 16);
      this.Registers.PF = (Bit) (this.Registers.BC != (short) 0);
      this.Registers.NF = (Bit) 1;
      byte num3 = (byte) ((uint) num2 - (uint) (int) this.Registers.HF);
      this.Registers.Flag3 = num3.GetBit(3);
      this.Registers.Flag5 = num3.GetBit(1);
      return 16;
    }

    private byte CPIR()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      byte num2 = (byte) ((int) a - (int) num1 & (int) byte.MaxValue);
      short bc = this.Registers.BC;
      ++this.Registers.HL;
      short num3 = (short) ((int) bc - 1);
      this.Registers.BC = num3;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) num1) & 16);
      this.Registers.PF = (Bit) (this.Registers.BC != (short) 0);
      this.Registers.NF = (Bit) 1;
      byte num4 = (byte) ((uint) num2 - (uint) (int) this.Registers.HF);
      this.Registers.Flag3 = num4.GetBit(3);
      this.Registers.Flag5 = num4.GetBit(1);
      if (num3 == (short) 0 || !(this.Registers.ZF == 0))
        return 16;
      this.Registers.PC -= (ushort) 2;
      return 21;
    }

    private byte CPDR()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      byte num2 = (byte) ((int) a - (int) num1 & (int) byte.MaxValue);
      short bc = this.Registers.BC;
      --this.Registers.HL;
      short num3 = (short) ((int) bc - 1);
      this.Registers.BC = num3;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) num1) & 16);
      this.Registers.PF = (Bit) (this.Registers.BC != (short) 0);
      this.Registers.NF = (Bit) 1;
      byte num4 = (byte) ((uint) num2 - (uint) (int) this.Registers.HF);
      this.Registers.Flag3 = num4.GetBit(3);
      this.Registers.Flag5 = num4.GetBit(1);
      if (num3 == (short) 0 || !(this.Registers.ZF == 0))
        return 16;
      this.Registers.PC -= (ushort) 2;
      return 21;
    }

    private byte ADC_A_B()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte b = this.Registers.B;
      int num1 = (int) a + ((int) b + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) b) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) b ^ 128) & ((int) b ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SBC_A_B()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte b = this.Registers.B;
      int num1 = (int) a - ((int) b + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) b) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) b) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte ADD_A_B()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte b = this.Registers.B;
      int num1 = (int) a + (int) b;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) b) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) b ^ 128) & ((int) b ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SUB_B()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte b = this.Registers.B;
      int num1 = (int) a - (int) b;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) b) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) b) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte CP_B()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte b = this.Registers.B;
      int num1 = (int) a - (int) b;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) b) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) b) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(b);
      return 4;
    }

    private byte ADC_A_C()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte c = this.Registers.C;
      int num1 = (int) a + ((int) c + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) c) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) c ^ 128) & ((int) c ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SBC_A_C()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte c = this.Registers.C;
      int num1 = (int) a - ((int) c + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) c) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) c) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte ADD_A_C()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte c = this.Registers.C;
      int num1 = (int) a + (int) c;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) c) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) c ^ 128) & ((int) c ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SUB_C()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte c = this.Registers.C;
      int num1 = (int) a - (int) c;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) c) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) c) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte CP_C()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte c = this.Registers.C;
      int num1 = (int) a - (int) c;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) c) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) c) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(c);
      return 4;
    }

    private byte ADC_A_D()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte d = this.Registers.D;
      int num1 = (int) a + ((int) d + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) d) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) d ^ 128) & ((int) d ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SBC_A_D()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte d = this.Registers.D;
      int num1 = (int) a - ((int) d + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) d) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) d) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte ADD_A_D()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte d = this.Registers.D;
      int num1 = (int) a + (int) d;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) d) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) d ^ 128) & ((int) d ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SUB_D()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte d = this.Registers.D;
      int num1 = (int) a - (int) d;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) d) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) d) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte CP_D()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte d = this.Registers.D;
      int num1 = (int) a - (int) d;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) d) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) d) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(d);
      return 4;
    }

    private byte ADC_A_E()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte e = this.Registers.E;
      int num1 = (int) a + ((int) e + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) e) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) e ^ 128) & ((int) e ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SBC_A_E()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte e = this.Registers.E;
      int num1 = (int) a - ((int) e + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) e) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) e) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte ADD_A_E()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte e = this.Registers.E;
      int num1 = (int) a + (int) e;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) e) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) e ^ 128) & ((int) e ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SUB_E()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte e = this.Registers.E;
      int num1 = (int) a - (int) e;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) e) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) e) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte CP_E()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte e = this.Registers.E;
      int num1 = (int) a - (int) e;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) e) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) e) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(e);
      return 4;
    }

    private byte ADC_A_H()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte h = this.Registers.H;
      int num1 = (int) a + ((int) h + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) h) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) h ^ 128) & ((int) h ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SBC_A_H()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte h = this.Registers.H;
      int num1 = (int) a - ((int) h + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) h) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) h) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte ADD_A_H()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte h = this.Registers.H;
      int num1 = (int) a + (int) h;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) h) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) h ^ 128) & ((int) h ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SUB_H()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte h = this.Registers.H;
      int num1 = (int) a - (int) h;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) h) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) h) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte CP_H()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte h = this.Registers.H;
      int num1 = (int) a - (int) h;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) h) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) h) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(h);
      return 4;
    }

    private byte ADC_A_L()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte l = this.Registers.L;
      int num1 = (int) a + ((int) l + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) l) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) l ^ 128) & ((int) l ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SBC_A_L()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte l = this.Registers.L;
      int num1 = (int) a - ((int) l + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) l) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) l) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte ADD_A_L()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte l = this.Registers.L;
      int num1 = (int) a + (int) l;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) l) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) l ^ 128) & ((int) l ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte SUB_L()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte l = this.Registers.L;
      int num1 = (int) a - (int) l;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) l) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) l) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 4;
    }

    private byte CP_L()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte l = this.Registers.L;
      int num1 = (int) a - (int) l;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) l) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) l) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(l);
      return 4;
    }

    private byte ADC_A_aHL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      int num2 = (int) a + ((int) num1 + (int) this.Registers.CF);
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.A = num3;
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1 ^ 128) & ((int) num1 ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num3);
      return 7;
    }

    private byte SBC_A_aHL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      int num2 = (int) a - ((int) num1 + (int) this.Registers.CF);
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.A = num3;
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1) & ((int) a ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num3);
      return 7;
    }

    private byte ADD_A_aHL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      int num2 = (int) a + (int) num1;
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.A = num3;
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1 ^ 128) & ((int) num1 ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num3);
      return 7;
    }

    private byte SUB_aHL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      int num2 = (int) a - (int) num1;
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.A = num3;
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1) & ((int) a ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num3);
      return 7;
    }

    private byte CP_aHL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      int num2 = (int) a - (int) num1;
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1) & ((int) a ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num1);
      return 7;
    }

    private byte ADC_A_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      int num2 = (int) a + ((int) num1 + (int) this.Registers.CF);
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.A = num3;
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1 ^ 128) & ((int) num1 ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num3);
      return 7;
    }

    private byte SBC_A_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      int num2 = (int) a - ((int) num1 + (int) this.Registers.CF);
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.A = num3;
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1) & ((int) a ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num3);
      return 7;
    }

    private byte ADD_A_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      int num2 = (int) a + (int) num1;
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.A = num3;
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1 ^ 128) & ((int) num1 ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num3);
      return 7;
    }

    private byte SUB_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      int num2 = (int) a - (int) num1;
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.A = num3;
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1) & ((int) a ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num3);
      return 7;
    }

    private byte CP_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      int num2 = (int) a - (int) num1;
      byte num3 = (byte) (num2 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num3 & 128);
      this.Registers.ZF = (Bit) (num3 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num3 ^ (int) num1) & 16);
      this.Registers.CF = (Bit) (num2 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num1) & ((int) a ^ (int) num3) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num1);
      return 7;
    }

    private byte ADC_A_IXH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixh = this.Registers.IXH;
      int num1 = (int) a + ((int) ixh + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixh ^ 128) & ((int) ixh ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte SBC_A_IXH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixh = this.Registers.IXH;
      int num1 = (int) a - ((int) ixh + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixh) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte ADD_A_IXH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixh = this.Registers.IXH;
      int num1 = (int) a + (int) ixh;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixh ^ 128) & ((int) ixh ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte SUB_IXH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixh = this.Registers.IXH;
      int num1 = (int) a - (int) ixh;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixh) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte CP_IXH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixh = this.Registers.IXH;
      int num1 = (int) a - (int) ixh;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixh) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(ixh);
      return 8;
    }

    private byte ADC_A_IXL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixl = this.Registers.IXL;
      int num1 = (int) a + ((int) ixl + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixl ^ 128) & ((int) ixl ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte SBC_A_IXL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixl = this.Registers.IXL;
      int num1 = (int) a - ((int) ixl + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixl) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte ADD_A_IXL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixl = this.Registers.IXL;
      int num1 = (int) a + (int) ixl;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixl ^ 128) & ((int) ixl ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte SUB_IXL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixl = this.Registers.IXL;
      int num1 = (int) a - (int) ixl;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixl) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte CP_IXL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte ixl = this.Registers.IXL;
      int num1 = (int) a - (int) ixl;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) ixl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) ixl) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(ixl);
      return 8;
    }

    private byte ADC_A_IYH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyh = this.Registers.IYH;
      int num1 = (int) a + ((int) iyh + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyh ^ 128) & ((int) iyh ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte SBC_A_IYH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyh = this.Registers.IYH;
      int num1 = (int) a - ((int) iyh + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyh) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte ADD_A_IYH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyh = this.Registers.IYH;
      int num1 = (int) a + (int) iyh;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyh ^ 128) & ((int) iyh ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte SUB_IYH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyh = this.Registers.IYH;
      int num1 = (int) a - (int) iyh;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyh) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte CP_IYH()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyh = this.Registers.IYH;
      int num1 = (int) a - (int) iyh;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyh) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyh) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(iyh);
      return 8;
    }

    private byte ADC_A_IYL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyl = this.Registers.IYL;
      int num1 = (int) a + ((int) iyl + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyl ^ 128) & ((int) iyl ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte SBC_A_IYL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyl = this.Registers.IYL;
      int num1 = (int) a - ((int) iyl + (int) this.Registers.CF);
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyl) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte ADD_A_IYL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyl = this.Registers.IYL;
      int num1 = (int) a + (int) iyl;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyl ^ 128) & ((int) iyl ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte SUB_IYL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyl = this.Registers.IYL;
      int num1 = (int) a - (int) iyl;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.A = num2;
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyl) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 8;
    }

    private byte CP_IYL()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte iyl = this.Registers.IYL;
      int num1 = (int) a - (int) iyl;
      byte num2 = (byte) (num1 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num2 & 128);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num2 ^ (int) iyl) & 16);
      this.Registers.CF = (Bit) (num1 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) iyl) & ((int) a ^ (int) num2) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(iyl);
      return 8;
    }

    private byte ADC_A_aIX_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num1));
      int num3 = (int) a + ((int) num2 + (int) this.Registers.CF);
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.A = num4;
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2 ^ 128) & ((int) num2 ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num4);
      return 19;
    }

    private byte SBC_A_aIX_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num1));
      int num3 = (int) a - ((int) num2 + (int) this.Registers.CF);
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.A = num4;
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2) & ((int) a ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num4);
      return 19;
    }

    private byte ADD_A_aIX_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num1));
      int num3 = (int) a + (int) num2;
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.A = num4;
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2 ^ 128) & ((int) num2 ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num4);
      return 19;
    }

    private byte SUB_aIX_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num1));
      int num3 = (int) a - (int) num2;
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.A = num4;
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2) & ((int) a ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num4);
      return 19;
    }

    private byte CP_aIX_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num1));
      int num3 = (int) a - (int) num2;
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2) & ((int) a ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 19;
    }

    private byte ADC_A_aIY_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num1));
      int num3 = (int) a + ((int) num2 + (int) this.Registers.CF);
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.A = num4;
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2 ^ 128) & ((int) num2 ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num4);
      return 19;
    }

    private byte SBC_A_aIY_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num1));
      int num3 = (int) a - ((int) num2 + (int) this.Registers.CF);
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.A = num4;
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2) & ((int) a ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num4);
      return 19;
    }

    private byte ADD_A_aIY_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num1));
      int num3 = (int) a + (int) num2;
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.A = num4;
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2 ^ 128) & ((int) num2 ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num4);
      return 19;
    }

    private byte SUB_aIY_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num1));
      int num3 = (int) a - (int) num2;
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.A = num4;
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2) & ((int) a ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num4);
      return 19;
    }

    private byte CP_aIY_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num2 = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num1));
      int num3 = (int) a - (int) num2;
      byte num4 = (byte) (num3 & (int) byte.MaxValue);
      this.Registers.SF = (Bit) ((int) num4 & 128);
      this.Registers.ZF = (Bit) (num4 == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num4 ^ (int) num2) & 16);
      this.Registers.CF = (Bit) (num3 & 256);
      this.Registers.PF = (Bit) (((int) a ^ (int) num2) & ((int) a ^ (int) num4) & 128);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 19;
    }

    private byte ADD_HL_BC()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short bc = this.Registers.BC;
      int num1 = (int) (ushort) hl + (int) (ushort) bc;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.HF = (Bit) (((int) hl ^ (int) num2 ^ (int) bc) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 11;
    }

    private byte ADD_HL_DE()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short de = this.Registers.DE;
      int num1 = (int) (ushort) hl + (int) (ushort) de;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.HF = (Bit) (((int) hl ^ (int) num2 ^ (int) de) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 11;
    }

    private byte ADD_HL_HL()
    {
      this.FetchFinished();
      short hl1 = this.Registers.HL;
      short hl2 = this.Registers.HL;
      int num1 = (int) (ushort) hl1 + (int) (ushort) hl2;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.HF = (Bit) (((int) hl1 ^ (int) num2 ^ (int) hl2) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 11;
    }

    private byte ADD_HL_SP()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short sp = this.Registers.SP;
      int num1 = (int) (ushort) hl + (int) (ushort) sp;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.HF = (Bit) (((int) hl ^ (int) num2 ^ (int) sp) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 11;
    }

    private byte ADD_IX_BC()
    {
      this.FetchFinished();
      short ix = this.Registers.IX;
      short bc = this.Registers.BC;
      int num1 = (int) (ushort) ix + (int) (ushort) bc;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.IX = num2;
      this.Registers.HF = (Bit) (((int) ix ^ (int) num2 ^ (int) bc) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADD_IX_DE()
    {
      this.FetchFinished();
      short ix = this.Registers.IX;
      short de = this.Registers.DE;
      int num1 = (int) (ushort) ix + (int) (ushort) de;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.IX = num2;
      this.Registers.HF = (Bit) (((int) ix ^ (int) num2 ^ (int) de) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADD_IX_IX()
    {
      this.FetchFinished();
      short ix1 = this.Registers.IX;
      short ix2 = this.Registers.IX;
      int num1 = (int) (ushort) ix1 + (int) (ushort) ix2;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.IX = num2;
      this.Registers.HF = (Bit) (((int) ix1 ^ (int) num2 ^ (int) ix2) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADD_IX_SP()
    {
      this.FetchFinished();
      short ix = this.Registers.IX;
      short sp = this.Registers.SP;
      int num1 = (int) (ushort) ix + (int) (ushort) sp;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.IX = num2;
      this.Registers.HF = (Bit) (((int) ix ^ (int) num2 ^ (int) sp) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADD_IY_BC()
    {
      this.FetchFinished();
      short iy = this.Registers.IY;
      short bc = this.Registers.BC;
      int num1 = (int) (ushort) iy + (int) (ushort) bc;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.IY = num2;
      this.Registers.HF = (Bit) (((int) iy ^ (int) num2 ^ (int) bc) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADD_IY_DE()
    {
      this.FetchFinished();
      short iy = this.Registers.IY;
      short de = this.Registers.DE;
      int num1 = (int) (ushort) iy + (int) (ushort) de;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.IY = num2;
      this.Registers.HF = (Bit) (((int) iy ^ (int) num2 ^ (int) de) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADD_IY_IY()
    {
      this.FetchFinished();
      short iy1 = this.Registers.IY;
      short iy2 = this.Registers.IY;
      int num1 = (int) (ushort) iy1 + (int) (ushort) iy2;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.IY = num2;
      this.Registers.HF = (Bit) (((int) iy1 ^ (int) num2 ^ (int) iy2) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADD_IY_SP()
    {
      this.FetchFinished();
      short iy = this.Registers.IY;
      short sp = this.Registers.SP;
      int num1 = (int) (ushort) iy + (int) (ushort) sp;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.IY = num2;
      this.Registers.HF = (Bit) (((int) iy ^ (int) num2 ^ (int) sp) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte EX_aSP_HL()
    {
      this.FetchFinished();
      ushort sp = (ushort) this.Registers.SP;
      short num = this.ReadShortFromMemory(sp);
      this.WriteShortToMemory(sp, this.Registers.HL);
      this.Registers.HL = num;
      return 19;
    }

    private byte EX_aSP_IX()
    {
      this.FetchFinished();
      ushort sp = (ushort) this.Registers.SP;
      short num = this.ReadShortFromMemory(sp);
      this.WriteShortToMemory(sp, this.Registers.IX);
      this.Registers.IX = num;
      return 23;
    }

    private byte EX_aSP_IY()
    {
      this.FetchFinished();
      ushort sp = (ushort) this.Registers.SP;
      short num = this.ReadShortFromMemory(sp);
      this.WriteShortToMemory(sp, this.Registers.IY);
      this.Registers.IY = num;
      return 23;
    }

    private byte EX_DE_HL()
    {
      this.FetchFinished();
      short de = this.Registers.DE;
      this.Registers.DE = this.Registers.HL;
      this.Registers.HL = de;
      return 4;
    }

    private byte JP_aHL()
    {
      this.FetchFinished();
      this.Registers.PC = (ushort) this.Registers.HL;
      return 4;
    }

    private byte JP_aIX()
    {
      this.FetchFinished();
      this.Registers.PC = (ushort) this.Registers.IX;
      return 8;
    }

    private byte JP_aIY()
    {
      this.FetchFinished();
      this.Registers.PC = (ushort) this.Registers.IY;
      return 8;
    }

    private byte LD_aHL_N()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, num);
      return 10;
    }

    private byte LD_aIX_plus_n_N()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      byte num2 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num1), num2);
      return 19;
    }

    private byte LD_aIY_plus_n_N()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      byte num2 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num1), num2);
      return 19;
    }

    private byte LD_SP_HL()
    {
      this.FetchFinished(isLdSp: true);
      this.Registers.SP = this.Registers.HL;
      return 6;
    }

    private byte LD_SP_IX()
    {
      this.FetchFinished(isLdSp: true);
      this.Registers.SP = this.Registers.IX;
      return 10;
    }

    private byte LD_SP_IY()
    {
      this.FetchFinished(isLdSp: true);
      this.Registers.SP = this.Registers.IY;
      return 10;
    }

    private byte SET_0_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(0, (Bit) 1);
      return 8;
    }

    private byte SET_1_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(1, (Bit) 1);
      return 8;
    }

    private byte SET_2_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(2, (Bit) 1);
      return 8;
    }

    private byte SET_3_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(3, (Bit) 1);
      return 8;
    }

    private byte SET_4_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(4, (Bit) 1);
      return 8;
    }

    private byte SET_5_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(5, (Bit) 1);
      return 8;
    }

    private byte SET_6_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(6, (Bit) 1);
      return 8;
    }

    private byte SET_7_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(7, (Bit) 1);
      return 8;
    }

    private byte RES_0_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(0, (Bit) 0);
      return 8;
    }

    private byte RES_1_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(1, (Bit) 0);
      return 8;
    }

    private byte RES_2_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(2, (Bit) 0);
      return 8;
    }

    private byte RES_3_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(3, (Bit) 0);
      return 8;
    }

    private byte RES_4_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(4, (Bit) 0);
      return 8;
    }

    private byte RES_5_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(5, (Bit) 0);
      return 8;
    }

    private byte RES_6_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(6, (Bit) 0);
      return 8;
    }

    private byte RES_7_A()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.A.WithBit(7, (Bit) 0);
      return 8;
    }

    private byte SET_0_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(0, (Bit) 1);
      return 8;
    }

    private byte SET_1_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(1, (Bit) 1);
      return 8;
    }

    private byte SET_2_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(2, (Bit) 1);
      return 8;
    }

    private byte SET_3_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(3, (Bit) 1);
      return 8;
    }

    private byte SET_4_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(4, (Bit) 1);
      return 8;
    }

    private byte SET_5_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(5, (Bit) 1);
      return 8;
    }

    private byte SET_6_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(6, (Bit) 1);
      return 8;
    }

    private byte SET_7_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(7, (Bit) 1);
      return 8;
    }

    private byte RES_0_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(0, (Bit) 0);
      return 8;
    }

    private byte RES_1_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(1, (Bit) 0);
      return 8;
    }

    private byte RES_2_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(2, (Bit) 0);
      return 8;
    }

    private byte RES_3_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(3, (Bit) 0);
      return 8;
    }

    private byte RES_4_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(4, (Bit) 0);
      return 8;
    }

    private byte RES_5_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(5, (Bit) 0);
      return 8;
    }

    private byte RES_6_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(6, (Bit) 0);
      return 8;
    }

    private byte RES_7_B()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.B.WithBit(7, (Bit) 0);
      return 8;
    }

    private byte SET_0_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(0, (Bit) 1);
      return 8;
    }

    private byte SET_1_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(1, (Bit) 1);
      return 8;
    }

    private byte SET_2_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(2, (Bit) 1);
      return 8;
    }

    private byte SET_3_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(3, (Bit) 1);
      return 8;
    }

    private byte SET_4_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(4, (Bit) 1);
      return 8;
    }

    private byte SET_5_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(5, (Bit) 1);
      return 8;
    }

    private byte SET_6_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(6, (Bit) 1);
      return 8;
    }

    private byte SET_7_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(7, (Bit) 1);
      return 8;
    }

    private byte RES_0_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(0, (Bit) 0);
      return 8;
    }

    private byte RES_1_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(1, (Bit) 0);
      return 8;
    }

    private byte RES_2_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(2, (Bit) 0);
      return 8;
    }

    private byte RES_3_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(3, (Bit) 0);
      return 8;
    }

    private byte RES_4_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(4, (Bit) 0);
      return 8;
    }

    private byte RES_5_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(5, (Bit) 0);
      return 8;
    }

    private byte RES_6_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(6, (Bit) 0);
      return 8;
    }

    private byte RES_7_C()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.C.WithBit(7, (Bit) 0);
      return 8;
    }

    private byte SET_0_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(0, (Bit) 1);
      return 8;
    }

    private byte SET_1_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(1, (Bit) 1);
      return 8;
    }

    private byte SET_2_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(2, (Bit) 1);
      return 8;
    }

    private byte SET_3_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(3, (Bit) 1);
      return 8;
    }

    private byte SET_4_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(4, (Bit) 1);
      return 8;
    }

    private byte SET_5_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(5, (Bit) 1);
      return 8;
    }

    private byte SET_6_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(6, (Bit) 1);
      return 8;
    }

    private byte SET_7_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(7, (Bit) 1);
      return 8;
    }

    private byte RES_0_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(0, (Bit) 0);
      return 8;
    }

    private byte RES_1_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(1, (Bit) 0);
      return 8;
    }

    private byte RES_2_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(2, (Bit) 0);
      return 8;
    }

    private byte RES_3_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(3, (Bit) 0);
      return 8;
    }

    private byte RES_4_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(4, (Bit) 0);
      return 8;
    }

    private byte RES_5_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(5, (Bit) 0);
      return 8;
    }

    private byte RES_6_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(6, (Bit) 0);
      return 8;
    }

    private byte RES_7_D()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.D.WithBit(7, (Bit) 0);
      return 8;
    }

    private byte SET_0_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(0, (Bit) 1);
      return 8;
    }

    private byte SET_1_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(1, (Bit) 1);
      return 8;
    }

    private byte SET_2_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(2, (Bit) 1);
      return 8;
    }

    private byte SET_3_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(3, (Bit) 1);
      return 8;
    }

    private byte SET_4_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(4, (Bit) 1);
      return 8;
    }

    private byte SET_5_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(5, (Bit) 1);
      return 8;
    }

    private byte SET_6_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(6, (Bit) 1);
      return 8;
    }

    private byte SET_7_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(7, (Bit) 1);
      return 8;
    }

    private byte RES_0_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(0, (Bit) 0);
      return 8;
    }

    private byte RES_1_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(1, (Bit) 0);
      return 8;
    }

    private byte RES_2_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(2, (Bit) 0);
      return 8;
    }

    private byte RES_3_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(3, (Bit) 0);
      return 8;
    }

    private byte RES_4_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(4, (Bit) 0);
      return 8;
    }

    private byte RES_5_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(5, (Bit) 0);
      return 8;
    }

    private byte RES_6_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(6, (Bit) 0);
      return 8;
    }

    private byte RES_7_E()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.E.WithBit(7, (Bit) 0);
      return 8;
    }

    private byte SET_0_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(0, (Bit) 1);
      return 8;
    }

    private byte SET_1_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(1, (Bit) 1);
      return 8;
    }

    private byte SET_2_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(2, (Bit) 1);
      return 8;
    }

    private byte SET_3_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(3, (Bit) 1);
      return 8;
    }

    private byte SET_4_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(4, (Bit) 1);
      return 8;
    }

    private byte SET_5_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(5, (Bit) 1);
      return 8;
    }

    private byte SET_6_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(6, (Bit) 1);
      return 8;
    }

    private byte SET_7_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(7, (Bit) 1);
      return 8;
    }

    private byte RES_0_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(0, (Bit) 0);
      return 8;
    }

    private byte RES_1_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(1, (Bit) 0);
      return 8;
    }

    private byte RES_2_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(2, (Bit) 0);
      return 8;
    }

    private byte RES_3_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(3, (Bit) 0);
      return 8;
    }

    private byte RES_4_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(4, (Bit) 0);
      return 8;
    }

    private byte RES_5_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(5, (Bit) 0);
      return 8;
    }

    private byte RES_6_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(6, (Bit) 0);
      return 8;
    }

    private byte RES_7_H()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.H.WithBit(7, (Bit) 0);
      return 8;
    }

    private byte SET_0_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(0, (Bit) 1);
      return 8;
    }

    private byte SET_1_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(1, (Bit) 1);
      return 8;
    }

    private byte SET_2_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(2, (Bit) 1);
      return 8;
    }

    private byte SET_3_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(3, (Bit) 1);
      return 8;
    }

    private byte SET_4_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(4, (Bit) 1);
      return 8;
    }

    private byte SET_5_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(5, (Bit) 1);
      return 8;
    }

    private byte SET_6_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(6, (Bit) 1);
      return 8;
    }

    private byte SET_7_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(7, (Bit) 1);
      return 8;
    }

    private byte RES_0_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(0, (Bit) 0);
      return 8;
    }

    private byte RES_1_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(1, (Bit) 0);
      return 8;
    }

    private byte RES_2_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(2, (Bit) 0);
      return 8;
    }

    private byte RES_3_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(3, (Bit) 0);
      return 8;
    }

    private byte RES_4_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(4, (Bit) 0);
      return 8;
    }

    private byte RES_5_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(5, (Bit) 0);
      return 8;
    }

    private byte RES_6_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(6, (Bit) 0);
      return 8;
    }

    private byte RES_7_L()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.L.WithBit(7, (Bit) 0);
      return 8;
    }

    private byte SET_0_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte SET_1_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte SET_2_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte SET_3_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte SET_4_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte SET_5_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte SET_6_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte SET_7_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte RES_0_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte RES_1_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte RES_2_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte RES_3_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte RES_4_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte RES_5_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte RES_6_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte RES_7_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(hl, num);
      return 15;
    }

    private byte SET_0_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_1_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_2_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_3_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_4_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_5_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_6_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_7_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_0_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_1_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_2_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_3_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_4_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_5_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_6_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_7_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_0_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_1_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_2_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_3_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_4_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_5_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_6_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_7_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_0_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_1_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_2_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_3_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_4_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_5_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_6_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_7_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_0_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_1_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_2_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_3_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_4_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_5_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_6_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_7_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_0_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_1_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_2_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_3_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_4_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_5_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_6_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_7_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_0_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_1_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_2_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_3_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_4_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_5_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_6_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_7_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_0_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_1_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_2_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_3_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_4_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_5_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_6_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_7_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_0_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_1_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_2_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_3_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_4_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_5_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_6_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_7_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_0_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_1_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_2_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_3_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_4_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_5_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_6_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_7_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_0_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_1_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_2_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_3_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_4_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_5_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_6_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_7_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_0_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_1_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_2_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_3_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_4_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_5_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_6_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_7_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_0_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_1_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_2_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_3_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_4_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_5_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_6_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_7_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_0_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_1_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_2_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_3_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_4_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_5_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_6_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_7_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_0_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_1_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_2_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_3_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_4_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_5_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_6_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_7_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_0_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_1_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_2_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_3_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_4_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_5_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_6_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_7_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_0_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_1_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_2_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_3_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_4_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_5_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_6_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_7_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte SET_0_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_1_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_2_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_3_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_4_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_5_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_6_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_7_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte SET_0_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_1_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_2_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_3_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_4_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_5_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_6_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_7_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte SET_0_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_1_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_2_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_3_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_4_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_5_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_6_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_7_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte SET_0_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_1_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_2_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_3_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_4_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_5_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_6_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_7_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte SET_0_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_1_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_2_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_3_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_4_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_5_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_6_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_7_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte SET_0_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_1_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_2_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_3_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_4_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_5_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_6_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_7_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte SET_0_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_1_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_2_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_3_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_4_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_5_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_6_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte SET_7_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 1);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_0_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_1_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_2_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_3_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_4_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_5_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_6_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_7_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.A = num;
      return 23;
    }

    private byte RES_0_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_1_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_2_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_3_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_4_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_5_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_6_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_7_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.B = num;
      return 23;
    }

    private byte RES_0_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_1_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_2_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_3_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_4_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_5_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_6_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_7_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.C = num;
      return 23;
    }

    private byte RES_0_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_1_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_2_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_3_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_4_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_5_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_6_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_7_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.D = num;
      return 23;
    }

    private byte RES_0_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_1_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_2_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_3_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_4_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_5_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_6_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_7_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.E = num;
      return 23;
    }

    private byte RES_0_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_1_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_2_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_3_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_4_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_5_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_6_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_7_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.H = num;
      return 23;
    }

    private byte RES_0_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_1_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_2_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_3_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_4_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_5_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_6_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_7_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      this.Registers.L = num;
      return 23;
    }

    private byte RES_0_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(0, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_1_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(1, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_2_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(2, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_3_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(3, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_4_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(4, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_5_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(5, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_6_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(6, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte RES_7_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address).WithBit(7, (Bit) 0);
      this.ProcessorAgent.WriteToMemory(address, num);
      return 23;
    }

    private byte BIT_0_A()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.A.GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_1_A()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.A.GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_2_A()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.A.GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_3_A()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.A.GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_4_A()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.A.GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_5_A()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.A.GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_6_A()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.A.GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_7_A()
    {
      this.FetchFinished();
      Bit bit = this.Registers.A.GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_0_B()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.B.GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_1_B()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.B.GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_2_B()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.B.GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_3_B()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.B.GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_4_B()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.B.GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_5_B()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.B.GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_6_B()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.B.GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_7_B()
    {
      this.FetchFinished();
      Bit bit = this.Registers.B.GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_0_C()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.C.GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_1_C()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.C.GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_2_C()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.C.GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_3_C()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.C.GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_4_C()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.C.GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_5_C()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.C.GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_6_C()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.C.GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_7_C()
    {
      this.FetchFinished();
      Bit bit = this.Registers.C.GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_0_D()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.D.GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_1_D()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.D.GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_2_D()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.D.GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_3_D()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.D.GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_4_D()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.D.GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_5_D()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.D.GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_6_D()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.D.GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_7_D()
    {
      this.FetchFinished();
      Bit bit = this.Registers.D.GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_0_E()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.E.GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_1_E()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.E.GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_2_E()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.E.GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_3_E()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.E.GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_4_E()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.E.GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_5_E()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.E.GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_6_E()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.E.GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_7_E()
    {
      this.FetchFinished();
      Bit bit = this.Registers.E.GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_0_H()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.H.GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_1_H()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.H.GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_2_H()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.H.GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_3_H()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.H.GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_4_H()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.H.GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_5_H()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.H.GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_6_H()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.H.GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_7_H()
    {
      this.FetchFinished();
      Bit bit = this.Registers.H.GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_0_L()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.L.GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_1_L()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.L.GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_2_L()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.L.GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_3_L()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.L.GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_4_L()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.L.GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_5_L()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.L.GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_6_L()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.Registers.L.GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_7_L()
    {
      this.FetchFinished();
      Bit bit = this.Registers.L.GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 8;
    }

    private byte BIT_0_aHL()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL).GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 12;
    }

    private byte BIT_1_aHL()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL).GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 12;
    }

    private byte BIT_2_aHL()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL).GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 12;
    }

    private byte BIT_3_aHL()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL).GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 12;
    }

    private byte BIT_4_aHL()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL).GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 12;
    }

    private byte BIT_5_aHL()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL).GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 12;
    }

    private byte BIT_6_aHL()
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL).GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 12;
    }

    private byte BIT_7_aHL()
    {
      this.FetchFinished();
      Bit bit = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL).GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 12;
    }

    private byte BIT_0_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset)).GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_1_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset)).GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_2_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset)).GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_3_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset)).GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_4_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset)).GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_5_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset)).GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_6_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset)).GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_7_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      Bit bit = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset)).GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_0_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset)).GetBit(0);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_1_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset)).GetBit(1);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_2_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset)).GetBit(2);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_3_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset)).GetBit(3);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_4_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset)).GetBit(4);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_5_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset)).GetBit(5);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_6_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      this.Registers.ZF = this.Registers.PF = ~this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset)).GetBit(6);
      this.Registers.SF = (Bit) 0;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte BIT_7_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      Bit bit = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset)).GetBit(7);
      this.Registers.ZF = this.Registers.PF = ~bit;
      this.Registers.SF = bit;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 0;
      return 20;
    }

    private byte CCF()
    {
      this.FetchFinished();
      Bit cf = this.Registers.CF;
      this.Registers.NF = (Bit) 0;
      this.Registers.HF = cf;
      this.Registers.CF = !cf;
      this.SetFlags3and5From(this.Registers.A);
      return 4;
    }

    private byte INI()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.ProcessorAgent.ReadFromPort((ushort) this.Registers.C));
      ++this.Registers.HL;
      byte num = --this.Registers.B;
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.NF = (Bit) 1;
      this.Registers.SF = num.GetBit(7);
      this.SetFlags3and5From(num);
      return 16;
    }

    private byte IND()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.ProcessorAgent.ReadFromPort((ushort) this.Registers.C));
      --this.Registers.HL;
      byte num = --this.Registers.B;
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.NF = (Bit) 1;
      this.Registers.SF = num.GetBit(7);
      this.SetFlags3and5From(num);
      return 16;
    }

    private byte INIR()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.ProcessorAgent.ReadFromPort((ushort) this.Registers.C));
      ++this.Registers.HL;
      byte num = --this.Registers.B;
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.NF = (Bit) 1;
      this.Registers.SF = num.GetBit(7);
      this.SetFlags3and5From(num);
      if (num == (byte) 0)
        return 16;
      this.Registers.PC -= (ushort) 2;
      return 21;
    }

    private byte INDR()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.ProcessorAgent.ReadFromPort((ushort) this.Registers.C));
      --this.Registers.HL;
      byte num = --this.Registers.B;
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.NF = (Bit) 1;
      this.Registers.SF = num.GetBit(7);
      this.SetFlags3and5From(num);
      if (num == (byte) 0)
        return 16;
      this.Registers.PC -= (ushort) 2;
      return 21;
    }

    private byte OUTI()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL));
      ++this.Registers.HL;
      byte num = --this.Registers.B;
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.NF = (Bit) 1;
      this.Registers.SF = num.GetBit(7);
      this.SetFlags3and5From(num);
      return 16;
    }

    private byte OUTD()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL));
      --this.Registers.HL;
      byte num = --this.Registers.B;
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.NF = (Bit) 1;
      this.Registers.SF = num.GetBit(7);
      this.SetFlags3and5From(num);
      return 16;
    }

    private byte OTIR()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL));
      ++this.Registers.HL;
      byte num = --this.Registers.B;
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.NF = (Bit) 1;
      this.Registers.SF = num.GetBit(7);
      this.SetFlags3and5From(num);
      if (num == (byte) 0)
        return 16;
      this.Registers.PC -= (ushort) 2;
      return 21;
    }

    private byte OTDR()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL));
      --this.Registers.HL;
      byte num = --this.Registers.B;
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.NF = (Bit) 1;
      this.Registers.SF = num.GetBit(7);
      this.SetFlags3and5From(num);
      if (num == (byte) 0)
        return 16;
      this.Registers.PC -= (ushort) 2;
      return 21;
    }

    private byte LD_A_I()
    {
      this.FetchFinished();
      byte i = this.Registers.I;
      this.Registers.A = i;
      this.Registers.SF = i.GetBit(7);
      this.Registers.ZF = (Bit) (i == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.Registers.PF = this.Registers.IFF2;
      this.SetFlags3and5From(i);
      return 9;
    }

    private byte LD_A_R()
    {
      this.FetchFinished();
      byte r = this.Registers.R;
      this.Registers.A = r;
      this.Registers.SF = r.GetBit(7);
      this.Registers.ZF = (Bit) (r == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.Registers.PF = this.Registers.IFF2;
      this.SetFlags3and5From(r);
      return 9;
    }

    private byte LD_I_A()
    {
      this.FetchFinished();
      this.Registers.I = this.Registers.A;
      return 9;
    }

    private byte LD_R_A()
    {
      this.FetchFinished();
      this.Registers.R = this.Registers.A;
      return 9;
    }

    private byte LDI()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short de = this.Registers.DE;
      short bc = this.Registers.BC;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) hl);
      this.ProcessorAgent.WriteToMemory((ushort) de, num1);
      this.Registers.HL = (short) ((int) hl + 1);
      this.Registers.DE = (short) ((int) de + 1);
      short num2 = (short) ((int) bc - 1);
      this.Registers.BC = num2;
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.Registers.PF = (Bit) (num2 != (short) 0);
      byte num3 = (byte) ((uint) num1 + (uint) this.Registers.A);
      this.Registers.Flag3 = num3.GetBit(3);
      this.Registers.Flag5 = num3.GetBit(1);
      return 16;
    }

    private byte LDD()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short de = this.Registers.DE;
      short bc = this.Registers.BC;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) hl);
      this.ProcessorAgent.WriteToMemory((ushort) de, num1);
      this.Registers.HL = (short) ((int) hl - 1);
      this.Registers.DE = (short) ((int) de - 1);
      short num2 = (short) ((int) bc - 1);
      this.Registers.BC = num2;
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.Registers.PF = (Bit) (num2 != (short) 0);
      byte num3 = (byte) ((uint) num1 + (uint) this.Registers.A);
      this.Registers.Flag3 = num3.GetBit(3);
      this.Registers.Flag5 = num3.GetBit(1);
      return 16;
    }

    private byte LDIR()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short de = this.Registers.DE;
      short bc = this.Registers.BC;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) hl);
      this.ProcessorAgent.WriteToMemory((ushort) de, num1);
      this.Registers.HL = (short) ((int) hl + 1);
      this.Registers.DE = (short) ((int) de + 1);
      short num2 = (short) ((int) bc - 1);
      this.Registers.BC = num2;
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.Registers.PF = (Bit) (num2 != (short) 0);
      byte num3 = (byte) ((uint) num1 + (uint) this.Registers.A);
      this.Registers.Flag3 = num3.GetBit(3);
      this.Registers.Flag5 = num3.GetBit(1);
      if (num2 == (short) 0)
        return 16;
      this.Registers.PC -= (ushort) 2;
      return 21;
    }

    private byte LDDR()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short de = this.Registers.DE;
      short bc = this.Registers.BC;
      byte num1 = this.ProcessorAgent.ReadFromMemory((ushort) hl);
      this.ProcessorAgent.WriteToMemory((ushort) de, num1);
      this.Registers.HL = (short) ((int) hl - 1);
      this.Registers.DE = (short) ((int) de - 1);
      short num2 = (short) ((int) bc - 1);
      this.Registers.BC = num2;
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.Registers.PF = (Bit) (num2 != (short) 0);
      byte num3 = (byte) ((uint) num1 + (uint) this.Registers.A);
      this.Registers.Flag3 = num3.GetBit(3);
      this.Registers.Flag5 = num3.GetBit(1);
      if (num2 == (short) 0)
        return 16;
      this.Registers.PC -= (ushort) 2;
      return 21;
    }

    private byte NEG()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num = (byte)(-a);
      this.Registers.A = num;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) a ^ (int) num) & 16);
      this.Registers.PF = (Bit) (a == (byte) 128);
      this.Registers.NF = (Bit) 1;
      this.Registers.CF = (Bit) (a > (byte) 0);
      this.SetFlags3and5From(num);
      return 8;
    }

    private byte EI()
    {
      this.FetchFinished(isEiOrDi: true);
      this.Registers.IFF1 = (Bit) 1;
      this.Registers.IFF2 = (Bit) 1;
      return 4;
    }

    private byte IN_A_C()
    {
      this.FetchFinished();
      byte index = this.ProcessorAgent.ReadFromPort((ushort) this.Registers.BC);
      this.Registers.A = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.NF = (Bit) 0;
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.SetFlags3and5From(index);
      return 12;
    }

    private byte IN_B_C()
    {
      this.FetchFinished();
      byte index = this.ProcessorAgent.ReadFromPort((ushort) this.Registers.BC);
      this.Registers.B = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.NF = (Bit) 0;
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.SetFlags3and5From(index);
      return 12;
    }

    private byte IN_C_C()
    {
      this.FetchFinished();
      byte index = this.ProcessorAgent.ReadFromPort((ushort) this.Registers.BC);
      this.Registers.C = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.NF = (Bit) 0;
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.SetFlags3and5From(index);
      return 12;
    }

    private byte IN_D_C()
    {
      this.FetchFinished();
      byte index = this.ProcessorAgent.ReadFromPort((ushort) this.Registers.BC);
      this.Registers.D = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.NF = (Bit) 0;
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.SetFlags3and5From(index);
      return 12;
    }

    private byte IN_E_C()
    {
      this.FetchFinished();
      byte index = this.ProcessorAgent.ReadFromPort((ushort) this.Registers.BC);
      this.Registers.E = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.NF = (Bit) 0;
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.SetFlags3and5From(index);
      return 12;
    }

    private byte IN_H_C()
    {
      this.FetchFinished();
      byte index = this.ProcessorAgent.ReadFromPort((ushort) this.Registers.BC);
      this.Registers.H = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.NF = (Bit) 0;
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.SetFlags3and5From(index);
      return 12;
    }

    private byte IN_L_C()
    {
      this.FetchFinished();
      byte index = this.ProcessorAgent.ReadFromPort((ushort) this.Registers.BC);
      this.Registers.L = index;
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.NF = (Bit) 0;
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.SetFlags3and5From(index);
      return 12;
    }

    private byte IN_F_C()
    {
      this.FetchFinished();
      byte index = this.ProcessorAgent.ReadFromPort((ushort) this.Registers.BC);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.NF = (Bit) 0;
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.SetFlags3and5From(index);
      return 12;
    }

    private byte OUT_C_A()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.Registers.A);
      return 12;
    }

    private byte OUT_C_B()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.Registers.B);
      return 12;
    }

    private byte OUT_C_C()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.Registers.C);
      return 12;
    }

    private byte OUT_C_D()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.Registers.D);
      return 12;
    }

    private byte OUT_C_E()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.Registers.E);
      return 12;
    }

    private byte OUT_C_H()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.Registers.H);
      return 12;
    }

    private byte OUT_C_L()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, this.Registers.L);
      return 12;
    }

    private byte OUT_C_0()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) this.Registers.C, (byte) 0);
      return 12;
    }

    private byte OUT_n_A()
    {
      byte portNumber = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.ProcessorAgent.WriteToPort((ushort) portNumber, this.Registers.A);
      return 11;
    }

    private byte LD_A_A()
    {
      this.FetchFinished();
      return 4;
    }

    private byte LD_B_A()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.A;
      return 4;
    }

    private byte LD_C_A()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.A;
      return 4;
    }

    private byte LD_D_A()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.A;
      return 4;
    }

    private byte LD_E_A()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.A;
      return 4;
    }

    private byte LD_H_A()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.A;
      return 4;
    }

    private byte LD_L_A()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.A;
      return 4;
    }

    private byte LD_IXH_A()
    {
      this.FetchFinished();
      this.Registers.IXH = this.Registers.A;
      return 8;
    }

    private byte LD_IXL_A()
    {
      this.FetchFinished();
      this.Registers.IXL = this.Registers.A;
      return 8;
    }

    private byte LD_IYH_A()
    {
      this.FetchFinished();
      this.Registers.IYH = this.Registers.A;
      return 8;
    }

    private byte LD_IYL_A()
    {
      this.FetchFinished();
      this.Registers.IYL = this.Registers.A;
      return 8;
    }

    private byte LD_A_B()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.B;
      return 4;
    }

    private byte LD_B_B()
    {
      this.FetchFinished();
      return 4;
    }

    private byte LD_C_B()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.B;
      return 4;
    }

    private byte LD_D_B()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.B;
      return 4;
    }

    private byte LD_E_B()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.B;
      return 4;
    }

    private byte LD_H_B()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.B;
      return 4;
    }

    private byte LD_L_B()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.B;
      return 4;
    }

    private byte LD_IXH_B()
    {
      this.FetchFinished();
      this.Registers.IXH = this.Registers.B;
      return 8;
    }

    private byte LD_IXL_B()
    {
      this.FetchFinished();
      this.Registers.IXL = this.Registers.B;
      return 8;
    }

    private byte LD_IYH_B()
    {
      this.FetchFinished();
      this.Registers.IYH = this.Registers.B;
      return 8;
    }

    private byte LD_IYL_B()
    {
      this.FetchFinished();
      this.Registers.IYL = this.Registers.B;
      return 8;
    }

    private byte LD_A_C()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.C;
      return 4;
    }

    private byte LD_B_C()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.C;
      return 4;
    }

    private byte LD_C_C()
    {
      this.FetchFinished();
      return 4;
    }

    private byte LD_D_C()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.C;
      return 4;
    }

    private byte LD_E_C()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.C;
      return 4;
    }

    private byte LD_H_C()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.C;
      return 4;
    }

    private byte LD_L_C()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.C;
      return 4;
    }

    private byte LD_IXH_C()
    {
      this.FetchFinished();
      this.Registers.IXH = this.Registers.C;
      return 8;
    }

    private byte LD_IXL_C()
    {
      this.FetchFinished();
      this.Registers.IXL = this.Registers.C;
      return 8;
    }

    private byte LD_IYH_C()
    {
      this.FetchFinished();
      this.Registers.IYH = this.Registers.C;
      return 8;
    }

    private byte LD_IYL_C()
    {
      this.FetchFinished();
      this.Registers.IYL = this.Registers.C;
      return 8;
    }

    private byte LD_A_D()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.D;
      return 4;
    }

    private byte LD_B_D()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.D;
      return 4;
    }

    private byte LD_C_D()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.D;
      return 4;
    }

    private byte LD_D_D()
    {
      this.FetchFinished();
      return 4;
    }

    private byte LD_E_D()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.D;
      return 4;
    }

    private byte LD_H_D()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.D;
      return 4;
    }

    private byte LD_L_D()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.D;
      return 4;
    }

    private byte LD_IXH_D()
    {
      this.FetchFinished();
      this.Registers.IXH = this.Registers.D;
      return 8;
    }

    private byte LD_IXL_D()
    {
      this.FetchFinished();
      this.Registers.IXL = this.Registers.D;
      return 8;
    }

    private byte LD_IYH_D()
    {
      this.FetchFinished();
      this.Registers.IYH = this.Registers.D;
      return 8;
    }

    private byte LD_IYL_D()
    {
      this.FetchFinished();
      this.Registers.IYL = this.Registers.D;
      return 8;
    }

    private byte LD_A_E()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.E;
      return 4;
    }

    private byte LD_B_E()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.E;
      return 4;
    }

    private byte LD_C_E()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.E;
      return 4;
    }

    private byte LD_D_E()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.E;
      return 4;
    }

    private byte LD_E_E()
    {
      this.FetchFinished();
      return 4;
    }

    private byte LD_H_E()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.E;
      return 4;
    }

    private byte LD_L_E()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.E;
      return 4;
    }

    private byte LD_IXH_E()
    {
      this.FetchFinished();
      this.Registers.IXH = this.Registers.E;
      return 8;
    }

    private byte LD_IXL_E()
    {
      this.FetchFinished();
      this.Registers.IXL = this.Registers.E;
      return 8;
    }

    private byte LD_IYH_E()
    {
      this.FetchFinished();
      this.Registers.IYH = this.Registers.E;
      return 8;
    }

    private byte LD_IYL_E()
    {
      this.FetchFinished();
      this.Registers.IYL = this.Registers.E;
      return 8;
    }

    private byte LD_A_H()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.H;
      return 4;
    }

    private byte LD_B_H()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.H;
      return 4;
    }

    private byte LD_C_H()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.H;
      return 4;
    }

    private byte LD_D_H()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.H;
      return 4;
    }

    private byte LD_E_H()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.H;
      return 4;
    }

    private byte LD_H_H()
    {
      this.FetchFinished();
      return 4;
    }

    private byte LD_L_H()
    {
      this.FetchFinished();
      this.Registers.L = this.Registers.H;
      return 4;
    }

    private byte LD_IXH_H()
    {
      this.FetchFinished();
      this.Registers.IXH = this.Registers.H;
      return 8;
    }

    private byte LD_IXL_H()
    {
      this.FetchFinished();
      this.Registers.IXL = this.Registers.H;
      return 8;
    }

    private byte LD_IYL_H()
    {
      this.FetchFinished();
      this.Registers.IYL = this.Registers.H;
      return 8;
    }

    private byte LD_A_L()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.L;
      return 4;
    }

    private byte LD_B_L()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.L;
      return 4;
    }

    private byte LD_C_L()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.L;
      return 4;
    }

    private byte LD_D_L()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.L;
      return 4;
    }

    private byte LD_E_L()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.L;
      return 4;
    }

    private byte LD_H_L()
    {
      this.FetchFinished();
      this.Registers.H = this.Registers.L;
      return 4;
    }

    private byte LD_L_L()
    {
      this.FetchFinished();
      return 4;
    }

    private byte LD_A_IXH()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.IXH;
      return 8;
    }

    private byte LD_B_IXH()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.IXH;
      return 8;
    }

    private byte LD_C_IXH()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.IXH;
      return 8;
    }

    private byte LD_D_IXH()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.IXH;
      return 8;
    }

    private byte LD_E_IXH()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.IXH;
      return 8;
    }

    private byte LD_IXH_IXH()
    {
      this.FetchFinished();
      return 8;
    }

    private byte LD_A_IXL()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.IXL;
      return 8;
    }

    private byte LD_B_IXL()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.IXL;
      return 8;
    }

    private byte LD_C_IXL()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.IXL;
      return 8;
    }

    private byte LD_D_IXL()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.IXL;
      return 8;
    }

    private byte LD_E_IXL()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.IXL;
      return 8;
    }

    private byte LD_IXH_IXL()
    {
      this.FetchFinished();
      this.Registers.IXH = this.Registers.IXL;
      return 8;
    }

    private byte LD_IXL_IXL()
    {
      this.FetchFinished();
      return 8;
    }

    private byte LD_A_IYH()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.IYH;
      return 8;
    }

    private byte LD_B_IYH()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.IYH;
      return 8;
    }

    private byte LD_C_IYH()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.IYH;
      return 8;
    }

    private byte LD_D_IYH()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.IYH;
      return 8;
    }

    private byte LD_E_IYH()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.IYH;
      return 8;
    }

    private byte LD_IYH_IYH()
    {
      this.FetchFinished();
      return 8;
    }

    private byte LD_A_IYL()
    {
      this.FetchFinished();
      this.Registers.A = this.Registers.IYL;
      return 8;
    }

    private byte LD_B_IYL()
    {
      this.FetchFinished();
      this.Registers.B = this.Registers.IYL;
      return 8;
    }

    private byte LD_C_IYL()
    {
      this.FetchFinished();
      this.Registers.C = this.Registers.IYL;
      return 8;
    }

    private byte LD_D_IYL()
    {
      this.FetchFinished();
      this.Registers.D = this.Registers.IYL;
      return 8;
    }

    private byte LD_E_IYL()
    {
      this.FetchFinished();
      this.Registers.E = this.Registers.IYL;
      return 8;
    }

    private byte LD_IYH_IYL()
    {
      this.FetchFinished();
      this.Registers.IYH = this.Registers.IYL;
      return 8;
    }

    private byte LD_IYL_IYL()
    {
      this.FetchFinished();
      return 8;
    }

    private byte HALT()
    {
      this.FetchFinished(isHalt: true);
      return 4;
    }

    private byte EXX()
    {
      this.FetchFinished();
      short bc = this.Registers.BC;
      short de = this.Registers.DE;
      short hl = this.Registers.HL;
      this.Registers.BC = this.Registers.Alternate.BC;
      this.Registers.DE = this.Registers.Alternate.DE;
      this.Registers.HL = this.Registers.Alternate.HL;
      this.Registers.Alternate.BC = bc;
      this.Registers.Alternate.DE = de;
      this.Registers.Alternate.HL = hl;
      return 4;
    }

    private byte DI()
    {
      this.FetchFinished(isEiOrDi: true);
      this.Registers.IFF1 = (Bit) 0;
      this.Registers.IFF2 = (Bit) 0;
      return 4;
    }

    private byte IN_A_n()
    {
      byte portNumber = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.A = this.ProcessorAgent.ReadFromPort((ushort) portNumber);
      return 11;
    }

    private byte PUSH_AF()
    {
      this.FetchFinished();
      short af = this.Registers.AF;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, af.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, af.GetLowByte());
      this.Registers.SP = (short) address2;
      return 11;
    }

    private byte POP_AF()
    {
      this.FetchFinished();
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.AF = NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 10;
    }

    private byte PUSH_BC()
    {
      this.FetchFinished();
      short bc = this.Registers.BC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, bc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, bc.GetLowByte());
      this.Registers.SP = (short) address2;
      return 11;
    }

    private byte POP_BC()
    {
      this.FetchFinished();
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.BC = NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 10;
    }

    private byte PUSH_DE()
    {
      this.FetchFinished();
      short de = this.Registers.DE;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, de.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, de.GetLowByte());
      this.Registers.SP = (short) address2;
      return 11;
    }

    private byte POP_DE()
    {
      this.FetchFinished();
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.DE = NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 10;
    }

    private byte PUSH_HL()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, hl.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, hl.GetLowByte());
      this.Registers.SP = (short) address2;
      return 11;
    }

    private byte POP_HL()
    {
      this.FetchFinished();
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.HL = NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 10;
    }

    private byte PUSH_IX()
    {
      this.FetchFinished();
      short ix = this.Registers.IX;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, ix.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, ix.GetLowByte());
      this.Registers.SP = (short) address2;
      return 15;
    }

    private byte POP_IX()
    {
      this.FetchFinished();
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.IX = NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 14;
    }

    private byte PUSH_IY()
    {
      this.FetchFinished();
      short iy = this.Registers.IY;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, iy.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, iy.GetLowByte());
      this.Registers.SP = (short) address2;
      return 15;
    }

    private byte POP_IY()
    {
      this.FetchFinished();
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.IY = NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 14;
    }

    private byte IM_0()
    {
      this.FetchFinished();
      this.ProcessorAgent.SetInterruptMode((byte) 0);
      return 8;
    }

    private byte IM_1()
    {
      this.FetchFinished();
      this.ProcessorAgent.SetInterruptMode((byte) 1);
      return 8;
    }

    private byte IM_2()
    {
      this.FetchFinished();
      this.ProcessorAgent.SetInterruptMode((byte) 2);
      return 8;
    }

    private byte RETN()
    {
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      this.Registers.IFF1 = this.Registers.IFF2;
      return 14;
    }

    private byte RLC_A()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte index = (byte) ((int) a << 1 | (int) a >> 7);
      this.Registers.A = index;
      this.Registers.CF = a.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RLC_B()
    {
      this.FetchFinished();
      byte b = this.Registers.B;
      byte index = (byte) ((int) b << 1 | (int) b >> 7);
      this.Registers.B = index;
      this.Registers.CF = b.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RLC_C()
    {
      this.FetchFinished();
      byte c = this.Registers.C;
      byte index = (byte) ((int) c << 1 | (int) c >> 7);
      this.Registers.C = index;
      this.Registers.CF = c.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RLC_D()
    {
      this.FetchFinished();
      byte d = this.Registers.D;
      byte index = (byte) ((int) d << 1 | (int) d >> 7);
      this.Registers.D = index;
      this.Registers.CF = d.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RLC_E()
    {
      this.FetchFinished();
      byte e = this.Registers.E;
      byte index = (byte) ((int) e << 1 | (int) e >> 7);
      this.Registers.E = index;
      this.Registers.CF = e.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RLC_H()
    {
      this.FetchFinished();
      byte h = this.Registers.H;
      byte index = (byte) ((int) h << 1 | (int) h >> 7);
      this.Registers.H = index;
      this.Registers.CF = h.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RLC_L()
    {
      this.FetchFinished();
      byte l = this.Registers.L;
      byte index = (byte) ((int) l << 1 | (int) l >> 7);
      this.Registers.L = index;
      this.Registers.CF = l.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RLC_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(hl, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 15;
    }

    private byte RLCA()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num = (byte) ((int) a << 1 | (int) a >> 7);
      this.Registers.A = num;
      this.Registers.CF = a.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte RLC_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RLC_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | (int) num >> 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_A()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte index = (byte) ((int) a >> 1 | (int) a << 7);
      this.Registers.A = index;
      this.Registers.CF = a.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RRC_B()
    {
      this.FetchFinished();
      byte b = this.Registers.B;
      byte index = (byte) ((int) b >> 1 | (int) b << 7);
      this.Registers.B = index;
      this.Registers.CF = b.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RRC_C()
    {
      this.FetchFinished();
      byte c = this.Registers.C;
      byte index = (byte) ((int) c >> 1 | (int) c << 7);
      this.Registers.C = index;
      this.Registers.CF = c.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RRC_D()
    {
      this.FetchFinished();
      byte d = this.Registers.D;
      byte index = (byte) ((int) d >> 1 | (int) d << 7);
      this.Registers.D = index;
      this.Registers.CF = d.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RRC_E()
    {
      this.FetchFinished();
      byte e = this.Registers.E;
      byte index = (byte) ((int) e >> 1 | (int) e << 7);
      this.Registers.E = index;
      this.Registers.CF = e.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RRC_H()
    {
      this.FetchFinished();
      byte h = this.Registers.H;
      byte index = (byte) ((int) h >> 1 | (int) h << 7);
      this.Registers.H = index;
      this.Registers.CF = h.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RRC_L()
    {
      this.FetchFinished();
      byte l = this.Registers.L;
      byte index = (byte) ((int) l >> 1 | (int) l << 7);
      this.Registers.L = index;
      this.Registers.CF = l.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RRC_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(hl, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 15;
    }

    private byte RRCA()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num = (byte) ((int) a >> 1 | (int) a << 7);
      this.Registers.A = num;
      this.Registers.CF = a.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte RRC_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRC_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num << 7);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_A()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte index = (byte) ((uint) a << 1 | (uint) (byte) (int) this.Registers.CF);
      this.Registers.A = index;
      this.Registers.CF = a.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RL_B()
    {
      this.FetchFinished();
      byte b = this.Registers.B;
      byte index = (byte) ((uint) b << 1 | (uint) (byte) (int) this.Registers.CF);
      this.Registers.B = index;
      this.Registers.CF = b.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RL_C()
    {
      this.FetchFinished();
      byte c = this.Registers.C;
      byte index = (byte) ((uint) c << 1 | (uint) (byte) (int) this.Registers.CF);
      this.Registers.C = index;
      this.Registers.CF = c.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RL_D()
    {
      this.FetchFinished();
      byte d = this.Registers.D;
      byte index = (byte) ((uint) d << 1 | (uint) (byte) (int) this.Registers.CF);
      this.Registers.D = index;
      this.Registers.CF = d.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RL_E()
    {
      this.FetchFinished();
      byte e = this.Registers.E;
      byte index = (byte) ((uint) e << 1 | (uint) (byte) (int) this.Registers.CF);
      this.Registers.E = index;
      this.Registers.CF = e.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RL_H()
    {
      this.FetchFinished();
      byte h = this.Registers.H;
      byte index = (byte) ((uint) h << 1 | (uint) (byte) (int) this.Registers.CF);
      this.Registers.H = index;
      this.Registers.CF = h.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RL_L()
    {
      this.FetchFinished();
      byte l = this.Registers.L;
      byte index = (byte) ((uint) l << 1 | (uint) (byte) (int) this.Registers.CF);
      this.Registers.L = index;
      this.Registers.CF = l.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RL_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(hl, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 15;
    }

    private byte RLA()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num = (byte) ((uint) a << 1 | (uint) (byte) (int) this.Registers.CF);
      this.Registers.A = num;
      this.Registers.CF = a.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte RL_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RL_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1 | (uint) (byte) (int) this.Registers.CF);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_A()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte index = (byte) ((int) a >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.Registers.A = index;
      this.Registers.CF = a.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RR_B()
    {
      this.FetchFinished();
      byte b = this.Registers.B;
      byte index = (byte) ((int) b >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.Registers.B = index;
      this.Registers.CF = b.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RR_C()
    {
      this.FetchFinished();
      byte c = this.Registers.C;
      byte index = (byte) ((int) c >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.Registers.C = index;
      this.Registers.CF = c.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RR_D()
    {
      this.FetchFinished();
      byte d = this.Registers.D;
      byte index = (byte) ((int) d >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.Registers.D = index;
      this.Registers.CF = d.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RR_E()
    {
      this.FetchFinished();
      byte e = this.Registers.E;
      byte index = (byte) ((int) e >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.Registers.E = index;
      this.Registers.CF = e.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RR_H()
    {
      this.FetchFinished();
      byte h = this.Registers.H;
      byte index = (byte) ((int) h >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.Registers.H = index;
      this.Registers.CF = h.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RR_L()
    {
      this.FetchFinished();
      byte l = this.Registers.L;
      byte index = (byte) ((int) l >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.Registers.L = index;
      this.Registers.CF = l.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte RR_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(hl, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 15;
    }

    private byte RRA()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte num = (byte) ((int) a >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.Registers.A = num;
      this.Registers.CF = a.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte RR_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RR_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | ((bool) this.Registers.CF ? 128 : 0));
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_A()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte index = (byte) ((uint) a << 1);
      this.Registers.A = index;
      this.Registers.CF = a.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLA_B()
    {
      this.FetchFinished();
      byte b = this.Registers.B;
      byte index = (byte) ((uint) b << 1);
      this.Registers.B = index;
      this.Registers.CF = b.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLA_C()
    {
      this.FetchFinished();
      byte c = this.Registers.C;
      byte index = (byte) ((uint) c << 1);
      this.Registers.C = index;
      this.Registers.CF = c.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLA_D()
    {
      this.FetchFinished();
      byte d = this.Registers.D;
      byte index = (byte) ((uint) d << 1);
      this.Registers.D = index;
      this.Registers.CF = d.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLA_E()
    {
      this.FetchFinished();
      byte e = this.Registers.E;
      byte index = (byte) ((uint) e << 1);
      this.Registers.E = index;
      this.Registers.CF = e.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLA_H()
    {
      this.FetchFinished();
      byte h = this.Registers.H;
      byte index = (byte) ((uint) h << 1);
      this.Registers.H = index;
      this.Registers.CF = h.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLA_L()
    {
      this.FetchFinished();
      byte l = this.Registers.L;
      byte index = (byte) ((uint) l << 1);
      this.Registers.L = index;
      this.Registers.CF = l.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLA_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(hl, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 15;
    }

    private byte SLA_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLA_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num << 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_A()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte index = (byte) ((int) a >> 1 | (int) a & 128);
      this.Registers.A = index;
      this.Registers.CF = a.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRA_B()
    {
      this.FetchFinished();
      byte b = this.Registers.B;
      byte index = (byte) ((int) b >> 1 | (int) b & 128);
      this.Registers.B = index;
      this.Registers.CF = b.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRA_C()
    {
      this.FetchFinished();
      byte c = this.Registers.C;
      byte index = (byte) ((int) c >> 1 | (int) c & 128);
      this.Registers.C = index;
      this.Registers.CF = c.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRA_D()
    {
      this.FetchFinished();
      byte d = this.Registers.D;
      byte index = (byte) ((int) d >> 1 | (int) d & 128);
      this.Registers.D = index;
      this.Registers.CF = d.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRA_E()
    {
      this.FetchFinished();
      byte e = this.Registers.E;
      byte index = (byte) ((int) e >> 1 | (int) e & 128);
      this.Registers.E = index;
      this.Registers.CF = e.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRA_H()
    {
      this.FetchFinished();
      byte h = this.Registers.H;
      byte index = (byte) ((int) h >> 1 | (int) h & 128);
      this.Registers.H = index;
      this.Registers.CF = h.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRA_L()
    {
      this.FetchFinished();
      byte l = this.Registers.L;
      byte index = (byte) ((int) l >> 1 | (int) l & 128);
      this.Registers.L = index;
      this.Registers.CF = l.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRA_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(hl, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 15;
    }

    private byte SRA_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRA_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num >> 1 | (int) num & 128);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_A()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte index = (byte) ((int) a << 1 | 1);
      this.Registers.A = index;
      this.Registers.CF = a.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLL_B()
    {
      this.FetchFinished();
      byte b = this.Registers.B;
      byte index = (byte) ((int) b << 1 | 1);
      this.Registers.B = index;
      this.Registers.CF = b.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLL_C()
    {
      this.FetchFinished();
      byte c = this.Registers.C;
      byte index = (byte) ((int) c << 1 | 1);
      this.Registers.C = index;
      this.Registers.CF = c.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLL_D()
    {
      this.FetchFinished();
      byte d = this.Registers.D;
      byte index = (byte) ((int) d << 1 | 1);
      this.Registers.D = index;
      this.Registers.CF = d.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLL_E()
    {
      this.FetchFinished();
      byte e = this.Registers.E;
      byte index = (byte) ((int) e << 1 | 1);
      this.Registers.E = index;
      this.Registers.CF = e.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLL_H()
    {
      this.FetchFinished();
      byte h = this.Registers.H;
      byte index = (byte) ((int) h << 1 | 1);
      this.Registers.H = index;
      this.Registers.CF = h.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLL_L()
    {
      this.FetchFinished();
      byte l = this.Registers.L;
      byte index = (byte) ((int) l << 1 | 1);
      this.Registers.L = index;
      this.Registers.CF = l.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SLL_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(hl, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 15;
    }

    private byte SLL_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SLL_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((int) num << 1 | 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(7);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_A()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte index = (byte) ((uint) a >> 1);
      this.Registers.A = index;
      this.Registers.CF = a.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRL_B()
    {
      this.FetchFinished();
      byte b = this.Registers.B;
      byte index = (byte) ((uint) b >> 1);
      this.Registers.B = index;
      this.Registers.CF = b.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRL_C()
    {
      this.FetchFinished();
      byte c = this.Registers.C;
      byte index = (byte) ((uint) c >> 1);
      this.Registers.C = index;
      this.Registers.CF = c.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRL_D()
    {
      this.FetchFinished();
      byte d = this.Registers.D;
      byte index = (byte) ((uint) d >> 1);
      this.Registers.D = index;
      this.Registers.CF = d.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRL_E()
    {
      this.FetchFinished();
      byte e = this.Registers.E;
      byte index = (byte) ((uint) e >> 1);
      this.Registers.E = index;
      this.Registers.CF = e.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRL_H()
    {
      this.FetchFinished();
      byte h = this.Registers.H;
      byte index = (byte) ((uint) h >> 1);
      this.Registers.H = index;
      this.Registers.CF = h.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRL_L()
    {
      this.FetchFinished();
      byte l = this.Registers.L;
      byte index = (byte) ((uint) l >> 1);
      this.Registers.L = index;
      this.Registers.CF = l.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 8;
    }

    private byte SRL_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(hl, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 15;
    }

    private byte SRL_aIX_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIX_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIX_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIX_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIX_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIX_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIX_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIX_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIY_plus_n_and_load_A(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.A = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIY_plus_n_and_load_B(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.B = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIY_plus_n_and_load_C(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.C = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIY_plus_n_and_load_D(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.D = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIY_plus_n_and_load_E(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.E = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIY_plus_n_and_load_H(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.H = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIY_plus_n_and_load_L(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.L = index;
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte SRL_aIY_plus_n(byte offset)
    {
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) offset);
      byte num = this.ProcessorAgent.ReadFromMemory(address);
      byte index = (byte) ((uint) num >> 1);
      this.ProcessorAgent.WriteToMemory(address, index);
      this.Registers.CF = num.GetBit(0);
      this.Registers.HF = (Bit) 0;
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      return 23;
    }

    private byte RRD()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((int) a & 240 | (int) num1 & 15);
      byte num2 = (byte) ((int) num1 >> 4 & 15 | (int) a << 4 & 240);
      this.Registers.A = index;
      this.ProcessorAgent.WriteToMemory(hl, num2);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 18;
    }

    private byte RLD()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte a = this.Registers.A;
      byte num1 = this.ProcessorAgent.ReadFromMemory(hl);
      byte index = (byte) ((int) a & 240 | (int) num1 >> 4 & 15);
      byte num2 = (byte) ((int) num1 << 4 & 240 | (int) a & 15);
      this.Registers.A = index;
      this.ProcessorAgent.WriteToMemory(hl, num2);
      this.Registers.SF = index.GetBit(7);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.HF = (Bit) 0;
      this.Registers.PF = this.Parity[(int) index];
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(index);
      return 18;
    }

    private byte RST_00()
    {
      this.FetchFinished();
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = (ushort) 0;
      return 11;
    }

    private byte RST_08()
    {
      this.FetchFinished();
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = (ushort) 8;
      return 11;
    }

    private byte RST_10()
    {
      this.FetchFinished();
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = (ushort) 16;
      return 11;
    }

    private byte RST_18()
    {
      this.FetchFinished();
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = (ushort) 24;
      return 11;
    }

    private byte RST_20()
    {
      this.FetchFinished();
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = (ushort) 32;
      return 11;
    }

    private byte RST_28()
    {
      this.FetchFinished();
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = (ushort) 40;
      return 11;
    }

    private byte RST_30()
    {
      this.FetchFinished();
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = (ushort) 48;
      return 11;
    }

    private byte RST_38()
    {
      this.FetchFinished();
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = (ushort) 56;
      return 11;
    }

    private byte RET()
    {
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 10;
    }

    private byte RETI()
    {
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 14;
    }

    private byte JP_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      this.Registers.PC = num;
      return 10;
    }

    private byte CALL_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = num;
      return 17;
    }

    private byte RET_C()
    {
      if (this.Registers.CF == 0)
      {
        this.FetchFinished();
        return 5;
      }
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 11;
    }

    private byte JP_C_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.CF == 0)
        return 10;
      this.Registers.PC = num;
      return 10;
    }

    private byte CALL_C_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.CF == 0)
        return 10;
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = num;
      return 17;
    }

    private byte RET_NC()
    {
      if (this.Registers.CF == 1)
      {
        this.FetchFinished();
        return 5;
      }
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 11;
    }

    private byte JP_NC_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.CF == 1)
        return 10;
      this.Registers.PC = num;
      return 10;
    }

    private byte CALL_NC_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.CF == 1)
        return 10;
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = num;
      return 17;
    }

    private byte RET_Z()
    {
      if (this.Registers.ZF == 0)
      {
        this.FetchFinished();
        return 5;
      }
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 11;
    }

    private byte JP_Z_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.ZF == 0)
        return 10;
      this.Registers.PC = num;
      return 10;
    }

    private byte CALL_Z_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.ZF == 0)
        return 10;
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = num;
      return 17;
    }

    private byte RET_NZ()
    {
      if (this.Registers.ZF == 1)
      {
        this.FetchFinished();
        return 5;
      }
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 11;
    }

    private byte JP_NZ_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.ZF == 1)
        return 10;
      this.Registers.PC = num;
      return 10;
    }

    private byte CALL_NZ_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.ZF == 1)
        return 10;
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = num;
      return 17;
    }

    private byte RET_PE()
    {
      if (this.Registers.PF == 0)
      {
        this.FetchFinished();
        return 5;
      }
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 11;
    }

    private byte JP_PE_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.PF == 0)
        return 10;
      this.Registers.PC = num;
      return 10;
    }

    private byte CALL_PE_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.PF == 0)
        return 10;
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = num;
      return 17;
    }

    private byte RET_PO()
    {
      if (this.Registers.PF == 1)
      {
        this.FetchFinished();
        return 5;
      }
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 11;
    }

    private byte JP_PO_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.PF == 1)
        return 10;
      this.Registers.PC = num;
      return 10;
    }

    private byte CALL_PO_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.PF == 1)
        return 10;
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = num;
      return 17;
    }

    private byte RET_M()
    {
      if (this.Registers.SF == 0)
      {
        this.FetchFinished();
        return 5;
      }
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 11;
    }

    private byte JP_M_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.SF == 0)
        return 10;
      this.Registers.PC = num;
      return 10;
    }

    private byte CALL_M_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.SF == 0)
        return 10;
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = num;
      return 17;
    }

    private byte RET_P()
    {
      if (this.Registers.SF == 1)
      {
        this.FetchFinished();
        return 5;
      }
      this.FetchFinished(true);
      ushort sp = (ushort) this.Registers.SP;
      this.Registers.PC = (ushort) NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(sp), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) sp + 1U)));
      this.Registers.SP += (short) 2;
      return 11;
    }

    private byte JP_P_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.SF == 1)
        return 10;
      this.Registers.PC = num;
      return 10;
    }

    private byte CALL_P_nn()
    {
      ushort num = (ushort) this.FetchWord();
      this.FetchFinished();
      if (this.Registers.SF == 1)
        return 10;
      short pc = (short) this.Registers.PC;
      ushort address1 = (ushort) ((uint) this.Registers.SP - 1U);
      this.ProcessorAgent.WriteToMemory(address1, pc.GetHighByte());
      ushort address2 = (ushort) ((uint) address1 - 1U);
      this.ProcessorAgent.WriteToMemory(address2, pc.GetLowByte());
      this.Registers.SP = (short) address2;
      this.Registers.PC = num;
      return 17;
    }

    private byte ADC_HL_BC()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short bc = this.Registers.BC;
      int num1 = (int) (ushort) hl + (int) (ushort) bc + (int) this.Registers.CF;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.SF = (Bit) ((int) num2 & 32768);
      this.Registers.ZF = (Bit) (num2 == (short) 0);
      this.Registers.HF = (Bit) (((int) hl ^ (int) num2 ^ (int) bc) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.PF = (Bit) (((int) hl ^ (int) bc ^ 32768) & ((int) bc ^ (int) num2) & 32768);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte SBC_HL_BC()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short bc = this.Registers.BC;
      int num1 = (int) (ushort) hl - (int) (ushort) bc - (int) this.Registers.CF;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.SF = (Bit) ((int) num2 & 32768);
      this.Registers.ZF = (Bit) (num2 == (short) 0);
      this.Registers.HF = (Bit) (((int) hl ^ (int) num2 ^ (int) bc) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.PF = (Bit) (((int) hl ^ (int) bc) & ((int) hl ^ (int) num2) & 32768);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADC_HL_DE()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short de = this.Registers.DE;
      int num1 = (int) (ushort) hl + (int) (ushort) de + (int) this.Registers.CF;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.SF = (Bit) ((int) num2 & 32768);
      this.Registers.ZF = (Bit) (num2 == (short) 0);
      this.Registers.HF = (Bit) (((int) hl ^ (int) num2 ^ (int) de) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.PF = (Bit) (((int) hl ^ (int) de ^ 32768) & ((int) de ^ (int) num2) & 32768);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte SBC_HL_DE()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short de = this.Registers.DE;
      int num1 = (int) (ushort) hl - (int) (ushort) de - (int) this.Registers.CF;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.SF = (Bit) ((int) num2 & 32768);
      this.Registers.ZF = (Bit) (num2 == (short) 0);
      this.Registers.HF = (Bit) (((int) hl ^ (int) num2 ^ (int) de) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.PF = (Bit) (((int) hl ^ (int) de) & ((int) hl ^ (int) num2) & 32768);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADC_HL_HL()
    {
      this.FetchFinished();
      short hl1 = this.Registers.HL;
      short hl2 = this.Registers.HL;
      int num1 = (int) (ushort) hl1 + (int) (ushort) hl2 + (int) this.Registers.CF;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.SF = (Bit) ((int) num2 & 32768);
      this.Registers.ZF = (Bit) (num2 == (short) 0);
      this.Registers.HF = (Bit) (((int) hl1 ^ (int) num2 ^ (int) hl2) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.PF = (Bit) (((int) hl1 ^ (int) hl2 ^ 32768) & ((int) hl2 ^ (int) num2) & 32768);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte SBC_HL_HL()
    {
      this.FetchFinished();
      short hl1 = this.Registers.HL;
      short hl2 = this.Registers.HL;
      int num1 = (int) (ushort) hl1 - (int) (ushort) hl2 - (int) this.Registers.CF;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.SF = (Bit) ((int) num2 & 32768);
      this.Registers.ZF = (Bit) (num2 == (short) 0);
      this.Registers.HF = (Bit) (((int) hl1 ^ (int) num2 ^ (int) hl2) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.PF = (Bit) (((int) hl1 ^ (int) hl2) & ((int) hl1 ^ (int) num2) & 32768);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte ADC_HL_SP()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short sp = this.Registers.SP;
      int num1 = (int) (ushort) hl + (int) (ushort) sp + (int) this.Registers.CF;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.SF = (Bit) ((int) num2 & 32768);
      this.Registers.ZF = (Bit) (num2 == (short) 0);
      this.Registers.HF = (Bit) (((int) hl ^ (int) num2 ^ (int) sp) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.PF = (Bit) (((int) hl ^ (int) sp ^ 32768) & ((int) sp ^ (int) num2) & 32768);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte SBC_HL_SP()
    {
      this.FetchFinished();
      short hl = this.Registers.HL;
      short sp = this.Registers.SP;
      int num1 = (int) (ushort) hl - (int) (ushort) sp - (int) this.Registers.CF;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      this.Registers.HL = num2;
      this.Registers.SF = (Bit) ((int) num2 & 32768);
      this.Registers.ZF = (Bit) (num2 == (short) 0);
      this.Registers.HF = (Bit) (((int) hl ^ (int) num2 ^ (int) sp) & 4096);
      this.Registers.CF = (Bit) (num1 & 65536);
      this.Registers.PF = (Bit) (((int) hl ^ (int) sp) & ((int) hl ^ (int) num2) & 32768);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2.GetHighByte());
      return 15;
    }

    private byte SCF()
    {
      this.FetchFinished();
      this.Registers.F = (byte) ((int) this.Registers.F & 237 | 1);
      this.SetFlags3and5From(this.Registers.A);
      return 4;
    }

    private byte DAA()
    {
      this.FetchFinished();
      byte a = this.Registers.A;
      byte index = a;
      if ((bool) (this.Registers.HF || (Bit) (((int) a & 15) > 9)))
        index += (bool) this.Registers.NF ? (byte) 250 : (byte) 6;
      if ((bool) (this.Registers.CF || (Bit) (a > (byte) 153)))
        index += (bool) this.Registers.NF ? (byte) 160 : (byte) 96;
      IZ80Registers registers = this.Registers;
      registers.CF = registers.CF | (Bit) (a > (byte) 153);
      this.Registers.HF = (Bit) (((int) a ^ (int) index) & 16);
      this.Registers.SF = (Bit) ((int) index & 128);
      this.Registers.ZF = (Bit) (index == (byte) 0);
      this.Registers.PF = this.Parity[(int) index];
      this.SetFlags3and5From(index);
      this.Registers.A = index;
      return 4;
    }

    private byte CPL()
    {
      this.FetchFinished();
      this.Registers.A ^= byte.MaxValue;
      this.Registers.HF = (Bit) 1;
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(this.Registers.A);
      return 4;
    }

    private byte JR_d()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.PC += (ushort) (sbyte) num;
      return 12;
    }

    private byte JR_C_d()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      if (this.Registers.CF == 0)
        return 7;
      this.Registers.PC += (ushort) (sbyte) num;
      return 12;
    }

    private byte JR_NC_d()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      if (this.Registers.CF == 1)
        return 7;
      this.Registers.PC += (ushort) (sbyte) num;
      return 12;
    }

    private byte JR_Z_d()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      if (this.Registers.ZF == 0)
        return 7;
      this.Registers.PC += (ushort) (sbyte) num;
      return 12;
    }

    private byte JR_NZ_d()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      if (this.Registers.ZF == 1)
        return 7;
      this.Registers.PC += (ushort) (sbyte) num;
      return 12;
    }

    private byte LD_A_aa()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.Registers.A = this.ProcessorAgent.ReadFromMemory(address);
      return 13;
    }

    private byte LD_aa_A()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory(address, this.Registers.A);
      return 13;
    }

    private byte LD_HL_aa()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.Registers.HL = this.ReadShortFromMemory(address);
      return 16;
    }

    private byte LD_DE_aa()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.Registers.DE = this.ReadShortFromMemory(address);
      return 20;
    }

    private byte LD_BC_aa()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.Registers.BC = this.ReadShortFromMemory(address);
      return 20;
    }

    private byte LD_SP_aa()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.Registers.SP = this.ReadShortFromMemory(address);
      return 20;
    }

    private byte LD_IX_aa()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.Registers.IX = this.ReadShortFromMemory(address);
      return 20;
    }

    private byte LD_IY_aa()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.Registers.IY = this.ReadShortFromMemory(address);
      return 20;
    }

    private byte LD_aa_HL()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.WriteShortToMemory(address, this.Registers.HL);
      return 16;
    }

    private byte LD_aa_DE()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.WriteShortToMemory(address, this.Registers.DE);
      return 20;
    }

    private byte LD_aa_BC()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.WriteShortToMemory(address, this.Registers.BC);
      return 20;
    }

    private byte LD_aa_SP()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.WriteShortToMemory(address, this.Registers.SP);
      return 20;
    }

    private byte LD_aa_IX()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.WriteShortToMemory(address, this.Registers.IX);
      return 20;
    }

    private byte LD_aa_IY()
    {
      ushort address = (ushort) this.FetchWord();
      this.FetchFinished();
      this.WriteShortToMemory(address, this.Registers.IY);
      return 20;
    }

    private byte INC_A()
    {
      this.FetchFinished();
      byte num = ++this.Registers.A;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte DEC_A()
    {
      this.FetchFinished();
      byte num = --this.Registers.A;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte INC_B()
    {
      this.FetchFinished();
      byte num = ++this.Registers.B;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte DEC_B()
    {
      this.FetchFinished();
      byte num = --this.Registers.B;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte INC_C()
    {
      this.FetchFinished();
      byte num = ++this.Registers.C;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte DEC_C()
    {
      this.FetchFinished();
      byte num = --this.Registers.C;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte INC_D()
    {
      this.FetchFinished();
      byte num = ++this.Registers.D;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte DEC_D()
    {
      this.FetchFinished();
      byte num = --this.Registers.D;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte INC_E()
    {
      this.FetchFinished();
      byte num = ++this.Registers.E;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte DEC_E()
    {
      this.FetchFinished();
      byte num = --this.Registers.E;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte INC_H()
    {
      this.FetchFinished();
      byte num = ++this.Registers.H;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte DEC_H()
    {
      this.FetchFinished();
      byte num = --this.Registers.H;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte INC_L()
    {
      this.FetchFinished();
      byte num = ++this.Registers.L;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte DEC_L()
    {
      this.FetchFinished();
      byte num = --this.Registers.L;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 4;
    }

    private byte INC_IXH()
    {
      this.FetchFinished();
      byte num = ++this.Registers.IXH;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 8;
    }

    private byte DEC_IXH()
    {
      this.FetchFinished();
      byte num = --this.Registers.IXH;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 8;
    }

    private byte INC_IXL()
    {
      this.FetchFinished();
      byte num = ++this.Registers.IXL;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 8;
    }

    private byte DEC_IXL()
    {
      this.FetchFinished();
      byte num = --this.Registers.IXL;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 8;
    }

    private byte INC_IYH()
    {
      this.FetchFinished();
      byte num = ++this.Registers.IYH;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 8;
    }

    private byte DEC_IYH()
    {
      this.FetchFinished();
      byte num = --this.Registers.IYH;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 8;
    }

    private byte INC_IYL()
    {
      this.FetchFinished();
      byte num = ++this.Registers.IYL;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 8;
    }

    private byte DEC_IYL()
    {
      this.FetchFinished();
      byte num = --this.Registers.IYL;
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 8;
    }

    private byte INC_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = (byte) ((uint) this.ProcessorAgent.ReadFromMemory(hl) + 1U);
      this.ProcessorAgent.WriteToMemory(hl, num);
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 0);
      this.Registers.PF = (Bit) (num == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num);
      return 11;
    }

    private byte DEC_aHL()
    {
      this.FetchFinished();
      ushort hl = (ushort) this.Registers.HL;
      byte num = (byte) ((uint) this.ProcessorAgent.ReadFromMemory(hl) - 1U);
      this.ProcessorAgent.WriteToMemory(hl, num);
      this.Registers.SF = num.GetBit(7);
      this.Registers.ZF = (Bit) (num == (byte) 0);
      this.Registers.HF = (Bit) (((int) num & 15) == 15);
      this.Registers.PF = (Bit) (num == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num);
      return 11;
    }

    private byte INC_aIX_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) num1);
      byte num2 = (byte) ((uint) this.ProcessorAgent.ReadFromMemory(address) + 1U);
      this.ProcessorAgent.WriteToMemory(address, num2);
      this.Registers.SF = num2.GetBit(7);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) num2 & 15) == 0);
      this.Registers.PF = (Bit) (num2 == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 23;
    }

    private byte DEC_aIX_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IX + (uint) (sbyte) num1);
      byte num2 = (byte) ((uint) this.ProcessorAgent.ReadFromMemory(address) - 1U);
      this.ProcessorAgent.WriteToMemory(address, num2);
      this.Registers.SF = num2.GetBit(7);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) num2 & 15) == 15);
      this.Registers.PF = (Bit) (num2 == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 23;
    }

    private byte INC_aIY_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) num1);
      byte num2 = (byte) ((uint) this.ProcessorAgent.ReadFromMemory(address) + 1U);
      this.ProcessorAgent.WriteToMemory(address, num2);
      this.Registers.SF = num2.GetBit(7);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) num2 & 15) == 0);
      this.Registers.PF = (Bit) (num2 == (byte) 128);
      this.Registers.NF = (Bit) 0;
      this.SetFlags3and5From(num2);
      return 23;
    }

    private byte DEC_aIY_plus_n()
    {
      byte num1 = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      ushort address = (ushort) ((uint) this.Registers.IY + (uint) (sbyte) num1);
      byte num2 = (byte) ((uint) this.ProcessorAgent.ReadFromMemory(address) - 1U);
      this.ProcessorAgent.WriteToMemory(address, num2);
      this.Registers.SF = num2.GetBit(7);
      this.Registers.ZF = (Bit) (num2 == (byte) 0);
      this.Registers.HF = (Bit) (((int) num2 & 15) == 15);
      this.Registers.PF = (Bit) (num2 == (byte) 127);
      this.Registers.NF = (Bit) 1;
      this.SetFlags3and5From(num2);
      return 23;
    }

    private byte INC_BC()
    {
      this.FetchFinished();
      ++this.Registers.BC;
      return 6;
    }

    private byte DEC_BC()
    {
      this.FetchFinished();
      --this.Registers.BC;
      return 6;
    }

    private byte INC_DE()
    {
      this.FetchFinished();
      ++this.Registers.DE;
      return 6;
    }

    private byte DEC_DE()
    {
      this.FetchFinished();
      --this.Registers.DE;
      return 6;
    }

    private byte INC_HL()
    {
      this.FetchFinished();
      ++this.Registers.HL;
      return 6;
    }

    private byte DEC_HL()
    {
      this.FetchFinished();
      --this.Registers.HL;
      return 6;
    }

    private byte INC_SP()
    {
      this.FetchFinished();
      ++this.Registers.SP;
      return 6;
    }

    private byte DEC_SP()
    {
      this.FetchFinished();
      --this.Registers.SP;
      return 6;
    }

    private byte INC_IX()
    {
      this.FetchFinished();
      ++this.Registers.IX;
      return 10;
    }

    private byte DEC_IX()
    {
      this.FetchFinished();
      --this.Registers.IX;
      return 10;
    }

    private byte INC_IY()
    {
      this.FetchFinished();
      ++this.Registers.IY;
      return 10;
    }

    private byte DEC_IY()
    {
      this.FetchFinished();
      --this.Registers.IY;
      return 10;
    }

    private byte LD_A_aBC()
    {
      this.FetchFinished();
      this.Registers.A = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.BC);
      return 7;
    }

    private byte LD_aBC_A()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.BC, this.Registers.A);
      return 7;
    }

    private byte LD_A_aDE()
    {
      this.FetchFinished();
      this.Registers.A = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.DE);
      return 7;
    }

    private byte LD_aDE_A()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.DE, this.Registers.A);
      return 7;
    }

    private byte LD_A_aHL()
    {
      this.FetchFinished();
      this.Registers.A = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      return 7;
    }

    private byte LD_aHL_A()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.Registers.A);
      return 7;
    }

    private byte LD_B_aHL()
    {
      this.FetchFinished();
      this.Registers.B = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      return 7;
    }

    private byte LD_aHL_B()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.Registers.B);
      return 7;
    }

    private byte LD_C_aHL()
    {
      this.FetchFinished();
      this.Registers.C = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      return 7;
    }

    private byte LD_aHL_C()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.Registers.C);
      return 7;
    }

    private byte LD_D_aHL()
    {
      this.FetchFinished();
      this.Registers.D = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      return 7;
    }

    private byte LD_aHL_D()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.Registers.D);
      return 7;
    }

    private byte LD_E_aHL()
    {
      this.FetchFinished();
      this.Registers.E = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      return 7;
    }

    private byte LD_aHL_E()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.Registers.E);
      return 7;
    }

    private byte LD_H_aHL()
    {
      this.FetchFinished();
      this.Registers.H = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      return 7;
    }

    private byte LD_aHL_H()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.Registers.H);
      return 7;
    }

    private byte LD_L_aHL()
    {
      this.FetchFinished();
      this.Registers.L = this.ProcessorAgent.ReadFromMemory((ushort) this.Registers.HL);
      return 7;
    }

    private byte LD_aHL_L()
    {
      this.FetchFinished();
      this.ProcessorAgent.WriteToMemory((ushort) this.Registers.HL, this.Registers.L);
      return 7;
    }

    private byte LD_A_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.A = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIX_plus_n_A()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num), a);
      return 19;
    }

    private byte LD_B_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.B = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIX_plus_n_B()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte b = this.Registers.B;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num), b);
      return 19;
    }

    private byte LD_C_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.C = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIX_plus_n_C()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte c = this.Registers.C;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num), c);
      return 19;
    }

    private byte LD_D_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.D = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIX_plus_n_D()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte d = this.Registers.D;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num), d);
      return 19;
    }

    private byte LD_E_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.E = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIX_plus_n_E()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte e = this.Registers.E;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num), e);
      return 19;
    }

    private byte LD_H_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.H = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIX_plus_n_H()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte h = this.Registers.H;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num), h);
      return 19;
    }

    private byte LD_L_aIX_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.L = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIX_plus_n_L()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte l = this.Registers.L;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IX + (uint) (sbyte) num), l);
      return 19;
    }

    private byte LD_A_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.A = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIY_plus_n_A()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte a = this.Registers.A;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num), a);
      return 19;
    }

    private byte LD_B_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.B = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIY_plus_n_B()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte b = this.Registers.B;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num), b);
      return 19;
    }

    private byte LD_C_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.C = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIY_plus_n_C()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte c = this.Registers.C;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num), c);
      return 19;
    }

    private byte LD_D_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.D = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIY_plus_n_D()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte d = this.Registers.D;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num), d);
      return 19;
    }

    private byte LD_E_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.E = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIY_plus_n_E()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte e = this.Registers.E;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num), e);
      return 19;
    }

    private byte LD_H_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.H = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIY_plus_n_H()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte h = this.Registers.H;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num), h);
      return 19;
    }

    private byte LD_L_aIY_plus_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.L = this.ProcessorAgent.ReadFromMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num));
      return 19;
    }

    private byte LD_aIY_plus_n_L()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      byte l = this.Registers.L;
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) this.Registers.IY + (uint) (sbyte) num), l);
      return 19;
    }

    private byte LD_A_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.A = num;
      return 7;
    }

    private byte LD_B_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.B = num;
      return 7;
    }

    private byte LD_C_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.C = num;
      return 7;
    }

    private byte LD_D_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.D = num;
      return 7;
    }

    private byte LD_E_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.E = num;
      return 7;
    }

    private byte LD_H_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.H = num;
      return 7;
    }

    private byte LD_L_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.L = num;
      return 7;
    }

    private byte LD_IXH_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.IXH = num;
      return 11;
    }

    private byte LD_IXL_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.IXL = num;
      return 11;
    }

    private byte LD_IYH_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.IYH = num;
      return 11;
    }

    private byte LD_IYL_n()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      this.Registers.IYL = num;
      return 11;
    }

    private byte LD_BC_nn()
    {
      short num = this.FetchWord();
      this.FetchFinished();
      this.Registers.BC = num;
      return 10;
    }

    private byte LD_DE_nn()
    {
      short num = this.FetchWord();
      this.FetchFinished();
      this.Registers.DE = num;
      return 10;
    }

    private byte LD_HL_nn()
    {
      short num = this.FetchWord();
      this.FetchFinished();
      this.Registers.HL = num;
      return 10;
    }

    private byte LD_SP_nn()
    {
      short num = this.FetchWord();
      this.FetchFinished(isLdSp: true);
      this.Registers.SP = num;
      return 10;
    }

    private byte LD_IX_nn()
    {
      short num = this.FetchWord();
      this.FetchFinished();
      this.Registers.IX = num;
      return 14;
    }

    private byte LD_IY_nn()
    {
      short num = this.FetchWord();
      this.FetchFinished();
      this.Registers.IY = num;
      return 14;
    }

    private byte EX_AF_AF()
    {
      this.FetchFinished();
      short af = this.Registers.AF;
      this.Registers.AF = this.Registers.Alternate.AF;
      this.Registers.Alternate.AF = af;
      return 4;
    }

    private byte DJNZ_d()
    {
      byte num = this.ProcessorAgent.FetchNextOpcode();
      this.FetchFinished();
      if (this.Registers.B-- == (byte) 1)
        return 8;
      this.Registers.PC += (ushort) (sbyte) num;
      return 13;
    }

    private byte NOP2()
    {
      this.FetchFinished();
      return 8;
    }

    private byte NOP()
    {
      this.FetchFinished();
      return 4;
    }

    public IZ80ProcessorAgent ProcessorAgent { get; set; }

    public Z80InstructionExecutor()
    {
      this.Initialize_CB_InstructionsTable();
      this.Initialize_DD_InstructionsTable();
      this.Initialize_DDCB_InstructionsTable();
      this.Initialize_ED_InstructionsTable();
      this.Initialize_FD_InstructionsTable();
      this.Initialize_FDCB_InstructionsTable();
      this.Initialize_SingleByte_InstructionsTable();
      this.GenerateParityTable();
    }

    public int Execute(byte firstOpcodeByte)
    {
      this.Registers = this.ProcessorAgent.Registers;
      switch (firstOpcodeByte)
      {
        case 203:
          return this.Execute_CB_Instruction();
        case 221:
          return this.Execute_DD_Instruction();
        case 237:
          return this.Execute_ED_Instruction();
        case 253:
          return this.Execute_FD_Instruction();
        default:
          return this.Execute_SingleByte_Instruction(firstOpcodeByte);
      }
    }

    private int Execute_CB_Instruction()
    {
      this.Inc_R();
      this.Inc_R();
      return (int) this.CB_InstructionExecutors[(int) this.ProcessorAgent.FetchNextOpcode()]();
    }

    private int Execute_ED_Instruction()
    {
      this.Inc_R();
      this.Inc_R();
      byte secondOpcodeByte = this.ProcessorAgent.FetchNextOpcode();
      if (Z80InstructionExecutor.IsUnsupportedInstruction(secondOpcodeByte))
        return this.ExecuteUnsopported_ED_Instruction(secondOpcodeByte);
      return secondOpcodeByte >= (byte) 160 ? (int) this.ED_Block_InstructionExecutors[(int) secondOpcodeByte - 160]() : (int) this.ED_InstructionExecutors[(int) secondOpcodeByte - 64]();
    }

    private static bool IsUnsupportedInstruction(byte secondOpcodeByte) => secondOpcodeByte < (byte) 64 || secondOpcodeByte.Between((byte) 128, (byte) 159) || secondOpcodeByte.Between((byte) 164, (byte) 167) || secondOpcodeByte.Between((byte) 172, (byte) 175) || secondOpcodeByte.Between((byte) 180, (byte) 183) || secondOpcodeByte.Between((byte) 188, (byte) 191) || secondOpcodeByte > (byte) 191;

    protected virtual int ExecuteUnsopported_ED_Instruction(byte secondOpcodeByte) => (int) this.NOP2();

    private int Execute_SingleByte_Instruction(byte firstOpcodeByte)
    {
      this.Inc_R();
      return (int) this.SingleByte_InstructionExecutors[(int) firstOpcodeByte]();
    }

    public event EventHandler<InstructionFetchFinishedEventArgs> InstructionFetchFinished;

    private void FetchFinished(bool isRet = false, bool isHalt = false, bool isLdSp = false, bool isEiOrDi = false) => this.InstructionFetchFinished((object) this, new InstructionFetchFinishedEventArgs()
    {
      IsRetInstruction = isRet,
      IsHaltInstruction = isHalt,
      IsLdSpInstruction = isLdSp,
      IsEiOrDiInstruction = isEiOrDi
    });

    private void Inc_R() => this.ProcessorAgent.Registers.R = this.ProcessorAgent.Registers.R.Inc7Bits();

    private short FetchWord() => NumberUtils.CreateShort(this.ProcessorAgent.FetchNextOpcode(), this.ProcessorAgent.FetchNextOpcode());

    private void WriteShortToMemory(ushort address, short value)
    {
      this.ProcessorAgent.WriteToMemory(address, value.GetLowByte());
      this.ProcessorAgent.WriteToMemory((ushort) ((uint) address + 1U), value.GetHighByte());
    }

    private short ReadShortFromMemory(ushort address) => NumberUtils.CreateShort(this.ProcessorAgent.ReadFromMemory(address), this.ProcessorAgent.ReadFromMemory((ushort) ((uint) address + 1U)));

    private void SetFlags3and5From(byte value) => this.Registers.F = (byte) ((int) this.Registers.F & -41 | (int) value & 40);
  }
}
