using SharedLibrary.CustomAttributes;
using SharedLibrary.Layouts;
using System;

namespace SharedLibrary.SkipInstructions
{
    [Opcode(0x30)]
    public class SKP : Instruction
    {
        public override string RegexPattern => skipLayout.RegexPattern;

        private readonly SkipLayout skipLayout = new SkipLayout();
        protected override ILayout Layout => skipLayout;

        public SKP()
        {
        }
        public SKP(byte[] instructionData)
        {
            this.instructionData = instructionData;
        }
    }
}
