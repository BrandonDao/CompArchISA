using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            @"C:\Users\brand\Documents\GitHub\CompArchISA\ISA\Assembler\bin\Debug\netcoreapp3.1\AssembledProgramBytes.bin");

        private static readonly byte[] RAM = new byte[16]; // 4 possible commands

        private static readonly short[] registers = new short[8];
        private const int rIP = 6; // instruction pointer
        private const int rBP = 7; // base pointer

        private static Dictionary<byte, Instruction> opcodeToInstruction;



        public static void LoadProgram()
        {
            // gets all types in the project
            Type instructionType = typeof(Instruction);
            Type[] allTypes = Assembly.GetAssembly(instructionType).GetTypes();

            // gets all types that inherit 'Instruction' and have the Opcode attribute
            IEnumerable<Type> validInstructions =
                allTypes.Where(type => type.IsSubclassOf(instructionType)
             && type.GetCustomAttribute(typeof(SharedLibrary.CustomAttributes.OpcodeAttribute)) != null);

            // Creates a dictionary from the types
            opcodeToInstruction = validInstructions.ToDictionary(
                type => type.GetCustomAttribute<SharedLibrary.CustomAttributes.OpcodeAttribute>().Opcode,
                type => (Instruction)Activator.CreateInstance(type));



        }
        
        public static void Emulate()
        {

            if (registers[rBP] + registers[rIP] >= programBytes.Length) registers[rIP] = registers[rBP];

            Opcodes opCode = (Opcodes)programBytes[registers[rBP] + registers[rIP]];

            // Do I individually load each byte of the assembled program into memory or do I do entire instructions at a time?

            // For actually emulating:
            //  - large switch case for every instruction
            //  - use reflection to get a dictionary from opcode to instruction then run 'Execute' which I enforce on every instruction



            registers[rIP] += 4;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Emulator.LoadProgram(); // allowed? please?
            
            while(true)
            {
                Emulator.Emulate();
            }
        }
    }
}
