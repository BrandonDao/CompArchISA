using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedLibrary.Layouts
{
    public class ConditionalSkipLayout : ILayout
    {
        public string RegexPattern => @"(SKP[FT]) R([012]\d|\d) (?:([0-9ABCDEF][0-9ABCDEF]?) ([0-9ABCDEF][0-9ABCDEF]?)|(?:(.*:)))(?:\r)?";

        public byte[] Parse(Match match)
        {
            // Uses label name
            if (match.Groups[4].Success)
            {
                var lineNum = Instruction.LabelToLineMap[match.Groups[2].Value];

                return new byte[]
                {
                    Instruction.NameToOpcodeMap[match.Groups[1].Value],
                    byte.Parse(match.Groups[2].Value),
                    (byte)(lineNum & (0xFF << 8)),
                    (byte)(lineNum & 0xFF),
                };
            }

            // Uses line number
            return new byte[]
            {
                Instruction.NameToOpcodeMap[match.Groups[1].Value],
                byte.Parse(match.Groups[2].Value),
                Instruction.HexToDec(match.Groups[3].Value),
                Instruction.HexToDec(match.Groups[4].Value),
            };
        }
    }
}
