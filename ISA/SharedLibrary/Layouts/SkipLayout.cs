using System.Text.RegularExpressions;

namespace SharedLibrary.Layouts
{
    public class SkipLayout : ILayout
    {
        public string RegexPattern => @"(SKP) (?:([0-9ABCDEF][0-9ABCDEF]?) ([0-9ABCDEF][0-9ABCDEF]?)|(?:(.*:)))(?:\r)?";

        public byte[] Parse(Match match)
        {
            // Uses label name
            if (match.Groups[4].Success)
            {
                var lineNum = Instruction.LabelToLineMap[match.Groups[4].Value + "\r"];

                return new byte[]
                {
                    Instruction.NameToOpcodeMap["SKP"],
                    00,
                    (byte)(lineNum & (0xFF << 8)),
                    (byte)(lineNum & 0xFF),
                };
            }

            // Uses line number
            return new byte[]
            {
                Instruction.NameToOpcodeMap["SKP"],
                00,
                Instruction.HexToDec(match.Groups[3].Value),
                Instruction.HexToDec(match.Groups[4].Value),
            };
        }
    }
}
