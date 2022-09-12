using JVM.ConstantInfos;
using System;

namespace JVM.Attributes
{
    public class Code_Attribute_Info : Attribute_Info
    {
        public ushort Max_Stack { get; private set; }
        public ushort Max_Locals { get; private set; }
        public uint Code_Length { get; private set; }
        public byte[] Code { get; private set; }
        public ushort Exception_Table_Length { get; private set; }
        public Exception_Table[] Exception_Table { get; private set; }
        public ushort Attributes_Count { get; private set; }
        public Attribute_Info[] Attributes { get; private set; }

        public Code_Attribute_Info(ushort attributeNameIndex, uint attributeLength, CP_Info[] Constant_Pool, ref ReadOnlySpan<byte> span)
            : base(attributeNameIndex, attributeLength, ref span)
        {
            Max_Stack = span.U2();
            Max_Locals = span.U2();

            Code_Length = span.U4();
            Code = new byte[Code_Length];
            for(int i = 0; i < Code_Length; i++)
            {
                Code[i] = span.U1();
            }

            Exception_Table_Length = span.U2();
            Exception_Table = new Exception_Table[Exception_Table_Length];
            for (int i = 0; i < Exception_Table_Length; i++)
            {
                Exception_Table[i] = new Exception_Table(ref span);
            }

            Attributes_Count = span.U2();
            Attributes = new Attribute_Info[Attributes_Count];
            ClassFile.ParseAttributes(Attributes_Count, Constant_Pool, Attributes, ref span);
        }
    }
}
