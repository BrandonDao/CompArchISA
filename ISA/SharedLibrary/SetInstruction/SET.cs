using SharedLibrary.CustomAttributes;
using SharedLibrary.Layouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.SetInstruction
{
    [Opcode(0x40)]
    public class SET : Instruction
    {
        public override string RegexPattern => setLayout.RegexPattern;

        private readonly SetLayout setLayout = new SetLayout();
        protected override ILayout Layout => setLayout;

        public SET()
        {
        }
        public SET(byte[] instructionData)
        {
            this.instructionData = instructionData;
        }
    }
}
