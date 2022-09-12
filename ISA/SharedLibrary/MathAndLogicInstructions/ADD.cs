using SharedLibrary.CustomAttributes;
using SharedLibrary.Layouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.MathAndLogicInstructions
{
    [Opcode(0x10)]
    public class ADD : Instruction
    {
        public override string RegexPattern => mathAndLogicLayout.RegexPattern;

        private readonly MathAndLogicLayout mathAndLogicLayout = new MathAndLogicLayout();
        protected override ILayout Layout => mathAndLogicLayout;

        public ADD()
        {
        }
        public ADD(byte[] instructionData)
        {
            this.instructionData = instructionData;
        }
    }
}
