using System.Text.RegularExpressions;

namespace SharedLibrary.Layouts
{
    public class MathAndLogicLayout : ILayout
    {
        public string RegexPattern => @"(ADD|SUB|MUL|DIV|MOD|AND|OR|XOR|GT|LT|EQ) R(0*[1-6]?) R(0*[1-6]?) R(0*[1-6]?)";

        public byte[] Parse(Match match)
        {
            return new byte[4]
            {
                Instruction.NameToOpcodeMap[match.Groups[1].Value],
                byte.Parse(match.Groups[2].Value),
                byte.Parse(match.Groups[3].Value),
                byte.Parse(match.Groups[4].Value)
            };
        }
    }
}
