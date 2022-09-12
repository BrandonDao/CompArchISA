using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BitshiftingPractice.Instructions
{
    public abstract class MathAndLogic : Instruction
    {
        public override string RegexPattern => "(?:ADD|SUB|MUL|DIV|MOD|AND|OR|XOR|GT|LT|EQ) R([012][\\d]|\\d) R([012][\\d]|\\d) R([012][\\d]|\\d)";

        public override byte[] InstructionToInstructionData(string instructionStr, OpCodes opCode)
        {
            Match match = Regex.Match(instructionStr, RegexPattern);

            if (!match.Success) throw new ArgumentException($"Invalid {opCode} instruction!");

            return new byte[4]
            {
               (byte)opCode,
                byte.Parse(match.Groups[1].Value),
                byte.Parse(match.Groups[2].Value),
                byte.Parse(match.Groups[3].Value),
            };
        }

        public override string Disassemble()
        {
            return $"{Enum.GetName(typeof(OpCodes), OpCode)} R{instructionData[1]} {instructionData[2]} R{instructionData[3]}";
        }
    }
}