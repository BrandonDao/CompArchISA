using System.Text.RegularExpressions;

namespace SharedLibrary.Layouts
{
    public class SetLayout : ILayout
    {
        public string RegexPattern => @"(SET) R(0*[1-6]?) ([0-9A-F][0-9A-F]?) ([0-9A-F][0-9A-F]?)";

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
