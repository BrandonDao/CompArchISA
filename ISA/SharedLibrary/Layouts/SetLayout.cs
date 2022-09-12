using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedLibrary.Layouts
{
    public class SetLayout : ILayout
    {
        public string RegexPattern => @"(SET) R([012]\d|\d) ([0-9ABCDEF][0-9ABCDEF]?) ([0-9ABCDEF][0-9ABCDEF]?)(?:\r)?";

        public byte[] Parse(Match match)
        {
            return new byte[4]
            {
                Instruction.NameToOpcodeMap["SET"],
                byte.Parse(match.Groups[2].Value),
                Instruction.HexToDec(match.Groups[3].Value),
                Instruction.HexToDec(match.Groups[4].Value)
            };
        }
    }
}
