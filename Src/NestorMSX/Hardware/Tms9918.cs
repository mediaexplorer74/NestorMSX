﻿using System;
using System.Diagnostics;
using System.Timers;
using Konamiman.NestorMSX.Exceptions;
using Konamiman.Z80dotNet;

namespace Konamiman.NestorMSX.Hardware
{
    /// <summary>
    /// Represents a TMS9918 video display processor (text mode only supported)
    /// </summary>
    public class Tms9918 : IExternallyControlledTms9918, IDisposable
    {
        private const int colorTableLength = 32;
        private const int patternGeneratorTableLength = 2048;

        private readonly ITms9918DisplayRenderer displayRenderer;
        private PlainMemory Vram;
        private bool generateInterrupts;
        
        private int _patternGeneratorTableAddress;
        private int patternGeneratorTableAddress
        {
            get
            {
                return _patternGeneratorTableAddress;
            }
            set
            {
                _patternGeneratorTableAddress = value;
                for(var position = 0; position < patternGeneratorTableLength; position++)
                    displayRenderer.WriteToPatternGeneratorTable(position, Vram[patternGeneratorTableAddress + position]);
            }
        }

        private byte? valueWrittenToPort1;
        private byte readAheadBuffer;
        private Timer interruptTimer;
        private byte statusRegisterValue;
        private int vramPointer;
        private Bit[] modeBits;
        private int[] PatternNameTableSizes = { 768, 960 };
        
        private int screenMode = 0;

        private int _PatternNameTableAddress;
        public int PatternNameTableAddress
        {
            get
            {
                return _PatternNameTableAddress;
            }
            private set
            {
                _PatternNameTableAddress = value;
                ReprintAll();
            }
        }

        private int _colorTableAddress;
        private int colorTableAddress
        {
            get
            {
                return _colorTableAddress;
            }
            set
            {
                _colorTableAddress = value;
                for(int i = 0; i < colorTableLength; i++)
                    displayRenderer.WriteToColourTable(i, Vram[_colorTableAddress + i]);
            }
        }

        public Tms9918(ITms9918DisplayRenderer displayRenderer, Configuration config)
        {
            _PatternNameTableAddress = 0x1800;
            _colorTableAddress = 0x2000;

            Vram = new PlainMemory(16384);
            modeBits = new Bit[] {0, 0, 0};

            this.displayRenderer = displayRenderer;
            displayRenderer.BlankScreen();
            SetScreenMode(0);

            if(config.VdpFrequencyMultiplier < 0.01M || config.VdpFrequencyMultiplier > 100)
                throw new ConfigurationException("The VDP frequency multiplier must be a number between 0.01 and 100.");

            interruptTimer = new Timer(TimeSpan.FromSeconds(((double)1)/60).TotalMilliseconds / (double)config.VdpFrequencyMultiplier);
            interruptTimer.Elapsed += InterruptTimerOnElapsed;
            interruptTimer.Start();
        }

        private void SetScreenMode(int mode)
        {
            screenMode = mode;
            displayRenderer.SetScreenMode((byte)mode);
            PatternNameTableSize = PatternNameTableSizes[mode & 1];
        }

        void ReprintAll()
        {
            for(int i = 0; i < PatternNameTableSize; i++)
                displayRenderer.WriteToNameTable(i, Vram[PatternNameTableAddress + i]);
        }

        private void InterruptTimerOnElapsed(object sender, ElapsedEventArgs args)
        {
            statusRegisterValue |= 0x80;
            if(generateInterrupts)
                IntLineIsActive = true;
        }

        public event EventHandler NmiInterruptPulse;
        public bool IntLineIsActive { get; private set; }
        public byte? ValueOnDataBus { get; private set; }

