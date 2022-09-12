using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.CustomAttributes
{
    public class OpcodeAttribute : Attribute
    {
        public byte Opcode { get; }
        public OpcodeAttribute(byte opcode)
        {
            Opcode = opcode;
        }
    }
}
