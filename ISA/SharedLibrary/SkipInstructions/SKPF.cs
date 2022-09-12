using SharedLibrary.CustomAttributes;
using SharedLibrary.Layouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.SkipInstructions
{
    [Opcode(0x32)]
    public class SKPF : Instruction
    {
        public override string RegexPattern => skipLayout.RegexPattern;

        private readonly SkipLayout skipLayout = new SkipLayout();
        protected override ILayout Layout => skipLayout;

        public SKPF()
        {
        }
        public SKPF(byte[] instructionData)
        {
            this.instructionData = instructionData;
        }
    }
}
