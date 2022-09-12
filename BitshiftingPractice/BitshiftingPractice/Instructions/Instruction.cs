using System;
using System.Collections.Generic;

namespace BitshiftingPractice.Instructions
{
    public enum OpCodes : byte
    {
        ADD = 0x10,
        SUB = 0x11,
        LT = 0x25,
        JMP = 0x30,
        JMPT = 0x31,
        JMPF = 0x32,
        SET = 0x40
    }

    public abstract class Instruction
    {
        protected byte[] instructionData;
        private static readonly Dictionary<string, Func<string, Instruction>> instructionMap = new Dictionary<string, Func<string, Instruction>>()
        {
            ["ADD"] = (instructionStr) => new ADD(instructionStr),
            ["SUB"] = (instructionStr) => new SUB(instructionStr)
        };

        public uint MachineCode => (uint)(instructionData[0] << 24 | instructionData[1] << 16 | instructionData[2] << 8 | instructionData[3]);
        public abstract byte OpCode { get; }
        public abstract string RegexPattern { get; }

        public abstract byte[] InstructionToInstructionData(string instructionStr, OpCodes opCode);
        public abstract string Disassemble();

        public static Instruction Parse(string instructionStr)
        {
            string opCodeStr = instructionStr.Split(' ')[0];

            return instructionMap[opCodeStr].Invoke(instructionStr);
        }
    }
}
