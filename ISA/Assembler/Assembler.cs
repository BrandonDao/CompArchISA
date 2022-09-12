using SharedLibrary;
using System.Collections.Generic;

namespace Assembler
{
   public static class Assembler
    {
        public static byte[] Assemble(string[] assemblyProgram)
        {
            List<Instruction> parsedInstructions = Instruction.Parse(assemblyProgram);

            return Instruction.ToByteArray(parsedInstructions);
        }
    }
}
