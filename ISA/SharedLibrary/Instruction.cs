using SharedLibrary.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SharedLibrary
{
    public abstract class Instruction
    {
        public static byte HexToDec(string hex)
        {
            int pos = 1;
            int dec = 0;

            for (int i = hex.Length - 1; i >= 0; i--)
            {
                var hexVal = hex[i] - '0';
                if (hexVal > 9)
                {
                    hexVal = hex[i] - '7';
                }

                dec += hexVal * pos;
                pos *= 16;
            }
            return (byte)dec;
        }
        public static Dictionary<string, Instruction> InstructionNameMap = new Dictionary<string, Instruction>();
        public static Dictionary<string, byte> NameToOpcodeMap = new Dictionary<string, byte>();
        public static Dictionary<string, short> LabelToLineMap = new Dictionary<string, short>();

        protected byte[] instructionData;
        public uint MachineCode => (uint)(instructionData[0] << 24 | instructionData[1] << 16 | instructionData[2] << 8 | instructionData[3]);
        public abstract string RegexPattern { get; }
        protected abstract ILayout Layout { get; }
        //public abstract void Execute();

        private static Instruction[] GetAllInstructions()
        {
            // gets all types in the project
            Type instructionType = typeof(Instruction);
            Type[] allTypes = Assembly.GetAssembly(instructionType).GetTypes();

            // gets all types that inherit 'Instruction' and have the Opcode attribute
            IEnumerable<Type> validInstructions =
                allTypes.Where(type => type.IsSubclassOf(instructionType)
             && type.GetCustomAttribute(typeof(CustomAttributes.OpcodeAttribute)) != null);

            // Maps all instruction names to the corresponding instruction's opcode
            NameToOpcodeMap = validInstructions.ToDictionary(type => type.Name, type => type.GetCustomAttribute<CustomAttributes.OpcodeAttribute>().Opcode);

            // Instantiates all valid instructions and stores them
            IEnumerable<Instruction> instantiatedInstructions = validInstructions.Select(type => (Instruction)Activator.CreateInstance(type));

            // Maps all instruction names to the corresponding instruction
            InstructionNameMap = instantiatedInstructions.ToDictionary(type => type.GetType().Name, type => type);

            return instantiatedInstructions.ToArray();
        }
        public static byte[] ToByteArray(List<Instruction> instructions)
        {
            var instructionDataLength = instructions[0].instructionData.Length;
            var output = new byte[instructions.Count * instructionDataLength];

            for (int i = 0; i < instructions.Count; i++)
            {
                for (int x = 0; x < instructionDataLength; x++)
                {
                    output[i * instructionDataLength + x] = instructions[i].instructionData[x];
                }
            }

            return output;
        }

        private static readonly string labelPattern = @"(?i)^([a-z]+):";
        public static List<Instruction> Parse(string[] assemblyInstructions)
        {
            Instruction[] allInstructions = GetAllInstructions();
            var parsedInstructions = new List<Instruction>();

            for (short lineNum = 0; lineNum < assemblyInstructions.Length; lineNum++)
            {
                Match match = Regex.Match(assemblyInstructions[lineNum], labelPattern);

                if (!match.Success) continue;

                LabelToLineMap.Add(assemblyInstructions[lineNum], lineNum);
            }

            foreach (var assemblyInstruction in assemblyInstructions)
            {
                foreach (Instruction instruction in allInstructions)
                {
                    Match match = Regex.Match(assemblyInstruction, instruction.RegexPattern);
                    if (!match.Success) continue;

                    Instruction tempInstruction = InstructionNameMap[match.Groups[1].Value];
                    parsedInstructions.Add((Instruction)Activator.CreateInstance(tempInstruction.GetType(), tempInstruction.Layout.Parse(match)));
                    break;
                }
            }

            return parsedInstructions;
        }
    }
}
