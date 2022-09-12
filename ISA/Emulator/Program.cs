using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Emulator
{
    public static class Emulator
    {
        enum Opcodes : byte
        {
            ADD = 0x10,
            SKP = 0x30,
            SET = 0x40
        }

        private static readonly byte[] programBytes = System.IO.File.ReadAllBytes(
            @"C:\Users\brand\Documents\GitHub\Projects\CSharp\InstructionSetArchitecture\ISA\Assembler\bin\Debug\netcoreapp3.1\AssembledProgramBytes.bin");

        private static readonly short[] Registers = new short[32];
        //private static readonly uint[] RAM = new uint[8];
        private const int rIP = 30;
        private const int rBP = 31;
        private static Dictionary<byte, Instruction> opcodeToInstruction;

        private static byte NthByte(this uint number, int byteIndex)
        {
            return (byte)((number & (255 << (byteIndex * 8))) >> (byteIndex * 8));
        }
        private static void CreateOpcodeToInstruction()
        {
            // gets all types in the project
            Type instructionType = typeof(Instruction);
            Type[] allTypes = Assembly.GetAssembly(instructionType).GetTypes();

            // gets all types that inherit 'Instruction' and have the Opcode attribute
            IEnumerable<Type> validInstructions =
                allTypes.Where(type => type.IsSubclassOf(instructionType)
             && type.GetCustomAttribute(typeof(SharedLibrary.CustomAttributes.OpcodeAttribute)) != null);

            opcodeToInstruction = validInstructions.ToDictionary(
                type => type.GetCustomAttribute<SharedLibrary.CustomAttributes.OpcodeAttribute>().Opcode,
                type => (Instruction)Activator.CreateInstance(type));
        }
        
        public static void Emulate()
        {
            CreateOpcodeToInstruction();

            if (Registers[rBP] + Registers[rIP] >= programBytes.Length) Registers[rIP] = Registers[rBP];

            Opcodes opCode = (Opcodes)programBytes[Registers[rBP] + Registers[rIP]];

            // Do I individually load each byte of the assembled program into memory or do I do entire instructions at a time?

            // For actually emulating:
            //  - large switch case for every instruction
            //  - use reflection to get a dictionary from opcode to instruction then run 'Execute' which I enforce on every instruction



            Registers[rIP] += 4;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Emulator.Load();

            
            while(true)
            {
                Emulator.Emulate();
            }
        }
    }
}
