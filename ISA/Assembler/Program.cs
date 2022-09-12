using System;

namespace Assembler
{
    class Program
    {
        public static string[] LoadAssemblyProgram()
        {
            return System.IO.File.ReadAllText("AssemblyProgram.asm").Split("\n");
        }

        static void Main(string[] args)
        {
            var asm = Assembler.Assemble(LoadAssemblyProgram());

            System.IO.File.WriteAllBytes("AssembledProgramBytes.bin", Linker.Link(asm));
        }
    }
}