        public void WriteToPort(Bit portNumber, byte value)
        {
            if(portNumber == 0) {
                WriteVram(vramPointer, value);
                vramPointer = (vramPointer+1) & 0x3FFF;
                readAheadBuffer = value;
                valueWrittenToPort1 = null;
                return;
            }

            if(valueWrittenToPort1 == null) {
                valueWrittenToPort1 = value;
                return;
            }

            if((value & 0x80) == 0) {
                SetVramAccess(valueWrittenToPort1.Value, value);
            } else {
                WriteControlRegister(valueWrittenToPort1.Value, value);
            }

            valueWrittenToPort1 = null;
        }

        private void SetVramAccess(byte firstByte, byte secondByte)
        {
            vramPointer = firstByte | ((secondByte & 0x3F) << 8);

            if((secondByte & 0x40) == 0) {
                readAheadBuffer = Vram[vramPointer];
                vramPointer = (vramPointer++) & 0x3FFF;
            }
        }

        private void WriteControlRegister(byte value, byte register)
        {
            register &= 7;

            switch(register) {
                case 0:
                    SetModeBit(2, value.GetBit(1), true);
                    break;

                case 1:
                    SetModeBit(1, value.GetBit(4), false);
                    SetModeBit(3, value.GetBit(3), true);

                    generateInterrupts = value.GetBit(5);
                    if(generateInterrupts && statusRegisterValue.GetBit(7))
                        IntLineIsActive = true;
                    else
                        IntLineIsActive = false;

                    if(value.GetBit(6))
                        displayRenderer.ActivateScreen();
                    else
                        displayRenderer.BlankScreen();

                    break;

                case 2:
                    PatternNameTableAddress = value << 10;
                    break;

                case 3:
                    colorTableAddress = value << 6;
                    break;

                case 4:
                    patternGeneratorTableAddress = value << 11;
                    break;

                case 7:
                    displayRenderer.SetBackdropColor((byte)(value & 0x0F));
                    displayRenderer.SetTextColor((byte)(value >> 4));
                    break;
            }
        }

        private void SetModeBit(int mode, Bit value, bool changeScreenMode)
        {
            modeBits[mode - 1] = value;
            if(!changeScreenMode)
                return;

            for(byte i = 0; i <= 2; i++) {
                if(modeBits[i]) {
                    SetScreenMode((byte)(i + 1));
                    return;
                }
            }

            SetScreenMode(0);
        }

        public byte ReadFromPort(Bit portNumber)
        {
            valueWrittenToPort1 = null;

            if(portNumber == 0) {
                var value = readAheadBuffer;
                vramPointer = (vramPointer+1) & 0x3FFF;
                readAheadBuffer = Vram[vramPointer];
                return value;
            }
            else {
                var value = statusRegisterValue;
                statusRegisterValue &= 0x7F;
                IntLineIsActive = false;
                return value;
            }
        }

        public void WriteVram(int address, byte value)
        {
            Vram[address] = value;
            if(address >= PatternNameTableAddress && address < PatternNameTableAddress + PatternNameTableSize) {
                displayRenderer.WriteToNameTable(address - PatternNameTableAddress, value);
            }
            if(address >= patternGeneratorTableAddress && address < patternGeneratorTableAddress + patternGeneratorTableLength) {
                displayRenderer.WriteToPatternGeneratorTable(address - patternGeneratorTableAddress, value);
            }
            if(screenMode != 1 && address >= colorTableAddress && address < colorTableAddress + colorTableLength) {
                displayRenderer.WriteToColourTable(address - colorTableAddress, value);
            }
        }

        public byte ReadVram(int address)
        {
            return Vram[address];
        }

        public byte[] GetVramContents(int startAddress, int length)
        {
            return Vram.GetContents(startAddress, length);
        }

        public int PatternNameTableSize { get; private set; }

        public void SetVramContents(int startAddress, byte[] contents, int startIndex = 0, int? length = null)
        {
            var actualLength = length.GetValueOrDefault(contents.Length);
            for(int i = 0; i < actualLength; i++)
                WriteVram(startAddress + i, contents[startIndex + i]);
        }

        public void Dispose()
        {
            interruptTimer.Dispose();
        }
    }
}
